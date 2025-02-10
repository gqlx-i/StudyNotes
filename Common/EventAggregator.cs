﻿using StudyNotes.Interface;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace StudyNotes.Common
{
    /// <summary>
    /// Default implementation of IEventAggregator
    /// </summary>
    public class EventAggregator : IEventAggregator
    {
        /// <summary>
        /// Channel which handlers are subscribed to / messages are published to, if no channels are named
        /// </summary>
        public static readonly string DefaultChannel = "DefaultChannel";

        private readonly List<Handler> handlers = new();
        private readonly object handlersLock = new();

        /// <summary>
        /// Register an instance as wanting to receive events. Implement IHandle{T} for each event type you want to receive.
        /// </summary>
        /// <param name="handler">Instance that will be registered with the EventAggregator</param>
        /// <param name="channels">Channel(s) which should be subscribed to. Defaults to EventAggregator.DefaultChannel if none given</param>
        public void Subscribe(IHandle handler, params string[] channels)
        {
            lock (this.handlersLock)
            {
                // Is it already subscribed?
                Handler subscribed = this.handlers.FirstOrDefault(x => x.IsHandlerForInstance(handler));
                if (subscribed == null)
                    this.handlers.Add(new Handler(handler, channels)); // Adds default topic if appropriate
                else
                    subscribed.SubscribeToChannels(channels);
            }
        }

        /// <summary>
        /// Unregister as wanting to receive events. The instance will no longer receive events after this is called.
        /// </summary>
        /// <param name="handler">Instance to unregister</param>
        /// <param name="channels">Channel(s) to unsubscribe from. Unsubscribes from everything if no channels given</param>
        public void Unsubscribe(IHandle handler, params string[] channels)
        {
            lock (this.handlersLock)
            {
                Handler existingHandler = this.handlers.FirstOrDefault(x => x.IsHandlerForInstance(handler));
                if (existingHandler != null && existingHandler.UnsubscribeFromChannels(channels)) // Handles default topic appropriately
                    this.handlers.Remove(existingHandler);
            }
        }

        /// <summary>
        /// Publish an event to all subscribers, using the specified dispatcher
        /// </summary>
        /// <param name="message">Event to publish</param>
        /// <param name="dispatcher">Dispatcher to use to call each subscriber's handle method(s)</param>
        /// <param name="channels">Channel(s) to publish the message to. Defaults to EventAggregator.DefaultChannel none given</param>
        public void PublishWithDispatcher(object message, Action<Action> dispatcher, params string[] channels)
        {
            // We have to be re-entrant, since a handler can fire another message, or subscribe. This means that we can't
            // be in the middle of iterating this.handlers when we invoke a handler.

            List<HandlerInvoker> invokers;
            lock (this.handlersLock)
            {
                // Start by clearing up dead handlers
                this.handlers.RemoveAll(x => !x.IsAlive);

                Type messageType = message.GetType();
                invokers = this.handlers.SelectMany(x => x.GetInvokers(messageType, channels)).ToList();
            }

            foreach (HandlerInvoker invoker in invokers)
            {
                dispatcher(() => invoker.Invoke(message));
            }
        }

        private class Handler
        {
            private static readonly string[] DefaultChannelArray = new[] { DefaultChannel };

            private readonly WeakReference target;
            private readonly List<HandlerInvoker> invokers = new();
            private readonly HashSet<string> channels = new();

            public bool IsAlive => this.target.IsAlive;

            public Handler(object handler, string[] channels)
            {
                Type handlerType = handler.GetType();
                this.target = new WeakReference(handler);

                foreach (Type implementation in handler.GetType().GetInterfaces().Where(x => x.IsGenericType && typeof(IHandle).IsAssignableFrom(x)))
                {
                    Type messageType = implementation.GetGenericArguments()[0];
                    this.invokers.Add(new HandlerInvoker(this.target, handlerType, messageType, implementation.GetMethod("Handle")));
                }

                if (channels.Length == 0)
                    channels = DefaultChannelArray;
                this.SubscribeToChannels(channels);
            }

            public bool IsHandlerForInstance(object subscriber)
            {
                return this.target.Target == subscriber;
            }

            public void SubscribeToChannels(string[] channels)
            {
                this.channels.UnionWith(channels);
            }

            public bool UnsubscribeFromChannels(string[] channels)
            {
                // If channels is empty, unsubscribe from everything
                if (channels.Length == 0)
                    return true;
                this.channels.ExceptWith(channels);
                return this.channels.Count == 0;
            }

            public IEnumerable<HandlerInvoker> GetInvokers(Type messageType, string[] channels)
            {
                if (!this.IsAlive)
                    return Enumerable.Empty<HandlerInvoker>();

                if (channels.Length == 0)
                    channels = DefaultChannelArray;

                // We're not subscribed to any of the channels
                if (!channels.Any(x => this.channels.Contains(x)))
                    return Enumerable.Empty<HandlerInvoker>();

                return this.invokers.Where(x => x.CanInvoke(messageType));
            }
        }

        private class HandlerInvoker
        {
            private readonly WeakReference target;
            private readonly Type messageType;
            private readonly Action<object, object> invoker;

            public HandlerInvoker(WeakReference target, Type targetType, Type messageType, MethodInfo invocationMethod)
            {
                this.target = target;
                this.messageType = messageType;
                ParameterExpression targetParam = Expression.Parameter(typeof(object), "target");
                ParameterExpression messageParam = Expression.Parameter(typeof(object), "message");
                UnaryExpression castTarget = Expression.Convert(targetParam, targetType);
                UnaryExpression castMessage = Expression.Convert(messageParam, messageType);
                MethodCallExpression callExpression = Expression.Call(castTarget, invocationMethod, castMessage);
                this.invoker = Expression.Lambda<Action<object, object>>(callExpression, targetParam, messageParam).Compile();
            }

            public bool CanInvoke(Type messageType)
            {
                return this.messageType.IsAssignableFrom(messageType);
            }

            public void Invoke(object message)
            {
                object target = this.target.Target;
                // Just in case it's expired...
                if (target != null)
                    this.invoker(target, message);
            }
        }
    }

    /// <summary>
    /// Extension methods on IEventAggregator, to give more dispatching options
    /// </summary>
    public static class EventAggregatorExtensions
    {
        /// <summary>
        /// Publish an event to all subscribers, calling the handle methods on the UI thread
        /// </summary>
        /// <param name="eventAggregator">EventAggregator to publish the message with</param>
        /// <param name="message">Event to publish</param>
        /// <param name="channels">Channel(s) to publish the message to. Defaults to EventAggregator.DefaultChannel none given</param>
        public static void PublishOnUIThread(this IEventAggregator eventAggregator, object message, params string[] channels)
        {
            eventAggregator.PublishWithDispatcher(message, Execute.OnUIThread, channels);
        }

        /// <summary>
        /// Publish an event to all subscribers, calling the handle methods synchronously on the current thread
        /// </summary>
        /// <param name="eventAggregator">EventAggregator to publish the message with</param>
        /// <param name="message">Event to publish</param>
        /// <param name="channels">Channel(s) to publish the message to. Defaults to EventAggregator.DefaultChannel none given</param>
        public static void Publish(this IEventAggregator eventAggregator, object message, params string[] channels)
        {
            eventAggregator.PublishWithDispatcher(message, a => a(), channels);
        }
    }
}

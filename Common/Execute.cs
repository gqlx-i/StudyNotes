﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Threading;
using System.Windows;
using StudyNotes.Interface;

namespace StudyNotes.Common
{
    /// <summary>
    /// Static class providing methods to easily run an action on the UI thread in various ways, and some other things
    /// </summary>
    public static class Execute
    {
        private static IDispatcher _dispatcher;

        /// <summary>
        /// Gets or sets Execute's dispatcher
        /// </summary>
        /// <remarks>
        /// Should be set a <see cref="ApplicationDispatcher"/> wrapping the current application's dispatcher, which is
        /// normally done by the Bootstrapper. Can also be set to <see cref="SynchronousDispatcher.Instance"/>, or a
        /// custom <see cref="IDispatcher"/> implementation.
        /// </remarks>
        public static IDispatcher Dispatcher
        {
            get => _dispatcher ?? SynchronousDispatcher.Instance;
            set => _dispatcher = value ?? throw new ArgumentNullException(nameof(Dispatcher));
        }

        private static bool? inDesignMode;

        /// <summary>
        /// Gets or sets the default dispatcher used by PropertyChanged events.
        /// Defaults to OnUIThread
        /// </summary>
        public static Action<Action> DefaultPropertyChangedDispatcher { get; set; }

        static Execute()
        {
            DefaultPropertyChangedDispatcher = a => a();
        }

        /// <summary>
        /// Dispatches the given action to be run on the UI thread asynchronously, even if the current thread is the UI thread
        /// </summary>
        /// <param name="action">Action to run on the UI thread</param>
        public static void PostToUIThread(Action action)
        {
            Dispatcher.Post(action);
        }

        /// <summary>
        /// Dispatches the given action to be run on the UI thread asynchronously, and returns a task which completes when the action completes, even if the current thread is the UI thread
        /// </summary>
        /// <remarks>DO NOT BLOCK waiting for this Task - you'll cause a deadlock. Use PostToUIThread instead</remarks>
        /// <param name="action">Action to run on the UI thread</param>
        /// <returns>Task which completes when the action has been run</returns>
        public static Task PostToUIThreadAsync(Action action)
        {
            return PostOnUIThreadInternalAsync(action);
        }

        /// <summary>
        /// Dispatches the given action to be run on the UI thread asynchronously, or runs it synchronously if the current thread is the UI thread
        /// </summary>
        /// <param name="action">Action to run on the UI thread</param>
        public static void OnUIThread(Action action)
        {
            if (Dispatcher.IsCurrent)
                action();
            else
                Dispatcher.Post(action);
        }

        /// <summary>
        /// Dispatches the given action to be run on the UI thread and blocks until it completes, or runs it synchronously if the current thread is the UI thread
        /// </summary>
        /// <param name="action">Action to run on the UI thread</param>
        public static void OnUIThreadSync(Action action)
        {
            Exception exception = null;
            if (Dispatcher.IsCurrent)
            {
                action();
            }
            else
            {
                Dispatcher.Send(() =>
                {
                    try
                    {
                        action();
                    }
                    catch (Exception e)
                    {
                        exception = e;
                    }
                });

                if (exception != null)
                    throw new System.Reflection.TargetInvocationException("An error occurred while dispatching a call to the UI Thread", exception);
            }
        }

        /// <summary>
        /// Dispatches the given action to be run on the UI thread and returns a task that completes when the action completes, or runs it synchronously if the current thread is the UI thread
        /// </summary>
        /// <param name="action">Action to run on the UI thread</param>
        /// <returns>Task which completes when the action has been run</returns>
        public static Task OnUIThreadAsync(Action action)
        {
            if (Dispatcher.IsCurrent)
            {
                action();
                return Task.FromResult(false);
            }
            else
            {
                return PostOnUIThreadInternalAsync(action);
            }
        }

        private static Task PostOnUIThreadInternalAsync(Action action)
        {
            var tcs = new TaskCompletionSource<object>();
            Dispatcher.Post(() =>
            {
                try
                {
                    action();
                    tcs.SetResult(null);
                }
                catch (Exception e)
                {
                    tcs.SetException(e);
                }
            });
            return tcs.Task;
        }

        /// <summary>
        /// Gets or sets a value indicating whether design mode is currently active.
        /// Settable for really obscure unit testing only
        /// </summary>
        public static bool InDesignMode
        {
            get
            {
                if (inDesignMode == null)
                {
                    var descriptor = DependencyPropertyDescriptor.FromProperty(DesignerProperties.IsInDesignModeProperty, typeof(FrameworkElement));
                    inDesignMode = (bool)descriptor.Metadata.DefaultValue;
                }

                return inDesignMode.Value;
            }

            set => inDesignMode = value;
        }
    }
    /// <summary>
    /// <see cref="IDispatcher"/> implementation which can dispatch using <see cref="Dispatcher"/>
    /// </summary>
    public class ApplicationDispatcher : IDispatcher
    {
        private readonly Dispatcher dispatcher;

        /// <summary>
        /// Initialises a new instance of the <see cref="ApplicationDispatcher"/> class with the given <see cref="Dispatcher"/>
        /// </summary>
        /// <param name="dispatcher"><see cref="Dispatcher"/> to use, normally Application.Current.Dispatcher</param>
        public ApplicationDispatcher(Dispatcher dispatcher)
        {
            this.dispatcher = dispatcher ?? throw new ArgumentNullException(nameof(dispatcher));
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ApplicationDispatcher"/> class with the given <see cref="Application"/>
        /// </summary>
        /// <param name="application"><see cref="Application"/> to use, normally Application</param>
        public ApplicationDispatcher(Application application)
            : this(application?.Dispatcher ?? throw new ArgumentNullException(nameof(application)))
        {
        }

        /// <inheritdoc/>
        public void Post(Action action)
        {
            this.dispatcher.BeginInvoke(action);
        }

        /// <inheritdoc/>
        public void Send(Action action)
        {
            this.dispatcher.Invoke(action);
        }

        /// <inheritdoc/>
        public bool IsCurrent => this.dispatcher.CheckAccess();
    }

    /// <summary>
    /// <see cref="IDispatcher"/> implementation whcih dispatches synchronously.
    /// Usually used for unit testing.
    /// </summary>
    public class SynchronousDispatcher : IDispatcher
    {
        /// <summary>
        /// Gets the singleton instance of <see cref="SynchronousDispatcher"/>
        /// </summary>
        public static SynchronousDispatcher Instance { get; } = new SynchronousDispatcher();
        private SynchronousDispatcher() { }

        /// <inheritdoc/>
        public void Post(Action action)
        {
            action();
        }

        /// <inheritdoc/>
        public void Send(Action action)
        {
            action();
        }

        /// <inheritdoc/>
        public bool IsCurrent => true;
    }
}

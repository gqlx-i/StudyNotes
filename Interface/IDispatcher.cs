using StudyNotes.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyNotes.Interface
{
    /// <summary>
    /// Generalised dispatcher, which can post and send.
    /// Used by <see cref="Execute"/>.
    /// </summary>
    public interface IDispatcher
    {
        /// <summary>
        /// Execute asynchronously
        /// </summary>
        /// <param name="action">Action to execute</param>
        void Post(Action action);

        /// <summary>
        /// Execute synchronously
        /// </summary>
        /// <param name="action">Action to execute</param>
        void Send(Action action);

        /// <summary>
        /// Gets a value indicating whether the current thread is the thread being dispatched to
        /// </summary>
        bool IsCurrent { get; }
    }
}

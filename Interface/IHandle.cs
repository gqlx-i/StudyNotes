using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyNotes.Interface
{
    /// <summary>
    /// Marker for types which we might be interested in
    /// </summary>
    public interface IHandle
    {
    }

    /// <summary>
    /// Implement this to handle a particular message type
    /// </summary>
    /// <typeparam name="TMessageType">Message type to handle. Can be a base class of the messsage type(s) to handle</typeparam>
    public interface IHandle<in TMessageType> : IHandle
    {
        /// <summary>
        /// Called whenever a message of type TMessageType is posted
        /// </summary>
        /// <param name="message">Message which was posted</param>
        void Handle(TMessageType message);
    }

}

using System;

namespace Workflow.BusinessCore.BusinessLayer.Process.Exceptions
{
    /// <summary>
    /// SequenceException class
    /// </summary>
    /// <remarks>
    /// This class permits to define exception methods
    /// </remarks>
    [Serializable]
    public class SequenceException : Exception
    {
        /// <summary>
        /// Class constructor
        /// </summary>
        public SequenceException()
        {
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="message">Exception message</param>
        public SequenceException(string message) : base(message)
        {
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="innerException">Exception</param>
        public SequenceException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
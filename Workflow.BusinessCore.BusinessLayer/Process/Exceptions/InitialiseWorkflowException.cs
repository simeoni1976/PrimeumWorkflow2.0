using System;
using System.Collections.Generic;
using System.Text;

namespace Workflow.BusinessCore.BusinessLayer.Process.Exceptions
{
    /// <summary>
    /// InitialiseWorkflowException
    /// </summary>
    /// <remarks>
    /// This class permits to define exception methods
    /// </remarks>
    [Serializable]
    public class InitialiseWorkflowException : Exception
    {
        /// <summary>
        /// Constructeur vide de référence.
        /// </summary>
        public InitialiseWorkflowException()
        {
        }

        /// <summary>
        /// Constructeur avec message
        /// </summary>
        /// <param name="message"></param>
        public InitialiseWorkflowException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructeur avec message et exception parente.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public InitialiseWorkflowException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}

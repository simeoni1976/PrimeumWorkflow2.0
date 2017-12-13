using System;
using System.Collections.Generic;
using System.Text;

namespace Workflow.BusinessCore.BusinessLayer.Process.Exceptions
{
    /// <summary>
    /// DataLoadingException
    /// </summary>
    /// <remarks>
    /// This class permits to define exception methods
    /// </remarks>
    [Serializable]
    public class DataLoadingException : Exception
    {
        /// <summary>
        /// Constructeur vide de référence.
        /// </summary>
        public DataLoadingException()
        {
        }

        /// <summary>
        /// Constructeur avec message
        /// </summary>
        /// <param name="message"></param>
        public DataLoadingException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructeur avec message et exception parente.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public DataLoadingException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

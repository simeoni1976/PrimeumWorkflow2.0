using System;
using System.Collections.Generic;
using System.Text;

namespace Workflow.BusinessCore.BusinessLayer.Process.Exceptions
{
    public class ConfigurationException : Exception
    {
        /// <summary>
        /// Constructeur de référence
        /// </summary>
        public ConfigurationException()
        {
        }

        /// <summary>
        /// Construction avec message
        /// </summary>
        /// <param name="message"></param>
        public ConfigurationException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructeur avec message et exception héritante.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ConfigurationException(string message, Exception innerException) : base(message, innerException)
        {
        }

    }
}

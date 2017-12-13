using System;
using System.Collections.Generic;
using System.Text;

namespace Workflow.BusinessCore.BusinessLayer.Process.Exceptions
{
    public class ManageDataException : Exception
    {
        /// <summary>
        /// Constructeur vide de référence.
        /// </summary>
        public ManageDataException()
        {
        }

        /// <summary>
        /// Constructeur avec message
        /// </summary>
        /// <param name="message"></param>
        public ManageDataException(string message) : base(message)
        {
        }

        /// <summary>
        /// Constructeur avec message et exception parente.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ManageDataException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

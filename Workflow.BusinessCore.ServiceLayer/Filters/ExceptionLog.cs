using log4net;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace Workflow.BusinessCore.ServiceLayer.Filters
{
    /// <summary>
    ///  ExceptionLogAttribute class.
    /// </summary>
    /// <remarks>
    /// This class define an attribute to get an exception log.
    /// </remarks>
    public class ExceptionLogAttribute : ExceptionFilterAttribute
    {
        private readonly ILog _logger;

        /// <summary>
        /// Class constructor.
        /// </summary>
        public ExceptionLogAttribute()
        {
            _logger = LogManager.GetLogger(typeof(ExceptionLogAttribute));
        }

        /// <summary>
        /// OnException event.
        /// </summary>
        /// <param name="filterContext">Context</param>
		public override void OnException(ExceptionContext filterContext)
        {
            String message = String.Empty;

            var exceptionType = filterContext.Exception.GetType();
            if (exceptionType == typeof(UnauthorizedAccessException))
            {
                message = "Unauthorized Access";
            }
            else if (exceptionType == typeof(NotImplementedException))
            {
                message = "A server error occurred.";
            }
            else
            {
                message = filterContext.Exception.Message;
            }

            _logger.Debug(Logger.LogMvcAction(filterContext, message));

            base.OnException(filterContext);
        }
    }
}
using log4net;
using Microsoft.AspNetCore.Mvc.Filters;
using Workflow.BusinessCore.ServiceLayer.Filters;

namespace Workflow.BusinessCore.ServiceLayer.Filters
{
    /// <summary>
    ///  ActionLogAttribute class.
    /// </summary>
    /// <remarks>
    /// This class define an attribute to get an action log.
    /// </remarks>
    public class ActionLogAttribute : ActionFilterAttribute 
    {
        private readonly ILog _logger;
		
        /// <summary>
        /// Class constructor.
        /// </summary>
        public ActionLogAttribute()
        {
			 _logger = LogManager.GetLogger(typeof(ActionLogAttribute));
        }

        /// <summary>
        /// OnActionExecuting event.
        /// </summary>
        /// <param name="filterContext">Context</param>
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            OnAction(filterContext, "1-ActionExecutingContext");
            
            string message = string.Format("Calls {0} method...", filterContext.ActionDescriptor.RouteValues["Action"]);
    		_logger.Debug(Logger.LogMvcAction(filterContext, message));

            base.OnActionExecuting(filterContext);
        }

        /// <summary>
        /// OnActionExecuted event.
        /// </summary>
        /// <param name="filterContext">Context</param>
        public override void OnActionExecuted(ActionExecutedContext filterContext) 
        {
            OnAction(filterContext, "2-ActionExecutedContext");
            base.OnActionExecuted(filterContext);
        }

        /// <summary>
        /// OnResultExecuting event.
        /// </summary>
        /// <param name="filterContext">Context</param>
        public override void OnResultExecuting(ResultExecutingContext filterContext) 
        {
            OnAction(filterContext, "3-ResultExecutingContext");
            base.OnResultExecuting(filterContext);
        }

        /// <summary>
        /// OnResultExecuted event.
        /// </summary>
        /// <param name="filterContext">Context</param>
        public override void OnResultExecuted(ResultExecutedContext filterContext) 
        {
            OnAction(filterContext, "4-ResultExecutedContext");
            base.OnResultExecuted(filterContext);
        }

        /// <summary>
        /// this method permits to write an action in a log file.
        /// </summary>
        /// <param name="filterContext">Context</param>
        private void OnAction(FilterContext filterContext, string loggerEvent)
        { 
    		_logger.Debug(Logger.LogMvcAction(filterContext, loggerEvent));
		}
    }
}
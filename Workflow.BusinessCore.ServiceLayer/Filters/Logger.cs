using log4net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Net;

namespace Workflow.BusinessCore.ServiceLayer.Filters
{
    /// <summary>
    ///  Logger class.
    /// </summary>
    /// <remarks>
    /// This code is static and permits to format a log message.
    /// </remarks>
    public static class Logger
    {
        /// <summary>
        /// This function permits to prepare the log format.
        /// </summary>
        /// <param name="filterContext">Context</param>
        /// <param name="loggerEvent">Event name</param>
        public static string LogMvcAction(FilterContext filterContext, string loggerEvent)
        {  
            string ipAddress = filterContext.HttpContext.Request.Host.Host.ToString();
            string protocol = filterContext.HttpContext.Request.Protocol;
            string method = filterContext.HttpContext.Request.Method;
            string action = filterContext.ActionDescriptor.RouteValues["Action"];
            string controller = filterContext.ActionDescriptor.RouteValues["Controller"];
            int statusCode = filterContext is ExceptionContext ? HttpStatusCode.BadRequest.GetHashCode() : filterContext.HttpContext.Response.StatusCode;
            string statusMessage =  ((HttpStatusCode)statusCode).ToString();
            string pattern =  "{0} {1} {2} ({3}) /BusinessCore.{4}Controller.{5} : {6} ({7})";
			
			return string.Format(
                pattern, 
                ipAddress, 
                protocol, 
                method, 
                loggerEvent, 
                controller, 
                action, 
                statusMessage, 
                statusCode);
		}
    }
}
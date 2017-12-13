using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using Workflow.BusinessCore.ServiceLayer.Filters;
using Workflow.Transverse.DTO.Common;
using Workflow.Transverse.Environment;
using Workflow.Transverse.Helpers;
using Microsoft.Extensions.DependencyInjection;

namespace Workflow.BusinessCore.ServiceLayer.Controllers
{
    /// <summary>
    ///  BaseController abstract class.
    /// </summary>
    /// <remarks>
    /// This abstract class permits to do generic actions for a controller.
    /// In our case, we have one Controller equals one DTO equals one Adapter.
    /// </remarks>
    /// <typeparam name="TModel">Model</typeparam>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ActionLog, ExceptionLog]
    public abstract class BaseController<TModel, TAdapter> : Controller
        where TModel : BaseDTO
        where TAdapter : IBaseAdapter<TModel>
    {
        protected readonly IServiceProvider _serviceProvider = null;
        protected TAdapter _adapter = default(TAdapter);

        protected abstract TAdapter Adapter
        {
            get;
        }

        protected ILogger Logger
        {
            get
            {
                return _serviceProvider?.GetService<ILogger<BaseController<TModel, TAdapter>>>();
            }
        }

        protected IStringLocalizer<SharedResource> Localizer
        {
            get
            {
                return _serviceProvider?.GetService<IStringLocalizer<SharedResource>>();
            }
        }

        protected IOptions<AppSettings> AppSettings
        {
            get
            {
                return _serviceProvider?.GetService<IOptions<AppSettings>>();
            }
        }


        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de services</param>
        public BaseController(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Initialiaze results for the GET action.
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet("")]
        public async Task<IActionResult> Get()
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                IEnumerable<TModel> dtos = await Adapter.GetAll();
                res.GetObjectForJson(dtos);

                return Ok(res);
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.PROCESS_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Initialiaze results for the GET action with a parameter.
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                TModel dto = await Adapter.GetById(id);
                res.GetObjectForJson(dto);

                return Ok(res);
            }
            catch(Exception ex)
            {
                Logger.LogError(LoggingEvents.PROCESS_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

    }
}

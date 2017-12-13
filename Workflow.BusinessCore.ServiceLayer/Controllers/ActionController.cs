using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using Workflow.Transverse.DTO;
using DTO = Workflow.Transverse.DTO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Workflow.Transverse.Helpers;
using Microsoft.Extensions.Logging;
using Workflow.Transverse.Environment;

namespace Workflow.BusinessCore.ServiceLayer.Controllers
{
    public class ActionController : BaseController<DTO.Action, IActionAdapter>
    {
        protected override IActionAdapter Adapter
        {
            get
            {
                return _serviceProvider?.GetService<IActionAdapter>();
            }
        }

        public ActionController(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }


        /// <summary>
        /// Ajoute une action en base, indépendamment d'un workflow.
        /// </summary>
        /// <param name="action">Nouvelle action</param>
        /// <returns>Message de retour</returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody]DTO.Action action)
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                action = await Adapter.AddAction(action);
                res.GetObjectForJson(action);

                return Ok(res);
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

    }
}

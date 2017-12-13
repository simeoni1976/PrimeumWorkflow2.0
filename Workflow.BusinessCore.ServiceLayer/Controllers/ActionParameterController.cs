using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.Transverse.DTO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Workflow.Transverse.Helpers;
using Microsoft.Extensions.Logging;
using Workflow.Transverse.Environment;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;

namespace Workflow.BusinessCore.ServiceLayer.Controllers
{
    public class ActionParameterController : BaseController<ActionParameter, IActionParameterAdapter>
    {
        protected override IActionParameterAdapter Adapter
        {
            get
            {
                return _serviceProvider?.GetService<IActionParameterAdapter>();
            }
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de services</param>
        public ActionParameterController(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        /// <summary>
        /// Ajoute un paramètre dans l'action d'une séquence d'action.
        /// </summary>
        /// <param name="actionParameter">ActionParameter à ajouter</param>
        /// <param name="actionSequenceId">Id de la séquence d'action cible</param>
        /// <returns>Message de retour</returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody]ActionParameter actionParameter, long actionSequenceId)
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                actionParameter = await Adapter.AddActionParameter(actionParameter, actionSequenceId);
                res.GetObjectForJson(actionParameter);

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

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
    public class ActionSequenceController : BaseController<ActionSequence, IActionSequenceAdapter>
    {
        protected override IActionSequenceAdapter Adapter
        {
            get
            {
                return _serviceProvider?.GetService<IActionSequenceAdapter>();
            }
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de services</param>
        public ActionSequenceController(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }


        /// <summary>
        /// Ajoute une nouvelle action dans une séquence (existante ou non)
        /// </summary>
        /// <param name="actionSequence">Nouvelle ActionSequence</param>
        /// <param name="actionId">Id de l'Action à exécuter</param>
        /// <returns>Message de retour</returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody]ActionSequence actionSequence, long actionId)
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                actionSequence = await Adapter.AddActionSequence(actionSequence, actionId);
                res.GetObjectForJson(actionSequence);

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

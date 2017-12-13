using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using Workflow.Transverse.DTO;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.AspNetCore.Mvc;
using Workflow.Transverse.Helpers;
using Microsoft.Extensions.Logging;
using Workflow.Transverse.Environment;

namespace Workflow.BusinessCore.ServiceLayer.Controllers
{
    public class ConstraintSequenceController : BaseController<ConstraintSequence, IConstraintSequenceAdapter>
    {
        protected override IConstraintSequenceAdapter Adapter
        {
            get
            {
                return _serviceProvider?.GetService<IConstraintSequenceAdapter>();
            }
        }

        public ConstraintSequenceController(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        /// <summary>
        /// Ajoute une nouvelle séquence de contrainte.
        /// </summary>
        /// <param name="constraintSequence">Nouvelle SequenceConstraint</param>
        /// <param name="constraintId">Id de la contrainte</param>
        /// <returns>Message de retour</returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody]ConstraintSequence constraintSequence, long constraintId)
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                constraintSequence = await Adapter.AddConstraintSequence(constraintSequence, constraintId);
                res.GetObjectForJson(constraintSequence);

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

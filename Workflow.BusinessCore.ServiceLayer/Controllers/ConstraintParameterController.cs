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
    public class ConstraintParameterController : BaseController<ConstraintParameter, IConstraintParameterAdapter>
    {
        protected override IConstraintParameterAdapter Adapter
        {
            get
            {
                return _serviceProvider?.GetService<IConstraintParameterAdapter>();
            }
        }

        public ConstraintParameterController(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }


        /// <summary>
        /// Ajoute un nouveau ConstraintParameter
        /// </summary>
        /// <param name="constraintParameter">Nouveau ConstraintParameter</param>
        /// <param name="constraintSequenceId">Id de la ConstraintSequence</param>
        /// <returns>Message de retour</returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody]ConstraintParameter constraintParameter, long constraintSequenceId)
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                constraintParameter = await Adapter.AddConstraintParameter(constraintParameter, constraintSequenceId);
                res.GetObjectForJson(constraintParameter);

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

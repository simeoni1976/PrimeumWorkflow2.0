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
    public class ConstraintController : BaseController<Constraint, IConstraintAdapter>
    {
        protected override IConstraintAdapter Adapter
        {
            get
            {
                return _serviceProvider?.GetService<IConstraintAdapter>();
            }
        }

        public ConstraintController(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }


        /// <summary>
        /// Ajoute une nouvelle contrainte (indépente des Wrokflow)
        /// </summary>
        /// <param name="constraint">Nouvelle contrainte</param>
        /// <returns>Message de retour</returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody]Constraint constraint)
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                constraint = await Adapter.AddConstraint(constraint);
                res.GetObjectForJson(constraint);

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

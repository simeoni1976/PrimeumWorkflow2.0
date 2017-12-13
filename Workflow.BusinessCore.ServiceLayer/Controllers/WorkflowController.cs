using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using Workflow.BusinessCore.ServiceLayer.Filters;
using Workflow.Transverse.Environment;
using Workflow.Transverse.Helpers;
using Workflow.Transverse.DTO;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Workflow.BusinessCore.ServiceLayer.Controllers
{
    /// <summary>
    /// WorkflowController class.
    /// </summary>
    public class WorkflowConfigController : BaseController<WorkflowConfig, IWorkflowConfigAdapter>
    {
        protected override IWorkflowConfigAdapter Adapter
        {
            get
            {
                return _serviceProvider?.GetService<IWorkflowConfigAdapter>();
            }
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de services</param>
        public WorkflowConfigController(IServiceProvider serviceProvider) : base (serviceProvider)
        {
        }

        /// <summary>
        /// This function permits to post a new workflow
        /// </summary>
        /// <param name="workflowConfig">Workflow Config</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        public async Task<IActionResult> Add([FromBody]WorkflowConfig workflowConfig)
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                workflowConfig = await Adapter.Post(workflowConfig);
                if (workflowConfig != null)
                    res.Json = JsonConvert.SerializeObject(workflowConfig);
                else
                {
                    res.IsSuccess = false;
                    res.Message = "Unable to create WorkflowConfig object.";
                }

                return Ok(res);
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// This function permits to update a workflow
        /// </summary>
        /// <param name="workflowConfig">Workflow Config</param>
        /// <returns>ActionResult</returns>
        [HttpPut]
        public async Task<IActionResult> Update([FromBody] WorkflowConfig workflowConfig)
        {
            try
            {
                return Ok(await Adapter.Put(workflowConfig));
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// This function permits to delete a workflow
        /// </summary>
        /// <param name="id">ID</param>
        /// <returns>ActionResult</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                await Adapter.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Ajoute un WorkflowDimension à un WorkflowConfig
        /// </summary>
        /// <param name="workflowConfigId">Id du WorkflowConfig cible</param>
        /// <param name="workflowDimension">WorkflowDimension</param>
        /// <returns>Message de retour</returns>
        [HttpPost("AddWorkflowDimension")]
        public async Task<IActionResult> AddWorkflowDimension(long workflowConfigId, [FromBody]WorkflowDimension workflowDimension)
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                workflowDimension = await Adapter.AddWorkflowDimension(workflowConfigId, workflowDimension);
                res.GetObjectForJson(workflowDimension);

                return Ok(res);
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }

    }
}

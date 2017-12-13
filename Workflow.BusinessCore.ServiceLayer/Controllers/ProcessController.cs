using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Workflow.BusinessCore.DataLayer.Repositories;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using Workflow.BusinessCore.ServiceLayer.Filters;
using Workflow.Transverse.DTO;
using Workflow.Transverse.Environment;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.ServiceLayer.Controllers
{
    /// <summary>
    ///  WorkflowController class.
    /// </summary>
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ActionLog, ExceptionLog]
    public class ProcessController : Controller
    {
        private readonly IProcessAdapter _processAdapter;
        private readonly ILogger _logger;
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly IServiceProvider _serviceProvider;
        private readonly IOptions<AppSettings> _appSettings;
        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="processAdapter">WorkflowAdapter</param>
        /// <param name="logger">logger</param>
        /// <param name="localizer">Dico pour les locales</param>
        public ProcessController(
            IProcessAdapter processAdapter, 
            ILogger<ProcessController> logger, 
            IStringLocalizer<SharedResource> localizer, 
            IServiceProvider serviceProvider,
            IOptions<AppSettings> appSettings)
        {
            _processAdapter = processAdapter;
            _logger = logger;
            _localizer = localizer;
            _serviceProvider = serviceProvider;
            _appSettings = appSettings;
        }

        /// <summary>
        /// This function permits to run the workflow after the configuration
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Get(long workflowConfigId, long datasetId)
        {
            string culture = CultureInfo.CurrentCulture.Name;

            HttpResponseMessageResult rm = new HttpResponseMessageResult()
            {
                IsSuccess = false,
                ErrorHasDisplay = true,
                //AdditionalMessage = HttpContext.Request.QueryString.ToString(),
                //AdditionalMessage = _localizer["HelloTest"] + " " + culture,
                StatusCode = 500
            };

            try
            {
                if (Request.QueryString.HasValue)
                {
                    _logger.LogInformation("Start of workflow process...");

                    //await _processAdapter.ProcessStart(workflowConfigId, datasetId);

                    rm.IsSuccess = true;
                    rm.ErrorHasDisplay = false;
                    rm.StatusCode = 200;

                    _logger.LogInformation("End of workflow process.");
                }
                else throw new Exception();
            }
            catch (Exception ex)
            {
                rm.Message = ex.Message;
                _logger.LogError(LoggingEvents.PROCESS_ERROR, ex.Message, ex);
            }

            if (rm.IsSuccess)
                return Ok(rm);
            else
                return StatusCode(500, rm);
        }

        /// <summary>
        /// Démarre un nouveau Workflow, en créant un WorkflowInstance.
        /// </summary>
        /// <param name="idWorkflowConfig">Id du WorkflowConfig à démarrer</param>
        /// <param name="idDataSet">Id du DataSet à associer</param>
        /// <param name="idUserSet">Id du UserSet à associer</param>
        /// <returns>Message standard selon le résultat de l'opération</returns>
        [HttpGet("Start")]
        public async Task<IActionResult> Start(long idWorkflowConfig, long idDataSet, long idUserSet)
        {
            try
            {
                _logger.LogInformation("Start of workflow process...");

                HttpResponseMessageResult res = await _processAdapter.ProcessStart(idWorkflowConfig, idDataSet, idUserSet);

                if (res.IsSuccess)
                {
                    _logger.LogInformation("Starting workflow process with success.");
                    return Ok(res);
                }
                else
                {
                    _logger.LogWarning(LoggingEvents.WARNING_ERROR, res.Message);
                    return StatusCode(500, res.Message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.CRITICAL_ERROR, ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }


        /// <summary>
        /// Arrête un workflow (arrêt brutal) .
        /// </summary>
        /// <param name="id">Identifiant de WorkConfig</param>
        /// <returns>Code retour Http</returns>
        [HttpGet("StopImmediate")]
        public async Task<IActionResult> Stop(long id)
        {
            try
            {
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(LoggingEvents.CRITICAL_ERROR, ex.Message, ex);
                return StatusCode(500, ex.Message);
            }
        }

    }
}
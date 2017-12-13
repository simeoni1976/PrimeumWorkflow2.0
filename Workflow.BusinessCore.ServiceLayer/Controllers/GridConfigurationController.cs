using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using Workflow.Transverse.DTO;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Workflow.Transverse.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Workflow.Transverse.Environment;
using Newtonsoft.Json;

namespace Workflow.BusinessCore.ServiceLayer.Controllers
{
    /// <summary>
    ///  Classe controler GridConfigurationController
    /// </summary>
    public class GridConfigurationController : BaseController<GridConfig, IGridConfigurationAdapter>
    {
        protected override IGridConfigurationAdapter Adapter
        {
            get
            {
                return _serviceProvider?.GetService<IGridConfigurationAdapter>();
            }
        }


        /// <summary>
        /// Constructeur par défaut, avec les paramètres pour l'ID
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de services</param>
        public GridConfigurationController(IServiceProvider serviceProvider) 
            : base(serviceProvider)
        {
        }

        /// <summary>
        /// Récupére la configuration d'une grid selon un SelectorInstance
        /// </summary>
        /// <param name="selectorInstanceId">Id du SelectorInstance</param>
        /// <returns>Configuration de la grid dans le message de retour.</returns>
        [HttpGet("GetBySelectorInstanceId")]
        public async Task<IActionResult> GetBySelectorInstanceId(long selectorInstanceId)
        {
            try
            {
                HttpResponseMessageResult data = new HttpResponseMessageResult() { IsSuccess = true };
                GridConfig conf = await Adapter.GetBySelectorInstanceId(selectorInstanceId);

                data.Json = JsonConvert.SerializeObject(conf);

                return Ok(data);
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.PROCESS_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }


        /// <summary>
        /// Ajoute un configuration de grid.
        /// </summary>
        /// <param name="gridConfig">Nouvelle configuration de grid</param>
        /// <param name="workflowConfigId">Id du workflowConfig auquel lier la nouvelle configuration de grid</param>
        /// <returns>Message de retour</returns>
        [HttpPost("AddGridConfig")]
        public async Task<IActionResult> AddGridConfig(long workflowConfigId, [FromBody]GridConfig gridConfig)
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                gridConfig = await Adapter.Add(workflowConfigId, gridConfig);
                res.GetObjectForJson(gridConfig);

                return Ok(res);
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.PROCESS_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Ajoute une configuration de dimension à une configuration de grid existante.
        /// </summary>
        /// <param name="gridDimensionConfig">Nouvelle configuration de dimension</param>
        /// <param name="gcColumnId">Facultatif : id de la config grid lorsque la config dimension est en colonne</param>
        /// <param name="gcRowId">Facultatif : id de la config grid lorsque la config dimension est en ligne</param>
        /// <param name="gcFixedId">Facultatif : id de la config grid lorsque la config dimension est fixée en dehors de la grid</param>
        /// <returns>Message de retour</returns>
        /// <remarks>On ne peut pas avoir les 3 id de GridConfig réglés en même temps.</remarks>
        [HttpPost("AddGridDimensionConfig")]
        public async Task<IActionResult> AddGridDimensionConfig([FromBody]GridDimensionConfig gridDimensionConfig, long? gcColumnId = null, long? gcRowId = null, long? gcFixedId = null)
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                gridDimensionConfig = await Adapter.Add(gridDimensionConfig, gcColumnId, gcRowId, gcFixedId);
                res.GetObjectForJson(gridDimensionConfig);

                return Ok(res);
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.PROCESS_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Ajoute une configuration de valeur à une configuration de dimension existante.
        /// </summary>
        /// <param name="gridValueConfig">Nouvelle configuration de valeur</param>
        /// <param name="gridDimensionConfigId">Id de la configuration de dimension cible.</param>
        /// <returns>Message de retour</returns>
        [HttpPost("AddGridValueConfig")]
        public async Task<IActionResult> AddGridValueConfig([FromBody]GridValueConfig gridValueConfig, long gridDimensionConfigId)
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                gridValueConfig = await Adapter.Add(gridValueConfig, gridDimensionConfigId);
                res.GetObjectForJson(gridValueConfig);

                return Ok(res);
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.PROCESS_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }


    }
}

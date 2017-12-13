using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Process.Interfaces;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using Workflow.Transverse.Environment;
using Workflow.Transverse.Helpers;
using Workflow.Transverse.DTO;
using Microsoft.Extensions.DependencyInjection;

namespace Workflow.BusinessCore.ServiceLayer.Controllers
{
    /// <summary>
    /// DataSetController class.
    /// </summary>
    public class DataSetController : BaseController<DataSet, IDataSetAdapter>
    {
        protected override IDataSetAdapter Adapter
        {
            get
            {
                return _serviceProvider?.GetService<IDataSetAdapter>();
            }
        }

        private IDimensionAdapter DimensionAdapter
        {
            get
            {
                return _serviceProvider?.GetService<IDimensionAdapter>();
            }
        }


        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de services</param>
        public DataSetController(IServiceProvider serviceProvider) :
            base(serviceProvider)
        {
        }

        /// <summary>
        /// Ajout un nouveau DataSet.
        /// </summary>
        /// <param name="dataSet">Nouveau DataSet</param>
        /// <returns>Message de retour</returns>
        [HttpPost("Add")]
        public async Task<IActionResult> Add([FromBody]DataSet dataSet)
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                dataSet = await Adapter.AddDataSet(dataSet);
                res.GetObjectForJson(dataSet);

                return Ok(res);
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Ajoute un DataSetDimension à un DataSet.
        /// </summary>
        /// <param name="dataSetDimension">DataSetDimension</param>
        /// <param name="dataSetId">Id du DataSet cible</param>
        /// <param name="dimensionId">Id de la dimension à associer</param>
        /// <returns>Message de retour</returns>
        [HttpPost("AddDataSetDimension")]
        public async Task<IActionResult> AddDataSetDimension([FromBody]DataSetDimension dataSetDimension, long dataSetId, long dimensionId)
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                dataSetDimension = await Adapter.AddDataSetDimension(dataSetDimension, dataSetId, dimensionId);
                res.GetObjectForJson(dataSetDimension);

                return Ok(res);
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }


        /// <summary>
        /// Récupére toutes les DistinctValue associées à un DataSet.
        /// </summary>
        /// <param name="dataSetId">Id du DataSet</param>
        /// <returns>Message de retour</returns>
        [HttpGet("GetAllDistinctValue")]
        public async Task<IActionResult> GetAllDistinctValue(long dataSetId)
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                IEnumerable<DistinctValue> lst = await Adapter.GetAllDistinctValue(dataSetId);
                res.GetObjectForJson(lst);

                return Ok(res);
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Récupére les DistinctValue associées à un DataSet d'une dimension
        /// </summary>
        /// <param name="dataSetId">Id du DataSet</param>
        /// <param name="dimensionId">Id de la Dimension</param>
        /// <returns>Message de retour</returns>
        [HttpGet("GetDistinctValueByDimension")]
        public async Task<IActionResult> GetDistinctValueByDimension(long dataSetId, long dimensionId)
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                IEnumerable<DistinctValue> lst = await Adapter.GetDistinctValueByDimension(dataSetId, dimensionId);
                res.GetObjectForJson(lst);

                return Ok(res);
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Lie une liste de ValueObject à un DataSet.
        /// </summary>
        /// <param name="dataSetId">Id du DataSet</param>
        /// <param name="valueObjectIds">Liste des ids des ValueObject à lier.</param>
        /// <returns>Message de retour</returns>
        [HttpPost("BindValueObjectToDataSet")]
        public async Task<IActionResult> BindValueObjectToDataSet(long dataSetId, [FromBody]IEnumerable<long> valueObjectIds)
        {
            try
            {
                HttpResponseMessageResult res = await Adapter.BindValueObjectToDataSet(dataSetId, valueObjectIds);
                return Ok(res);
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }


        /// <summary>
        /// Permet de générer la liste de valeur distinct d'un DataSet.
        /// </summary>
        /// <param name="dataSetId">Id du DataSet</param>
        /// <returns>Message de retour</returns>
        /// <remarks>S'il existe déjà des données, elles sont effacées au profit des nouvelles.</remarks>
        [HttpGet("InitializeDistinctValue")]
        public async Task<IActionResult> InitializeDistinctValue(long dataSetId)
        {
            try
            {
                HttpResponseMessageResult res = await Adapter.InitializeDistinctValue(dataSetId);
                return Ok(res);
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Récupére toutes les entités DTO existantes.
        /// </summary>
        /// <returns>Message de retour avec la liste en json</returns>
        [HttpGet("GetAllDimension")]
        public async Task<IActionResult> GetAllDimension()
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                IEnumerable<Dimension> lst = await DimensionAdapter.GetAll();
                res.GetObjectForJson(lst);

                return Ok(res);
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }


        /// <summary>
        /// Récupére l'entité désignée par l'id en paramétre.
        /// </summary>
        /// <param name="id">Id de l'entité</param>
        /// <returns>Message de retour avec l'entité</returns>
        [HttpGet("GetDimensionById")]
        public async Task<IActionResult> GetDimensionById(long dimensionId)
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                Dimension dimension = await DimensionAdapter.GetById(dimensionId);
                res.GetObjectForJson(dimension);

                return Ok(res);
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Ajoute une dimension.
        /// </summary>
        /// <param name="dimension">Nouvelle Dimension</param>
        /// <returns>Message de retour</returns>
        [HttpPost("AddDimension")]
        public async Task<IActionResult> AddDimension([FromBody]Dimension dimension)
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                dimension = await DimensionAdapter.AddDimension(dimension);
                res.GetObjectForJson(dimension);

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

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Threading.Tasks;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using Workflow.Transverse.DTO;
using Workflow.Transverse.Environment;
using Workflow.Transverse.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.Linq;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Workflow.BusinessCore.ServiceLayer.Controllers
{
    /// <summary>
    ///  ValueObjectController class.
    /// </summary>
    public class ValueObjectController : BaseController<ValueObject, IValueObjectAdapter>
    {
        protected override IValueObjectAdapter Adapter
        {
            get
            {
                return _serviceProvider?.GetService<IValueObjectAdapter>();
            }
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de services</param>
        public ValueObjectController(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        /// <summary>
        /// This function permits to update a value for dimensions
        /// </summary>
        /// <param name="id"></param>
        /// <param name="initialValue"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateValue(long id, [FromBody] ValueObject valueOject)
        {
            try
            {
                return Ok(await Adapter.Put(id, valueOject));
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// This function permits to get ValueObjects by filter
        /// </summary>
        /// <param name="select">Selection list</param>
        /// <param name="where"></param>
        /// <param name="sort_asc"></param>
        /// <param name="sort_desc"></param>
        /// <param name="grouping"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <remarks>
        /// ALLO
        /// </remarks>
        /// <returns></returns>
        [HttpGet]
        [Route("Filter")]
        public async Task<IActionResult> Filter(
            string[] select = null,
            string[] where = null,
            string[] sort_asc = null,
            string[] sort_desc = null,
            bool grouping = false,
            int? page = null,
            int? pageSize = null)
        {
            try
            {
                IEnumerable<ValueObject> lst = await Adapter.Filter(select, where, sort_asc, sort_desc, grouping, page, pageSize);
                HttpResponseMessageResult data = new HttpResponseMessageResult() { IsSuccess = true, Json = JsonConvert.SerializeObject(lst) };

                return Ok(data);
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.PROCESS_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }


        /// <summary>
        /// Permet de lire les données selon un format configuré par l'opérateur, avec des filtres, des tris, la pagination, etc...
        /// </summary>
        /// <param name="selectorInstanceId">Id du SelectorInstance concerné</param>
        /// <param name="filter">Chaines à filtrer, l'indexe représente le numéro de colonne sur lequel appliquer le filtre.</param>
        /// <param name="start">Numéro de ligne à partir duquel il faut commencer la selection</param>
        /// <param name="length">Nombre de ligne à sélectionner</param>
        /// <param name="sortCol">Numéro de colonne à trier</param>
        /// <param name="sortDir">Ordre du tri : ASC ou DESC</param>
        /// <returns>Message de retour + données</returns>
        [HttpGet("ReadData")]
        public async Task<IActionResult> ReadData(long selectorInstanceId, string[] filter, int start, int length, int sortCol, string sortDir)
        {
            try
            {
                HttpResponseMessageResult data = await Adapter.ReadData(selectorInstanceId, filter, start, length, sortCol, sortDir);

                return Ok(data);
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.PROCESS_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Permet d'importer une liste de ValueObject.
        /// </summary>
        /// <param name="valueObjects">Liste de ValueObject à enregistrer</param>
        /// <returns>Liste des ids enregistrés</returns>
        [HttpPost("Import")]
        public async Task<IActionResult> Import([FromBody]IEnumerable<ValueObject> valueObjects)
        {
            try
            {
                if (ModelState.IsValid)
                {

                    HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                    IEnumerable<long> data = await Adapter.Import(valueObjects);
                    res.GetObjectForJson(data);

                    return Ok(res);
                }

                HttpResponseMessageResult err = new HttpResponseMessageResult() { IsSuccess = false };
                foreach (ModelError mdErr in ModelState.Values.SelectMany(v => v.Errors))
                    err.Message += $"{mdErr?.ErrorMessage} : {mdErr?.Exception?.Message}";

                return StatusCode(500, err);
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.PROCESS_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using Workflow.BusinessCore.ServiceLayer.Filters;
using Workflow.Transverse.Environment;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using Workflow.Transverse.Helpers;
using Workflow.Transverse.DTO;

namespace Workflow.BusinessCore.ServiceLayer.Controllers
{
    /// <summary>
    ///  SelectorController class.
    /// </summary>
    public class SelectorController : BaseController<SelectorInstance, ISelectorInstanceAdapter>
    {
        protected override ISelectorInstanceAdapter Adapter
        {
            get
            {
                return _serviceProvider.GetService<ISelectorInstanceAdapter>();
            }
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de service (DI)</param>
        public SelectorController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }


        /// <summary>
        /// Modifie les données d'un SelectorInstance.
        /// </summary>
        /// <param name="values">Données à modifier</param>
        /// <param name="selectorInstanceId">Id du SelectorInstance</param>
        /// <returns>Message de retour</returns>
        [HttpPost("WriteData")]
        public async Task<IActionResult> SaveData(long selectorInstanceId, [FromBody]Dictionary<long, double> values)
        {
            try
            {
                // On simplifie le passage en json avec le dictionary.
                List<KeyValuePair<long, double>> lst = new List<KeyValuePair<long, double>>();
                foreach (KeyValuePair<long, double> val in values)
                    lst.Add(val);

                HttpResponseMessageResult data = await Adapter.SaveData(selectorInstanceId, lst);

                return Ok(data);
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.PROCESS_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

    }
}
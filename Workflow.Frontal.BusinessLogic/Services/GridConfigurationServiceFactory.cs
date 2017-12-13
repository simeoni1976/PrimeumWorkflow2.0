using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Workflow.Transverse.Helpers;

namespace Workflow.Frontal.BusinessLogic.Services
{
    public class GridConfigurationServiceFactory : AbstractServiceFactory
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="serviceUrl"></param>
        public GridConfigurationServiceFactory(string serviceUrl) : base(serviceUrl)
        {
        }

        /// <summary>
        /// Constructeur vide par défaut
        /// </summary>
        public GridConfigurationServiceFactory()
        {
        }


        /// <summary>
        /// Récupére la configuration des headers d'une grille par rapport à un SelectorInstance.
        /// </summary>
        /// <param name="selectorInstanceId">Id du SelectorInstance</param>
        /// <returns></returns>
        public async Task<HttpResponseMessageResult> GetBySelectorInstanceId(long selectorInstanceId)
        {
            try
            {
                using (HttpClient client = InitializeClient())
                {
                    HttpResponseMessage response = await client.GetAsync($"{_serviceUrl}/GetBySelectorInstanceId?selectorInstanceId={selectorInstanceId}");
                    response.EnsureSuccessStatusCode();

                    return await SuccessResponse(response);
                }
            }
            catch (HttpRequestException ex)
            {
                return ExceptionResponse(ex);
            }
        }


    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Workflow.Transverse.Helpers;

namespace Workflow.Frontal.BusinessLogic.Services
{
    public class ValueObjectServiceFactory : AbstractServiceFactory
    {
        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="serviceUrl"></param>
        public ValueObjectServiceFactory(string serviceUrl) : base(serviceUrl)
        {
        }

        /// <summary>
        /// Constructeur vide par défaut
        /// </summary>
        public ValueObjectServiceFactory()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="selectorInstanceId"></param>
        /// <param name="filter"></param>
        /// <param name="start"></param>
        /// <param name="length"></param>
        /// <param name="sortCol"></param>
        /// <param name="sortDir"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessageResult> ReadData(long selectorInstanceId, string[] filter, int start, int length, int sortCol, string sortDir)
        {
            try
            {
                using (HttpClient client = InitializeClient())
                {
                    StringBuilder sbFilter = new StringBuilder();
                    foreach (string eltFiltre in filter)
                    {
                        if (sbFilter.Length > 0)
                            sbFilter.Append("&");
                        sbFilter.Append($"filter={eltFiltre}"); // TODO : url encoding ici
                    }

                    HttpResponseMessage response = await client.GetAsync($"{_serviceUrl}/ReadData?selectorInstanceId={selectorInstanceId}{sbFilter.ToString()}&start={start}&length={length}&sortCol={sortCol}&sortDir={sortDir}");
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

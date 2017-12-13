using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Workflow.Transverse.DTO.Common;
using Workflow.Transverse.Helpers;

namespace Workflow.Frontal.BusinessLogic.Services
{
    /// <summary>
    /// Classe abstraite des ServiceFactory
    /// </summary>
    public abstract class AbstractServiceFactory
    {
        protected string _serviceUrl = null;

        /// <summary>
        /// Constructeur sans paramétre
        /// </summary>
        public AbstractServiceFactory()
        {
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="serviceUrl">Service URL</param>
        public AbstractServiceFactory(string serviceUrl)
        {
            _serviceUrl = serviceUrl;
        }

        /// <summary>
        /// Initialise l'url de service.
        /// </summary>
        /// <param name="serviceUrl">Url de service</param>
        /// <remarks>On attend que cette méthode soit appelée en début de d'initialisation</remarks>
        public void InitializeServiceUrl(string serviceUrl)
        {
            _serviceUrl = serviceUrl;
        }


        /// <summary>
        /// Initialize the HttpClient for the queries.
        /// </summary>
        /// <returns>HttpClient</returns>
        public HttpClient InitializeClient()
        {
            HttpClient httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            return httpClient;
        }


        /// <summary>
        /// Envoi une requête HTTP POST en précisant une querystring et/ou un objet DTO dans le coprs de la requête.
        /// </summary>
        /// <param name="subUri">Partie supplémentaire de l'uri du service, si nécessaire.</param>
        /// <param name="lstQueryString">Liste de variables à mettre en querystring</param>
        /// <param name="jsonBody">Object DTO à faire passer dans le corps de la requête.</param>
        /// <returns>Message de retour de la requête.</returns>
        public async Task<HttpResponseMessageResult> Post(string subUri, IEnumerable<KeyValuePair<string, string>> lstQueryString, string jsonBody)
        {
            try
            {
                using (HttpClient client = InitializeClient())
                {
                    StringContent contentBody = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    string queryString = "";
                    if ((lstQueryString != null) && (lstQueryString.Count() > 0))
                        queryString = "?" + string.Join("&", lstQueryString.Select((x) => WebUtility.UrlEncode(x.Key) + "=" + WebUtility.UrlEncode(x.Value)));

                    string uri = _serviceUrl;
                    if (!string.IsNullOrWhiteSpace(subUri))
                        uri = string.Join("/", _serviceUrl, subUri);

                    HttpResponseMessage response = await client.PostAsync(uri + queryString, contentBody);
                    response.EnsureSuccessStatusCode();

                    return await SuccessResponse(response);
                }
            }
            catch (HttpRequestException ex)
            {
                return ExceptionResponse(ex);
            }
        }

        /// <summary>
        /// Envoi une requête HTTP PUT en précisant une querystring et/ou un objet DTO dans le coprs de la requête.
        /// </summary>
        /// <param name="subUri">Partie supplémentaire de l'uri du service, si nécessaire.</param>
        /// <param name="lstQueryString">Liste de variables à mettre en querystring</param>
        /// <param name="jsonBody">Object DTO à faire passer dans le corps de la requête.</param>
        /// <returns>Message de retour de la requête.</returns>
        public async Task<HttpResponseMessageResult> Put(string subUri, IEnumerable<KeyValuePair<string, string>> lstQueryString, string jsonBody)
        {
            try
            {
                using (HttpClient client = InitializeClient())
                {
                    StringContent contentBody = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                    string queryString = "";
                    if ((lstQueryString != null) && (lstQueryString.Count() > 0))
                        queryString = "?" + string.Join("&", lstQueryString.Select((x) => WebUtility.UrlEncode(x.Key) + "=" + WebUtility.UrlEncode(x.Value)));

                    string uri = _serviceUrl;
                    if (!string.IsNullOrWhiteSpace(subUri))
                        uri = string.Join("/", _serviceUrl, subUri);

                    HttpResponseMessage response = await client.PostAsync(uri + queryString, contentBody);
                    response.EnsureSuccessStatusCode();

                    return await SuccessResponse(response);
                }
            }
            catch (HttpRequestException ex)
            {
                return ExceptionResponse(ex);
            }
        }

        /// <summary>
        /// Envoi une requête HTTP GET en précisant la querystring.
        /// </summary>
        /// <param name="subUri">Partie supplémentaire de l'uri du service, si nécessaire.</param>
        /// <param name="lstQueryString">Liste de variables à mettre en querystring</param>
        /// <returns>Message de retour</returns>
        public async Task<HttpResponseMessageResult> Get(string subUri, IEnumerable<KeyValuePair<string, string>> lstQueryString)
        {
            try
            {
                using (HttpClient client = InitializeClient())
                {
                    string queryString = "";
                    if ((lstQueryString != null) && (lstQueryString.Count() > 0))
                        queryString = "?" + string.Join("&", lstQueryString.Select((x) => WebUtility.UrlEncode(x.Key) + "=" + WebUtility.UrlEncode(x.Value)));

                    string uri = _serviceUrl;
                    if (!string.IsNullOrWhiteSpace(subUri))
                        uri = string.Join("/", _serviceUrl, subUri);

                    HttpResponseMessage response = await client.GetAsync(uri + queryString);
                    response.EnsureSuccessStatusCode();

                    return await SuccessResponse(response);
                }
            }
            catch (HttpRequestException ex)
            {
                return ExceptionResponse(ex);
            }
        }


        /// <summary>
        /// This function permits to build an exception response.
        /// </summary>
        /// <param name="exception">HttpRequestException</param>
        /// <returns>HttpResponseMessageResult</returns>
        protected static HttpResponseMessageResult ExceptionResponse(HttpRequestException exception)
        {
            HttpResponseMessageResult responseResult = new HttpResponseMessageResult()
            {
                ErrorHasDisplay = true,
                Message = string.Format("{0}({1})", exception?.Message ?? "", exception?.Source ?? ""),
                StatusCode = (int)(((WebException)exception?.InnerException)?.Status ?? 0),
            };

            return responseResult;
        }

        /// <summary>
        /// This function permits to build a success response.
        /// </summary>
        /// <param name="response">HttpResponseMessage</param>
        /// <returns>HttpResponseMessageResult</returns>
        protected async Task<HttpResponseMessageResult> SuccessResponse(HttpResponseMessage response)
        {
            string body = await response.Content.ReadAsStringAsync();
            HttpResponseMessageResult responseResult = null;
            try
            {
                responseResult = JsonConvert.DeserializeObject<HttpResponseMessageResult>(body);
            }
            catch
            {
            }
            if (responseResult == null)
                responseResult = new HttpResponseMessageResult() { IsSuccess = response.StatusCode == HttpStatusCode.OK, Json = body };


            // Déjà testé
            //responseResult.IsSuccess = response.StatusCode == HttpStatusCode.OK;
            responseResult.StatusCode = (int)response.StatusCode;

            return responseResult;
        }
    }
}

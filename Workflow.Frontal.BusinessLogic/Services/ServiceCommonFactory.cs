using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using Microsoft.AspNetCore.WebUtilities;
using System.Net.Http.Headers;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Workflow.Transverse.DTO.Common;
using Workflow.Transverse.Helpers;

namespace Workflow.Frontal.BusinessLogic.Services
{
    /// <summary>
    /// ServiceFactory class.
    /// </summary>
    /// <remarks>
    /// This class permits to do API queries with a factory pattern.
    /// </remarks>
    public class ServiceCommonFactory : AbstractServiceFactory
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="serviceUrl">Service URL</param>
        public ServiceCommonFactory(string serviceUrl) : base (serviceUrl)
        {
        }

        /// <summary>
        /// Constructeur vide par défaut
        /// </summary>
        public ServiceCommonFactory()
        {
        }


        /// <summary>
        /// This function permits to get a value in async.
        /// </summary>
        /// <returns>HttpResponseMessageResult</returns>
        public async Task<HttpResponseMessageResult> Get()
        {
            try
            {
                using (HttpClient client = InitializeClient())
                {
                    HttpResponseMessage response = await client.GetAsync(_serviceUrl);
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
        /// This function permits to get a value in async.
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns>HttpResponseMessageResult</returns>
        public async Task<HttpResponseMessageResult> Get(long id)
        {
            try
            {
                using (HttpClient client = InitializeClient())
                {
                    HttpResponseMessage response = await client.GetAsync(_serviceUrl + $"/{id}");
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
        /// This function permits to get a value in async.
        /// </summary>
        /// <param name="criterias">Criteria filter</param>
        /// <returns>HttpResponseMessageResult</returns>
        public async Task<HttpResponseMessageResult> Find(List<KeyValuePair<string, string>> criterias)
        {
            try
            {
                using (HttpClient client = InitializeClient())
                {
                    string querystring = string.Join("&", criterias.Select((x) => x.Key + "=" + x.Value.ToString()));

                    HttpResponseMessage response = await client.GetAsync(_serviceUrl + $"?{querystring}");
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
        /// This function permits to post a model in async.
        /// </summary>
        /// <typeparam name="T">DTO Model type</typeparam>
        /// <param name="id">Model Id</param>
        /// <param name="model">Model</param>
        /// <returns>HttpResponseMessage</returns>
        public async Task<HttpResponseMessageResult> Post<T>(long id, T model) where T : class
        {
            try
            {
                using (HttpClient client = InitializeClient())
                {
                    string stringData = JsonConvert.SerializeObject(model);
                    StringContent contentData = new StringContent(stringData, System.Text.Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PostAsync(_serviceUrl + $"/{id}", contentData);
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
        /// This function permits to put a model in async.
        /// </summary>
        /// <typeparam name="T">DTO Model type</typeparam>
        /// <param name="model">Model</param>
        /// <returns>HttpResponseMessageResult</returns>
        public async Task<HttpResponseMessageResult> Put<T>(long id, T model) where T : class
        {
            try
            {
                using (HttpClient client = InitializeClient())
                {
                    string stringData = JsonConvert.SerializeObject(model);
                    StringContent contentData = new StringContent(stringData, Encoding.UTF8, "application/json");

                    HttpResponseMessage response = await client.PutAsync(_serviceUrl + $"/{id}", contentData);
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
        /// This function permits to delete a model in async.
        /// </summary>
        /// <param name="id">Primary key</param>
        /// <returns>HttpResponseMessageResult</returns>
        public async Task<HttpResponseMessageResult> Delete(long id)
        {
            try
            {
                using (HttpClient client = InitializeClient())
                {
                    HttpResponseMessage response = await client.DeleteAsync(_serviceUrl + $"/{id}");
                    response.EnsureSuccessStatusCode();

                    return await SuccessResponse(response);
                }
            }
            catch (HttpRequestException ex)
            {
                return ExceptionResponse(ex);
            }
        }

        ///// <summary>
        ///// This function permits to build an exception response.
        ///// </summary>
        ///// <param name="exception">HttpRequestException</param>
        ///// <returns>HttpResponseMessageResult</returns>
        //private static HttpResponseMessageResult ExceptionResponse(HttpRequestException exception)
        //{
        //    HttpResponseMessageResult _responseResult = new HttpResponseMessageResult();
            
        //    _responseResult.ErrorHasDisplay = true;
        //    _responseResult.Message = string.Format("{0}({1})", exception.Message, exception.Source);
        //    _responseResult.StatusCode = ((WebException)exception.InnerException).Status.GetHashCode();
        //    _responseResult.Json = _responseResult.Message;

        //    return _responseResult;
        //}

        ///// <summary>
        ///// This function permits to build a success response.
        ///// </summary>
        ///// <param name="response">HttpResponseMessage</param>
        ///// <returns>HttpResponseMessageResult</returns>
        //private static HttpResponseMessageResult SuccessResponse(HttpResponseMessage response)
        //{
        //    HttpResponseMessageResult _responseResult = new HttpResponseMessageResult();

        //    _responseResult.IsSuccess = true;
        //    _responseResult.Message = response.ReasonPhrase;
        //    _responseResult.StatusCode = (int)response.StatusCode;
        //    _responseResult.Json = response.Content != null ? response.Content.ReadAsStringAsync().Result : string.Empty;

        //    return _responseResult;
        //}
    }
}

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using Workflow.Transverse.DTO;
using Workflow.Transverse.Environment;
using Workflow.Transverse.Helpers;
using DTO = Workflow.Transverse.DTO;
using Microsoft.Extensions.DependencyInjection;

namespace Workflow.BusinessCore.ServiceLayer.Controllers
{
    /// <summary>
    /// UserController class
    /// </summary>
    public class UserSetController : BaseController<DTO.UserSet, IUserSetAdapter>
    {
        protected override IUserSetAdapter Adapter
        {
            get
            {
                return _serviceProvider?.GetService<IUserSetAdapter>();
            }
        }


        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de services</param>
        public UserSetController(IServiceProvider serviceProvider) 
            : base(serviceProvider)
        {
        }

        ///// <summary>
        ///// This function permits to put an UserSet.
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[HttpPut]
        //public async Task<IActionResult> Put(long id, [FromBody] UserSet model)
        //{
        //    try
        //    {
        //        return Ok(await Adapter.Put(id, model));
        //    }
        //    catch (Exception ex)
        //    {
        //        Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex, ex.Message);
        //        return StatusCode(500, ex.Message);
        //    }
        //}



        /// <summary>
        /// Ajout d'un nouvel UserSet
        /// </summary>
        /// <param name="user">nouveau UserSet</param>
        /// <returns>UserSet enregistré</returns>
        [HttpPost("Add")]
        public async Task<IActionResult> AddUserSet([FromBody]UserSet userSet)
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                userSet = await Adapter.Add(userSet);
                res.GetObjectForJson(userSet);

                return Ok(res);
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.PROCESS_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetUsers")]
        public async Task<IActionResult> GetUsers(long userSetId)
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                List<User> users = await Adapter.GetUsers(userSetId);
                res.GetObjectForJson(users);

                return Ok(res);
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.PROCESS_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Lie les utilisateurs donnés en entrée à un UserSet.
        /// </summary>
        /// <param name="userSetId">Id de l'UserSet</param>
        /// <param name="userSetUser">Liste des liens d'utilisateur</param>
        /// <returns>Message de retour</returns>
        [HttpPost("BindUserToUserSet")]
        public async Task<IActionResult> BindUserToUserSet(long userSetId, [FromBody]IEnumerable<DTO.UserSetUser> userSetUser)
        {
            try
            {
                HttpResponseMessageResult res = await Adapter.BindUserToUserSet(userSetId, userSetUser);

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

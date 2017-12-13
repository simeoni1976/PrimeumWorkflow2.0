using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using Workflow.Transverse.Environment;
using Workflow.Transverse.Helpers;
using Workflow.Transverse.DTO;
using Microsoft.Extensions.DependencyInjection;

namespace Workflow.BusinessCore.ServiceLayer.Controllers
{
    /// <summary>
    /// UserController class
    /// </summary>
    public class UserController : BaseController<User, IUserAdapter>
    {
        protected override IUserAdapter Adapter
        {
            get
            {
                return _serviceProvider?.GetService<IUserAdapter>();
            }
        }


        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de services</param>
        public UserController(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }

        /// <summary>
        /// This function permits to put a model.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Put(long id, User model)
        {
            try
            {
                return Ok(await Adapter.Put(id, model));
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// This function permits to get access.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Login")]
        public async Task<IActionResult> GetLogin(string login, string password)
        {
            try
            {
                return Ok(await Adapter.Get(login, password));
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// This function permits to get access.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("Role")]
        public async Task<IActionResult> GetRole(string employeeId)
        {
            try
            {
                return Ok(await Adapter.GetUserSets(employeeId));
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        /// <summary>
        /// Renvoit la liste des SelectorInstance dont le loginModificator est modificateur (en attente de modification).
        /// </summary>
        /// <param name="loginModificator">Login de l'utilisateur recherché</param>
        /// <param name="workflowInstanceId">Id du workflowInstance cible</param>
        /// <returns>Liste des SelectorInstance dans le message de retour</returns>
        [HttpGet("GetSelectorInstanceByModificator")]
        public async Task<IActionResult> GetSelectorInstanceByModificator(long modificatorId, long workflowInstanceId)
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                IEnumerable<SelectorInstance> lstSI = await Adapter.GetSelectorInstanceByModificator(modificatorId, workflowInstanceId);
                if (lstSI != null)
                    res.GetObjectForJson(lstSI);

                return Ok(res);
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }


        /// <summary>
        /// Enregistre une série de nouveaux utilisateurs.
        /// </summary>
        /// <param name="users">Liste de nouveaux utilisateurs.</param>
        /// <returns>Renvoit la liste des ids des utilisateurs.</returns>
        [HttpPost("ImportUsers")]
        public async Task<IActionResult> ImportUsers([FromBody]IEnumerable<User> users)
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                IEnumerable<long> data = await Adapter.Import(users);
                res.GetObjectForJson(data);

                return Ok(res);
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }


        /// <summary>
        /// Récupére un utilisateur par son login.
        /// </summary>
        /// <param name="login">Login recherché</param>
        /// <returns>Utilisateur</returns>
        [HttpGet("GetByLogin")]
        public async Task<IActionResult> GetByLogin(string login)
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                User user = await Adapter.GetByLogin(login);
                if (user != null)
                    res.GetObjectForJson(user);

                return Ok(res);
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }


        /// <summary>
        /// Récupére les WorkflowInstances dans lesquels un utilisateur opére.
        /// </summary>
        /// <param name="userId">Id utilisateur</param>
        /// <returns>Liste de WorkflowInstance</returns>
        [HttpGet("GetWorkflowInstance")]
        public async Task<IActionResult> GetWorkflowInstance(long userId)
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                IEnumerable<WorkflowInstance> wfIns = await Adapter.GetWorkflowInstance(userId);
                if (wfIns != null)
                    res.GetObjectForJson(wfIns);

                return Ok(res);
            }
            catch (Exception ex)
            {
                Logger.LogError(LoggingEvents.CRITICAL_ERROR, ex, ex.Message);
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("GetSelectorInstanceByValidator")]
        public async Task<IActionResult> GetSelectorInstanceByValidator(long validatorId, long workflowInstanceId)
        {
            try
            {
                HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
                IEnumerable<SelectorInstance> lstSI = await Adapter.GetSelectorInstanceByValidator(validatorId, workflowInstanceId);
                if (lstSI != null)
                    res.GetObjectForJson(lstSI);

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

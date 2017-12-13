using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.BusinessLayer.UnitOfWork.Interfaces;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using DTO = Workflow.Transverse.DTO;
using ENT = Workflow.BusinessCore.DataLayer.Entities;
using System;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Workflow.Transverse.Helpers;
using Workflow.BusinessCore.BusinessLayer.Process.Exceptions;

namespace Workflow.BusinessCore.ServiceLayer.Adapters
{
    /// <summary>
    ///  UserAdapter interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the user adapter.
    /// </remarks>
    public class UserAdapter : IUserAdapter
    {
        private readonly IServiceProvider _serviceProvider;

        private IUserDomain UserDomain
        {
            get
            {
                return _serviceProvider?.GetService<IUserDomain>();
            }
        }


        private IMapper Mapper
        {
            get
            {
                return _serviceProvider?.GetService<IMapper>();
            }
        }

        private IUserSetDomain UserSetDomain
        {
            get
            {
                return _serviceProvider?.GetService<IUserSetDomain>();
            }
        }


        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de services</param>
        public UserAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// This function permits to delete an user
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Task</returns>
        public async Task Delete(long id)
        {
            await UserDomain.Delete(id);
        }

        /// <summary>
        /// This function permits to get the usersets for an user id.
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>DTO</returns>
        public async Task<IEnumerable<DTO.UserSet>> GetUserSets(string employeeId)
        {
            IEnumerable<ENT.UserSet> userSet = await UserDomain.GetUserSets(employeeId);

            if (userSet != null)
                return Mapper.Map<List<ENT.UserSet>, List<DTO.UserSet>>(userSet.ToList());
            else
                return null;
        }

        /// <summary>
        /// This function permits to post a new User
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>User</returns>
        public async Task<DTO.User> Post(DTO.User model)
        {
            ENT.User element = Mapper.Map<DTO.User, ENT.User>(model);

            return Mapper.Map<ENT.User, DTO.User>(await UserDomain.Add(element));
        }

        /// <summary>
        /// This function permits to update a User
        /// </summary>
        /// <param name="user">User</param>
        /// <returns>User</returns>
        public async Task<DTO.User> Put(long id, DTO.User dto)
        {
            DTO.User element = Mapper.Map<ENT.User, DTO.User>(await UserDomain.Get(id));
            if (element.Id != 0)
            {
                dto.Id = element.Id;
                return Mapper.Map<ENT.User, DTO.User>(
                    await UserDomain.Add(Mapper.Map<DTO.User, ENT.User>(dto)));
            }
            else
                return new DTO.User();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public async Task<DTO.User> Get(string login, string password)
        {
            return await Task.FromResult(new DTO.User());
        }

        /// <summary>
        /// Renvoit la liste des SelectorInstance dont le loginModificator est modificateur (en attente de modification).
        /// </summary>
        /// <param name="modificatorId">Id de l'utilisateur recherché</param>
        /// <param name="workflowInstanceId">Id du workflowInstance cible</param>
        /// <returns>Liste des SelectorInstance</returns>
        public async Task<IEnumerable<DTO.SelectorInstance>> GetSelectorInstanceByModificator(long modificatorId, long workflowInstanceId)
        {
            IEnumerable<ENT.SelectorInstance> lstSel = await UserDomain.GetSelectorInstanceByModificator(modificatorId, workflowInstanceId);

            List<DTO.SelectorInstance> lstSelDto = new List<DTO.SelectorInstance>();
            if ((lstSel != null) && (lstSel.Count() > 0))
            {
                foreach (ENT.SelectorInstance entSi in lstSel)
                    lstSelDto.Add(Mapper.Map<ENT.SelectorInstance, DTO.SelectorInstance>(entSi));
            }

            return lstSelDto;
        }

        /// <summary>
        /// Récupére toutes les entités DTO existantes.
        /// </summary>
        /// <returns>Message de retour avec la liste en json</returns>
        public async Task<IEnumerable<DTO.User>> GetAll()
        {
            IEnumerable<ENT.User> users = await UserDomain.Get();

            IEnumerable<DTO.User> dtoUsers = null;
            if (users != null)
                dtoUsers = Mapper.Map<IEnumerable<ENT.User>, IEnumerable<DTO.User>>(users);
            else
                dtoUsers = new List<DTO.User>();

            return dtoUsers;
        }

        /// <summary>
        /// Récupére l'entité désignée par l'id en paramétre.
        /// </summary>
        /// <param name="id">Id de l'entité</param>
        /// <returns>Message de retour avec l'entité</returns>
        public async Task<DTO.User> GetById(long id)
        {
            ENT.User user = await UserDomain.Get(id);

            DTO.User dtoUser = null;
            if (user != null)
                dtoUser = Mapper.Map<ENT.User, DTO.User>(user);

            return dtoUser;
        }

        /// <summary>
        /// Enregistre une série de nouveaux utilisateurs.
        /// </summary>
        /// <param name="users">Liste de nouveaux utilisateurs.</param>
        /// <returns>Renvoit la liste des ids des utilisateurs.</returns>
        public async Task<IEnumerable<long>> Import(IEnumerable<DTO.User> users)
        {
            return await UserDomain.Import(Mapper.Map<IEnumerable<DTO.User>, IEnumerable<ENT.User>>(users));
        }

        /// <summary>
        /// Récupére un utilisateur par son login.
        /// </summary>
        /// <param name="login">Login recherché</param>
        /// <returns>Utilisateur</returns>
        public async Task<DTO.User> GetByLogin(string login)
        {
            return Mapper.Map<ENT.User, DTO.User>(await UserDomain.GetByLogin(login));
        }

        /// <summary>
        /// Récupére les WorkflowInstances dans lesquels un utilisateur opére.
        /// </summary>
        /// <param name="userId">Id utilisateur</param>
        /// <returns>Liste de WorkflowInstance</returns>
        public async Task<IEnumerable<DTO.WorkflowInstance>> GetWorkflowInstance(long userId)
        {
            return Mapper.Map<IEnumerable<ENT.WorkflowInstance>, IEnumerable<DTO.WorkflowInstance>>(await UserDomain.GetWorkflowInstance(userId));
        }

        /// <summary>
        /// Récupére les SelectorInstance pour lesquels l'utilisateur est un validateur.
        /// </summary>
        /// <param name="validatorId">Id de l'utilisateur validateur</param>
        /// <param name="workflowInstanceId">Id du WorkflowInstance</param>
        /// <returns>Liste des SelectorInstance</returns>
        public async Task<IEnumerable<DTO.SelectorInstance>> GetSelectorInstanceByValidator(long validatorId, long workflowInstanceId)
        {
            return Mapper.Map<IEnumerable<ENT.SelectorInstance>, IEnumerable<DTO.SelectorInstance>>(await UserDomain.GetSelectorInstanceByValidator(validatorId, workflowInstanceId));
        }

    }
}

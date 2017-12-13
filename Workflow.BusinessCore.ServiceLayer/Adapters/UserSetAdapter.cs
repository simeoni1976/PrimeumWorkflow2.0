using AutoMapper;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using DTO = Workflow.Transverse.DTO;
using ENT = Workflow.BusinessCore.DataLayer.Entities;
using System;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.ServiceLayer.Adapters
{
    /// <summary>
    ///  UserAdapter interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the user adapter.
    /// </remarks>
    public class UserSetAdapter : IUserSetAdapter
    {
        private readonly IServiceProvider _serviceProvider = null;

        protected IUserSetDomain UserSetDomain
        {
            get
            {
                return _serviceProvider?.GetService<IUserSetDomain>();
            }
        }

        protected IMapper Mapper
        {
            get
            {
                return _serviceProvider?.GetService<IMapper>();
            }
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de services</param>
        public UserSetAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        ///// <summary>
        ///// This function permits to delete an user
        ///// </summary>
        ///// <param name="id">Id</param>
        ///// <returns>Task</returns>
        //public async Task Delete(long id)
        //{
        //    await UserSetDomain.Delete(id);
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<DTO.User>> GetUsers(long userSetId)
        {
            IEnumerable<ENT.User> userSet = await UserSetDomain.GetUsers(userSetId);

            if (userSet != null)
                return Mapper.Map<List<ENT.User>, List<DTO.User>>(userSet.ToList());
            else
                return null;
        }

        /// <summary>
        /// Ajout d'un nouvel UserSet
        /// </summary>
        /// <param name="user">nouveau UserSet</param>
        /// <returns>UserSet enregistré</returns>
        public async Task<DTO.UserSet> Add(DTO.UserSet userSet)
        {
            ENT.UserSet element = Mapper.Map<DTO.UserSet, ENT.UserSet>(userSet);

            return Mapper.Map<ENT.UserSet, DTO.UserSet>(await UserSetDomain.Add(element));
        }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //public async Task<DTO.UserSet> Put(long id, DTO.UserSet dto)
        //{
        //    DTO.UserSet element = Mapper.Map<ENT.UserSet, DTO.UserSet>(await UserSetDomain.Get(id));
        //    if (element.Id != 0)
        //    {
        //        dto.Id = element.Id;
        //        return Mapper.Map<ENT.UserSet, DTO.UserSet>(
        //            await UserSetDomain.Add(Mapper.Map<DTO.UserSet, ENT.UserSet>(dto)));
        //    }
        //    else
        //        return new DTO.UserSet();
        //}

        /// <summary>
        /// Récupére toutes les entités DTO existantes.
        /// </summary>
        /// <returns>Message de retour avec la liste en json</returns>
        public async Task<IEnumerable<DTO.UserSet>> GetAll()
        {
            IEnumerable<ENT.UserSet> users = await UserSetDomain.Get();

            IEnumerable<DTO.UserSet> dtoUsers = null;
            if (users != null)
                dtoUsers = Mapper.Map<IEnumerable<ENT.UserSet>, IEnumerable<DTO.UserSet>>(users);
            else
                dtoUsers = new List<DTO.UserSet>();

            return dtoUsers;
        }

        /// <summary>
        /// Récupére l'entité désignée par l'id en paramétre.
        /// </summary>
        /// <param name="id">Id de l'entité</param>
        /// <returns>Message de retour avec l'entité</returns>
        public async Task<DTO.UserSet> GetById(long id)
        {
            ENT.UserSet user = await UserSetDomain.Get(id);

            DTO.UserSet dtoUserSet = null;
            if (user != null)
                dtoUserSet = Mapper.Map<ENT.UserSet, DTO.UserSet>(user);

            return dtoUserSet;
        }

        /// <summary>
        /// Lie les utilisateurs donnés en entrée à un UserSet.
        /// </summary>
        /// <param name="userSetId">Id de l'UserSet</param>
        /// <param name="userSetUser">Liste des liens d'utilisateur</param>
        /// <returns>Message de retour</returns>
        public async Task<HttpResponseMessageResult> BindUserToUserSet(long userSetId, IEnumerable<DTO.UserSetUser> userSetUser)
        {
            return await UserSetDomain.BindUserToUserSet(userSetId, Mapper.Map<IEnumerable<DTO.UserSetUser>, IEnumerable<ENT.UserSetUser>>(userSetUser));
        }

    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.Transverse.Helpers;
using Workflow.Transverse.DTO;

namespace Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces
{
    /// <summary>
    ///  IUserSetAdapter interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the UserSet adapter.
    /// </remarks>
    public interface IUserSetAdapter : IBaseAdapter<UserSet>
    {
        ///// <summary>
        ///// This function permits to delete an user
        ///// </summary>
        ///// <param name="id">Id</param>
        ///// <returns>HttpResponseMessageResult</returns>
        //Task Delete(long id);

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="model"></param>
        ///// <returns></returns>
        //Task<DTO.UserSet> Put(long id, DTO.UserSet model);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<List<User>> GetUsers(long userSetId);

        /// <summary>
        /// Ajout d'un nouvel UserSet
        /// </summary>
        /// <param name="user">nouveau UserSet</param>
        /// <returns>UserSet enregistré</returns>
        Task<UserSet> Add(UserSet userSet);

        /// <summary>
        /// Lie les utilisateurs donnés en entrée à un UserSet.
        /// </summary>
        /// <param name="userSetId">Id de l'UserSet</param>
        /// <param name="userSetUser">Liste des liens d'utilisateur</param>
        /// <returns>Message de retour</returns>
        Task<HttpResponseMessageResult> BindUserToUserSet(long userSetId, IEnumerable<UserSetUser> userSetUser);
    }
}

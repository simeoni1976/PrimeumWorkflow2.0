using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.BusinessLayer.Domains.Interfaces
{
    /// <summary>
    /// IUserSetDomain interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for a user business.
    /// </remarks>
    public interface IUserSetDomain
    {
        /// <summary>
        /// This function permits to get all the UserSets
        /// </summary>
        /// <returns>IEnumerable</returns>
        Task<IEnumerable<UserSet>> Get();

        /// <summary>
        /// This function permits to get all the UserSets by an id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>User</returns>
        Task<UserSet> Get(long id);

        /// <summary> 
        /// This function permits to add a new UserSet
        /// </summary>
        /// <param name="UserSet">UserSet</param>
        /// <returns>UserSet</returns>
        Task<UserSet> Add(UserSet User);

        /// <summary>
        /// This function permits to update UserSet
        /// </summary>
        /// <param name="UserSet">UserSet</param>
        /// <returns>UserSet</returns>
        Task<UserSet> Update(UserSet User);

        /// <summary>
        /// This function permits to delete UserSet
        /// </summary>
        /// <param name="UserSet">UserSet</param>
        /// <returns>Task</returns>
        Task Delete(UserSet User);

        /// <summary>
        /// This function permits to delete UserSet
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Task</returns>
        Task Delete(long id);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userSetId"></param>
        /// <returns></returns>
        Task<List<User>> GetUsers(long userSetId);

        /// <summary>
        /// Donne les utilisateurs (dans une liste de UserSetUser) chargé d'un droit par rapport aux CriteriaValues passés en paramétre.
        /// </summary>
        /// <param name="criteriaValues">Liste de criteriaValues</param>
        /// <param name="dataSetId">Id du DataSet</param>
        /// <param name="right">Droit recherché</param>
        /// <returns>Liste des utilisateurs.</returns>
        /// <remarks>La liste de UserSetUser peut contenir 0, 1 ou n utilisateurs (non distinct).</remarks>
        Task<IEnumerable<UserSetUser>> GetUsersByRight(IEnumerable<IEnumerable<CriteriaValues>> criteriaValues, long dataSetId, RightEnum right);

        /// <summary>
        /// Lie les utilisateurs donnés en entrée à un UserSet.
        /// </summary>
        /// <param name="userSetId">Id de l'UserSet</param>
        /// <param name="userSetUser">Liste des liens d'utilisateur</param>
        /// <returns>Message de retour</returns>
        Task<HttpResponseMessageResult> BindUserToUserSet(long userSetId, IEnumerable<UserSetUser> userSetUser);
    }
}

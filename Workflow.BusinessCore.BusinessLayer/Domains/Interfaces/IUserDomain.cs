using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.DataLayer.Entities;

namespace Workflow.BusinessCore.BusinessLayer.Domains.Interfaces
{
    /// <summary>
    /// IUserDomain interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for a user business.
    /// </remarks>
    public interface IUserDomain
    {
        /// <summary>
        /// This function permits to get all the Users
        /// </summary>
        /// <returns>IEnumerable</returns>
        Task<IEnumerable<User>> Get();

        /// <summary>
        /// This function permits to get all the Users by an id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>User</returns>
        Task<User> Get(long id);

        /// <summary> 
        /// This function permits to add a new User
        /// </summary>
        /// <param name="User">User</param>
        /// <returns>User</returns>
        Task<User> Add(User User);

        /// <summary>
        /// This function permits to update User
        /// </summary>
        /// <param name="User">User</param>
        /// <returns>User</returns>
        Task<User> Update(User User);

        /// <summary>
        /// This function permits to delete User
        /// </summary>
        /// <param name="User">User</param>
        /// <returns>Task</returns>
        Task Delete(User User);

        /// <summary>
        /// This function permits to delete User
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Task</returns>
        Task Delete(long id);

        /// <summary>
        /// This function permits to get the usersets for an user id.
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Task</returns>
        Task<List<UserSet>> GetUserSets(string employeeId);

        /// <summary>
        /// Renvoit la liste des SelectorInstance dont le loginModificator est modificateur (en attente de modification).
        /// </summary>
        /// <param name="modificatorId">Id de l'utilisateur recherché</param>
        /// <param name="workflowInstanceId">Id du workflowInstance cible</param>
        /// <returns>Liste des SelectorInstance</returns>
        Task<IEnumerable<SelectorInstance>> GetSelectorInstanceByModificator(long modificatorId, long workflowInstanceId);

        /// <summary>
        /// Enregistre une série de nouveaux utilisateurs.
        /// </summary>
        /// <param name="users">Liste de nouveaux utilisateurs.</param>
        /// <returns>Renvoit la liste des ids des utilisateurs.</returns>
        Task<IEnumerable<long>> Import(IEnumerable<User> users);

        /// <summary>
        /// Récupére un utilisateur par son login.
        /// </summary>
        /// <param name="login">Login recherché</param>
        /// <returns>Utilisateur</returns>
        Task<User> GetByLogin(string login);

        /// <summary>
        /// Récupére les WorkflowInstances dans lesquels un utilisateur opére.
        /// </summary>
        /// <param name="userId">Id utilisateur</param>
        /// <returns>Liste de WorkflowInstance</returns>
        Task<IEnumerable<WorkflowInstance>> GetWorkflowInstance(long userId);

        /// <summary>
        /// Récupére les SelectorInstance pour lesquels l'utilisateur est un validateur.
        /// </summary>
        /// <param name="validatorId">Id de l'utilisateur validateur</param>
        /// <param name="workflowInstanceId">Id du WorkflowInstance</param>
        /// <returns>Liste des SelectorInstance</returns>
        Task<IEnumerable<SelectorInstance>> GetSelectorInstanceByValidator(long validatorId, long workflowInstanceId);


    }
}

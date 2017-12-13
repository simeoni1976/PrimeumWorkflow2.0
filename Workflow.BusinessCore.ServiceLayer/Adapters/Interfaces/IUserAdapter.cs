using System.Collections.Generic;
using System.Threading.Tasks;
using DTO = Workflow.Transverse.DTO;

namespace Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces
{
    /// <summary>
    ///  IUserAdapter interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the user adapter.
    /// </remarks>
    public interface IUserAdapter : IBaseAdapter<DTO.User>
    {
        /// <summary>
        /// This function permits to delete an user
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>HttpResponseMessageResult</returns>
        Task Delete(long id);

        /// <summary>
        /// This function permits to put a model.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<DTO.User> Put(long id, DTO.User model);

        /// <summary>
        /// This function permits to get an access for a login and his password.
        /// </summary>
        /// <param name="login"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        Task<DTO.User> Get(string login, string password);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<IEnumerable<DTO.UserSet>> GetUserSets(string employeeId);

        /// <summary>
        /// Renvoit la liste des SelectorInstance dont le loginModificator est modificateur (en attente de modification).
        /// </summary>
        /// <param name="modificatorId">Id de l'utilisateur recherché</param>
        /// <param name="workflowInstanceId">Id du workflowInstance cible</param>
        /// <returns>Liste des SelectorInstance</returns>
        Task<IEnumerable<DTO.SelectorInstance>> GetSelectorInstanceByModificator(long modificatorId, long workflowInstanceId);


        /// <summary>
        /// Enregistre une série de nouveaux utilisateurs.
        /// </summary>
        /// <param name="users">Liste de nouveaux utilisateurs.</param>
        /// <returns>Renvoit la liste des ids des utilisateurs.</returns>
        Task<IEnumerable<long>> Import(IEnumerable<DTO.User> users);

        /// <summary>
        /// Récupére un utilisateur par son login.
        /// </summary>
        /// <param name="login">Login recherché</param>
        /// <returns>Utilisateur</returns>
        Task<DTO.User> GetByLogin(string login);

        /// <summary>
        /// Récupére les WorkflowInstances dans lesquels un utilisateur opére.
        /// </summary>
        /// <param name="userId">Id utilisateur</param>
        /// <returns>Liste de WorkflowInstance</returns>
        Task<IEnumerable<DTO.WorkflowInstance>> GetWorkflowInstance(long userId);

        /// <summary>
        /// Récupére les SelectorInstance pour lesquels l'utilisateur est un validateur.
        /// </summary>
        /// <param name="validatorId">Id de l'utilisateur validateur</param>
        /// <param name="workflowInstanceId">Id du WorkflowInstance</param>
        /// <returns>Liste des SelectorInstance</returns>
        Task<IEnumerable<DTO.SelectorInstance>> GetSelectorInstanceByValidator(long validatorId, long workflowInstanceId);
    }
}

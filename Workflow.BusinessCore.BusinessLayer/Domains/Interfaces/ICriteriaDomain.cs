using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.DataLayer.Entities;

namespace Workflow.BusinessCore.BusinessLayer.Domains.Interfaces
{
    /// <summary>
    /// ICriteria interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the Criteria business.
    /// </remarks>
    public interface ICriteriaDomain
    {
        /// <summary>
        /// Envoi la liste des CriteriaValues existants pour ce Criteria.
        /// </summary>
        /// <param name="criteria">Criteria</param>
        /// <param name="wfInstance">Instance du WorkflowInstance</param>
        /// <returns>Liste des CriteriaValues</returns>
        Task<IEnumerable<CriteriaValues>> ExtractCriteriaValues(Criteria criteria, WorkflowInstance wfInstance);

        /// <summary>
        /// Envoi la liste des listes de CriteriaValues existants pour une liste de Criteria.
        /// </summary>
        /// <param name="criterias">Liste de criteria</param>
        /// <param name="wfInstance">Instance du WorkflowInstance</param>
        /// <returns>Liste de liste des CriteriaValues</returns>
        Task<IEnumerable<IEnumerable<CriteriaValues>>> ExtractAllCriteriaValues(IEnumerable<Criteria> criterias, WorkflowInstance wfInstance);

        /// <summary>
        /// This function permits to get all the Criterias
        /// </summary>
        /// <returns>IEnumerable</returns>
        Task<IEnumerable<Criteria>> Get();

        /// <summary>
        /// This function permits to get all the criterias by an id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Criteria</returns>
        Task<Criteria> Get(long id);

        /// <summary>
        /// Ajoute un criteria dans un SelectorConfig.
        /// </summary>
        /// <param name="criteria">Criteria à ajouter</param>
        /// <returns>Objet Criteria ajouter (id à jour).</returns>
        /// <remarks>L'objet Criteria doit contenir l'id du SelectorConfig dans lequel il faut l'ajouter. L'API retourne une erreur
        /// lorsque la dimension ou la valeur du Criteria n'est pas définie. 
        /// Les valeurs possibles d'un Criteria sont '*', chaine-de-caractères, '{valeur1, valeur2, ..., valeurn}' </remarks>
        Task<Criteria> Add(Criteria criteria);

        /// <summary>
        /// This function permits to update criteria
        /// </summary>
        /// <param name="Criteria">Criteria object</param>
        /// <returns>Criteria</returns>
        Task<Criteria> Update(Criteria Criteria);

        /// <summary>
        /// This function permits to delete criteria
        /// </summary>
        /// <param name="Criteria">Criteria object</param>
        /// <returns>Task</returns>
        Task Delete(Criteria Criteria);

        /// <summary>
        /// This function permits to delete criteria
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Task</returns>
        Task Delete(long id);

        /// <summary>
        /// Duplique un Criteria pour l'instance d'un WorkflowConfig.
        /// </summary>
        /// <param name="criteria">Criteria original</param>
        /// <returns>Duplicat du Criteria original</returns>
        Task<Criteria> CopyForStatic(Criteria criteria);

        /// <summary>
        /// Duplique un ConditionnedCriteria pour l'instance d'un WorkflowConfig.
        /// </summary>
        /// <param name="criteria">ConditionnedCriteria original</param>
        /// <returns>Duplicat du ConditionnedCriteria original</returns>
        Task<ConditionnedCriteria> CopyForStatic(ConditionnedCriteria condCriteria);


    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.BusinessLayer.Domains.Interfaces
{
    /// <summary>
    ///  ISelectorConfig instance interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the SelectorConfig instance business.
    /// </remarks>
    public interface ISelectorConfigDomain
    {
        /// <summary>
        /// This function permits to get all the Workflow instances
        /// </summary>
        /// <returns>IEnumerable</returns>
        Task<IEnumerable<SelectorConfig>> Get();

        /// <summary>
        /// This function permits to get all the Workflow instances by an id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>SelectorConfig</returns>
        Task<SelectorConfig> Get(long id);

        /// <summary>
        /// This function permits to add a new Workflow instance
        /// </summary>
        Task<SelectorConfig> Add(SelectorConfig selectorConfig);

        /// <summary>
        /// This function permits to update Workflow instance
        /// </summary>
        /// <param name="selectorConfig">SelectorConfig</param>
        /// <returns>SelectorConfig</returns>
        Task<SelectorConfig> Update(SelectorConfig selectorConfig);

        /// <summary>
        /// This function permits to delete Workflow instance
        /// </summary>
        /// <param name="selectorConfig">SelectorConfig</param>
        /// <returns>Task</returns>
        Task Delete(SelectorConfig selectorConfig);

        /// <summary>
        /// This function permits to delete Workflow instance
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Task</returns>
        Task Delete(long id);

        /// <summary>
        /// Ajoute un SelectorConfig en Previous Propagate (PrevPropagate) d'un autre SelectorConfig.
        /// </summary>
        /// <param name="idSelectorConfig">Id du SelectorConfig cible</param>
        /// <param name="previousSelectorConf">SelectorConfig à ajouter en Previous Propagate</param>
        /// <returns>Message de resultat</returns>
        /// <remarks>Il faut que les id donnés en paramètre existent en base de donnée. Si le SelectorConfig cible
        /// posséde déjà le SelectorConfig en PrevPropagate, il ne se passe rien.</remarks>
        Task<HttpResponseMessageResult> AddPreviousPropagate(long idSelectorConfig, SelectorConfig previousSelectorConf);

        /// <summary>
        /// Ajoute un SelectorConfig en Propagate d'un autre SelectorConfig.
        /// </summary>
        /// <param name="idSelectorConfig">Id du SelectorConfig cible</param>
        /// <param name="nextSelectorConf">SelectorConfig à ajouter en Propagate</param>
        /// <returns>Message de resultat</returns>
        /// <remarks>Il faut que les id donnés en paramètre existent en base de donnée. Si le SelectorConfig cible
        /// posséde déjà le SelectorConfig en Propagate, il ne se passe rien.</remarks>
        Task<HttpResponseMessageResult> AddPropagate(long idSelectorConfig, SelectorConfig nextSelectorConf);

        /// <summary>
        /// Ajoute un Criteria à un SelectorConfig pour cibler les données à modifier
        /// </summary>
        /// <param name="idSelectorConfig">Id du SelectorConfig cible</param>
        /// <param name="criteria">Criteria définissant les valeurs à modifier</param>
        /// <returns>Message de résultat</returns>
        /// <remarks>Ajoute juste l'objet Criteria au SelectorConfig. Vérifie l'existance du SelectorConfig mais ne controle pas si le résultat 
        /// du Criteria sur les potentiels subset du SelectorConfig contient bel et bien des données.</remarks>
        Task<HttpResponseMessageResult> AddCriteriaToModifyValue(long idSelectorConfig, Criteria criteria);

        /// <summary>
        /// Ajoute un criteria dans la liste ordonnée des criterias de modificateurs d'un SelectorConfig.
        /// </summary>
        /// <param name="idSelectorConfig">Id du SelectorConfig cible</param>
        /// <param name="criteria">Criteria destiné à la liste des modificateurs</param>
        /// <returns>Message de retour</returns>
        Task<HttpResponseMessageResult> AddModifiersCriteria(long idSelectorConfig, Criteria criteria);

        /// <summary>
        /// Ajoute un criteria dans la liste ordonnée des criterias de validateurs d'un SelectorConfig.
        /// </summary>
        /// <param name="idSelectorConfig">Id du SelectorConfig cible</param>
        /// <param name="criteria">Criteria destiné à la liste des validateurs</param>
        /// <returns>Message de retour</returns>
        Task<HttpResponseMessageResult> AddValidatorsCriteria(long idSelectorConfig, Criteria criteria);

        /// <summary>
        /// Duplicate un SelectorConfig pour l'instance d'un WorkflowConfig.
        /// </summary>
        /// <param name="selectorConfig">SelectorConfig original</param>
        /// <returns>Duplicat du SelectorConfig original</returns>
        Task<SelectorConfig> CopyForStatic(SelectorConfig selectorConfig);


        /// <summary>
        /// Récupere la liste des SelectorConfig et de leurs SelectorInstance lancés d'un WorkflowInstance.
        /// </summary>
        /// <param name="workflowInstanceId">Id du workflowInstance</param>
        /// <returns>Liste de SelectorConfig (et donc de leur SelectorInstance)</returns>
        Task<IEnumerable<SelectorConfig>> GetSelectors(long workflowInstanceId);

    }
}

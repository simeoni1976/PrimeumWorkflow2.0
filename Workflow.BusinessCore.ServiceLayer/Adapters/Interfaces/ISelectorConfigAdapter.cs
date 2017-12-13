using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.Transverse.Helpers;
using DTO = Workflow.Transverse.DTO;

namespace Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces
{
    /// <summary>
    ///  ISelectorConfigAdapter interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the selector adapter.
    /// </remarks>
    public interface ISelectorConfigAdapter : IBaseAdapter<DTO.SelectorConfig>
    {
        /// <summary>
        /// Permet d'ajouter un SelectorConfig.
        /// </summary>
        /// <param name="selectConf">Objet SelectorConfig</param>
        /// <returns>Message de résultat</returns>
        /// <remarks>L'objet SelectorConfig doit connaitre l'id de son WorkflowConfig parent. De plus, il doit avoir un nom unique. 
        /// L'opération sort en erreur si l'une des deux conditions, ou les deux, n'est pas respectée.</remarks>
        Task<HttpResponseMessageResult> Add(DTO.SelectorConfig selectConf);


        /// <summary>
        /// Ajoute un SelectorConfig en Previous Propagate (PrevPropagate) d'un autre SelectorConfig.
        /// </summary>
        /// <param name="idSelectorConfig">Id du SelectorConfig cible</param>
        /// <param name="PreviousSelectorConf">SelectorConfig à ajouter en Previous Propagate</param>
        /// <returns>Message de resultat</returns>
        /// <remarks>Il faut que les id donnés en paramètre existent en base de donnée. Si le SelectorConfig cible
        /// posséde déjà le SelectorConfig en PrevPropagate, il ne se passe rien.</remarks>
        Task<HttpResponseMessageResult> AddPreviousPropagate(long idSelectorConfig, DTO.SelectorConfig previousSelectorConf);

        /// <summary>
        /// Ajoute un SelectorConfig en Propagate d'un autre SelectorConfig.
        /// </summary>
        /// <param name="idSelectorConfig">Id du SelectorConfig cible</param>
        /// <param name="nextSelectorConf">SelectorConfig à ajouter en Propagate</param>
        /// <returns>Message de resultat</returns>
        /// <remarks>Il faut que les id donnés en paramètre existent en base de donnée. Si le SelectorConfig cible
        /// posséde déjà le SelectorConfig en Propagate, il ne se passe rien.</remarks>
        Task<HttpResponseMessageResult> AddPropagate(long idSelectorConfig, DTO.SelectorConfig nextSelectorConf);

        /// <summary>
        /// Ajoute un Criteria à un SelectorConfig pour cibler les données à modifier
        /// </summary>
        /// <param name="idSelectorConfig">Id du SelectorConfig cible</param>
        /// <param name="criteria">Criteria définissant les valeurs à modifier</param>
        /// <returns>Message de résultat</returns>
        /// <remarks>Ajoute juste l'objet Criteria au SelectorConfig. Vérifie l'existance du SelectorConfig mais ne controle pas si le résultat 
        /// du Criteria sur les potentiels subset du SelectorConfig contient bel et bien des données.</remarks>
        Task<HttpResponseMessageResult> AddCriteriaToModifyValue(long idSelectorConfig, DTO.Criteria criteria);


        /// <summary>
        /// Ajoute un criteria dans la liste ordonnée des criterias de modificateurs d'un SelectorConfig.
        /// </summary>
        /// <param name="idSelectorConfig">Id du SelectorConfig cible</param>
        /// <param name="criteria">Criteria destiné à la liste des modificateurs</param>
        /// <returns>Message de retour</returns>
        Task<HttpResponseMessageResult> AddModifiersCriteria(long idSelectorConfig, DTO.Criteria criteria);

        /// <summary>
        /// Ajoute un criteria dans la liste ordonnée des criterias de validateurs d'un SelectorConfig.
        /// </summary>
        /// <param name="idSelectorConfig">Id du SelectorConfig cible</param>
        /// <param name="criteria">Criteria destiné à la liste des validateurs</param>
        /// <returns>Message de retour</returns>
        Task<HttpResponseMessageResult> AddValidatorsCriteria(long idSelectorConfig, DTO.Criteria criteria);

        /// <summary>
        /// Récupere la liste des SelectorConfig et de leurs SelectorInstance lancés d'un WorkflowInstance.
        /// </summary>
        /// <param name="workflowInstanceId">Id du workflowInstance</param>
        /// <returns>Liste de SelectorConfig (et donc de leur SelectorInstance)</returns>
        Task<IEnumerable<DTO.SelectorConfig>> GetSelectors(long workflowInstanceId);

    }
}

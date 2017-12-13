using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.Transverse.Helpers;
using ENT = Workflow.BusinessCore.DataLayer.Entities;

namespace Workflow.BusinessCore.BusinessLayer.Domains.Interfaces
{
    public interface IActionSequenceDomain
    {
        /// <summary>
        /// Permet d'exécuter toutes les actions, dans l'ordre, d'une séquence sur un SelectorInstance.
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <param name="wfInstance">WorkflowInstance</param>
        /// <param name="sequences">Sequence d'actions</param>
        /// <param name="values">Valeurs à modifier</param>
        /// <returns>Message de retour</returns>
        Task<HttpResponseMessageResult> ExecuteActionSequence(IEnumerable<ActionSequence> sequences, SelectorInstance selectorInstance, WorkflowInstance wfInstance, IEnumerable<KeyValuePair<long, double>> values);

        /// <summary>
        /// Exécute une action sur un SelectorInstance
        /// </summary>
        /// <param name="action">Action</param>
        /// <param name="referenceSequence">Référence de la séquence d'action</param>
        /// <param name="OrderSequence">Numéro d'ordre de l'action dans la séquence</param>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <param name="wfInstance">WorkflowInstance</param>
        /// <param name="values">Valeurs à modifier</param>
        /// <returns>Message de retour</returns>
        Task<HttpResponseMessageResult> ExecuteAction(ENT.Action action, long referenceSequence, int OrderSequence, SelectorInstance selectorInstance, WorkflowInstance wfInstance, IEnumerable<KeyValuePair<long, double>> values);

        /// <summary>
        /// Ajoute une action en base, indépendamment d'un workflow.
        /// </summary>
        /// <param name="action">Nouvelle action</param>
        /// <returns>Action enregistrée (avec son id)</returns>
        Task<ENT.Action> AddAction(ENT.Action action);

        /// <summary>
        /// Ajoute une nouvelle action dans une séquence (existante ou non)
        /// </summary>
        /// <param name="actionSequence">Nouvelle ActionSequence</param>
        /// <param name="actionId">Id de l'Action à exécuter</param>
        /// <returns>Message de retour</returns>
        Task<ActionSequence> AddActionSequence(ActionSequence actionSequence, long actionId);

        /// <summary>
        /// Ajoute un paramètre dans l'action d'une séquence d'action.
        /// </summary>
        /// <param name="actionParameter">ActionParameter à ajouter</param>
        /// <param name="actionSequenceId">Id de la séquence d'action cible</param>
        /// <returns>Message de retour</returns>
        Task<ActionParameter> AddActionParameter(ActionParameter actionParameter, long actionSequenceId);

        /// <summary>
        /// Récupére toutes les actions existantes.
        /// </summary>
        /// <returns>Liste d'actions</returns>
        Task<IEnumerable<ENT.Action>> GetAllAction();

        /// <summary>
        /// Récupére une action par son Id
        /// </summary>
        /// <param name="actionId">Id de l'action</param>
        /// <returns>Action</returns>
        Task<ENT.Action> GetAction(long actionId);

        /// <summary>
        /// Récupére toutes les ActionSequence.
        /// </summary>
        /// <returns>Liste d'ActionSequence</returns>
        Task<IEnumerable<ActionSequence>> GetAllActionSequence();

        /// <summary>
        /// Récupére une ActionSequence par son Id
        /// </summary>
        /// <param name="actionSequenceId">Id de l'ActionSequence</param>
        /// <returns>ActionSequence</returns>
        Task<ActionSequence> GetActionSequence(long actionSequenceId);

        /// <summary>
        /// Récupére tous les ActionParameter
        /// </summary>
        /// <returns>Liste d'ActionParameter</returns>
        Task<IEnumerable<ActionParameter>> GetAllActionParameter();

        /// <summary>
        /// Récupére un ActionParameter par son Id
        /// </summary>
        /// <param name="actionParameterId">Id de l'ActionParameter</param>
        /// <returns>ActionParameter</returns>
        Task<ActionParameter> GetActionParameter(long actionParameterId);
    }
}

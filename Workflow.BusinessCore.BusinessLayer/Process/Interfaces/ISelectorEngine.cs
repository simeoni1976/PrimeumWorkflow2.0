using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.BusinessLayer.Process.Interfaces
{
    /// <summary>
    ///  ISelectorInstanceEngine interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the ISelectorInstanceEngine process.
    /// </remarks>
    public interface ISelectorEngine
    {
        /// <summary>
        /// Génére tous les SelectorInstance depuis les criteria donnés.
        /// </summary>
        /// <param name="selectConf">SelectorConfig cible</param>
        /// <param name="wfInstance">WorkflowInstance nouvellement démarré</param>
        /// <param name="parent">Eventuel SelectorInstance pouvant être à l'origine de la création de nouvelles instances</param>
        /// <returns>Message de résultat</returns>
        Task<HttpResponseMessageResult> GenerateSelectorsInstances(SelectorConfig selectConf, WorkflowInstance wfInstance, SelectorInstance parent = null);

        /// <summary>
        /// Pour un SelectorInstance donné, passe à l'étape suivant dans le flux.
        /// </summary>
        /// <param name="selectIns">SelectorInstance sujet de la transition</param>
        /// <param name="wfInstance">Workflow instance</param>
        /// <param name="scope">Etape vers laquelle le SelectorInstance s'oriente.</param>
        /// <param name="values">Valeurs à modifier</param>
        /// <returns>Message</returns>
        Task<HttpResponseMessageResult> NextStep(SelectorInstance selectIns, WorkflowInstance wfInstance, SelectorStateEnum scope = SelectorStateEnum.Void, IEnumerable<KeyValuePair<long, double>> values = null);

        /// <summary>
        /// Déclenche le PrevPropagate sur l'ensemble des selectorInstances issus du SelectorConfig original.
        /// </summary>
        /// <param name="selectIns">SelectorInstance</param>
        /// <param name="wfInstance">Workflow instance</param>
        /// <returns>Message</returns>
        Task<HttpResponseMessageResult> PrevPropagate(SelectorInstance selectIns, WorkflowInstance wfInstance);

        /// <summary>
        /// This function permits to do a INIT action.
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <param name="wfInstance">Workflow instance</param>
        /// <returns>Message</returns>
        Task<HttpResponseMessageResult> Init(SelectorInstance selectorInstance, WorkflowInstance wfInstance);

        /// <summary>
        /// This function permits to do a MODIFY action.
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <param name="wfInstance">Workflow instance</param>
        /// <param name="values">Valeurs à modifier</param>
        /// <returns>Message</returns>
        Task<HttpResponseMessageResult> Modify(SelectorInstance selectorInstance, WorkflowInstance wfInstance, IEnumerable<KeyValuePair<long, double>> values);

        /// <summary>
        /// This function permits to do a ACT action.
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <param name="wfInstance">Workflow instance</param>
        /// <param name="values">Valeurs à modifier</param>
        /// <returns>Message de retour</returns>
        Task<HttpResponseMessageResult> Act(SelectorInstance selectorInstance, WorkflowInstance wfInstance, IEnumerable<KeyValuePair<long, double>> values);

        /// <summary>
        /// Etabli l'étape Constraint.
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <param name="wfInstance">Workflow Instance</param>
        /// <param name="values">Valeurs à modifier</param>
        /// <returns>Message de retour</returns>
        Task<HttpResponseMessageResult> Constraint(SelectorInstance selectorInstance, WorkflowInstance wfInstance, IEnumerable<KeyValuePair<long, double>> values);


        /// <summary>
        /// This function permits to do a VALIDATE action.
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        /// <param name="wfInstance">Workflow Instance</param>
        /// <returns>Message de retour</returns>
        Task<HttpResponseMessageResult> Validate(SelectorInstance selectorInstance, WorkflowInstance wfInstance);

        /// <summary>
        /// This function permits to do a COMMIT action.
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        Task Commit(SelectorInstance selectorInstance);

        /// <summary>
        /// This function permits to do a PROPAGATE action.
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        Task PropagateAsync(SelectorInstance selectorInstance);

        /// <summary>
        /// This function permits to update values from the workflow.
        /// </summary>
        /// <param name="selectorInstance">SelectorInstance</param>
        Task UpdateFutureValues(SelectorInstance selectorInstance);

        /// <summary>
        /// Modifie les données d'un SelectorInstance. Les données sont d'abord sauvées, puis le SelectorInstance passe en Act et en Constraints.
        /// </summary>
        /// <param name="selectorInstanceId">Id du SelectorInstance</param>
        /// <param name="values">Valeurs à modifier</param>
        /// <remarks>Les valeurs à modifier sont au format suivant : {id de la cellule}:{nouvelle valeur}</remarks>
        /// <returns>Message à modifier</returns>
        Task<HttpResponseMessageResult> SaveData(long selectorInstanceId, IEnumerable<KeyValuePair<long, double>> values);
    }
}
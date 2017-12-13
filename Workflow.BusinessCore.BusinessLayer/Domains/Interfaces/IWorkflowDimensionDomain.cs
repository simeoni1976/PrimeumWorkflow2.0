using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.BusinessLayer.Domains.Interfaces
{
    /// <summary>
    ///  IWorkflowDimension instance interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the Workflow dimension business.
    /// </remarks>
    public interface IWorkflowDimensionDomain
    {
        /// <summary>
        /// This function permits to get all the Workflow dimension
        /// </summary>
        /// <returns>IEnumerable</returns>
        Task<IEnumerable<WorkflowDimension>> Get();

        /// <summary>
        /// This function permits to get the Workflow dimension
        /// </summary>
        /// <param name="columnName">Dimension column name</param>
        /// <returns>WorkflowDimension</returns>
        Task<WorkflowDimension> Get(string columnName);

        /// <summary>
        /// This function permits to get all the Workflow dimension by an id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>WorkflowDimension</returns>
        Task<WorkflowDimension> Get(long id);

        /// <summary>
        /// This function permits to add a new Workflow dimension
        /// </summary>
        /// <param name="workflow">WorkflowDimension</param>
        /// <returns>WorkflowDimension</returns>
        Task<WorkflowDimension> Add(WorkflowDimension workflow);

        /// <summary>
        /// This function permits to update Workflow dimension
        /// </summary>
        /// <param name="workflow">WorkflowDimension</param>
        /// <returns>WorkflowDimension</returns>
        Task<WorkflowDimension> Update(WorkflowDimension workflow);

        /// <summary>
        /// This function permits to delete Workflow dimension
        /// </summary>
        /// <param name="workflow">WorkflowDimension</param>
        /// <returns>Task</returns>
        Task Delete(WorkflowDimension workflow);

        /// <summary>
        /// This function permits to delete Workflow dimension
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Task</returns>
        Task Delete(long id);

        /// <summary>
        /// Duplique un WorkflowDimension pour l'instance d'un WorkflowConfig.
        /// </summary>
        /// <param name="workflowDimension">WorkflowDimension original</param>
        /// <returns>Duplicat du WorkflowDimension original</returns>
        Task<WorkflowDimension> CopyForStatic(WorkflowDimension workflowDimension);

        /// <summary>
        /// Ajoute un WorkflowDimension à un WorkflowConfig
        /// </summary>
        /// <param name="workflowConfigId">Id du WorkflowConfig cible</param>
        /// <param name="workflowDimension">WorkflowDimension</param>
        /// <returns>Message de retour</returns>
        Task<WorkflowDimension> AddWorkflowDimension(long workflowConfigId, WorkflowDimension workflowDimension);

    }
}

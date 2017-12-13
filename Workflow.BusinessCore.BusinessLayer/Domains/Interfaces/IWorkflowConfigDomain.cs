using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.DataLayer.Entities;

namespace Workflow.BusinessCore.BusinessLayer.Domains.Interfaces
{
    /// <summary>
    /// IWorkflowConfig interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for a workflow config business.
    /// </remarks>
    public interface IWorkflowConfigDomain
    {
        /// <summary>
        /// This function permits to get a workflow
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>WorkflowConfig</returns>
        Task<WorkflowConfig> Get(long id);

        /// <summary>
        /// This function permits to get a workflow
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>WorkflowConfig</returns>
        Task<WorkflowConfig> Get(string name);

        /// <summary>
        /// This function permits to get all the Workflows
        /// </summary>
        /// <returns>IEnumerable</returns>
        Task<IEnumerable<WorkflowConfig>> Get();

        /// <summary>
        /// This function permits to add a new Workflow
        /// </summary>
        /// <param name="workflow">WorkflowConfig</param>
        /// <returns>WorkflowConfig</returns>
        Task<WorkflowConfig> Add(WorkflowConfig workflow);

        /// <summary>
        /// This function permits to update Workflow
        /// </summary>
        /// <param name="workflow">WorkflowConfig</param>
        /// <returns>WorkflowConfig</returns>
        Task<WorkflowConfig> Update(WorkflowConfig workflow);

        /// <summary>
        /// This function permits to delete Workflow
        /// </summary>
        /// <param name="workflow">WorkflowConfig</param>
        /// <returns>Task</returns>
        Task Delete(WorkflowConfig workflow);
        
        /// <summary>
        /// This function permits to delete Workflow
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Task</returns>
        Task Delete(long id);

        /// <summary>
        /// Duplique un WorkflowConfig, en laissant dans la copie l'id du WorkflowConfig d'origine.
        /// </summary>
        /// <param name="workflowConfig">WorkflowConfig à copier</param>
        /// <returns>WorkflowConfig dupliqué</returns>
        Task<WorkflowConfig> CopyForStatic(WorkflowConfig workflowConfig);
    }
}

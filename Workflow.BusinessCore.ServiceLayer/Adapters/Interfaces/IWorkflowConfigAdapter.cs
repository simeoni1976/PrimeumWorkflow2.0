using System.Threading.Tasks;
using Workflow.Transverse.DTO;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces
{
    /// <summary>
    ///  IWorkflowConfigAdapter interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the workflow adapter.
    /// </remarks>
    public interface IWorkflowConfigAdapter : IBaseAdapter<WorkflowConfig>
    {
        /// <summary>
        /// This function permits to modify a workflow
        /// </summary>
        /// <param name="workflowConfig"></param>
        /// <returns></returns>
        Task<WorkflowConfig> Put(WorkflowConfig workflowConfig);

        /// <summary>
        /// This function permits to delete a workflow
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(long id);

        /// <summary>
        /// TODO : à modifier peut être selon les besoins.
        /// </summary>
        /// <param name="workflowConfig"></param>
        /// <returns></returns>
        Task<WorkflowConfig> Post(WorkflowConfig workflowConfig);

        /// <summary>
        /// Ajoute un WorkflowDimension à un WorkflowConfig
        /// </summary>
        /// <param name="workflowConfigId">Id du WorkflowConfig cible</param>
        /// <param name="workflowDimension">WorkflowDimension</param>
        /// <returns>Message de retour</returns>
        Task<WorkflowDimension> AddWorkflowDimension(long workflowConfigId, WorkflowDimension workflowDimension);
    }
}

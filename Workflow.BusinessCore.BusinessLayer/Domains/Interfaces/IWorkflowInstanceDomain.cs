using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.BusinessLayer.Domains.Interfaces
{
    /// <summary>
    ///  IWorkflowinstance instance interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the Workflow instance business.
    /// </remarks>
    public interface IWorkflowInstanceDomain
    {
        /// <summary>
        /// This function permits to create an instance
        /// </summary>
        /// <param name="workflowConfig">WorkflowConfig origne</param>
        /// <param name="dataSet">DataSet associé</param>
        /// <param name="UserSet">UserSet associé</param>
        /// <returns>WorkflowInstance</returns>
        Task<WorkflowInstance> CreateInstance(WorkflowConfig workflowConfig, DataSet dataSet, UserSet UserSet);

        /// <summary>
        /// This function permits to get all the Workflow instances
        /// </summary>
        /// <returns>IEnumerable</returns>
        Task<IEnumerable<WorkflowInstance>> Get();

        /// <summary>
        /// This function permits to get all the Workflow instances by an id
        /// </summary>
        /// <param name="id"> Id</param>
        /// <returns>WorkflowInstance</returns>
        Task<WorkflowInstance> Get(long id);

        /// <summary>
        /// This function permits to add a new Workflow instance
        /// </summary>
        /// <param name="workflow">Workflow instance</param>
        /// <returns>WorkflowInstance</returns>
        Task<WorkflowInstance> Add(WorkflowInstance workflow);

        /// <summary>
        /// This function permits to update Workflow instance
        /// </summary>
        /// <param name="workflow">Workflow instance</param>
        /// <returns>WorkflowInstance</returns>
        Task<WorkflowInstance> Update(WorkflowInstance workflow);

        /// <summary>
        /// This function permits to delete a Workflow instance
        /// </summary>
        /// <param name="workflow">Workflow instance</param>
        /// <returns>Task</returns>
        Task Delete(WorkflowInstance workflow);

        /// <summary>
        /// This function permits to delete a workflow instance
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Task</returns>
        Task Delete(long id);

        /// <summary>
        /// Démarre et initialise les SelectorConfig associés au WorkflowConfig original.
        /// </summary>
        /// <param name="wfInstance">WorkflowInstance nouvellement démarré</param>
        /// <returns>Message en cas de warning</returns>
        Task<HttpResponseMessageResult> StartSelectorsIntances(WorkflowInstance wfInstance);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using DTO = Workflow.Transverse.DTO;

namespace Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces
{
    /// <summary>
    ///  IWorkflowInstanceAdapter interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the workflow instance adapter.
    /// </remarks>
    public interface IWorkflowInstanceAdapter : IBaseAdapter<DTO.WorkflowInstance>
    {
        /// <summary>
        /// This function permits to modify a workflow instance
        /// </summary>
        /// <returns></returns>
        Task<DTO.WorkflowInstance> Put(long id, DTO.WorkflowInstance model);

        /// <summary>
        /// This function permits to delete a workflow
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(long id);
    }
}

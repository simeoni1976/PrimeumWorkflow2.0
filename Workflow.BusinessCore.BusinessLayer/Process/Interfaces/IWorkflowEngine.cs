using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.Transverse.Helpers;
using DTO = Workflow.Transverse.DTO;

namespace Workflow.BusinessCore.BusinessLayer.Process.Interfaces
{
    /// <summary>
    ///  IWorkflowInstanceEngine interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the Workflow instance engine.
    /// </remarks>
    public interface IWorkflowEngine
    {
        /// <summary>
        /// This method permits to open and start the workflow
        /// </summary>
        /// <param name="workflowConfigId">workflowConfig Id</param>
        /// <param name="dataSetId">dataSet Id</param>
        /// <param name="idUserSet">Id du UserSet à associer</param>
        Task<HttpResponseMessageResult> Open(long workflowConfigId, long dataSetId, long idUserSet);
    }
}

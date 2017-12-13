using System.Threading.Tasks;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces
{
    /// <summary>
    ///  IWorkflowAdapter interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the workflow adapter.
    /// </remarks>
    public interface IProcessAdapter
    {
        /// <summary>
        /// This method permits to start the workflow process
        /// </summary>
        /// <param name="workflowConfigId"></param>
        /// <param name="dataSetId"></param>
        /// <param name="idUserSet">Id du UserSet à associer</param>
        Task<HttpResponseMessageResult> ProcessStart(long workflowConfigId, long dataSetId, long idUserSet);
    }
}

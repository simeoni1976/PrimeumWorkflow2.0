using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Process.Interfaces;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using Workflow.Transverse.Helpers;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Workflow.BusinessCore.ServiceLayer.Adapters
{
    /// <summary>
    ///  WorkflowAdapter interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the Workflow adapter.
    /// </remarks>
    public class ProcessAdapter : IProcessAdapter
    {
        private readonly IServiceProvider _serviceProvider;

        private IWorkflowEngine WorkflowEngine
        {
            get
            {
                return _serviceProvider?.GetService<IWorkflowEngine>();
            }
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="workflowEngine"></param>
        public ProcessAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// This method permits to start the workflow process
        /// </summary>
        /// <param name="workflowConfigId"></param>
        /// <param name="dataSetId"></param>
        public async Task<HttpResponseMessageResult> ProcessStart(long workflowConfigId, long dataSetId, long idUserSet)
        {
           return await WorkflowEngine.Open(workflowConfigId, dataSetId, idUserSet);
        }
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Common;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.BusinessLayer.Process.Exceptions;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.Transverse.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Workflow.BusinessCore.BusinessLayer.Process.Interfaces;
using Workflow.BusinessCore.BusinessLayer.UnitOfWork.Interfaces;

namespace Workflow.BusinessCore.BusinessLayer.Domains
{
    /// <summary>
    /// WorkflowInstance domain class
    /// </summary>
    public class WorkflowInstanceDomain : AbstractDomain<WorkflowInstance>, IWorkflowInstanceDomain
    {
        private readonly IServiceProvider _serviceProvider = null;


        private IUnitOfWork UnitOfWork
        {
            get
            {
                return _serviceProvider?.GetService<IUnitOfWork>();
            }
        }

        private ISelectorConfigDomain SelectorConfigDomain
        {
            get
            {
                return _serviceProvider?.GetService<ISelectorConfigDomain>();
            }
        }

        private ISelectorEngine SelectorEngine
        {
            get
            {
                return _serviceProvider?.GetService<ISelectorEngine>();
            }
        }

        private IWorkflowConfigDomain WorkflowConfig
        {
            get
            {
                return _serviceProvider?.GetService<IWorkflowConfigDomain>();
            }
        }

        private IDataSetDomain DataSetDomain
        {
            get
            {
                return _serviceProvider?.GetService<IDataSetDomain>();
            }
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="unitOfWork">Unit Of Work</param>
        /// <param name="mapper">Mapper</param>
        public WorkflowInstanceDomain(IUnitOfWork unitOfWork, IServiceProvider serviceProvider) : base(unitOfWork.WorkflowInstanceRepository)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// This function permits to create an instance
        /// </summary>
        /// <param name="workflowConfig">WorkflowConfig origne</param>
        /// <param name="dataSet">DataSet associé</param>
        /// <param name="UserSet">UserSet associé</param>
        /// <returns>Task</returns>
        public async Task<WorkflowInstance> CreateInstance(WorkflowConfig workflowConfig, DataSet dataSet, UserSet UserSet)
        {
            // Contrôles
            if (workflowConfig == null)
                throw new InitialiseWorkflowException("Process: no WorkflowConfig! Impossible to continue.");
            if (dataSet == null)
                throw new InitialiseWorkflowException("Process: no DataSet! Impossible to continue.");
            if (UserSet == null)
                throw new InitialiseWorkflowException("Process: no UserSet! Impossible to continue.");

            // On duplique la configuration du workflow (WOR-222)
            WorkflowConfig duplWf = await WorkflowConfig.CopyForStatic(workflowConfig);

            WorkflowInstance newInstance = await Add(
                new WorkflowInstance()
                {
                    Name = workflowConfig.Name,
                    WorkflowConfig = duplWf,
                    DataSetId = dataSet.Id,
                    UserSetId = UserSet.Id,
                    AddedDate = DateTime.UtcNow,
                    Status = WorkflowStateEnum.Current,
                });

            // Initialise le DataSet
            int nbr = await DataSetDomain.InitializeData(dataSet);

            return newInstance;
        }


        /// <summary>
        /// Démarre et initialise les SelectorConfig associés au WorkflowConfig original.
        /// </summary>
        /// <param name="wfInstance">WorkflowInstance nouvellement démarré</param>
        /// <returns>Message en cas de warning</returns>
        public async Task<HttpResponseMessageResult> StartSelectorsIntances(WorkflowInstance wfInstance)
        {
            if ((wfInstance == null) || (wfInstance.WorkflowConfig == null))
                throw new InitialiseWorkflowException("Process: can't find an instance of WorkflowInstance or WorkflowConfig!");

            if (wfInstance.DataSetId == 0)
                throw new InitialiseWorkflowException("Process: no DataSet in Workflow!");

            if (wfInstance.UserSetId == 0)
                throw new InitialiseWorkflowException("Process: no UserSet in Workflow!");

            ICollection<SelectorConfig> lstSelectConf = wfInstance.WorkflowConfig.SelectorConfig;

            HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
            foreach (SelectorConfig selector in lstSelectConf)
            {
                HttpResponseMessageResult resLocal = await SelectorEngine.GenerateSelectorsInstances(selector, wfInstance);
                res.Append(resLocal);
            }

            return res;
        }
    }
}

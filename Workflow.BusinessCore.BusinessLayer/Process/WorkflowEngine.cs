using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.BusinessLayer.Process.Exceptions;
using Workflow.BusinessCore.BusinessLayer.Process.Interfaces;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.BusinessCore.DataLayer.Repositories;
using Workflow.Transverse.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.AspNetCore.Http;
using Workflow.Transverse.Environment;
using Workflow.BusinessCore.BusinessLayer.Helpers;
using Workflow.BusinessCore.BusinessLayer.UnitOfWork.Interfaces;

namespace Workflow.BusinessCore.BusinessLayer.Logic
{
    /// <summary>
    /// WorkflowProcess class.
    /// </summary>
    /// <remarks>
    /// This class permits to start the workflow process.
    /// </remarks>
    public class WorkflowEngine : IWorkflowEngine
    {
        private readonly IServiceProvider _serviceProvider = null;

        private IWorkflowConfigDomain _workflowConfigDomain = null;
        private IWorkflowInstanceDomain _workflowInstanceDomain = null;
        private IUnitOfWork _unitOfWork = null;

        private IWorkflowConfigDomain WorkflowConfigDomain
        {
            get
            {
                if ((_workflowConfigDomain == null) && (_serviceProvider != null))
                    _workflowConfigDomain = _serviceProvider.GetService<IWorkflowConfigDomain>();
                return _workflowConfigDomain;
            }
        }

        private IWorkflowInstanceDomain WorkflowInstanceDomain
        {
            get
            {
                if ((_workflowInstanceDomain == null) && (_serviceProvider != null))
                    _workflowInstanceDomain = _serviceProvider.GetService<IWorkflowInstanceDomain>();
                return _workflowInstanceDomain;
            }
        }

        private IUnitOfWork UnitOfWork
        {
            get
            {
                if ((_unitOfWork == null) && (_serviceProvider != null))
                    _unitOfWork = _serviceProvider.GetService<IUnitOfWork>();
                return _unitOfWork;
            }
        }



        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="serviceProvider">Fournisseur des services</param>
        public WorkflowEngine(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// This method permits to open and start the workflow
        /// </summary>
        /// <param name="workflowConfig">WorkflowConfig Id</param>
        /// <param name="dataSet">DataSet Id</param>
        public async Task<HttpResponseMessageResult> Open(long workflowConfigId, long dataSetId, long idUserSet)
        {
            // Création de la transaction
            using (IDbContextTransaction transaction = UnitOfWork.GetDbContext().Database.BeginTransaction())
            {
                SessionStatsHelper.HttpHitSaveDBTransaction(transaction, _serviceProvider);

                // On récupére les entités
                WorkflowConfig wfConf = await UnitOfWork.GetDbContext().WorkflowConfig
                    .Include(wf => wf.SelectorConfig)
                    .ThenInclude(sc => sc.Criterias)
                    .ThenInclude(c => c.Dimension)
                    .Include(wf => wf.SelectorConfig)
                    .ThenInclude(sc => sc.Modifiers)
                    .ThenInclude(c => c.Dimension)
                    .Include(wf => wf.SelectorConfig)
                    .ThenInclude(sc => sc.Validators)
                    .ThenInclude(c => c.Dimension)
                    .Include(wf => wf.SelectorConfig)
                    .ThenInclude(sc => sc.ModifyCriterias)
                    .ThenInclude(c => c.Dimension)
                    .Include(wf => wf.WorkflowDimension)
                    .ThenInclude(wd => wd.Dimension)
                    .Where(wf => wf.Id == workflowConfigId)
                    .AsNoTracking()
                    .ToAsyncEnumerable()
                    .FirstOrDefault();
                DataSet dtSet = await UnitOfWork.GetDbContext().DataSet
                    .Include(d => d.DataSetDimensions)
                    .Where(d => d.Id == dataSetId)
                    .AsNoTracking()
                    .ToAsyncEnumerable()
                    .FirstOrDefault();
                UserSet usrSet = await UnitOfWork.GetDbContext().UserSet
                    .Where(u => u.Id == idUserSet)
                    .AsNoTracking()
                    .ToAsyncEnumerable()
                    .FirstOrDefault();

                if ((wfConf == null) || (dtSet == null) || (usrSet == null))
                    return new HttpResponseMessageResult() { IsSuccess = false, Message = $"Can't get WorkflowConfig with id {workflowConfigId}, DataSet with id {dataSetId}, UserSet with id {idUserSet}" };

                if (!CheckDimensions(wfConf, dtSet, usrSet))
                    throw new SequenceException("Process.Open: WorkflowDimensions are not equals to DataSetDimensions.");

                // Create the workflow instance
                WorkflowInstance workflowInstance = await WorkflowInstanceDomain.CreateInstance(wfConf, dtSet, usrSet);
                // Générer les SelectorInstance.
                HttpResponseMessageResult res = await WorkflowInstanceDomain.StartSelectorsIntances(workflowInstance);

                transaction.Commit();
                return res;
            }
        }



        /// <summary>
        /// Vérifie que les dimensions sont concordantes entre le WorkflowConfig, le DataSet et le Userset donnés en paramètre.
        /// </summary>
        /// <param name="wfConf">WorkflowConfig à tester</param>
        /// <param name="dtSet">DataSet à associer</param>
        /// <param name="usrSet">UserSet à associer</param>
        /// <returns>True : les dimensions concordent entre les objets, False : certains objets ont des discordances...</returns>
        private bool CheckDimensions(WorkflowConfig wfConf, DataSet dtSet, UserSet usrSet)
        {
            bool res = wfConf.WorkflowDimension.All(o => dtSet.DataSetDimensions.Any(w => w.Dimension == o.Dimension));

            return true;
        }

    }
}

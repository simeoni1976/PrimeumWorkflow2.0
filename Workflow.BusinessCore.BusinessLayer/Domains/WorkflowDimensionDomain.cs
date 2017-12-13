using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Common;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.BusinessLayer.Process.Exceptions;
using Workflow.BusinessCore.DataLayer.Entities;
using Microsoft.Extensions.DependencyInjection;
using Workflow.Transverse.Helpers;
using Microsoft.EntityFrameworkCore;
using Workflow.BusinessCore.BusinessLayer.UnitOfWork.Interfaces;

namespace Workflow.BusinessCore.BusinessLayer.Domains
{
    /// <summary>
    /// WorkflowDimension domain class
    /// </summary>
    public class WorkflowDimensionDomain : AbstractDomain<WorkflowDimension>, IWorkflowDimensionDomain
    {
        private readonly IServiceProvider _serviceProvider;

        private IUnitOfWork UnitOfWork
        {
            get
            {
                return _serviceProvider?.GetService<IUnitOfWork>();
            }
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="unitOfWork">Unit Of Work</param>
        /// <param name="mapper">Mapper</param>
        public WorkflowDimensionDomain(IUnitOfWork unitOfWork, IServiceProvider serviceProvider) : base(unitOfWork.WorkflowDimensionRepository)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// This method permits to get an element in the base by a column name.
        /// </summary>
        /// <param name="columnName">Column name</param>
        /// <returns></returns>
        public async Task<WorkflowDimension> Get(string columnName)
        {
            IEnumerable<WorkflowDimension> workflowDimensions = await Get();
            return workflowDimensions.FirstOrDefault(s => s.ColumnName == columnName);
        }

        /// <summary>
        /// Duplique un WorkflowDimension pour l'instance d'un WorkflowConfig.
        /// </summary>
        /// <param name="workflowDimension">WorkflowDimension original</param>
        /// <returns>Duplicat du WorkflowDimension original</returns>
        public async Task<WorkflowDimension> CopyForStatic(WorkflowDimension workflowDimension)
        {
            if (workflowDimension == null)
                throw new WrongParameterException("WorkflowDimensionDomain.CopyForStatic: WorkflowDimension source is null!");

            WorkflowDimension duplicat = new WorkflowDimension();
            UnitOfWork.WorkflowDimensionRepository.PrepareAddForObject(duplicat);
            duplicat.ColumnName = workflowDimension.ColumnName;
            List<Dimension> lstDim = await UnitOfWork.GetDbContext().Dimension
                .Where(d => d.Id == workflowDimension.Dimension.Id)
                .ToAsyncEnumerable()
                .ToList();
            Dimension refDim = lstDim.FirstOrDefault();

            duplicat.Dimension = refDim;
            workflowDimension.Dimension.WorkflowDimension.Add(duplicat);

            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            return duplicat;
        }

        /// <summary>
        /// Ajoute un WorkflowDimension à un WorkflowConfig
        /// </summary>
        /// <param name="workflowConfigId">Id du WorkflowConfig cible</param>
        /// <param name="workflowDimension">WorkflowDimension</param>
        /// <returns>Message de retour</returns>
        public async Task<WorkflowDimension> AddWorkflowDimension(long workflowConfigId, WorkflowDimension workflowDimension)
        {
            if (workflowDimension == null)
                throw new WrongParameterException("WorkflowDimensionDomain.AddWorkflowDimension: WorkflowDimension is null!");
            if (workflowDimension.Dimension == null)
                throw new WrongParameterException("WorkflowDimensionDomain.AddWorkflowDimension: Dimension of WorkflowDimension is null!");
            int cnt = await UnitOfWork.GetDbContext().Dimension
                .Where(d => d.Id == workflowDimension.Dimension.Id)
                .AsNoTracking()
                .ToAsyncEnumerable()
                .Count();
            if (cnt < 1)
                throw new WrongParameterException($"WorkflowDimensionDomain.AddWorkflowDimension: No Dimension (id = {workflowDimension.Dimension.Id}) for WorkflowDimension!");

            WorkflowConfig wfConf = await UnitOfWork.GetDbContext().WorkflowConfig
                .Where(wfc => wfc.Id == workflowConfigId)
                .FirstOrDefaultAsync();
            if (wfConf == null)
                throw new WrongParameterException($"WorkflowDimensionDomain.AddWorkflowDimension: WorkflowConfig unknown for this id ({workflowConfigId})!");

            UnitOfWork.WorkflowConfigRepository.PrepareUpdateForObject(wfConf);
            UnitOfWork.WorkflowDimensionRepository.PrepareAddForObject(workflowDimension);

            wfConf.WorkflowDimension.Add(workflowDimension);
            workflowDimension.WorkflowConfig = wfConf;

            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            if (nbr <= 0)
                throw new DatabaseException("WorkflowDimensionDomain.AddWorkflowDimension: impossible to save WorkflowDimension.");

            return workflowDimension;
        }


    }
}

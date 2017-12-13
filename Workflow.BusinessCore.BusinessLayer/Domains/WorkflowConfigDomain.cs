using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Common;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.BusinessCore.BusinessLayer.UnitOfWork.Interfaces;
using Workflow.Transverse.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Workflow.BusinessCore.BusinessLayer.Process.Exceptions;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Workflow.Transverse.Environment;

namespace Workflow.BusinessCore.BusinessLayer.Domains
{
    /// <summary>
    /// WorkflowConfig domain class.
    /// </summary>
    public class WorkflowConfigDomain : AbstractDomain<WorkflowConfig>, IWorkflowConfigDomain
    {
        private readonly IServiceProvider _serviceProvider = null;

        private Regex _regexSubstringName = null;

        private IUnitOfWork UnitOfWork
        {
            get
            {
                return _serviceProvider?.GetService<IUnitOfWork>();
            }
        }

        private IWorkflowInstanceDomain WorkflowInstanceDomain
        {
            get
            {
                return _serviceProvider?.GetService<IWorkflowInstanceDomain>();
            }
        }

        private ISelectorConfigDomain SelectorConfigDomain
        {
            get
            {
                return _serviceProvider?.GetService<ISelectorConfigDomain>();
            }
        }

        private IWorkflowDimensionDomain WorkflowDimensionDomain
        {
            get
            {
                return _serviceProvider?.GetService<IWorkflowDimensionDomain>();
            }
        }

        private IGridConfigurationDomain GridConfigurationDomain
        {
            get
            {
                return _serviceProvider?.GetService<IGridConfigurationDomain>();
            }
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="unitOfWork">Unit Of Work</param>
        /// <param name="mapper">Mapper</param>
        public WorkflowConfigDomain(IUnitOfWork unitOfWork, IServiceProvider serviceProvider) : base(unitOfWork.WorkflowConfigRepository)
        {
            _serviceProvider = serviceProvider;
            _regexSubstringName = new Regex(Constant.SUBSTRING_NAME_WORKFLOW_CONFIG);
        }

        /// <summary>
        /// This method permits to get an element in the base.
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>WorkflowConfig</returns>
        public async Task<WorkflowConfig> Get(string name)
        {
            IEnumerable<WorkflowConfig> workflowConfigs = await Get();
            return workflowConfigs.First(s => s.Name == name);
        }


        /// <summary>
        /// Ajout d'un WorkflowConfig.
        /// </summary>
        /// <param name="entity">WorkflowConfig à ajouter</param>
        /// <returns>L'entité WorkflowConfig inserée (null en cas de problème)</returns>
        public override async Task<WorkflowConfig> Add(WorkflowConfig entity)
        {
            // On vérifie s'il n'y a pas d'autre WorkflowConfig avec le même nom.
            bool isAlreadyExist = await NameExisting(entity.Name);

            if (isAlreadyExist)
                return null;

            return await UnitOfWork.WorkflowConfigRepository.Insert(entity);
        }


        /// <summary>
        /// Détermine si le nom d'un workflowConfig existe déjà ou non.
        /// </summary>
        /// <param name="nameWorkflowConfig">Nom à tester</param>
        /// <returns>True : il existe déjà. False : il n'existe pas.</returns>
        public async Task<bool> NameExisting(string nameWorkflowConfig)
        {
            IEnumerable<WorkflowConfig> lst = await UnitOfWork.WorkflowConfigRepository.Find(x => x.Name == nameWorkflowConfig && !x.IdWorkflowConfigOriginal.HasValue);

            return (lst != null && lst.Count() > 0);
        }

        /// <summary>
        /// Duplique un WorkflowConfig, en laissant dans la copie l'id du WorkflowConfig d'origine.
        /// </summary>
        /// <param name="workflowConfig">WorkflowConfig à copier</param>
        /// <returns>WorkflowConfig dupliqué</returns>
        public async Task<WorkflowConfig> CopyForStatic(WorkflowConfig workflowConfig)
        {
            if (workflowConfig == null)
                throw new WrongParameterException("WorkflowConfigDomain.CopyForStatic: WorkflowConfig source is null!");
            if (workflowConfig.IdWorkflowConfigOriginal.HasValue)
                throw new WrongParameterException("WorkflowConfigDomain.CopyForStatic: WorkflowConfig source isn't orignal!");

            List<WorkflowConfig> lstSearch = await UnitOfWork.GetDbContext().WorkflowConfig
                .Where(wf => wf.Name.Contains(workflowConfig.Name) && wf.IdWorkflowConfigOriginal.HasValue)
                .AsNoTracking()
                .ToAsyncEnumerable()
                .ToList();

            int lastNumber = 1;
            List<int> allNumbers = new List<int>();
            foreach (WorkflowConfig wf in lstSearch)
            {
                Match m = _regexSubstringName.Match(wf.Name);
                if (m.Success)
                {
                    string name = m.Groups["name"].Value;
                    if (name == workflowConfig.Name)
                    {
                        if (Int32.TryParse(m.Groups["number"].Value, out int number))
                            allNumbers.Add(number);
                    }
                }
            }

            if (allNumbers.Count > 0)
                lastNumber = allNumbers.Max() + 1;

            WorkflowConfig duplicat = new WorkflowConfig();
            UnitOfWork.WorkflowConfigRepository.PrepareAddForObject(duplicat);

            duplicat.Name = string.Format(Constant.POSTFIX_NAME_DUPLICATE_WORKFLOW_CONFIG, workflowConfig.Name, lastNumber);
            duplicat.IdWorkflowConfigOriginal = workflowConfig.Id;
            duplicat.ActionSequenceRef = workflowConfig.ActionSequenceRef;
            duplicat.ConstraintSequenceRef = workflowConfig.ConstraintSequenceRef;
            foreach (SelectorConfig sc in workflowConfig.SelectorConfig)
            {
                SelectorConfig scNew = await SelectorConfigDomain.CopyForStatic(sc);
                scNew.WorkflowConfig = duplicat;
                duplicat.SelectorConfig.Add(scNew);
            }
            foreach (WorkflowDimension wd in workflowConfig.WorkflowDimension)
            {
                WorkflowDimension wfNew = await WorkflowDimensionDomain.CopyForStatic(wd);
                wfNew.WorkflowConfig = duplicat;
                duplicat.WorkflowDimension.Add(wfNew);
            }
            List<GridConfig> lstGridConf = await UnitOfWork.GetDbContext().GridConfig
                .Where(gc => gc.WorkflowConfig.Id == workflowConfig.Id)
                .Include(gc => gc.ColumnDimensions)
                .ThenInclude(gd => gd.Values)
                .Include(gc => gc.RowDimensions)
                .ThenInclude(gd => gd.Values)
                .Include(gc => gc.FixedDimensions)
                .ThenInclude(gd => gd.Values)
                .AsNoTracking()
                .ToAsyncEnumerable()
                .ToList();

            GridConfig gridConf = lstGridConf.FirstOrDefault();
            if (gridConf != null)
            {
                GridConfig gridConfNew = await GridConfigurationDomain.CopyForStatic(gridConf);
                gridConfNew.WorkflowConfig = duplicat;
            }

            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            return duplicat;
        }


    }
}
using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using DTO = Workflow.Transverse.DTO;
using ENT = Workflow.BusinessCore.DataLayer.Entities;
using System;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.ServiceLayer.Adapters
{
    /// <summary>
    ///  WorkflowConfigAdapter interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the Workflow adapter.
    /// </remarks>
    public class WorkflowConfigAdapter : IWorkflowConfigAdapter
    {
        private readonly IServiceProvider _serviceProvider;

        private IWorkflowConfigDomain WorkflowConfigDomain
        {
            get
            {
                return _serviceProvider?.GetService<IWorkflowConfigDomain>();
            }
        }

        private IWorkflowDimensionDomain WorkflowDimensionDomain
        {
            get
            {
                return _serviceProvider?.GetService<IWorkflowDimensionDomain>();
            }
        }

        private IMapper Mapper
        {
            get
            {
                return _serviceProvider?.GetService<IMapper>();
            }
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de services</param>
        public WorkflowConfigAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// This function permits to delete by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns>Task</returns>
        public async Task Delete(long id)
        {
            await WorkflowConfigDomain.Delete(id);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="workflowConfig"></param>
        /// <returns></returns>
        public async Task<DTO.WorkflowConfig> Post(DTO.WorkflowConfig workflowConfig)
        {
            ENT.WorkflowConfig element = Mapper.Map<DTO.WorkflowConfig, ENT.WorkflowConfig>(workflowConfig);

            return Mapper.Map<ENT.WorkflowConfig, DTO.WorkflowConfig>(await WorkflowConfigDomain.Add(element));
        }

        /// <summary>
        /// This function permits to update a config
        /// </summary>
        /// <param name="workflowConfig"></param>
        /// <returns>WorkflowConfig</returns>
        public async Task<DTO.WorkflowConfig> Put(DTO.WorkflowConfig workflowConfig)
        {
            return Mapper.Map<ENT.WorkflowConfig, DTO.WorkflowConfig>(
                await WorkflowConfigDomain.Update(Mapper.Map<DTO.WorkflowConfig, ENT.WorkflowConfig>(workflowConfig)));
        }

        /// <summary>
        /// Récupére toutes les entités DTO existantes.
        /// </summary>
        /// <returns>Message de retour avec la liste en json</returns>
        public async Task<IEnumerable<DTO.WorkflowConfig>> GetAll()
        {
            IEnumerable<ENT.WorkflowConfig> workflowConfig = await WorkflowConfigDomain.Get();

            IEnumerable<DTO.WorkflowConfig> dtoWorkflowConfig = null;
            if (workflowConfig != null)
                dtoWorkflowConfig = Mapper.Map<IEnumerable<ENT.WorkflowConfig>, IEnumerable<DTO.WorkflowConfig>>(workflowConfig);
            else
                dtoWorkflowConfig = new List<DTO.WorkflowConfig>();

            return dtoWorkflowConfig;
        }

        /// <summary>
        /// Récupére l'entité désignée par l'id en paramétre.
        /// </summary>
        /// <param name="id">Id de l'entité</param>
        /// <returns>Message de retour avec l'entité</returns>
        public async Task<DTO.WorkflowConfig> GetById(long id)
        {
            ENT.WorkflowConfig workflowConfig = await WorkflowConfigDomain.Get(id);

            DTO.WorkflowConfig dtoWorkflowConfig = null;
            if (workflowConfig != null)
                dtoWorkflowConfig = Mapper.Map<ENT.WorkflowConfig, DTO.WorkflowConfig>(workflowConfig);

            return dtoWorkflowConfig;
        }

        /// <summary>
        /// Ajoute un WorkflowDimension à un WorkflowConfig
        /// </summary>
        /// <param name="workflowConfigId">Id du WorkflowConfig cible</param>
        /// <param name="workflowDimension">WorkflowDimension</param>
        /// <returns>Message de retour</returns>
        public async Task<DTO.WorkflowDimension> AddWorkflowDimension(long workflowConfigId, DTO.WorkflowDimension workflowDimension)
        {
            ENT.WorkflowDimension entWorkflowDimension = Mapper.Map<DTO.WorkflowDimension, ENT.WorkflowDimension>(workflowDimension);

            return Mapper.Map<ENT.WorkflowDimension, DTO.WorkflowDimension>(await WorkflowDimensionDomain.AddWorkflowDimension(workflowConfigId, entWorkflowDimension));
        }

    }
}

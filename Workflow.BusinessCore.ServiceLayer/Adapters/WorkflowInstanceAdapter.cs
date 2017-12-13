using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using DTO = Workflow.Transverse.DTO;
using ENT = Workflow.BusinessCore.DataLayer.Entities;
using System;
using Workflow.BusinessCore.ServiceLayer.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.ServiceLayer.Adapters
{
    /// <summary>
    ///  WorkflowInstanceAdapter interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the Workflow instance adapter.
    /// </remarks>
    public class WorkflowInstanceAdapter : IWorkflowInstanceAdapter
    {
        private readonly IServiceProvider _serviceProvider = null;

        protected IWorkflowInstanceDomain WorkflowInstanceDomain
        {
            get
            {
                return _serviceProvider?.GetService<IWorkflowInstanceDomain>();
            }
        }

        protected IMapper Mapper
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
        public WorkflowInstanceAdapter(IServiceProvider serviceProvider)
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
            await WorkflowInstanceDomain.Delete(id);
        }

        /// <summary>
        /// This function permits to put an instance
        /// </summary>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public async Task<DTO.WorkflowInstance> Post(DTO.WorkflowInstance dto)
        {
            ENT.WorkflowInstance element = Mapper.Map<DTO.WorkflowInstance, ENT.WorkflowInstance>(dto);

            return Mapper.Map<ENT.WorkflowInstance, DTO.WorkflowInstance>(await WorkflowInstanceDomain.Add(element));
        }

        /// <summary>
        /// This function permits to update an instance
        /// </summary>
        /// <param name="workflowConfig"></param>
        /// <returns>WorkflowConfig</returns>
        public async Task<DTO.WorkflowInstance> Put(long id, DTO.WorkflowInstance dto)
        {
            DTO.WorkflowInstance element = Mapper.Map<ENT.WorkflowInstance, DTO.WorkflowInstance>(await WorkflowInstanceDomain.Get(id));
            if (element.Id != 0)
            {
                dto.Id = element.Id;
                return Mapper.Map<ENT.WorkflowInstance, DTO.WorkflowInstance>(
                    await WorkflowInstanceDomain.Add(Mapper.Map<DTO.WorkflowInstance, ENT.WorkflowInstance>(dto)));
            }
            else
                return new DTO.WorkflowInstance();
        }

        /// <summary>
        /// Récupére toutes les entités DTO existantes.
        /// </summary>
        /// <returns>Message de retour avec la liste en json</returns>
        public async Task<IEnumerable<DTO.WorkflowInstance>> GetAll()
        {
            IEnumerable<ENT.WorkflowInstance> workflowInstance = await WorkflowInstanceDomain.Get();

            IEnumerable<DTO.WorkflowInstance> dtoWorkflowInstance = null;
            if (workflowInstance != null)
                dtoWorkflowInstance = Mapper.Map<IEnumerable<ENT.WorkflowInstance>, IEnumerable<DTO.WorkflowInstance>>(workflowInstance);
            else
                dtoWorkflowInstance = new List<DTO.WorkflowInstance>();

            return dtoWorkflowInstance;
        }

        /// <summary>
        /// Récupére l'entité désignée par l'id en paramétre.
        /// </summary>
        /// <param name="id">Id de l'entité</param>
        /// <returns>Message de retour avec l'entité</returns>
        public async Task<DTO.WorkflowInstance> GetById(long id)
        {
            ENT.WorkflowInstance workflowInstance = await WorkflowInstanceDomain.Get(id);

            DTO.WorkflowInstance dtoWorkflowInstance = null;
            if (workflowInstance != null)
                dtoWorkflowInstance = Mapper.Map<ENT.WorkflowInstance, DTO.WorkflowInstance>(workflowInstance);

            return dtoWorkflowInstance;
        }
    }
}

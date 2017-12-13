using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using DTO = Workflow.Transverse.DTO;
using ENT = Workflow.BusinessCore.DataLayer.Entities;
using Workflow.Transverse.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Workflow.BusinessCore.ServiceLayer.Adapters
{
    public class ActionAdapter : IActionAdapter
    {
        private readonly IServiceProvider _serviceProvider = null;

        private IActionSequenceDomain ActionSequenceDomain
        {
            get
            {
                return _serviceProvider?.GetService<IActionSequenceDomain>();
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
        /// Constructeur pour l'ID
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de services</param>
        public ActionAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        /// <summary>
        /// Ajoute une action en base, indépendamment d'un workflow.
        /// </summary>
        /// <param name="action">Nouvelle action</param>
        /// <returns>Action enregistrée (avec son id)</returns>
        public async Task<DTO.Action> AddAction(DTO.Action action)
        {
            ENT.Action entAction = Mapper.Map<DTO.Action, ENT.Action>(action);

            return Mapper.Map<ENT.Action, DTO.Action>(await ActionSequenceDomain.AddAction(entAction));
        }

        /// <summary>
        /// Récupére toutes les entités DTO existantes.
        /// </summary>
        /// <returns>Message de retour avec la liste en json</returns>
        public async Task<IEnumerable<DTO.Action>> GetAll()
        {
            IEnumerable<ENT.Action> actions = await ActionSequenceDomain.GetAllAction();

            IEnumerable<DTO.Action> dtoActions = null;
            if (actions != null)
                dtoActions = Mapper.Map<IEnumerable<ENT.Action>, IEnumerable<DTO.Action>>(actions);
            else
                dtoActions = new List<DTO.Action>();

            return dtoActions;
        }

        /// <summary>
        /// Récupére l'entité désignée par l'id en paramétre.
        /// </summary>
        /// <param name="id">Id de l'entité</param>
        /// <returns>Message de retour avec l'entité</returns>
        public async Task<DTO.Action> GetById(long id)
        {
            ENT.Action action = await ActionSequenceDomain.GetAction(id);

            DTO.Action dtoAction = null;
            if (action != null)
                dtoAction = Mapper.Map<ENT.Action, DTO.Action>(action);

            return dtoAction;
        }
    }
}

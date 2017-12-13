using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using DTO = Workflow.Transverse.DTO;
using ENT = Workflow.BusinessCore.DataLayer.Entities;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Workflow.Transverse.Helpers;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using AutoMapper;

namespace Workflow.BusinessCore.ServiceLayer.Adapters
{
    public class ActionParameterAdapter : IActionParameterAdapter
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
        public ActionParameterAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Ajoute un paramètre dans l'action d'une séquence d'action.
        /// </summary>
        /// <param name="actionParameter">ActionParameter à ajouter</param>
        /// <param name="actionSequenceId">Id de la séquence d'action cible</param>
        /// <returns>Message de retour</returns>
        public async Task<DTO.ActionParameter> AddActionParameter(DTO.ActionParameter actionParameter, long actionSequenceId)
        {
            ENT.ActionParameter entActionParameter = Mapper.Map<DTO.ActionParameter, ENT.ActionParameter>(actionParameter);

            return Mapper.Map<ENT.ActionParameter, DTO.ActionParameter>(await ActionSequenceDomain.AddActionParameter(entActionParameter, actionSequenceId));
        }

        /// <summary>
        /// Récupére toutes les entités DTO existantes.
        /// </summary>
        /// <returns>Message de retour avec la liste en json</returns>
        public async Task<IEnumerable<DTO.ActionParameter>> GetAll()
        {
            IEnumerable<ENT.ActionParameter> actionParameters = await ActionSequenceDomain.GetAllActionParameter();

            IEnumerable<DTO.ActionParameter> dtoActionParameters = null;
            if (actionParameters != null)
                dtoActionParameters = Mapper.Map<IEnumerable<ENT.ActionParameter>, IEnumerable<DTO.ActionParameter>>(actionParameters);
            else
                dtoActionParameters = new List<DTO.ActionParameter>();

            return dtoActionParameters;
        }

        /// <summary>
        /// Récupére l'entité désignée par l'id en paramétre.
        /// </summary>
        /// <param name="id">Id de l'entité</param>
        /// <returns>Message de retour avec l'entité</returns>
        public async Task<DTO.ActionParameter> GetById(long id)
        {
            ENT.ActionParameter actionParameter = await ActionSequenceDomain.GetActionParameter(id);

            DTO.ActionParameter dtoActionParameter = null;
            if (actionParameter != null)
                dtoActionParameter = Mapper.Map<ENT.ActionParameter, DTO.ActionParameter>(actionParameter);

            return dtoActionParameter;
        }
    }
}

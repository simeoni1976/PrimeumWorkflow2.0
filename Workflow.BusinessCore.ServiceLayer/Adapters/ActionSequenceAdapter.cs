using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using DTO = Workflow.Transverse.DTO;
using ENT = Workflow.BusinessCore.DataLayer.Entities;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.ServiceLayer.Adapters
{
    public class ActionSequenceAdapter : IActionSequenceAdapter
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
        public ActionSequenceAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Ajoute une nouvelle action dans une séquence (existante ou non)
        /// </summary>
        /// <param name="actionSequence">Nouvelle ActionSequence</param>
        /// <param name="actionId">Id de l'Action à exécuter</param>
        /// <returns>Message de retour</returns>
        public async Task<DTO.ActionSequence> AddActionSequence(DTO.ActionSequence actionSequence, long actionId)
        {
            ENT.ActionSequence entActionSequence = Mapper.Map<DTO.ActionSequence, ENT.ActionSequence>(actionSequence);

            return Mapper.Map<ENT.ActionSequence, DTO.ActionSequence>(await ActionSequenceDomain.AddActionSequence(entActionSequence, actionId));
        }

        /// <summary>
        /// Récupére toutes les entités DTO existantes.
        /// </summary>
        /// <returns>Message de retour avec la liste en json</returns>
        public async Task<IEnumerable<DTO.ActionSequence>> GetAll()
        {
            IEnumerable<ENT.ActionSequence> actionSequences = await ActionSequenceDomain.GetAllActionSequence();

            IEnumerable<DTO.ActionSequence> dtoActionSequences = null;
            if (actionSequences != null)
                dtoActionSequences = Mapper.Map<IEnumerable<ENT.ActionSequence>, IEnumerable<DTO.ActionSequence>>(actionSequences);
            else
                dtoActionSequences = new List<DTO.ActionSequence>();

            return dtoActionSequences;
        }

        /// <summary>
        /// Récupére l'entité désignée par l'id en paramétre.
        /// </summary>
        /// <param name="id">Id de l'entité</param>
        /// <returns>Message de retour avec l'entité</returns>
        public async Task<DTO.ActionSequence> GetById(long id)
        {
            ENT.ActionSequence actionSequence = await ActionSequenceDomain.GetActionSequence(id);

            DTO.ActionSequence dtoActionSequence = null;
            if (actionSequence != null)
                dtoActionSequence = Mapper.Map<ENT.ActionSequence, DTO.ActionSequence>(actionSequence);

            return dtoActionSequence;
        }
    }
}

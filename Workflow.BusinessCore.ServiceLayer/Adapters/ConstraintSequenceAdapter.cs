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
using AutoMapper;

namespace Workflow.BusinessCore.ServiceLayer.Adapters
{
    public class ConstraintSequenceAdapter : IConstraintSequenceAdapter
    {
        private readonly IServiceProvider _serviceProvider = null;

        private IConstraintSequenceDomain ConstraintSequenceDomain
        {
            get
            {
                return _serviceProvider?.GetService<IConstraintSequenceDomain>();
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
        public ConstraintSequenceAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Ajoute une nouvelle séquence de contrainte.
        /// </summary>
        /// <param name="constraintSequence">Nouvelle SequenceConstraint</param>
        /// <param name="constraintId">Id de la contrainte</param>
        /// <returns>Message de retour</returns>
        public async Task<DTO.ConstraintSequence> AddConstraintSequence(DTO.ConstraintSequence constraintSequence, long constraintId)
        {
            ENT.ConstraintSequence entConstraintSequence = Mapper.Map<DTO.ConstraintSequence, ENT.ConstraintSequence>(constraintSequence);

            return Mapper.Map<ENT.ConstraintSequence, DTO.ConstraintSequence>(await ConstraintSequenceDomain.AddConstraintSequence(entConstraintSequence, constraintId));
        }

        /// <summary>
        /// Récupére toutes les entités DTO existantes.
        /// </summary>
        /// <returns>Message de retour avec la liste en json</returns>
        public async Task<IEnumerable<DTO.ConstraintSequence>> GetAll()
        {
            IEnumerable<ENT.ConstraintSequence> constraintSequences = await ConstraintSequenceDomain.GetAllConstraintSequence();

            IEnumerable<DTO.ConstraintSequence> dtoConstraintSequences = null;
            if (constraintSequences != null)
                dtoConstraintSequences = Mapper.Map<IEnumerable<ENT.ConstraintSequence>, IEnumerable<DTO.ConstraintSequence>>(constraintSequences);
            else
                dtoConstraintSequences = new List<DTO.ConstraintSequence>();

            return dtoConstraintSequences;
        }

        /// <summary>
        /// Récupére l'entité désignée par l'id en paramétre.
        /// </summary>
        /// <param name="id">Id de l'entité</param>
        /// <returns>Message de retour avec l'entité</returns>
        public async Task<DTO.ConstraintSequence> GetById(long id)
        {
            ENT.ConstraintSequence constraintSequence = await ConstraintSequenceDomain.GetConstraintSequence(id);

            DTO.ConstraintSequence dtoConstraintSequence = null;
            if (constraintSequence != null)
                dtoConstraintSequence = Mapper.Map<ENT.ConstraintSequence, DTO.ConstraintSequence>(constraintSequence);

            return dtoConstraintSequence;
        }
    }
}

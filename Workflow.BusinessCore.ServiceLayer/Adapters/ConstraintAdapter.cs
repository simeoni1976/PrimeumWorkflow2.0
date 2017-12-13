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
    public class ConstraintAdapter : IConstraintAdapter
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
        public ConstraintAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Ajoute une nouvelle contrainte (indépente des Wrokflow)
        /// </summary>
        /// <param name="constraint">Nouvelle contrainte</param>
        /// <returns>Message de retour</returns>
        public async Task<DTO.Constraint> AddConstraint(DTO.Constraint constraint)
        {
            ENT.Constraint entConstraint = Mapper.Map<DTO.Constraint, ENT.Constraint>(constraint);

            return Mapper.Map<ENT.Constraint, DTO.Constraint>(await ConstraintSequenceDomain.AddConstraint(entConstraint));
        }

        /// <summary>
        /// Récupére toutes les entités DTO existantes.
        /// </summary>
        /// <returns>Message de retour avec la liste en json</returns>
        public async Task<IEnumerable<DTO.Constraint>> GetAll()
        {
            IEnumerable<ENT.Constraint> constraints = await ConstraintSequenceDomain.GetAllConstraint();

            IEnumerable<DTO.Constraint> dtoConstraints = null;
            if (constraints != null)
                dtoConstraints = Mapper.Map<IEnumerable<ENT.Constraint>, IEnumerable<DTO.Constraint>>(constraints);
            else
                dtoConstraints = new List<DTO.Constraint>();

            return dtoConstraints;
        }

        /// <summary>
        /// Récupére l'entité désignée par l'id en paramétre.
        /// </summary>
        /// <param name="id">Id de l'entité</param>
        /// <returns>Message de retour avec l'entité</returns>
        public async Task<DTO.Constraint> GetById(long id)
        {
            ENT.Constraint constraint = await ConstraintSequenceDomain.GetConstraint(id);

            DTO.Constraint dtoConstraint = null;
            if (constraint != null)
                dtoConstraint = Mapper.Map<ENT.Constraint, DTO.Constraint>(constraint);

            return dtoConstraint;
        }

    }
}

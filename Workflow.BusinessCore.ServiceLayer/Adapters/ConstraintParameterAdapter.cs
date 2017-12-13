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
    public class ConstraintParameterAdapter : IConstraintParameterAdapter
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
        public ConstraintParameterAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Ajoute un nouveau ConstraintParameter
        /// </summary>
        /// <param name="constraintParameter">Nouveau ConstraintParameter</param>
        /// <param name="constraintSequenceId">Id de la ConstraintSequence</param>
        /// <returns>Message de retour</returns>
        public async Task<DTO.ConstraintParameter> AddConstraintParameter(DTO.ConstraintParameter constraintParameter, long constraintSequenceId)
        {
            ENT.ConstraintParameter entConstraintParameter = Mapper.Map<DTO.ConstraintParameter, ENT.ConstraintParameter>(constraintParameter);

            return Mapper.Map<ENT.ConstraintParameter, DTO.ConstraintParameter>(await ConstraintSequenceDomain.AddConstraintParameter(entConstraintParameter, constraintSequenceId));
        }

        /// <summary>
        /// Récupére toutes les entités DTO existantes.
        /// </summary>
        /// <returns>Message de retour avec la liste en json</returns>
        public async Task<IEnumerable<DTO.ConstraintParameter>> GetAll()
        {
            IEnumerable<ENT.ConstraintParameter> constraintParameter = await ConstraintSequenceDomain.GetAllConstraintParameter();

            IEnumerable<DTO.ConstraintParameter> dtoConstraintParameters = null;
            if (constraintParameter != null)
                dtoConstraintParameters = Mapper.Map<IEnumerable<ENT.ConstraintParameter>, IEnumerable<DTO.ConstraintParameter>>(constraintParameter);
            else
                dtoConstraintParameters = new List<DTO.ConstraintParameter>();

            return dtoConstraintParameters;
        }

        /// <summary>
        /// Récupére l'entité désignée par l'id en paramétre.
        /// </summary>
        /// <param name="id">Id de l'entité</param>
        /// <returns>Message de retour avec l'entité</returns>
        public async Task<DTO.ConstraintParameter> GetById(long id)
        {
            ENT.ConstraintParameter constraintParameter = await ConstraintSequenceDomain.GetConstraintParameter(id);

            DTO.ConstraintParameter dtoConstraintParameter = null;
            if (constraintParameter != null)
                dtoConstraintParameter = Mapper.Map<ENT.ConstraintParameter, DTO.ConstraintParameter>(constraintParameter);

            return dtoConstraintParameter;
        }
    }
}

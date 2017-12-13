using AutoMapper;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.BusinessLayer.Process.Exceptions;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using Workflow.Transverse.Helpers;
using DTO = Workflow.Transverse.DTO;
using ENT = Workflow.BusinessCore.DataLayer.Entities;
using Microsoft.Extensions.DependencyInjection;

namespace Workflow.BusinessCore.ServiceLayer.Adapters
{
    /// <summary>
    ///  CriteriaAdapter interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the Criteria adapter.
    /// </remarks>
    /// <typeparam name="T">Value DTO model</typeparam>
    public class CriteriaAdapter : ICriteriaAdapter
    {
        private readonly IServiceProvider _serviceProvider = null;

        private ICriteriaDomain CriteriaDomain
        {
            get
            {
                return _serviceProvider?.GetService<ICriteriaDomain>();
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
        /// Constructeur
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de services</param>
        public CriteriaAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Ajoute un criteria dans un SelectorConfig.
        /// </summary>
        /// <param name="criteria">Criteria à ajouter</param>
        /// <returns>Message de retour.</returns>
        /// <remarks>L'objet Criteria doit contenir l'id du SelectorConfig dans lequel il faut l'ajouter. L'API retourne une erreur
        /// lorsque la dimension ou la valeur du Criteria n'est pas définie. 
        /// Les valeurs possibles d'un Criteria sont '*', chaine-de-caractères, '{valeur1, valeur2, ..., valeurn}' </remarks>
        public async Task<DTO.Criteria> Add(DTO.Criteria criteria)
        {
            if (criteria == null)
                throw new WrongParameterException("Criteria object is null!");

            ENT.Criteria criterEntity = Mapper.Map<DTO.Criteria, ENT.Criteria>(criteria);
            ENT.Criteria addedCriteria = await CriteriaDomain.Add(criterEntity);
            return Mapper.Map<ENT.Criteria, DTO.Criteria>(addedCriteria);
        }


        /// <summary>
        /// Récupére toutes les entités DTO existantes.
        /// </summary>
        /// <returns>Message de retour avec la liste en json</returns>
        public async Task<IEnumerable<DTO.Criteria>> GetAll()
        {
            IEnumerable<ENT.Criteria> criterias = await CriteriaDomain.Get();


            IEnumerable<DTO.Criteria> dtoCriterias = null;
            if (criterias != null)
                dtoCriterias = Mapper.Map<IEnumerable<ENT.Criteria>, IEnumerable<DTO.Criteria>>(criterias);
            else
                dtoCriterias = new List<DTO.Criteria>();

            return dtoCriterias;
        }

        /// <summary>
        /// Récupére l'entité désignée par l'id en paramétre.
        /// </summary>
        /// <param name="id">Id de l'entité</param>
        /// <returns>Message de retour avec l'entité</returns>
        public async Task<DTO.Criteria> GetById(long id)
        {
            ENT.Criteria criterias = await CriteriaDomain.Get(id);

            DTO.Criteria dtoCriteria = null;
            if (criterias != null)
                dtoCriteria = Mapper.Map<ENT.Criteria, DTO.Criteria>(criterias);

            return dtoCriteria;
        }
    }
}
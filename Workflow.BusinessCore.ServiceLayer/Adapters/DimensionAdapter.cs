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
    ///  DimensionAdapter interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the Dimension adapter.
    /// </remarks>
    /// <typeparam name="T">Value DTO model</typeparam>
    public class DimensionAdapter : IDimensionAdapter
    {
        private readonly IServiceProvider _serviceProvider = null;

        private IDimensionDomain DimensionDomain
        {
            get
            {
                return _serviceProvider?.GetService<IDimensionDomain>();
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
        public DimensionAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        /// <summary>
        /// Récupére toutes les entités DTO existantes.
        /// </summary>
        /// <returns>Message de retour avec la liste en json</returns>
        public async Task<IEnumerable<DTO.Dimension>> GetAll()
        {
            IEnumerable<ENT.Dimension> dimensions = await DimensionDomain.Get();

            IEnumerable<DTO.Dimension> dtoDimensions = null;
            if (dimensions != null)
                dtoDimensions = Mapper.Map<IEnumerable<ENT.Dimension>, IEnumerable<DTO.Dimension>>(dimensions);
            else
                dtoDimensions = new List<DTO.Dimension>();

            return dtoDimensions;
        }

        /// <summary>
        /// Récupére l'entité désignée par l'id en paramétre.
        /// </summary>
        /// <param name="id">Id de l'entité</param>
        /// <returns>Message de retour avec l'entité</returns>
        public async Task<DTO.Dimension> GetById(long id)
        {
            ENT.Dimension dimension = await DimensionDomain.Get(id);

            DTO.Dimension dtoDimension = null;
            if (dimension != null)
                dtoDimension = Mapper.Map<ENT.Dimension, DTO.Dimension>(dimension);

            return dtoDimension;
        }

        /// <summary>
        /// Ajoute une dimension.
        /// </summary>
        /// <param name="dimension">Nouvelle Dimension</param>
        /// <returns>Message de retour</returns>
        public async Task<DTO.Dimension> AddDimension(DTO.Dimension dimension)
        {
            ENT.Dimension entDimension = Mapper.Map<DTO.Dimension, ENT.Dimension>(dimension);

            return Mapper.Map<ENT.Dimension, DTO.Dimension>(await DimensionDomain.AddDimension(entDimension));
        }

    }
}
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using Workflow.Transverse.Helpers;
using DTO = Workflow.Transverse.DTO;
using ENT = Workflow.BusinessCore.DataLayer.Entities;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Workflow.BusinessCore.ServiceLayer.Adapters
{
    public class GridConfigurationAdapter : IGridConfigurationAdapter
    {
        private readonly IServiceProvider _serviceProvider;

        private IGridConfigurationDomain GridConfigurationDomain
        {
            get
            {
                return _serviceProvider?.GetService<IGridConfigurationDomain>();
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
        /// Constructeur par défaut, pour l'ID
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de services</param>
        public GridConfigurationAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        /// <summary>
        /// Récupére la configuration d'une grid selon un SelectorInstance
        /// </summary>
        /// <param name="selectorInstanceId">Id du SelectorInstance</param>
        /// <returns>Configuration de la grid</returns>
        public async Task<DTO.GridConfig> GetBySelectorInstanceId(long selectorInstanceId)
        {
            ENT.GridConfig gridEntity = await GridConfigurationDomain.GetBySelectorInstanceId(selectorInstanceId);
            return Mapper.Map<ENT.GridConfig, DTO.GridConfig>(gridEntity);
        }

        /// <summary>
        /// Récupére toutes les entités DTO existantes.
        /// </summary>
        /// <returns>Message de retour avec la liste en json</returns>
        public async Task<IEnumerable<DTO.GridConfig>> GetAll()
        {
            IEnumerable<ENT.GridConfig> gridConfigs = await GridConfigurationDomain.Get();

            IEnumerable<DTO.GridConfig> dtoGridConfigs = null;
            if (gridConfigs != null)
                dtoGridConfigs = Mapper.Map<IEnumerable<ENT.GridConfig>, IEnumerable<DTO.GridConfig>>(gridConfigs);
            else
                dtoGridConfigs = new List<DTO.GridConfig>();

            return dtoGridConfigs;
        }

        /// <summary>
        /// Récupére l'entité désignée par l'id en paramétre.
        /// </summary>
        /// <param name="id">Id de l'entité</param>
        /// <returns>Message de retour avec l'entité</returns>
        public async Task<DTO.GridConfig> GetById(long id)
        {
            ENT.GridConfig gridConfig = await GridConfigurationDomain.Get(id);

            DTO.GridConfig dtoGridConfig = null;
            if (gridConfig != null)
                dtoGridConfig = Mapper.Map<ENT.GridConfig, DTO.GridConfig>(gridConfig);

            return dtoGridConfig;
        }

        /// <summary>
        /// Ajoute un configuration de grid.
        /// </summary>
        /// <param name="gridConfig">Nouvelle configuration de grid</param>
        /// <param name="workflowConfigId">Id du workflowConfig auquel lier la nouvelle configuration de grid</param>
        /// <returns>Message de retour</returns>
        public async Task<DTO.GridConfig> Add(long workflowConfigId, DTO.GridConfig gridConfig)
        {
            ENT.GridConfig entGridConfig = Mapper.Map<DTO.GridConfig, ENT.GridConfig>(gridConfig);

            return Mapper.Map<ENT.GridConfig, DTO.GridConfig>(await GridConfigurationDomain.Add(workflowConfigId, entGridConfig));
        }

        /// <summary>
        /// Ajoute une configuration de dimension à une configuration de grid existante.
        /// </summary>
        /// <param name="gridDimensionConfig">Nouvelle configuration de dimension</param>
        /// <param name="gcColumnId">Facultatif : id de la config grid lorsque la config dimension est en colonne</param>
        /// <param name="gcRowId">Facultatif : id de la config grid lorsque la config dimension est en ligne</param>
        /// <param name="gcFixedId">Facultatif : id de la config grid lorsque la config dimension est fixée en dehors de la grid</param>
        /// <returns>Message de retour</returns>
        /// <remarks>On ne peut pas avoir les 3 id de GridConfig réglés en même temps.</remarks>
        public async Task<DTO.GridDimensionConfig> Add(DTO.GridDimensionConfig gridDimensionConfig, long? gcColumnId = null, long? gcRowId = null, long? gcFixedId = null)
        {
            ENT.GridDimensionConfig entGridDimensionConfig = Mapper.Map<DTO.GridDimensionConfig, ENT.GridDimensionConfig>(gridDimensionConfig);

            return Mapper.Map<ENT.GridDimensionConfig, DTO.GridDimensionConfig>(await GridConfigurationDomain.Add(entGridDimensionConfig, gcColumnId, gcRowId, gcFixedId));
        }

        /// <summary>
        /// Ajoute une configuration de valeur à une configuration de dimension existante.
        /// </summary>
        /// <param name="gridValueConfig">Nouvelle configuration de valeur</param>
        /// <param name="gridDimensionConfigId">Id de la configuration de dimension cible.</param>
        /// <returns>Message de retour</returns>
        public async Task<DTO.GridValueConfig> Add(DTO.GridValueConfig gridValueConfig, long gridDimensionConfigId)
        {
            ENT.GridValueConfig entGridValueConfig = Mapper.Map<DTO.GridValueConfig, ENT.GridValueConfig>(gridValueConfig);

            return Mapper.Map<ENT.GridValueConfig, DTO.GridValueConfig>(await GridConfigurationDomain.Add(entGridValueConfig, gridDimensionConfigId));
        }

    }
}

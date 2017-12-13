using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.Transverse.DTO;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces
{
    public interface IGridConfigurationAdapter : IBaseAdapter<GridConfig>
    {
        /// <summary>
        /// Récupére la configuration d'une grid selon un SelectorInstance
        /// </summary>
        /// <param name="selectorInstanceId">Id du SelectorInstance</param>
        /// <returns>Configuration de la grid</returns>
        Task<GridConfig> GetBySelectorInstanceId(long selectorInstanceId);

        /// <summary>
        /// Ajoute un configuration de grid.
        /// </summary>
        /// <param name="gridConfig">Nouvelle configuration de grid</param>
        /// <param name="workflowConfigId">Id du workflowConfig auquel lier la nouvelle configuration de grid</param>
        /// <returns>Message de retour</returns>
        Task<GridConfig> Add(long workflowConfigId, GridConfig gridConfig);

        /// <summary>
        /// Ajoute une configuration de dimension à une configuration de grid existante.
        /// </summary>
        /// <param name="gridDimensionConfig">Nouvelle configuration de dimension</param>
        /// <param name="gcColumnId">Facultatif : id de la config grid lorsque la config dimension est en colonne</param>
        /// <param name="gcRowId">Facultatif : id de la config grid lorsque la config dimension est en ligne</param>
        /// <param name="gcFixedId">Facultatif : id de la config grid lorsque la config dimension est fixée en dehors de la grid</param>
        /// <returns>Message de retour</returns>
        /// <remarks>On ne peut pas avoir les 3 id de GridConfig réglés en même temps.</remarks>
        Task<GridDimensionConfig> Add(GridDimensionConfig gridDimensionConfig, long? gcColumnId = null, long? gcRowId = null, long? gcFixedId = null);

        /// <summary>
        /// Ajoute une configuration de valeur à une configuration de dimension existante.
        /// </summary>
        /// <param name="gridValueConfig">Nouvelle configuration de valeur</param>
        /// <param name="gridDimensionConfigId">Id de la configuration de dimension cible.</param>
        /// <returns>Message de retour</returns>
        Task<GridValueConfig> Add(GridValueConfig gridValueConfig, long gridDimensionConfigId);
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.BusinessLayer.Domains.Interfaces
{
    public interface IGridConfigurationDomain
    {
        /// <summary>
        /// Créé la table temporaire pour un selectorInstance, selon la configuration de grid définie pour le WorkflowConfig lié.
        /// </summary>
        /// <param name="selectorInstance">Instance du SelectorInstance</param>
        /// <param name="wfInstance">Instance du WorkflowInstance</param>
        /// <returns>Message de retour</returns>
        Task<HttpResponseMessageResult> CreateDataTableDB(SelectorInstance selectorInstance, WorkflowInstance wfInstance);

        /// <summary>
        /// Donne les colonnes et le nom de la table temporaire liée au SelectorInstance.
        /// </summary>
        /// <param name="selectorInstance">Instance du SelectorInstance</param>
        /// <param name="wfInstance">Instance du WorkflowInstance</param>
        /// <returns>Liste de string : en element 0, le nom de la table, puis suivent les noms des colones.</returns>
        Task<IEnumerable<string>> GetColumnsFromGridConfiguration(SelectorInstance selectorInstance, WorkflowInstance wfInstance);

        /// <summary>
        /// Récupére la configuration d'une grid selon un SelectorInstance
        /// </summary>
        /// <param name="selectorInstanceId">Id du SelectorInstance</param>
        /// <returns>Configuration de la grid</returns>
        Task<GridConfig> GetBySelectorInstanceId(long selectorInstanceId);

        /// <summary>
        /// Duplique un GridConfig pour l'instance d'un WorkflowConfig.
        /// </summary>
        /// <param name="gridConfig">GridConfig original</param>
        /// <returns>Duplicat du GridConfig original</returns>
        Task<GridConfig> CopyForStatic(GridConfig gridConfig);

        /// <summary>
        /// Duplique un GridDimensionConfig pour l'instance d'un WorkflowConfig.
        /// </summary>
        /// <param name="gridDimensionConfig">GridDimensionConfig original</param>
        /// <returns>Duplicat du GridDimensionConfig original</returns>
        Task<GridDimensionConfig> CopyForStatic(GridDimensionConfig gridDimensionConfig);

        /// <summary>
        /// Duplique un GridValueConfig pour l'instance d'un WorkflowConfig.
        /// </summary>
        /// <param name="gridValueConfig">GridValueConfig original</param>
        /// <returns>Duplicat du GridValueConfig original</returns>
        Task<GridValueConfig> CopyForStatic(GridValueConfig gridValueConfig);

        /// <summary>
        /// Enregistre les valeurs modifiées dans la table temporaire correspondant au SelectorInstance donné.
        /// </summary>
        /// <param name="selectIns">SelectorInstance</param>
        /// <param name="wfInst">WorkflowInstance</param>
        /// <param name="values">Valeurs à modifier</param>
        /// <returns>Message de retour</returns>
        Task<HttpResponseMessageResult> SaveDataInTemporyTable(SelectorInstance selectIns, WorkflowInstance wfInst, IEnumerable<KeyValuePair<long, double>> values);

        /// <summary>
        /// Récupére toutes les configurations de gird existantes.
        /// </summary>
        /// <returns>Liste de configurations de grid.</returns>
        Task<IEnumerable<GridConfig>> Get();

        /// <summary>
        /// Récupére une configuration de grid selon son Id.
        /// </summary>
        /// <param name="id">Ide dela configuration de grid recherchée</param>
        /// <returns>Configuration de grid</returns>
        Task<GridConfig> Get(long id);

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

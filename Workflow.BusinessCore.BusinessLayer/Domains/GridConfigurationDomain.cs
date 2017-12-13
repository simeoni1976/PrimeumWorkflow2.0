using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.BusinessCore.BusinessLayer.UnitOfWork.Interfaces;
using Workflow.Transverse.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Linq;
using Workflow.Transverse.Environment;
using Workflow.BusinessCore.BusinessLayer.Helpers;
using System.Data.Common;
using System.Data;
using Workflow.BusinessCore.BusinessLayer.Process.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Storage;
using System.Globalization;
using Newtonsoft.Json;

namespace Workflow.BusinessCore.BusinessLayer.Domains
{
    public class GridConfigurationDomain : IGridConfigurationDomain
    {
        private readonly IServiceProvider _serviceProvider;

        private IUnitOfWork UnitOfWork
        {
            get
            {
                return _serviceProvider?.GetService<IUnitOfWork>();
            }
        }

        /// <summary>
        /// Class constructor
        /// </summary>
        /// <param name="serviceProvider">Fournisseur des services</param>
        public GridConfigurationDomain(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// Créé la table temporaire pour un selectorInstance, selon la configuration de grid définie pour le WorkflowConfig lié.
        /// </summary>
        /// <param name="selectorInstance">Instance du SelectorInstance</param>
        /// <param name="wfInstance">Instance du WorkflowInstance</param>
        /// <returns>Message de retour</returns>
        public async Task<HttpResponseMessageResult> CreateDataTableDB(SelectorInstance selectorInstance, WorkflowInstance wfInstance)
        {
            // Transaction
            IDbContextTransaction transaction = SessionStatsHelper.HttpHitGetDBTransaction(_serviceProvider);
            if (transaction == null)
                throw new DatabaseException("GridConfigurationDomain.CreateDataTableDB: impossible to retrieve transaction connexion.");

            HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };

            // On récupére la configuration de l'opérateur
            List<GridConfig> lstGridConf = await UnitOfWork.GetDbContext().GridConfig
                .Include(gc => gc.ColumnDimensions)
                .ThenInclude(gdc => gdc.Values)
                .Include(gc => gc.RowDimensions)
                .ThenInclude(gdc => gdc.Values)
                .Include(gc => gc.FixedDimensions)
                .ThenInclude(gdc => gdc.Values)
                .Include(gc => gc.WorkflowConfig)
                .Where(gc => gc.WorkflowConfig.Id == wfInstance.WorkflowConfig.Id)
                .AsNoTracking()
                .ToAsyncEnumerable()
                .ToList();

            GridConfig gridConf = lstGridConf.FirstOrDefault();

            if (gridConf == null)
                throw new SequenceException($"GridConfigurationDomain.CreateDataTableDB: no grid configuration for WorkflowConfig ({wfInstance.WorkflowConfig.Id}).");

            // Construction de la requête
            StringBuilder query = new StringBuilder();

            string nomTable = string.Format(Constant.TEMPLATE_TEMPORARY_TABLENAME, selectorInstance.Id.ToString());
            List<string> nomCols = new List<string>();
            IEnumerable<DistributionDimensionGrid> lstDistCols = GenerateDistribution(gridConf.ColumnDimensions);

            query.AppendLine($"CREATE TABLE {nomTable}(");
            query.AppendLine("ID bigint IDENTITY(1,1) NOT NULL,");

            foreach (GridDimensionConfig fixes in gridConf.FixedDimensions.OrderBy(c => c.Order))
            {
                string nomCol = ((int)fixes.InternalName).ToString();
                query.AppendLine($"Dim{nomCol} nvarchar(max) NULL,");
                nomCols.Add($"Dim{nomCol}");
            }
            foreach (GridDimensionConfig row in gridConf.RowDimensions.OrderBy(r => r.Order))
            {
                string nomCol = ((int)row.InternalName).ToString();
                query.AppendLine($"Dim{nomCol} nvarchar(max) NULL,");
                nomCols.Add($"Dim{nomCol}");
            }
            foreach (DistributionDimensionGrid ddg in lstDistCols)
            {
                query.AppendLine($"{ddg.ColumnName}_ID bigint NOT NULL,");
                query.AppendLine($"{ddg.ColumnName}_VAL float NOT NULL,");
                nomCols.Add($"{ddg.ColumnName}_ID");
                nomCols.Add($"{ddg.ColumnName}_VAL");
            }

            query.SkipComma();
            query.AppendLine(")");


            // Exécution de la requete de création de table temporaire.
            int nbrQry = await ExecSqlHelper.ExecuteNonQueryTransactionAsync(query.ToString(), transaction);

            // Insertion des données dans la table temporaire.

            StringBuilder qryInsert = new StringBuilder();

            qryInsert.AppendLine($"INSERT INTO {nomTable} (");
            foreach (string nomCol in nomCols)
                qryInsert.AppendLine($"{nomCol},");
            // Nettoyage
            qryInsert.SkipComma();
            qryInsert.AppendLine(")");

            qryInsert.AppendLine("SELECT ");

            List<string> groupedCols = new List<string>();
            foreach (GridDimensionConfig fixes in gridConf.FixedDimensions.OrderBy(c => c.Order))
            {
                string nomCol = fixes.InternalName.ToString();
                qryInsert.AppendLine($"{nomCol},");
                groupedCols.Add(nomCol);
            }
            foreach (GridDimensionConfig row in gridConf.RowDimensions.OrderBy(r => r.Order))
            {
                string nomCol = row.InternalName.ToString();
                qryInsert.AppendLine($"{nomCol},");
                groupedCols.Add(nomCol);
            }
            foreach (DistributionDimensionGrid ddg in lstDistCols)
            {
                StringBuilder sbQry = new StringBuilder();
                foreach (KeyValuePair<InternalNameDimension, string> pair in ddg.Selection)
                {
                    if (sbQry.Length > 0)
                        sbQry.Append(" AND ");
                    sbQry.Append($"{pair.Key.ToString()} = '{pair.Value}'");
                }
                qryInsert.AppendLine("MAX(");
                qryInsert.AppendLine("CASE");
                qryInsert.Append("WHEN ");
                qryInsert.Append(sbQry);
                qryInsert.AppendLine(" THEN ID");
                qryInsert.AppendLine("ELSE 0");
                qryInsert.AppendLine("END");
                qryInsert.AppendLine($"),");

                qryInsert.AppendLine("MAX(");
                qryInsert.AppendLine("CASE");
                qryInsert.Append("WHEN ");
                qryInsert.Append(sbQry);
                qryInsert.AppendLine(" THEN CurrentValue");
                qryInsert.AppendLine("ELSE 0");
                qryInsert.AppendLine("END");
                qryInsert.AppendLine($"),");
            }


            // Nettoyage
            qryInsert.SkipComma();
            qryInsert.AppendLine("FROM ValueObject");

            // Jointure avec la table de liaison
            qryInsert.AppendLine("WHERE ID IN (");
            qryInsert.AppendLine($"SELECT ValueObjectId FROM SelectorInstanceValueObject WHERE SelectorInstanceId = {selectorInstance.Id} ");
            qryInsert.AppendLine(")");

            qryInsert.Append("GROUP BY ");
            groupedCols.Reverse(); // Inverser pour la clause GROUP BY
            foreach (string grpCol in groupedCols)
                qryInsert.Append($"{grpCol},");

            // Nettoyage
            qryInsert.SkipComma();

            nbrQry = await ExecSqlHelper.ExecuteNonQueryTransactionAsync(qryInsert.ToString(), transaction);

            return res;
        }


        /// <summary>
        /// Donne les colonnes et le nom de la table temporaire liée au SelectorInstance.
        /// </summary>
        /// <param name="selectorInstance">Instance du SelectorInstance</param>
        /// <param name="wfInstance">Instance du WorkflowInstance</param>
        /// <returns>Liste de string : en element 0, le nom de la table, puis suivent les noms des colones.</returns>
        public async Task<IEnumerable<string>> GetColumnsFromGridConfiguration(SelectorInstance selectorInstance, WorkflowInstance wfInstance)
        {
            if (selectorInstance == null)
                throw new WrongParameterException("GridConfiguration.GetColumnsFromGridConfiguration : SelectorInstance is null!");
            if (wfInstance == null)
                throw new WrongParameterException("GridConfiguration.GetColumnsFromGridConfiguration : WorkflowInstance is null!");
            if (wfInstance.WorkflowConfig == null)
                throw new WrongParameterException("GridConfiguration.GetColumnsFromGridConfiguration : WorkflowInstance.WorkflowConfig is null!");

            // On récupére la configuration de l'opérateur
            List<GridConfig> lstGridConf = await UnitOfWork.GetDbContext().GridConfig
                .Include(gc => gc.ColumnDimensions)
                .ThenInclude(gdc => gdc.Values)
                .Include(gc => gc.RowDimensions)
                .ThenInclude(gdc => gdc.Values)
                .Include(gc => gc.FixedDimensions)
                .ThenInclude(gdc => gdc.Values)
                .Include(gc => gc.WorkflowConfig)
                .Where(gc => gc.WorkflowConfig.Id == wfInstance.WorkflowConfig.Id)
                .AsNoTracking()
                .ToAsyncEnumerable()
                .ToList();

            GridConfig gridConf = lstGridConf.FirstOrDefault();


            IEnumerable<DistributionDimensionGrid> lstDistCols = GenerateDistribution(gridConf.ColumnDimensions);
            string nomTable = string.Format(Constant.TEMPLATE_TEMPORARY_TABLENAME, selectorInstance.Id.ToString());

            List<string> nomsTableEtCols = new List<string>();
            nomsTableEtCols.Add(nomTable);

            foreach (GridDimensionConfig fixes in gridConf.FixedDimensions.OrderBy(c => c.Order))
            {
                string nomCol = $"Dim{((int)fixes.InternalName).ToString()}";
                nomsTableEtCols.Add(nomCol);
            }
            foreach (GridDimensionConfig row in gridConf.RowDimensions.OrderBy(r => r.Order))
            {
                string nomCol = $"Dim{((int)row.InternalName).ToString()}";
                nomsTableEtCols.Add(nomCol);
            }
            foreach (DistributionDimensionGrid ddg in lstDistCols)
            {
                nomsTableEtCols.Add($"{ddg.ColumnName}_ID");
                nomsTableEtCols.Add($"{ddg.ColumnName}_VAL");
            }

            return nomsTableEtCols;
        }

        /// <summary>
        /// Récupére la configuration d'une grid selon un SelectorInstance
        /// </summary>
        /// <param name="selectorInstanceId">Id du SelectorInstance</param>
        /// <returns>Configuration de la grid</returns>
        public async Task<GridConfig> GetBySelectorInstanceId(long selectorInstanceId)
        {
            List<SelectorInstance> lst = await UnitOfWork.GetDbContext().SelectorInstance
                .Where(si => si.Id == selectorInstanceId)
                .Include(si => si.WorkflowInstance)
                .ThenInclude(wi => wi.WorkflowConfig)
                .AsNoTracking()
                .ToAsyncEnumerable()
                .ToList();

            if ((lst == null) || (lst.Count == 0))
                throw new WrongParameterException($"GridConfiguration.GetBySelectorInstanceId : Id SelectorInstance ({selectorInstanceId}) don't exist!");

            SelectorInstance selectorInstance = lst.FirstOrDefault();

            if (selectorInstance == null)
                throw new WrongParameterException("GridConfiguration.GetBySelectorInstanceId : SelectorInstance is null!");
            if (selectorInstance.WorkflowInstance == null)
                throw new WrongParameterException("GridConfiguration.GetBySelectorInstanceId : SelectorInstance.WorkflowInstance is null!");
            if (selectorInstance.WorkflowInstance.WorkflowConfig == null)
                throw new WrongParameterException("GridConfiguration.GetBySelectorInstanceId : SelectorInstance.WorkflowInstance.WorkflowConfig is null!");

            // On récupére la configuration de l'opérateur
            List<GridConfig> lstGridConf = await UnitOfWork.GetDbContext().GridConfig
                .Include(gc => gc.ColumnDimensions)
                .ThenInclude(gdc => gdc.Values)
                .Include(gc => gc.RowDimensions)
                .ThenInclude(gdc => gdc.Values)
                .Include(gc => gc.FixedDimensions)
                .ThenInclude(gdc => gdc.Values)
                .Include(gc => gc.WorkflowConfig)
                .Where(gc => gc.WorkflowConfig.Id == selectorInstance.WorkflowInstance.WorkflowConfig.Id)
                .AsNoTracking()
                .ToAsyncEnumerable()
                .ToList();

            GridConfig gridConf = lstGridConf.FirstOrDefault();

            return gridConf;
        }


        /// <summary>
        /// Enregistre les valeurs modifiées dans la table temporaire correspondant au SelectorInstance donné.
        /// </summary>
        /// <param name="selectIns">SelectorInstance</param>
        /// <param name="wfInst">WorkflowInstance</param>
        /// <param name="values">Valeurs à modifier</param>
        /// <returns>Message de retour</returns>
        public async Task<HttpResponseMessageResult> SaveDataInTemporyTable(SelectorInstance selectIns, WorkflowInstance wfInst, IEnumerable<KeyValuePair<long, double>> values)
        {
            // Transaction
            IDbContextTransaction transaction = SessionStatsHelper.HttpHitGetDBTransaction(_serviceProvider);
            if (transaction == null)
                throw new DatabaseException("GridConfigurationDomain.SaveDataInTemporyTable: impossible to retrieve transaction connexion.");

            IEnumerable<string> cols = await GetColumnsFromGridConfiguration(selectIns, wfInst);

            StringBuilder qryUpdate = new StringBuilder();
            string nomTable = cols.ElementAt(0); // le 1er élément est le nom de la table temporaire

            qryUpdate.AppendLine($"UPDATE {nomTable}");
            qryUpdate.AppendLine($"SET ");

            StringBuilder qryCase = new StringBuilder();
            foreach (KeyValuePair<long, double> kvp in values)
                qryCase.AppendLine($"WHEN {kvp.Key} THEN '{kvp.Value.ToString(CultureInfo.InvariantCulture)}'");

            foreach (string col in cols.Where(c => c.EndsWith("_ID")))
            {
                string colVal = col.Substring(0, col.IndexOf("_ID")) + "_VAL";
                qryUpdate.AppendLine($"{colVal} = ");
                qryUpdate.AppendLine($"CASE {col}");
                qryUpdate.Append(qryCase);
                qryUpdate.AppendLine($"ELSE {colVal}");
                qryUpdate.AppendLine("END,");
            }

            // Nettoyage
            qryUpdate.SkipComma();

            // Exécution de la requete de création de table temporaire.
            int nbrQry = await ExecSqlHelper.ExecuteNonQueryTransactionAsync(qryUpdate.ToString(), transaction);

            return new HttpResponseMessageResult() { IsSuccess = nbrQry > 0 };
        }



        /// <summary>
        /// Duplique un GridConfig pour l'instance d'un WorkflowConfig.
        /// </summary>
        /// <param name="gridConfig">GridConfig original</param>
        /// <returns>Duplicat du GridConfig original</returns>
        public async Task<GridConfig> CopyForStatic(GridConfig gridConfig)
        {
            if (gridConfig == null)
                throw new WrongParameterException("GridConfigurationDomain.CopyForStatic: GridConfig source is null!");

            GridConfig duplicat = new GridConfig();
            UnitOfWork.GridConfigRepository.PrepareAddForObject(duplicat);
            duplicat.Name = string.Format(Constant.POSTFIX_NAME_DUPLICATE_WORKFLOW_CONFIG, gridConfig.Name, 1);
            foreach (GridDimensionConfig gdc in gridConfig.ColumnDimensions)
            {
                GridDimensionConfig gdcNew = await CopyForStatic(gdc);
                gdcNew.GridColumn = duplicat;
                duplicat.ColumnDimensions.Add(gdcNew);
            }
            foreach (GridDimensionConfig gdc in gridConfig.RowDimensions)
            {
                GridDimensionConfig gdcNew = await CopyForStatic(gdc);
                gdcNew.GridRow = duplicat;
                duplicat.RowDimensions.Add(gdcNew);
            }
            foreach (GridDimensionConfig gdc in gridConfig.FixedDimensions)
            {
                GridDimensionConfig gdcNew = await CopyForStatic(gdc);
                gdcNew.GridFixed = duplicat;
                duplicat.FixedDimensions.Add(gdcNew);
            }

            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            return duplicat;
        }

        /// <summary>
        /// Duplique un GridDimensionConfig pour l'instance d'un WorkflowConfig.
        /// </summary>
        /// <param name="gridDimensionConfig">GridDimensionConfig original</param>
        /// <returns>Duplicat du GridDimensionConfig original</returns>
        public async Task<GridDimensionConfig> CopyForStatic(GridDimensionConfig gridDimensionConfig)
        {
            if (gridDimensionConfig == null)
                throw new WrongParameterException("GridConfigurationDomain.CopyForStatic: GridDimensionConfig source is null!");

            GridDimensionConfig duplicat = new GridDimensionConfig();
            UnitOfWork.GridDimensionConfigRepository.PrepareAddForObject(duplicat);
            duplicat.DisplayName = gridDimensionConfig.DisplayName;
            duplicat.InternalName = gridDimensionConfig.InternalName;
            duplicat.Order = gridDimensionConfig.Order;
            foreach (GridValueConfig gvc in gridDimensionConfig.Values)
            {
                GridValueConfig gvcNew = await CopyForStatic(gvc);
                gvcNew.GridDimensionConfig = duplicat;
                duplicat.Values.Add(gvcNew);
            }

            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            return duplicat;
        }

        /// <summary>
        /// Duplique un GridValueConfig pour l'instance d'un WorkflowConfig.
        /// </summary>
        /// <param name="gridValueConfig">GridValueConfig original</param>
        /// <returns>Duplicat du GridValueConfig original</returns>
        public async Task<GridValueConfig> CopyForStatic(GridValueConfig gridValueConfig)
        {
            if (gridValueConfig == null)
                throw new WrongParameterException("GridConfigurationDomain.CopyForStatic: GridValueConfig source is null!");

            GridValueConfig duplicat = new GridValueConfig();
            UnitOfWork.GridValueConfigRepository.PrepareAddForObject(duplicat);
            duplicat.Order = gridValueConfig.Order;
            duplicat.Value = gridValueConfig.Value;

            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            return duplicat;
        }


        /// <summary>
        /// Récupére toutes les configurations de gird existantes.
        /// </summary>
        /// <returns>Liste de configurations de grid.</returns>
        public async Task<IEnumerable<GridConfig>> Get()
        {
            return await UnitOfWork.GridConfigRepository.GetAll();
        }

        /// <summary>
        /// Récupére une configuration de grid selon son Id.
        /// </summary>
        /// <param name="id">Ide dela configuration de grid recherchée</param>
        /// <returns>Configuration de grid</returns>
        public async Task<GridConfig> Get(long id)
        {
            return await UnitOfWork.GridConfigRepository.GetById(id);
        }


        /// <summary>
        /// Ajoute un configuration de grid.
        /// </summary>
        /// <param name="gridConfig">Nouvelle configuration de grid</param>
        /// <param name="workflowConfigId">Id du workflowConfig auquel lier la nouvelle configuration de grid</param>
        /// <returns>Message de retour</returns>
        public async Task<GridConfig> Add(long workflowConfigId, GridConfig gridConfig)
        {
            if (gridConfig == null)
                throw new WrongParameterException("GridConfigurationDomain.Add : GridConfig is null.");
            if (string.IsNullOrWhiteSpace(gridConfig.Name))
                throw new WrongParameterException("GridConfigurationDomain.Add : GridConfig's name is null.");
            int cnt = await UnitOfWork.GetDbContext().GridConfig
                .Where(gc => gc.Name == gridConfig.Name)
                .AsNoTracking()
                .ToAsyncEnumerable()
                .Count();
            if (cnt > 0)
                throw new WrongParameterException($"GridConfigurationDomain.Add : GridConfig's name ({gridConfig.Name}) is not unique.");

            WorkflowConfig wfConf = await UnitOfWork.GetDbContext().WorkflowConfig
                .Where(wfc => wfc.Id == workflowConfigId)
                .FirstOrDefaultAsync();
            if (wfConf == null)
                throw new WrongParameterException($"GridConfigurationDomain.Add : WorkflowConfig don't exist (id = {workflowConfigId}).");

            UnitOfWork.GridConfigRepository.PrepareAddForObject(gridConfig);

            gridConfig.WorkflowConfig = wfConf;

            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            if (nbr <= 0)
                throw new DatabaseException("GridConfigurationDomain.Add : impossible to save GridConfig.");

            return gridConfig;
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
        public async Task<GridDimensionConfig> Add(GridDimensionConfig gridDimensionConfig, long? gcColumnId = null, long? gcRowId = null, long? gcFixedId = null)
        {
            if (gridDimensionConfig == null)
                throw new WrongParameterException("GridConfigurationDomain.Add : GridDimensionConfig is null.");
            int nbrId = gcColumnId.HasValue ? 1 : 0;
            nbrId += gcRowId.HasValue ? 1 : 0;
            nbrId += gcFixedId.HasValue ? 1 : 0;
            if (nbrId == 0)
                throw new WrongParameterException("GridConfigurationDomain.Add : No GridConfig id.");
            if (nbrId > 1)
                throw new WrongParameterException("GridConfigurationDomain.Add : Too many much GridConfig id.");
            long idGridConf = gcColumnId ?? gcRowId ?? gcFixedId ?? -1;
            if (idGridConf < 0) // Normalement impossible, mais on ne sait jamais
                throw new WrongParameterException("GridConfigurationDomain.Add : No GridConfig id.");

            GridConfig gridConfig = await UnitOfWork.GetDbContext().GridConfig
                .Where(gc => gc.Id == idGridConf)
                .FirstOrDefaultAsync();
            if (gridConfig == null)
                throw new WrongParameterException($"GridConfigurationDomain.Add : GridConfig don't exist for id ({idGridConf}).");

            UnitOfWork.GridConfigRepository.PrepareUpdateForObject(gridConfig);
            UnitOfWork.GridDimensionConfigRepository.PrepareAddForObject(gridDimensionConfig);

            if (gcColumnId.HasValue)
            {
                gridConfig.ColumnDimensions.Add(gridDimensionConfig);
                gridDimensionConfig.GridColumn = gridConfig;
            }
            if (gcRowId.HasValue)
            {
                gridConfig.RowDimensions.Add(gridDimensionConfig);
                gridDimensionConfig.GridRow = gridConfig;
            }
            if (gcFixedId.HasValue)
            {
                gridConfig.FixedDimensions.Add(gridDimensionConfig);
                gridDimensionConfig.GridFixed = gridConfig;
            }

            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            if (nbr <= 0)
                throw new DatabaseException("GridConfigurationDomain.Add : impossible to save GridDimensionConfig.");

            return gridDimensionConfig;
        }

        /// <summary>
        /// Ajoute une configuration de valeur à une configuration de dimension existante.
        /// </summary>
        /// <param name="gridValueConfig">Nouvelle configuration de valeur</param>
        /// <param name="gridDimensionConfigId">Id de la configuration de dimension cible.</param>
        /// <returns>Message de retour</returns>
        public async Task<GridValueConfig> Add(GridValueConfig gridValueConfig, long gridDimensionConfigId)
        {
            if (gridValueConfig == null)
                throw new WrongParameterException("GridConfigurationDomain.Add : GridValueConfig is null.");
            GridDimensionConfig gridDimensionConfig = await UnitOfWork.GetDbContext().GridDimensionConfig
                .Where(gdc => gdc.Id == gridDimensionConfigId)
                .FirstOrDefaultAsync();
            if (gridDimensionConfig == null)
                throw new WrongParameterException($"GridConfigurationDomain.Add : GridDimensionConfig don't exist with id = {gridDimensionConfigId}.");

            UnitOfWork.GridDimensionConfigRepository.PrepareUpdateForObject(gridDimensionConfig);
            UnitOfWork.GridValueConfigRepository.PrepareAddForObject(gridValueConfig);

            gridDimensionConfig.Values.Add(gridValueConfig);
            gridValueConfig.GridDimensionConfig = gridDimensionConfig;

            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            if (nbr <= 0)
                throw new DatabaseException("GridConfigurationDomain.Add : impossible to save GridValueConfig.");

            return gridValueConfig;
        }


        /// <summary>
        /// Méthode qui génére la distribution (produit cartésien) d'une liste de dimensions.
        /// </summary>
        /// <param name="dimensions">Liste de dimensions</param>
        /// <returns>Liste des objets de paires dimension/valeur distribuées</returns>
        private IEnumerable<DistributionDimensionGrid> GenerateDistribution(IEnumerable<GridDimensionConfig> dimensions)
        {
            List<DistributionDimensionGrid> distriLst = new List<DistributionDimensionGrid>();
            IEnumerable<GridDimensionConfig> orderedDimensions = dimensions.OrderBy(dim => dim.Order);

            RecursiveDistribution(distriLst, orderedDimensions, 0, null);

            int indexColTableTemp = 0;
            foreach (DistributionDimensionGrid ddg in distriLst)
            {
                ddg.ColumnName = $"C{indexColTableTemp}";
                indexColTableTemp++;
            }

            return distriLst;
        }

        /// <summary>
        /// Méthode récursive pour la distribution des dimensions/valeurs
        /// </summary>
        /// <param name="outList">Liste de sortie</param>
        /// <param name="dimensions">Liste des dimensions ordonnée à distribuer</param>
        /// <param name="indexDim">Indexe courant dans la liste des dimensions</param>
        /// <param name="current">Noeud courant temporaire</param>
        private void RecursiveDistribution(List<DistributionDimensionGrid> outList, IEnumerable<GridDimensionConfig> dimensions, int indexDim, DistributionDimensionGrid current)
        {
            if ((indexDim < 0) || (indexDim > dimensions.Count() - 1))
                return;

            GridDimensionConfig dim = dimensions.ElementAt(indexDim);
            foreach (GridValueConfig val in dim.Values.OrderBy(v => v.Order))
            {
                DistributionDimensionGrid ddg = new DistributionDimensionGrid();
                if (current != null)
                    ddg.CopyInSelection(current.Selection);
                if (ddg.Selection.ContainsKey(dim.InternalName))
                    continue;
                ddg.Selection.Add(dim.InternalName, val.Value);

                if (indexDim == (dimensions.Count() - 1))
                    outList.Add(ddg);
                else
                    RecursiveDistribution(outList, dimensions, indexDim + 1, ddg);
            }
        }



        /// <summary>
        /// Classe privée, uniquement pour l'usage du domaine.
        /// </summary>
        private class DistributionDimensionGrid
        {
            /// <summary>
            /// Nom de la colonne dans la table temporaire
            /// </summary>
            public string ColumnName { get; set; }

            /// <summary>
            /// Ensemble des paires dimension/valeur
            /// </summary>
            public Dictionary<InternalNameDimension, string> Selection { get; set; }

            /// <summary>
            /// Constructeur par défaut
            /// </summary>
            public DistributionDimensionGrid()
            {
                Selection = new Dictionary<InternalNameDimension, string>();
            }

            /// <summary>
            /// Copie les paires dim/val vers la selection de l'instance.
            /// </summary>
            /// <param name="selToCopy">Dictionnaire de paires à copier</param>
            public void CopyInSelection(Dictionary<InternalNameDimension, string> selToCopy)
            {
                foreach (InternalNameDimension k in selToCopy.Keys)
                    if (!Selection.ContainsKey(k))
                        Selection.Add(k, selToCopy[k]);
            }

        }

    }
}

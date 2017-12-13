using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Text;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Common;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.BusinessLayer.Helpers;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.BusinessCore.BusinessLayer.UnitOfWork.Interfaces;
using Workflow.Transverse.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Workflow.BusinessCore.BusinessLayer.Process.Exceptions;
using System.Data.Common;
using Newtonsoft.Json;
using System.Linq.Expressions;
using Workflow.Transverse.Environment;

namespace Workflow.BusinessCore.BusinessLayer.Domains
{
    /// <summary>
    ///  ValueObject domain class.
    /// </summary>
    /// <remarks>
    /// This class permits to define the ValueObject business object.
    /// </remarks>
    public class ValueObjectDomain : AbstractDomain<ValueObject>, IValueObjectDomain
    {
        private readonly IServiceProvider _serviceProvider;

        private string[] _allDimensionsNames = null;

        private IUnitOfWork UnitOfWork
        {
            get
            {
                return _serviceProvider?.GetService<IUnitOfWork>();
            }
        }

        private IGridConfigurationDomain GridConfigurationDomain
        {
            get
            {
                return _serviceProvider?.GetService<IGridConfigurationDomain>();
            }
        }

        private ICriteriaValuesDomain CriteriaValuesDomain
        {
            get
            {
                return _serviceProvider?.GetService<ICriteriaValuesDomain>();
            }
        }

        private IConfigVariableDomain ConfigVariableDomain
        {
            get
            {
                return _serviceProvider?.GetService<IConfigVariableDomain>();
            }
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="unitOfWork">Unit Of Work</param>
        public ValueObjectDomain(IUnitOfWork unitOfWork, IServiceProvider serviceProvider) : base(unitOfWork.ValueObjectRepository)
        {
            _serviceProvider = serviceProvider;

            _allDimensionsNames = new string[]
            {
                Constant.DATA_DIMENSION_1,
                Constant.DATA_DIMENSION_2,
                Constant.DATA_DIMENSION_3,
                Constant.DATA_DIMENSION_4,
                Constant.DATA_DIMENSION_5,
                Constant.DATA_DIMENSION_6,
                Constant.DATA_DIMENSION_7,
                Constant.DATA_DIMENSION_8,
                Constant.DATA_DIMENSION_9,
                Constant.DATA_DIMENSION_10,
                Constant.DATA_TYPEVALUE
            };
        }

        ///// <summary>
        ///// This function permits to get a ValueObject with his dataset.
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //public new async Task<ValueObject> Get(long id)
        //{
        //    return await _unitOfWork.GetDbContext().ValueObject
        //            .Include(i => i.DataSet)
        //            .SingleOrDefaultAsync(m => m.Id == id);
        //}

        /// <summary>
        /// This function permits to update an initial value for an item.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="initialValue"></param>
        /// <returns></returns>
        public async Task<bool> UpdateValue(long id, double initialValue)
        {
            throw new NotImplementedException();
            //ValueObject valueObject = await Get(id);
            //valueObject.InitialValue = initialValue;

            //return await UpdateDb.UpdateDbEntryAsync(valueObject, _unitOfWork, x => x.InitialValue);
        }


        /// <summary>
        /// Permet de lire les données selon un format configuré par l'opérateur, avec des filtres, des tris, la pagination, etc...
        /// </summary>
        /// <param name="selectorInstanceId">Id du SelectorInstance concerné</param>
        /// <param name="filter">Chaines à filtrer, l'indexe représente le numéro de colonne sur lequel appliquer le filtre.</param>
        /// <param name="start">Numéro de ligne à partir duquel il faut commencer la selection</param>
        /// <param name="length">Nombre de ligne à sélectionner</param>
        /// <param name="sortCol">Numéro de colonne à trier</param>
        /// <param name="sortDir">Ordre du tri : ASC ou DESC</param>
        /// <returns>Message de retour + données</returns>
        public async Task<HttpResponseMessageResult> ReadData(long selectorInstanceId, string[] filter, int start, int length, int sortCol, string sortDir)
        {
            // Récupération du SelectorInstance et controles
            List<SelectorInstance> lst = await UnitOfWork.GetDbContext().SelectorInstance
                .Where(s => s.Id == selectorInstanceId)
                .Include(s => s.WorkflowInstance)
                .ThenInclude(wi => wi.WorkflowConfig)
                .AsNoTracking()
                .ToAsyncEnumerable()
                .ToList();

            if ((lst == null) || (lst.Count == 0))
                throw new WrongParameterException($"ValueObject.ReadData : SelectorInstance ID {selectorInstanceId} not exist!");
            SelectorInstance sel = lst.FirstOrDefault();

            // On récupére les noms de table et de colonnes.
            IEnumerable<string> colsTableName = await GridConfigurationDomain.GetColumnsFromGridConfiguration(sel, sel.WorkflowInstance);

            // On extrait les noms de colonnes effectives (non ID)
            List<string> nomColsDisplay = new List<string>();
            for (int ic = 1; ic < colsTableName.Count(); ic++)
            {
                if (string.IsNullOrWhiteSpace(colsTableName.ElementAt(ic)))
                    continue;
                if (colsTableName.ElementAt(ic).EndsWith("ID"))
                    continue;
                nomColsDisplay.Add(colsTableName.ElementAt(ic));
            }

            // Construction de la requête de lecture des données

            StringBuilder querySel = new StringBuilder();

            // Choix des colonnes (toutes)
            querySel.AppendLine("SELECT ");
            foreach (string col in colsTableName.Skip(1))
                querySel.AppendLine($"{col},");
            querySel.SkipComma();
            querySel.AppendLine($"FROM {colsTableName.ElementAt(0)}");

            // Filtre
            StringBuilder where = new StringBuilder();
            if ((filter != null) && (filter.Count() > 0) && (filter.Any(f => !string.IsNullOrWhiteSpace(f))))
            {
                bool hasFilter = false;
                for (int i = 0; i < filter.Count(); i++)
                {
                    if (i >= nomColsDisplay.Count)
                        continue;

                    if (where.Length > 0)
                        where.AppendLine("AND");
                    where.AppendLine($"{nomColsDisplay[i]} LIKE '%{filter[i]}%'");

                    hasFilter = true;
                }
                if (hasFilter)
                {
                    querySel.AppendLine("WHERE");
                    querySel.Append(where);
                }
                else
                    where.Clear();
            }

            // Tri
            bool hasOrderBy = false;
            if ((sortCol >= 0) && (sortCol < nomColsDisplay.Count))
            {
                querySel.Append($"ORDER BY {nomColsDisplay[sortCol]} ");
                if (!string.IsNullOrWhiteSpace(sortDir) && sortDir.ToLower() == "asc")
                    querySel.AppendLine("ASC");
                else
                    querySel.AppendLine("DESC");
                hasOrderBy = true;
            }

            // Pagination
            if (start >= 0)
            {
                if (!hasOrderBy)
                    querySel.AppendLine("ORDER BY ID");
                querySel.AppendLine($"OFFSET {start} ROWS");
                if (length > 0)
                    querySel.AppendLine($"FETCH NEXT {length} ROWS ONLY");
            }

            DbConnection connection = UnitOfWork.GetDbContext().Database.GetDbConnection();
            IEnumerable<object[]> data = await ExecSqlHelper.ExecuteReaderAsync(querySel.ToString(), connection);
            Dictionary<string, object> dicoJson = new Dictionary<string, object>();

            dicoJson.Add("TableName", colsTableName.ElementAt(0));
            int numLine = 0;
            foreach (object[] row in data)
            {
                if (row.Length != colsTableName.Count() - 1) // -1 pour le nom de la table
                    continue;

                Dictionary<string, object> rowJson = new Dictionary<string, object>();
                for (int j = 0; j < row.Length; j++)
                    rowJson.Add(colsTableName.ElementAt(j + 1), row[j]);
                dicoJson.Add(numLine.ToString(), rowJson);
                numLine++;
            }


            // Requete de comptage
            StringBuilder queryCount = new StringBuilder();
            queryCount.AppendLine($"SELECT COUNT(1) FROM {colsTableName.ElementAt(0)} ");
            if (where.Length > 0)
            {
                queryCount.AppendLine("WHERE");
                queryCount.Append(where);
            }
            IEnumerable<object[]> dataCount = await ExecSqlHelper.ExecuteReaderAsync(queryCount.ToString(), connection);
            if ((dataCount.Count() == 1) && (dataCount.ElementAt(0).Length == 1))
            {
                int nbrTotal = (int)dataCount.ElementAt(0)[0];
                dicoJson.Add("TotalRows", nbrTotal);
            }

            // Données modifiables
            List<long> idsValueObjectEditable = await UnitOfWork.GetDbContext().SelectorInstanceValueObject
                .Where(sivo => sivo.SelectorInstanceId == selectorInstanceId && sivo.IsEditable)
                .Select(sivo => sivo.ValueObjectId)
                .ToAsyncEnumerable()
                .ToList();
            if ((idsValueObjectEditable != null) && (idsValueObjectEditable.Count > 0))
                dicoJson.Add("EditablesIds", idsValueObjectEditable);

            // Reponse du retour
            HttpResponseMessageResult resData = new HttpResponseMessageResult()
            {
                IsSuccess = true,
                Json = JsonConvert.SerializeObject(dicoJson)
            };

            return resData;
        }


        /// <summary>
        /// Remplie la liste where donnée en paramétre d'expression permettant de filtrer la table ValueObject selon les listes de CriteriaValues.
        /// </summary>
        /// <param name="where">Liste d'expression qui va recevoir les filtres (Sortie)</param>
        /// <param name="lstCriteriaValues">Listes des CriteriaValues utilisées pour les filtres</param>
        /// <param name="idsDimensionDS">Dictionnaire des DimensionDataSet par Id.</param>
        /// <param name="isLargeSearch">Créé une recherche stricte ou large (dans le cas d'arbre notamment)</param>
        /// <returns>Message de retour</returns>
        public HttpResponseMessageResult BuildFilterRequest(List<Expression<Func<ValueObject, bool>>> where, IEnumerable<IEnumerable<CriteriaValues>> lstCriteriaValues, Dictionary<long, DataSetDimension> idsDimensionDS, bool isLargeSearch)
        {
            HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };

            Dictionary<long, IEnumerable<CriteriaValues>> dico = CriteriaValuesDomain.GetCriteriaValuesByDimension(lstCriteriaValues);

            foreach (long dimensionId in dico.Keys)
            {
                DataSetDimension dsd = idsDimensionDS.Where(k => k.Value.Dimension.Id == dimensionId).Select(k => k.Value).FirstOrDefault();
                if (dsd == null)
                {
                    res.Message += $"ValueObjectDomain:BuildFilterRequest: Error, no Dimension column for this id ({dimensionId})!";
                    res.IsSuccess = false;
                    continue;
                }

                string nomDimension = idsDimensionDS[dsd.Id].ColumnName;
                IEnumerable<string> values = dico[dimensionId].Select(cv => cv.Value);

                if (isLargeSearch && (idsDimensionDS[dsd.Id].Dimension.TypeDimension == DimensionTypeEnum.Tree))
                {
                    if (values.Count() > 1)
                        where.Add(HelperGetFilterByTreeDimension(nomDimension, values));
                    else
                        if (values.Count() == 1)
                            where.Add(HelperGetFilterByTreeDimension(nomDimension, values.ElementAt(0)));
                }
                else
                {
                    if (values.Count() > 1)
                        where.Add(HelperGetFilterByDimension(nomDimension, values));
                    else
                        if (values.Count() == 1)
                            where.Add(HelperGetFilterByDimension(nomDimension, values.ElementAt(0)));
                }
            }

            return res;
        }


        /// <summary>
        /// Donne la valeur d'une dimension d'un ValueObject selon le nom de la dimension.
        /// </summary>
        /// <param name="vo">ValueObject</param>
        /// <param name="nomDimension">Nom de la dimension</param>
        /// <returns>Valeur de la dimension nommée.</returns>
        public string GetValueByDimensionName(ValueObject vo, string nomDimension)
        {
            if (nomDimension == Constant.DATA_DIMENSION_1)
                return vo.Dimension1;
            if (nomDimension == Constant.DATA_DIMENSION_2)
                return vo.Dimension2;
            if (nomDimension == Constant.DATA_DIMENSION_3)
                return vo.Dimension3;
            if (nomDimension == Constant.DATA_DIMENSION_4)
                return vo.Dimension4;
            if (nomDimension == Constant.DATA_DIMENSION_5)
                return vo.Dimension5;
            if (nomDimension == Constant.DATA_DIMENSION_6)
                return vo.Dimension6;
            if (nomDimension == Constant.DATA_DIMENSION_7)
                return vo.Dimension7;
            if (nomDimension == Constant.DATA_DIMENSION_8)
                return vo.Dimension8;
            if (nomDimension == Constant.DATA_DIMENSION_9)
                return vo.Dimension9;
            if (nomDimension == Constant.DATA_DIMENSION_10)
                return vo.Dimension10;
            return null;
        }

        /// <summary>
        /// Permet de créer une expression filtrante pour un DbQuery sur un ValueObject, avec un nom de dimension de type arbre et une valeur.
        /// </summary>
        /// <param name="nomDimension">Nom de la dimension de type arbre à filtrer</param>
        /// <param name="value">Valeur à filtrer</param>
        /// <returns>Expression filtrante</returns>
        public Expression<Func<ValueObject, bool>> HelperGetFilterByTreeDimension(string nomDimension, string value)
        {
            //value = Constant.SEPARATOR_TREE + value + Constant.SEPARATOR_TREE;

            if (nomDimension == Constant.DATA_DIMENSION_1)
                return vo => vo.Dimension1.Contains(value);
            if (nomDimension == Constant.DATA_DIMENSION_2)
                return vo => vo.Dimension2.Contains(value);
            if (nomDimension == Constant.DATA_DIMENSION_3)
                return vo => vo.Dimension3.Contains(value);
            if (nomDimension == Constant.DATA_DIMENSION_4)
                return vo => vo.Dimension4.Contains(value);
            if (nomDimension == Constant.DATA_DIMENSION_5)
                return vo => vo.Dimension5.Contains(value);
            if (nomDimension == Constant.DATA_DIMENSION_6)
                return vo => vo.Dimension6.Contains(value);
            if (nomDimension == Constant.DATA_DIMENSION_7)
                return vo => vo.Dimension7.Contains(value);
            if (nomDimension == Constant.DATA_DIMENSION_8)
                return vo => vo.Dimension8.Contains(value);
            if (nomDimension == Constant.DATA_DIMENSION_9)
                return vo => vo.Dimension9.Contains(value);
            if (nomDimension == Constant.DATA_DIMENSION_10)
                return vo => vo.Dimension10.Contains(value);
            return null;
        }

        /// <summary>
        /// Permet de créer une expression filtrante pour un DbQuery sur un ValueObject, avec un nom de dimension de type arbre et une liste de valeurs.
        /// </summary>
        /// <param name="nomDimension">Nom de la dimension de type arbre à filtrer</param>
        /// <param name="value">Liste de valeurs à filtrer</param>
        /// <returns>Expression filtrante</returns>
        public Expression<Func<ValueObject, bool>> HelperGetFilterByTreeDimension(string nomDimension, IEnumerable<string> values)
        {
            //value = Constant.SEPARATOR_TREE + value + Constant.SEPARATOR_TREE;

            if (nomDimension == Constant.DATA_DIMENSION_1)
                return vo => values.Any(s => vo.Dimension1.Contains(s));
            if (nomDimension == Constant.DATA_DIMENSION_2)
                return vo => values.Any(s => vo.Dimension2.Contains(s));
            if (nomDimension == Constant.DATA_DIMENSION_3)
                return vo => values.Any(s => vo.Dimension3.Contains(s));
            if (nomDimension == Constant.DATA_DIMENSION_4)
                return vo => values.Any(s => vo.Dimension4.Contains(s));
            if (nomDimension == Constant.DATA_DIMENSION_5)
                return vo => values.Any(s => vo.Dimension5.Contains(s));
            if (nomDimension == Constant.DATA_DIMENSION_6)
                return vo => values.Any(s => vo.Dimension6.Contains(s));
            if (nomDimension == Constant.DATA_DIMENSION_7)
                return vo => values.Any(s => vo.Dimension7.Contains(s));
            if (nomDimension == Constant.DATA_DIMENSION_8)
                return vo => values.Any(s => vo.Dimension8.Contains(s));
            if (nomDimension == Constant.DATA_DIMENSION_9)
                return vo => values.Any(s => vo.Dimension9.Contains(s));
            if (nomDimension == Constant.DATA_DIMENSION_10)
                return vo => values.Any(s => vo.Dimension10.Contains(s));
            return null;
        }

        /// <summary>
        /// Permet de créer une expression filtrante pour un DbQuery sur un ValueObject, avec un nom de dimension et une valeur.
        /// </summary>
        /// <param name="nomDimension">Nom de la dimension à filtrer</param>
        /// <param name="value">Valeur à filtrer</param>
        /// <returns>Expression filtrante</returns>
        public Expression<Func<ValueObject, bool>> HelperGetFilterByDimension(string nomDimension, string value)
        {
            if (nomDimension == Constant.DATA_DIMENSION_1)
                return vo => value == vo.Dimension1;
            if (nomDimension == Constant.DATA_DIMENSION_2)
                return vo => value == vo.Dimension2;
            if (nomDimension == Constant.DATA_DIMENSION_3)
                return vo => value == vo.Dimension3;
            if (nomDimension == Constant.DATA_DIMENSION_4)
                return vo => value == vo.Dimension4;
            if (nomDimension == Constant.DATA_DIMENSION_5)
                return vo => value == vo.Dimension5;
            if (nomDimension == Constant.DATA_DIMENSION_6)
                return vo => value == vo.Dimension6;
            if (nomDimension == Constant.DATA_DIMENSION_7)
                return vo => value == vo.Dimension7;
            if (nomDimension == Constant.DATA_DIMENSION_8)
                return vo => value == vo.Dimension8;
            if (nomDimension == Constant.DATA_DIMENSION_9)
                return vo => value == vo.Dimension9;
            if (nomDimension == Constant.DATA_DIMENSION_10)
                return vo => value == vo.Dimension10;
            return null;
        }

        /// <summary>
        /// Permet de créer une expression filtrante pour un DbQuery sur un ValueObject, avec un nom de dimension et une liste de valeurs possibles.
        /// </summary>
        /// <param name="nomDimension">Nom de la dimension à filtrer</param>
        /// <param name="value">Liste de valeurs possibles à filtrer</param>
        /// <returns>Expression filtrante</returns>
        public Expression<Func<ValueObject, bool>> HelperGetFilterByDimension(string nomDimension, IEnumerable<string> values)
        {
            if (nomDimension == Constant.DATA_DIMENSION_1)
                return vo => values.Contains(vo.Dimension1);
            if (nomDimension == Constant.DATA_DIMENSION_2)
                return vo => values.Contains(vo.Dimension2);
            if (nomDimension == Constant.DATA_DIMENSION_3)
                return vo => values.Contains(vo.Dimension3);
            if (nomDimension == Constant.DATA_DIMENSION_4)
                return vo => values.Contains(vo.Dimension4);
            if (nomDimension == Constant.DATA_DIMENSION_5)
                return vo => values.Contains(vo.Dimension5);
            if (nomDimension == Constant.DATA_DIMENSION_6)
                return vo => values.Contains(vo.Dimension6);
            if (nomDimension == Constant.DATA_DIMENSION_7)
                return vo => values.Contains(vo.Dimension7);
            if (nomDimension == Constant.DATA_DIMENSION_8)
                return vo => values.Contains(vo.Dimension8);
            if (nomDimension == Constant.DATA_DIMENSION_9)
                return vo => values.Contains(vo.Dimension9);
            if (nomDimension == Constant.DATA_DIMENSION_10)
                return vo => values.Contains(vo.Dimension10);
            return null;
        }

        /// <summary>
        /// Donne la valeur la plus actuelle d'un ValueObject.
        /// </summary>
        /// <param name="vo">ValueObject</param>
        /// <remarks>Tente de prendre la value FurutreValue si elle n'est pas nulle, sinon CurrentValue si elle n'est pas nulle, sinon InitialValue si elle n'est pas nulle.</remarks>
        /// <returns>Valeur la plus récente (Future sinon Current sinon Initial) ou si aucune valeur, NaN.</returns>
        public double GetMostCurrentValue(ValueObject vo)
        {
            if (vo == null)
                return double.NaN;

            return vo.VolatileValue ?? vo.FutureValue ?? vo.CurrentValue ?? vo.InitialValue ?? double.NaN;
        }


        /// <summary>
        /// Donne le format numérique pour un TypeValue donné.
        /// </summary>
        /// <param name="typeValueDefinition">TypeValue d'un ValueObject</param>
        /// <param name="dataSetId">Id du DataSet</param>
        /// <returns>Format numérique</returns>
        public async Task<string> GetNumericalFormat(string typeValueDefinition, long dataSetId)
        {
            await ConfigVariableDomain.LoadVariables();

            string defaultFormat = ConfigVariableDomain.Format;

            Dimension dimTypeValue = await UnitOfWork.GetDbContext().DataSetDimension
                .Include(d => d.Dimension)
                .Where(d => d.ColumnName == Constant.DATA_TYPEVALUE)
                .Select(d => d.Dimension)
                .FirstOrDefaultAsync();

            if (dimTypeValue == null)
                return defaultFormat;

            string format = await UnitOfWork.GetDbContext().DistinctValue
                .Where(d => d.DataSetId == dataSetId && d.DimensionId == dimTypeValue.Id && d.Value == typeValueDefinition)
                .Select(d => d.NumericalFormat)
                .FirstOrDefaultAsync();

            if (string.IsNullOrWhiteSpace(format))
                return defaultFormat;

            return format;
        }

        /// <summary>
        /// Donne une liste d'expression permettant de filtrer des ValueObject sur les mêmes dimensions sauf celle donnée en paramétre.
        /// </summary>
        /// <param name="node">ValueObject de référence</param>
        /// <param name="nomDimension">Nom de la dimension à exclure</param>
        /// <returns>Liste d'expression pouvant être utilisés comme filtre.</returns>
        /// <remarks>Chaque expression renvoit true lorsque toutes les dimensions sauf celle donnée en paramétre correspond au ValueObject de référence.</remarks>
        public IEnumerable<Expression<Func<ValueObject, bool>>> GetFilterForTreeDimension(ValueObject node, string nomDimension)
        {
            List<Expression<Func<ValueObject, bool>>> lstExp = new List<Expression<Func<ValueObject, bool>>>();

            if ((node == null) || string.IsNullOrWhiteSpace(nomDimension))
                return lstExp;

            foreach (string dim in _allDimensionsNames)
            {
                if (dim == nomDimension)
                    continue;

                string value = GetValueByDimensionName(node, dim);
                lstExp.Add(PropertyHelper.GetFilterEquality<ValueObject>(dim, value));
            }

            return lstExp;
        }


        /// <summary>
        /// Créé un dictionnaire de ValueObject avec comme clé l'id des ValueObject et présente pour chacun un tuple contenant les infos sur une dimension arborescente.
        /// </summary>
        /// <param name="nomDimension">Nom de la dimension arborescente</param>
        /// <param name="selectorInstance">SelectorInstance déterminant le périmétre des ValueObject</param>
        /// <returns>Dictionnaire des ValueObject</returns>
        /// <remarks>Le tuple présente le ValueObject, le niveau sur l'arbre donné, la valeur du noeud de l'arbre et un flag permettant de savoir si le ValueObject est éditable ou non.</remarks>
        public async Task<Dictionary<long, Tuple<ValueObject, int, string, bool>>> CreateDictionaryVO(string nomDimension, SelectorInstance selectorInstance)
        {
            await ConfigVariableDomain.LoadVariables();
            List<SelectorInstanceValueObject> lstSivoAll = await UnitOfWork.GetDbContext().SelectorInstanceValueObject
                .Include(sivo => sivo.ValueObject)
                .Where(sivo => sivo.SelectorInstanceId == selectorInstance.Id)
                .ToAsyncEnumerable()
                .ToList();

            Dictionary<long, Tuple<ValueObject, int, string, bool>> dicAllVo = new Dictionary<long, Tuple<ValueObject, int, string, bool>>();
            foreach (SelectorInstanceValueObject sivo in lstSivoAll)
            {
                if ((sivo.ValueObject == null) || (dicAllVo.ContainsKey(sivo.ValueObject.Id)))
                    continue;

                UnitOfWork.ValueObjectRepository.PrepareUpdateForObject(sivo.ValueObject);

                string valNodeTree = GetValueByDimensionName(sivo.ValueObject, nomDimension);
                int level = valNodeTree.Split(ConfigVariableDomain.AlignmentChar).Length;
                bool isEditable = sivo.IsEditable;

                dicAllVo.Add(sivo.ValueObject.Id, Tuple.Create(sivo.ValueObject, level, valNodeTree, isEditable));
            }

            return dicAllVo;
        }

        /// <summary>
        /// Construit une liste d'arbres de ValueObject basé sur un dimension.
        /// </summary>
        /// <param name="dicVO">Dictionnaire de ValueObject, avec les infos d'arborescence.</param>
        /// <param name="nomDimension">Nom de la dimension arborescente</param>
        /// <param name="topLvl">Niveau max (vers la racine) à parcourir</param>
        /// <param name="bottomLvl">Niveau min (vers les feuilles) à parcourir</param>
        /// <returns>Retourne un arbre des ValueObject</returns>
        public IEnumerable<TreeValueObject> BuildTreeVO(Dictionary<long, Tuple<ValueObject, int, string, bool>> dicVO, string nomDimension, int topLvl, int bottomLvl)
        {
            // Construction des arbres
            List<TreeValueObject> lstTree = new List<TreeValueObject>();

            HashSet<long> idOk = new HashSet<long>();
            IQueryable<ValueObject> qryVo = dicVO.Values
                .Select(val => val.Item1)
                .AsQueryable();
            foreach (long idVo in dicVO.Keys)
            {
                if (idOk.Contains(idVo))
                    continue;

                Tuple<ValueObject, int, string, bool> elt = dicVO[idVo];
                IEnumerable<Expression<Func<ValueObject, bool>>> filtres = GetFilterForTreeDimension(elt.Item1, nomDimension);
                List<ValueObject> lstVo = filtres.Aggregate(qryVo, (current, predicate) => current.Where(predicate)).ToList();
                List<TreeValueObject> parents = null;

                for (int currentLvl = topLvl; currentLvl <= bottomLvl; currentLvl++)
                {
                    IEnumerable<ValueObject> subVo = lstVo.Where(vo => dicVO[vo.Id].Item2 == currentLvl);
                    List<TreeValueObject> nextParents = new List<TreeValueObject>();
                    foreach (ValueObject voConv in subVo)
                    {
                        string valueNode = dicVO[voConv.Id].Item3;

                        TreeValueObject parent = parents?.Where(tvo => valueNode.StartsWith(tvo.TreeValue))?.FirstOrDefault();
                        TreeValueObject node = new TreeValueObject() { Node = voConv, TreeValue = valueNode, Parent = parent, Level = dicVO[voConv.Id].Item2 };
                        parent?.Children.Add(node);
                        idOk.Add(voConv.Id);
                        nextParents.Add(node);
                    }
                    parents = nextParents;
                    if (currentLvl == topLvl)
                        lstTree.AddRange(nextParents);
                }
            }

            return lstTree;
        }

        /// <summary>
        /// Permet d'importer une liste de ValueObject.
        /// </summary>
        /// <param name="valueObjects">Liste de ValueObject à enregistrer</param>
        /// <returns>Liste des ids enregistrés</returns>
        public async Task<IEnumerable<long>> Import(IEnumerable<ValueObject> valueObjects)
        {
            if (valueObjects == null)
                throw new WrongParameterException("ValueObjectDomain.Import : list of ValueObject is null.");

            foreach (ValueObject vo in valueObjects)
            {
                UnitOfWork.ValueObjectRepository.PrepareAddForObject(vo);
                vo.Status = ValueObjectStatusEnum.NotState;
            }

            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            if (nbr <= 0)
                throw new DatabaseException("ValueObjectDomain.Import : impossible to save list of ValueObject.");

            List<long> lstRet = new List<long>();
            foreach (ValueObject vo in valueObjects)
                lstRet.Add(vo.Id);

            return lstRet;
        }

        /// <summary>
        /// This function permits to get a ValueObject list by criteria in using SQL Raw.
        /// </summary>
        /// <remarks>
        /// After tests, we have opted for SQL calls directly in the database.
        /// </remarks>
        /// <param name="datasetId"></param>
        /// <param name="select"></param>
        /// <param name="where"></param>
        /// <param name="sort"></param>
        /// <param name="grouping"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public async Task<IEnumerable<ValueObject>> Filter(
            string[] select,
            string[] where = null,
            string[] sort_asc = null,
            string[] sort_desc = null,
            bool grouping = false,
            int? page = null,
            int? pageSize = null)
        {
            // Initialize the entity
            DbSet<ValueObject> valueObjectEntity = UnitOfWork.GetDbContext()
                .ValueObject;

            IEnumerable<ValueObject> valueObjects = valueObjectEntity
                .Include(i => i.DataSet)
                .AsQueryable();

            SqlRawBuilder builder = new SqlRawBuilder();

            Field fieldGroup = builder.Select.AddFieldGroup(select);
            LeftJoin leftJoin = new LeftJoin();
            Table table = new Table();

            // Grouping or not
            if (grouping)
            {
                leftJoin = fieldGroup
                     .AddField("InitialValue", FieldFormatEnum.Sum)
                     .AddField("CurrentValue", FieldFormatEnum.Sum)
                     .AddField("FutureValue", FieldFormatEnum.Sum)
                     .AddLeftJoin("ValueObject", "DataSetId", "DataSet", "Id");

                Having clause = leftJoin.AddGroupBy()
                    .AddFieldGroup(select)
                    .AddHaving();

                BuildClause(clause, where);

            }
            else
            {
                leftJoin = fieldGroup
                     .AddField("InitialValue", FieldFormatEnum.normal)
                     .AddField("CurrentValue", FieldFormatEnum.normal)
                     .AddField("FutureValue", FieldFormatEnum.normal)
                     .AddLeftJoin("ValueObject", "DataSetId", "DataSet", "Id");

                Where clause = leftJoin
                    .AddWhere(); 

                BuildClause(clause, where);
            }

            // Sorting
            if (sort_desc != null && sort_desc.Length > 0)
            {
                leftJoin.AddOrder(sort_desc, SortingEnum.Desc);
            }
            else if (sort_asc != null && sort_asc.Length > 0)
            {
                leftJoin.AddOrder(sort_asc, SortingEnum.Asc);
            }

            // Execute the script
            valueObjects = await UnitOfWork.ValueObjectRepository.ExecuteReader(builder.GetSQL);

            // Paging the results
            if (page != null && page.Value > 0 && pageSize != null && pageSize.Value > 0)
            {
                valueObjects = valueObjects.Skip(pageSize.Value * page.Value).Take(pageSize.Value);
            }

            return await Task.FromResult(valueObjects);
        }

        /// <summary>
        /// This function permits to build the clauses script.
        /// </summary>
        /// <param name="andNode"></param>
        /// <param name="where"></param>
        /// <remarks>
        /// Elements[0] => AND or OR command
        /// Elements[1] => Left parameter
        /// Elements[2] => Condition operator
        /// Elements[3] => Right parameter
        /// </remarks>
        /// <returns></returns>
        private void BuildClause(object andNode, string[] where)
        {
            foreach (string clause in where)
            {
                string[] elements = clause.Split('|');

                // Define the clause by parameter
                switch (elements[1].ToUpper().Trim())
                {
                    case "LOWER_THAN":
                        AddClause(andNode, ConditionOperatorEnum.LowerThan, elements);
                        ; break;
                    case "LOWER_OR_EQUALS":
                        AddClause(andNode, ConditionOperatorEnum.LowerOrEquals, elements);
                        ; break;
                    case "GREATER_THAN":
                        AddClause(andNode, ConditionOperatorEnum.GreaterThan, elements);
                        ; break;
                    case "GREATER_OR_EQUALS":
                        AddClause(andNode, ConditionOperatorEnum.GreaterOrEquals, elements);
                        ; break;
                    case "EQUALS":
                        AddClause(andNode, ConditionOperatorEnum.EqualsTo, elements);
                        ; break;
                    case "NOT_EQUALS":
                        AddClause(andNode, ConditionOperatorEnum.NotEqualsTo, elements);
                        ; break;
                    case "START_WITH":
                        AddClause(andNode, ConditionOperatorEnum.StartWith, elements);
                        ; break;
                    case "END_WITH":
                        AddClause(andNode, ConditionOperatorEnum.EndWith, elements);
                        ; break;
                    case "CONTAINS":
                        AddClause(andNode, ConditionOperatorEnum.Contains, elements);
                        ; break;
                    case "NOT_CONTAINS":
                        AddClause(andNode, ConditionOperatorEnum.NotContains, elements);
                        ; break;
                }
            }
        }

        /// <summary>
        /// This function permits to add a clause in WHERE or HAVING.
        /// </summary>
        /// <param name="builderFromAndNode"></param>
        /// <param name="ConditionOperatorEnum"></param>
        /// <param name="elements"></param>
        private static void AddClause(object builderFromAndNode, ConditionOperatorEnum conditionOperator, string[] elements)
        {
            if(builderFromAndNode is Where)
                ((Where)builderFromAndNode).AddAnd(elements[0], conditionOperator, elements[2]);
            else if(builderFromAndNode is Having)
                ((Having)builderFromAndNode).AddAnd(elements[0], conditionOperator, elements[2]);
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Workflow.Frontal.BusinessLogic.Models;
using Workflow.Transverse.DTO;
using Workflow.Transverse.Helpers;

namespace Workflow.Frontal.BusinessLogic.Grid
{
    /// <summary>
    /// Classe de convertion configuration TO DevExtreme
    /// </summary>
    public class GridConvertor : ClientSettings
    {
        /// <summary>
        /// SelectorInstanceId.
        /// </summary>
        public long SelectorInstanceId { get; set; }

        /// <summary>
        /// Nom de la treelist affichée.
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// List des cellules id modifiable en chaine 
        /// </summary>
        public string EditableIds { get; set; }

        /// <summary>
        /// Url de récupération des données.
        /// </summary>
        public string UrlData { get; set; }

        /// <summary>
        /// Lignes de données
        /// </summary>
        public List<Dictionary<string, object>> RowsData { get; set; }

        /// <summary>
        /// JSON pour les lignes de données (colonne de gauche compris)
        /// </summary>
        public string JsonDataRows { get; set; }

        /// <summary>
        /// Lignes de données (colonne de gauche compris)
        /// </summary>
        public List<Dictionary<string, object>> DataRows { get; set; }

        /// <summary>
        /// JSON pour les entêtes de colonnes stackées
        /// </summary>
        public string JsonStackedHeaders { get; set; }

        /// <summary>
        /// Nombre total du nombre de ligne du tableau
        /// </summary>
        public int TotalCountRows { get; set; }


        /// <summary>
        /// Indexes des colonnes de gauche, utilisées pour les dimensions en ligne.
        /// </summary>
        private Dictionary<int, KeyValuePair<InternalNameDimension, string>> _indexesLeftColumns;
        private NodeDimensionValue _treeColumnsDimension = null;
        private Dictionary<string, List<AtomicValue>> _dimensionsValuesByIndexes;
        private Dictionary<string, string> _indexesByDimensionsValues;
        private GridConfig _configurationGrid;

        private struct _EntityColumn
        {
            public string Fieldname { get; set; }
            public string HeaderText { get; set; }
            public bool ReadOnly { get; set; }
            //public ColumnType TypeField { get; set; }
        }
        
        /// <summary>
        /// Class constructor
        /// </summary>
        public GridConvertor()
        {
            _dimensionsValuesByIndexes = new Dictionary<string, List<AtomicValue>>();
            _indexesByDimensionsValues = new Dictionary<string, string>();
            _indexesLeftColumns = new Dictionary<int, KeyValuePair<InternalNameDimension, string>>();
        }


        /// <summary>
        /// Permet de construire la forme et l'entête de la grille
        /// </summary>
        /// <param name="model"></param>
        public void BuildStructureGrid(GridConfig model)
        {
            _configurationGrid = model;
            _treeColumnsDimension = ConvertConfigurationGrid(model);

            // Prepare the stacked headers for DevExtreme. 
            JsonStackedHeaders = GetJsonWithoutGuillemet(GetColumnHeaders(model, _treeColumnsDimension));
        }

        /// <summary>
        /// Construit la structure de données (RowsData) destiné à la grid Syncfusion.
        /// </summary>
        /// <param name="data"></param>
        public Dictionary<int, Dictionary<string, long>> BuildDataRowsGrid(string jsonData)
        {
            JObject jsobj = JObject.Parse(jsonData);

            var tableName = jsobj["TableName"].ToString();
            if (!string.IsNullOrWhiteSpace(tableName))
                TableName = tableName;

            var editables = JsonConvert.DeserializeObject<List<int>>(jsobj["EditablesIds"].ToString());
            if (editables != null && editables.Count > 0)
                EditableIds = string.Join(",", editables);

            if (Int32.TryParse(jsobj["TotalRows"].ToString(), out int nbTotal))
                TotalCountRows = nbTotal;

            RowsData = new List<Dictionary<string, object>>();
            Dictionary<int, Dictionary<string, long>> mapId = new Dictionary<int, Dictionary<string, long>>();
            for (int numLine = 0; jsobj[numLine.ToString()] != null && jsobj[numLine.ToString()].HasValues; numLine++)
            {
                Dictionary<string, object> line = new Dictionary<string, object>();
                Dictionary<string, object> jsLine = JsonConvert.DeserializeObject<Dictionary<string, object>>(jsobj[numLine.ToString()].ToString());

                foreach (string cle in jsLine.Keys)
                {
                    if (cle.EndsWith("_ID"))
                    {
                        string colName = cle.Substring(0, cle.Length - cle.IndexOf("_ID") - 1);
                        if (jsLine.ContainsKey($"{colName}_VAL"))
                        {
                            line.Add(colName, jsLine[$"{colName}_VAL"]);
                            long id = (long)jsLine[cle];
                            line.Add(colName + "_ID", id);
                            if (!mapId.ContainsKey(numLine + 1))
                                mapId.Add(numLine + 1, new Dictionary<string, long>());
                            mapId[numLine + 1].Add(colName, id);
                        }
                    }
                    else
                        line.Add(cle, jsLine[cle]);
                }

                line.Add("RC0", jsLine.Values.First());
                //line.Add("ID", 0);
                //line.Add("ParentID", 0);

                RowsData.Add(line);
            }

            DataRows = GetAlignments(RowsData);
            JsonDataRows = GetJson(DataRows);

            return mapId;
        }

        /// <summary>
        /// This function permits to get the left columns in a treeview format for DevExtreme. 
        /// </summary>
        /// <param name="rowsData">Date</param>
        /// <returns>List</returns>
        public List<Dictionary<string, object>> GetAlignments(List<Dictionary<string, object>> rowsData)
        {
            int id = 1;
            string idName = "ID";
            string parentIdName = "ParentID";
            string columnName = "RC0";

            rowsData[0][parentIdName] = 0;
            foreach (var row in rowsData)
            {
                string value = row[columnName].ToString();
                row.Add(idName, id);
                foreach (var other in rowsData.Where(x => x[columnName].ToString().StartsWith(value) && x[columnName].ToString() != value))
                {
                    if(other.ContainsKey(parentIdName))
                        other[parentIdName] = id;
                    else
                        other.Add(parentIdName, id);
                }

                id++;
            }

            return rowsData;
        }

        /// <summary>
        /// This function permits to get the column headers for DevExtreme. 
        /// </summary>
        /// <param name="columnNodes">Top node</param>
        /// <returns>List</returns>
        private List<ColumnHeaderModel> GetColumnHeaders(GridConfig model, NodeDimensionValue columnNodes)
        {
            List<ColumnHeaderModel> columnHeaders = new List<ColumnHeaderModel>();
            foreach (GridDimensionConfig rowDim in model.RowDimensions.OrderBy(r => r.Order))
            {
                columnHeaders.Add(new ColumnHeaderModel()
                {
                    caption = $"'{rowDim.DisplayName}'",
                    dataField = $"'Dim{(int)rowDim.InternalName}'",
                    allowEditing = false,
                    alignment = "'left'"
                });
            }
            SetColumnHeaders(columnHeaders, columnNodes.Childs, 0);

            return columnHeaders;
        }

        /// <summary>
        /// This function permits to prepare the grid for DevExtreme. 
        /// </summary>
        /// <param name="columnHeaders"></param>
        /// <param name="columnNodeChilds"></param>
        /// <param name="id"></param>
        private void SetColumnHeaders(List<ColumnHeaderModel> columnHeaders, List<NodeDimensionValue> columnNodeChilds, int id)
        {
            List<ColumnHeaderModel> newColumnHeaders = new List<ColumnHeaderModel>();
            string dataField = "";
            string cellTemplate = "";

            foreach (NodeDimensionValue columnNode in columnNodeChilds)
            {
                if (columnNode.Childs == null ||columnNode.Childs.Count < 1)
                {
                    dataField = "'C" + id + "'";
                    cellTemplate = "function(e,i){setCellValue(e,i);}";
                }
                else
                {
                    id++;
                    SetColumnHeaders(newColumnHeaders, columnNode.Childs, id);
                }

                columnHeaders.Add(new ColumnHeaderModel()
                {
                    caption = "'" + columnNode.NamedValue.Value + "'",
                    dataField = dataField,
                    cellTemplate = cellTemplate,
                    columns = newColumnHeaders,
                    alignment = "'center'"
                });
                AddIndexAndDimensionValue(dataField, NodeDimensionValue.GetPairedNamesFromNode(columnNode));

                id++;
            }
        }

        /// <summary>
        /// This function permits to format in Json without Guillemet.
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>Json</returns>
        public string GetJsonWithoutGuillemet(object model)
        {
            return JsonConvert.SerializeObject(model).Replace("\"", "");
        }

        /// <summary>
        /// This function permits to format in Json.
        /// </summary>
        /// <param name="model">Model</param>
        /// <returns>Json</returns>
        public string GetJson(object model)
        {
            return JsonConvert.SerializeObject(model);
        }

        /// <summary>
        /// Donne la liste des dimensions en colonne selon l'indexe de colonne fournie.
        /// </summary>
        /// <param name="indexColumn"></param>
        /// <returns></returns>
        public List<KeyValuePair<string, string>> GetColumnDimensionsValues(string indexColumn)
        {
            List<KeyValuePair<string, string>> dimsVals = new List<KeyValuePair<string, string>>();

            List<AtomicValue> vas = GetDimensionsValuesByIndex(indexColumn);
            if (vas == null)
                return dimsVals;

            foreach (AtomicValue va in vas)
                dimsVals.Add(new KeyValuePair<string, string>(va.DimensionName.ToString(), va.Value));

            return dimsVals;
        }

        /// <summary>
        /// Donne les dimensions de la ligne selon un indexe de ligne.
        /// </summary>
        /// <param name="indexRow"></param>
        /// <returns></returns>
        public List<KeyValuePair<string, string>> GetRowDimensionsValues(int indexRow)
        {
            List<KeyValuePair<string, string>> dimsVals = new List<KeyValuePair<string, string>>();

            if ((RowsData.Count < indexRow) || (indexRow < 0))
                return dimsVals;

            foreach (AtomicValue va in RowsData.ElementAt(indexRow).Values)
                dimsVals.Add(new KeyValuePair<string, string>(va.DimensionName.ToString(), va.Value));

            return dimsVals;
        }
        
        /// <summary>
        /// This function permits to create the flat data.
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private Dictionary<string, ValueObject> GetFlatData(List<ValueObject> data)
        {
            Dictionary<string, ValueObject> dico = new Dictionary<string, ValueObject>();

            foreach (ValueObject vo in data)
            {
                StringBuilder sb = new StringBuilder();

                sb.Append($"Dimension1:{vo.Dimension1}|");
                sb.Append($"Dimension2:{vo.Dimension2}|");
                sb.Append($"Dimension3:{vo.Dimension3}|");
                sb.Append($"Dimension4:{vo.Dimension4}|");
                sb.Append($"Dimension5:{vo.Dimension5}|");
                sb.Append($"Dimension6:{vo.Dimension6}|");
                sb.Append($"Dimension7:{vo.Dimension7}|");
                sb.Append($"Dimension8:{vo.Dimension8}|");
                sb.Append($"Dimension9:{vo.Dimension9}|");
                sb.Append($"Dimension10:{vo.Dimension10}");

                string key = sb.ToString();
                if (dico.ContainsKey(key))
                    dico[key] = vo;
                else
                    dico.Add(key, vo);
            }

            return dico;
        }

        /// <summary>
        /// This function permits to get a ValueObject from a search in a flat data.
        /// </summary>
        /// <param name="flatData"></param>
        /// <param name="search"></param>
        /// <returns></returns>
        private ValueObject GetDataFromFlatData(Dictionary<string, ValueObject> flatData, List<AtomicValue> search)
        {
            ValueObject vo = null;

            string[] dimensionsValues = new string[11] { "", "", "", "", "", "", "", "", "", "", "" } ;
            foreach (AtomicValue av in search.OrderBy(av => av.DimensionName))
            {
                int number = (int)av.DimensionName;
                dimensionsValues[number] = av.Value;
            }

            StringBuilder sb = new StringBuilder();

            sb.Append($"Dimension1:{dimensionsValues[1]}|");
            sb.Append($"Dimension2:{dimensionsValues[2]}|");
            sb.Append($"Dimension3:{dimensionsValues[3]}|");
            sb.Append($"Dimension4:{dimensionsValues[4]}|");
            sb.Append($"Dimension5:{dimensionsValues[5]}|");
            sb.Append($"Dimension6:{dimensionsValues[6]}|");
            sb.Append($"Dimension7:{dimensionsValues[7]}|");
            sb.Append($"Dimension8:{dimensionsValues[8]}|");
            sb.Append($"Dimension9:{dimensionsValues[9]}|");
            sb.Append($"Dimension10:{dimensionsValues[10]}");

            string key = sb.ToString();
            if (flatData.ContainsKey(key))
                vo = flatData[key];

            return vo;
        }

        /// <summary>
        /// Donne les paires dimension/valeurs selon l'indexe de la colonne.
        /// </summary>
        /// <param name="indexColumn"></param>
        /// <returns></returns>
        private List<AtomicValue> GetDimensionsValuesByIndex(string indexColumn)
        {
            if (_dimensionsValuesByIndexes.ContainsKey(indexColumn))
                return _dimensionsValuesByIndexes[indexColumn];
            return null;
        }

        /// <summary>
        /// Donne l'indexe de la colonne selon les paires dimension/Valeur.
        /// </summary>
        /// <param name="dimensionsValues"></param>
        /// <returns></returns>
        private string GetIndexByDimensionsValues(List<AtomicValue> dimensionsValues)
        {
            string key = null;

            if (dimensionsValues == null)
                return null;

            // Mise à plat de la liste pour la construction de la clé.
            key = dimensionsValues.Select(av => av.DimensionName.ToString() + "/" + av.DisplayName +  "=" + av.Value).Aggregate((c, n) => c + "," + n);
            if (_indexesByDimensionsValues.ContainsKey(key))
                return _indexesByDimensionsValues[key].Replace("'","");

            return null;
        }

        /// <summary>
        /// Ajoute un pont direct entre les indexes de colonne et les paires dimension/valeur
        /// </summary>
        /// <param name="indexColumn"></param>
        /// <param name="dimensionsValues"></param>
        private void AddIndexAndDimensionValue(string indexColumn, List<AtomicValue> dimensionsValues)
        {
            indexColumn = indexColumn.Replace("'", "");

            if (dimensionsValues == null || dimensionsValues.Count() < 1)
                return;

            if (_dimensionsValuesByIndexes.ContainsKey(indexColumn))
                _dimensionsValuesByIndexes[indexColumn] = dimensionsValues;
            else
                _dimensionsValuesByIndexes.Add(indexColumn, dimensionsValues);

            // Mise à plat de la liste pour la construction de la clé.
            string key = dimensionsValues.Select(av => av.DimensionName.ToString() + "/" + av.DisplayName + "=" + av.Value).Aggregate((c, n) => c + "," + n);
            if (_indexesByDimensionsValues.ContainsKey(key))
                _indexesByDimensionsValues[key] = indexColumn;
            else
                _indexesByDimensionsValues.Add(key, indexColumn);
        }

        /// <summary>
        /// This function permits to convert the config to a node structure 
        /// </summary>
        /// <param name="model"></param>
        /// <returns>NodeDimensionValue</returns>
        private NodeDimensionValue ConvertConfigurationGrid(GridConfig model)
        {
            NodeDimensionValue parent = new NodeDimensionValue();

            recursiveBuildTree(model.ColumnDimensions.OrderBy(d => d.Order), 0, parent);

            return parent;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="columns"></param>
        /// <param name="currentIndex"></param>
        /// <param name="parent"></param>
        private void recursiveBuildTree(IEnumerable<GridDimensionConfig> columns, int currentIndex, NodeDimensionValue parent)
        {
            GridDimensionConfig col = columns.ElementAt(currentIndex);

            foreach (string value in col.Values.OrderBy(o => o.Order).Select(s => s.Value).ToList())
            {
                NodeDimensionValue child = new NodeDimensionValue();
                child.NamedValue = new AtomicValue(col.InternalName, col.DisplayName, value);

                if (currentIndex < columns.Count() - 1)
                    recursiveBuildTree(columns, (currentIndex + 1), child);

                parent.Childs.Add(child);
                child.Parent = parent;
            }
        }
    }

    /// <summary>
    /// Information supplémentaire d'un noeud.
    /// </summary>
    class AtomicValue
    {
        /// <summary>
        /// Nom de la dimension
        /// </summary>
        public InternalNameDimension DimensionName { get; set; }

        /// <summary>
        /// Nom affiché de la dimension
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Valeur
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Champs modifiable ou non
        /// </summary>
        public bool ValueIsAlterable { get; set; }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="dimensionName"></param>
        /// <param name="displayName"></param>
        /// <param name="value"></param>
        public AtomicValue(InternalNameDimension dimensionName, string displayName, string value)
        {
            DimensionName = dimensionName;
            DisplayName = displayName;
            Value = value;
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="dimensionName"></param>
        /// <param name="displayName"></param>
        /// <param name="value"></param>
        /// <param name="valueIsAlterable"></param>
        public AtomicValue(InternalNameDimension dimensionName, string displayName, string value, bool valueIsAlterable)
        {
            DimensionName = dimensionName;
            DisplayName = displayName;
            Value = value;
            ValueIsAlterable = valueIsAlterable;
        }
    }

    /// <summary>
    /// Noeud de dimension utilisé pour les dimensions "stackées"
    /// </summary>
    class NodeDimensionValue
    {
        /// <summary>
        /// Enfants
        /// </summary>
        public List<NodeDimensionValue> Childs { get; set; }

        /// <summary>
        /// Parent du noeud. Null si c'est la racine.
        /// </summary>
        public NodeDimensionValue Parent { get; set; }

        /// <summary>
        /// Triplet Dimension/Valeur du noeud
        /// </summary>
        public AtomicValue NamedValue { get; set; }
        
        ///// <summary>
        ///// Nom de la dimension
        ///// </summary>
        //public NumberDimension DimensionName { get; set; }

        ///// <summary>
        ///// Nom affiché de la dimension
        ///// </summary>
        //public string DisplayName { get; set; }

        ///// <summary>
        ///// Valeur
        ///// </summary>
        //public string Value { get; set; }


        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public NodeDimensionValue()
        {
            Childs = new List<NodeDimensionValue>();
            Parent = null;
            NamedValue = null;
        }

        /// <summary>
        /// Renvoi une liste de tous les noeuds d'une arborescence.
        /// </summary>
        /// <param name="root">Noeud racine</param>
        /// <returns>Liste de tous les noeuds en partant de la racine.</returns>
        public static IEnumerable<NodeDimensionValue> GetFlatedTree(NodeDimensionValue root)
        {
            List<NodeDimensionValue> flatedNodes = new List<NodeDimensionValue>();

            flatedNodes.Add(root);
            if (root.Childs.Count() > 0)
                foreach (NodeDimensionValue child in root.Childs)
                    flatedNodes.AddRange(GetFlatedTree(child));

            return flatedNodes;
        }

        /// <summary>
        /// Fournie une liste de paires string/string (représentant le nom de la dimension et la valeur) selon le noeud donné en paramétre.
        /// </summary>
        /// <param name="node">Noeud</param>
        /// <returns>List de KeyValuePair </returns>
        public static List<AtomicValue> GetPairedNamesFromNode(NodeDimensionValue node)
        {
            List<AtomicValue> lst = new List<AtomicValue>();

            if ((node.NamedValue == null) || string.IsNullOrWhiteSpace(node.NamedValue.DisplayName) || string.IsNullOrWhiteSpace(node.NamedValue.Value))
                return lst;

            AtomicValue triplet = new AtomicValue(node.NamedValue.DimensionName, node.NamedValue.DisplayName, node.NamedValue.Value);
            lst.Add(triplet);

            if (node.Parent != null)
                lst.AddRange(GetPairedNamesFromNode(node.Parent));

            return lst;
        }
    }
}

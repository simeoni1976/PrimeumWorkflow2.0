using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Helpers;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.BusinessLayer.Domains.Interfaces
{
    /// <summary>
    ///  IValueObject interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the ValueObject business.
    /// </remarks>
    public interface IValueObjectDomain
    {
        /// <summary>
        /// This function permits to get all the ValueObject.
        /// </summary>
        /// <returns>ValueObject</returns>
        Task<ValueObject> Get(long id);

        /// <summary>
        /// This function permits to get all the ValueObject.
        /// </summary>
        /// <returns>IEnumerable</returns>
        Task<IEnumerable<ValueObject>> Get();

        /// <summary>
        /// This function permits to get the ValueObject by filter.
        /// </summary>
        /// <param name="datasetId"></param>
        /// <param name="dimension"></param>
        /// <param name="filter"></param>
        /// <param name="level"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="sortExpression"></param>
        /// <param name="sortByDesc"></param>
        /// <param name="group"></param>
        /// <returns></returns>
        Task<IEnumerable<ValueObject>> Filter(
            string[] select = null,
            string[] where = null,
            string[] sort_asc = null,
            string[] sort_desc = null,
            bool grouping = false,
            int? page = null,
            int? pageSize = null);

        /// <summary>
        /// This function permits to add a ValueObject item.
        /// </summary>
        /// <param name="valueObject">ValueObject</param>
        /// <returns>ValueObject</returns>
        Task<ValueObject> Add(ValueObject valueObject);

        /// <summary>
        /// This function permits to update a ValueObject item.
        /// </summary>
        /// <param name="valueObject">ValueObject</param>
        /// <returns>ValueObject</returns>
        Task<ValueObject> Update(ValueObject valueObject);

        /// <summary>
        /// This function permits to update an initial value for this item.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="initialValue"></param>
        /// <returns></returns>
        Task<bool> UpdateValue(long id, double initialValue);

        /// <summary>
        /// This function permits to delete a ValueObject item.
        /// </summary>
        /// <param name="valueObject">ValueObject</param>
        /// <returns>Task</returns>
        Task Delete(ValueObject valueObject);
        
        /// <summary>
        /// This function permits to delete a ValueObject item.
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Task</returns>
        Task Delete(long id);

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
        Task<HttpResponseMessageResult> ReadData(long selectorInstanceId, string[] filter, int start, int length, int sortCol, string sortDir);

        /// <summary>
        /// Remplie la liste where donnée en paramétre d'expression permettant de filtrer la table ValueObject selon les listes de CriteriaValues.
        /// </summary>
        /// <param name="where">Liste d'expression qui va recevoir les filtres (Sortie)</param>
        /// <param name="lstCriteriaValues">Listes des CriteriaValues utilisées pour les filtres</param>
        /// <param name="idsDimensionDS">Dictionnaire des DimensionDataSet par Id.</param>
        /// <param name="isLargeSearch">Créé une recherche stricte ou large (dans le cas d'arbre notamment)</param>
        /// <returns>Message de retour</returns>
        HttpResponseMessageResult BuildFilterRequest(List<Expression<Func<ValueObject, bool>>> where, IEnumerable<IEnumerable<CriteriaValues>> lstCriteriaValues, Dictionary<long, DataSetDimension> idsDimensionDS, bool isLargeSearch);

        /// <summary>
        /// Donne la valeur d'une dimension d'un ValueObject selon le nom de la dimension.
        /// </summary>
        /// <param name="vo">ValueObject</param>
        /// <param name="nomDimension">Nom de la dimension</param>
        /// <returns>Valeur de la dimension nommée.</returns>
        string GetValueByDimensionName(ValueObject vo, string nomDimension);

        /// <summary>
        /// Donne la valeur la plus actuelle d'un ValueObject.
        /// </summary>
        /// <param name="vo">ValueObject</param>
        /// <remarks>Tente de prendre la value FurutreValue si elle n'est pas nulle, sinon CurrentValue si elle n'est pas nulle, sinon InitialValue si elle n'est pas nulle.</remarks>
        /// <returns>Valeur la plus récente (Future sinon Current sinon Initial) ou si aucune valeur, NaN.</returns>
        double GetMostCurrentValue(ValueObject vo);

        /// <summary>
        /// Donne le format numérique pour un TypeValue donné.
        /// </summary>
        /// <param name="typeValueDefinition">TypeValue d'un ValueObject</param>
        /// <param name="dataSetId">Id du DataSet</param>
        /// <returns>Format numérique</returns>
        Task<string> GetNumericalFormat(string typeValueDefinition, long dataSetId);

        /// <summary>
        /// Donne une liste d'expression permettant de filtrer des ValueObject sur les mêmes dimensions sauf celle donnée en paramétre.
        /// </summary>
        /// <param name="node">ValueObject de référence</param>
        /// <param name="nomDimension">Nom de la dimension à exclure</param>
        /// <returns>Liste d'expression pouvant être utilisés comme filtre.</returns>
        /// <remarks>Chaque expression renvoit true lorsque toutes les dimensions sauf celle donnée en paramétre correspond au ValueObject de référence.</remarks>
        IEnumerable<Expression<Func<ValueObject, bool>>> GetFilterForTreeDimension(ValueObject node, string nomDimension);


        /// <summary>
        /// Permet de créer une expression filtrante pour un DbQuery sur un ValueObject, avec un nom de dimension de type arbre et une valeur.
        /// </summary>
        /// <param name="nomDimension">Nom de la dimension de type arbre à filtrer</param>
        /// <param name="value">Valeur à filtrer</param>
        /// <returns>Expression filtrante</returns>
        Expression<Func<ValueObject, bool>> HelperGetFilterByTreeDimension(string nomDimension, string value);

        /// <summary>
        /// Permet de créer une expression filtrante pour un DbQuery sur un ValueObject, avec un nom de dimension de type arbre et une liste de valeurs.
        /// </summary>
        /// <param name="nomDimension">Nom de la dimension de type arbre à filtrer</param>
        /// <param name="value">Liste de valeurs à filtrer</param>
        /// <returns>Expression filtrante</returns>
        Expression<Func<ValueObject, bool>> HelperGetFilterByTreeDimension(string nomDimension, IEnumerable<string> values);

        /// <summary>
        /// Permet de créer une expression filtrante pour un DbQuery sur un ValueObject, avec un nom de dimension et une valeur.
        /// </summary>
        /// <param name="nomDimension">Nom de la dimension à filtrer</param>
        /// <param name="value">Valeur à filtrer</param>
        /// <returns>Expression filtrante</returns>
        Expression<Func<ValueObject, bool>> HelperGetFilterByDimension(string nomDimension, string value);

        /// <summary>
        /// Permet de créer une expression filtrante pour un DbQuery sur un ValueObject, avec un nom de dimension et une liste de valeurs possibles.
        /// </summary>
        /// <param name="nomDimension">Nom de la dimension à filtrer</param>
        /// <param name="value">Liste de valeurs possibles à filtrer</param>
        /// <returns>Expression filtrante</returns>
        Expression<Func<ValueObject, bool>> HelperGetFilterByDimension(string nomDimension, IEnumerable<string> values);

        /// <summary>
        /// Créé un dictionnaire de ValueObject avec comme clé l'id des ValueObject et présente pour chacun un tuple contenant les infos sur une dimension arborescente.
        /// </summary>
        /// <param name="nomDimension">Nom de la dimension arborescente</param>
        /// <param name="selectorInstance">SelectorInstance déterminant le périmétre des ValueObject</param>
        /// <returns>Dictionnaire des ValueObject</returns>
        /// <remarks>Le tuple présente le ValueObject, le niveau sur l'arbre donné, la valeur du noeud de l'arbre et un flag permettant de savoir si le ValueObject est éditable ou non.</remarks>
        Task<Dictionary<long, Tuple<ValueObject, int, string, bool>>> CreateDictionaryVO(string nomDimension, SelectorInstance selectorInstance);

        /// <summary>
        /// Construit une liste d'arbres de ValueObject basé sur un dimension.
        /// </summary>
        /// <param name="dicVO">Dictionnaire de ValueObject, avec les infos d'arborescence.</param>
        /// <param name="nomDimension">Nom de la dimension arborescente</param>
        /// <param name="topLvl">Niveau max (vers la racine) à parcourir</param>
        /// <param name="bottomLvl">Niveau min (vers les feuilles) à parcourir</param>
        /// <returns>Retourne un arbre des ValueObject</returns>
        IEnumerable<TreeValueObject> BuildTreeVO(Dictionary<long, Tuple<ValueObject, int, string, bool>> dicVO, string nomDimension, int topLvl, int bottomLvl);


        /// <summary>
        /// Permet d'importer une liste de ValueObject.
        /// </summary>
        /// <param name="valueObjects">Liste de ValueObject à enregistrer</param>
        /// <returns>Liste des ids enregistrés</returns>
        Task<IEnumerable<long>> Import(IEnumerable<ValueObject> valueObjects);
    }
}

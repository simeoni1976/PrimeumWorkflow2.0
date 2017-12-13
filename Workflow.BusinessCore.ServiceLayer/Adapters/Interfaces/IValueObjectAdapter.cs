using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.Transverse.Helpers;
using Workflow.Transverse.DTO;

namespace Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces
{
    /// <summary>
    /// IValueAdapter interface
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the values .
    /// </remarks>
    public interface IValueObjectAdapter : IBaseAdapter<ValueObject>
    {
        /// <summary>
        /// This function permits to get by filter.
        /// </summary>
        /// <param name="datasetId"></param>
        /// <param name="select"></param>
        /// <param name="where"></param>
        /// <param name="sort_asc"></param>
        /// <param name="sort_desc"></param>
        /// <param name="grouping"></param>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        Task<IEnumerable<ValueObject>> Filter(string[] select, string[] where, string[] sort_asc, string[] sort_desc, bool grouping, int? page, int? pageSize);

        /// <summary>
        /// This function permits to update an object.
        /// </summary>
        /// <param name="id">id</param>
        /// <param name="valueObject">valueObject</param>
        /// <returns></returns>
        Task<ValueObject> Put(long id, ValueObject valueObject);

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
        /// Permet d'importer une liste de ValueObject.
        /// </summary>
        /// <param name="valueObjects">Liste de ValueObject à enregistrer</param>
        /// <returns>Liste des ids enregistrés</returns>
        Task<IEnumerable<long>> Import(IEnumerable<ValueObject> valueObjects);
    }
}
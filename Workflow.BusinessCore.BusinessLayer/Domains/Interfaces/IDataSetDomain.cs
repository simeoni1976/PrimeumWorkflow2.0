using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.BusinessLayer.Domains.Interfaces
{
    /// <summary>
    /// IDataSet interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the DataSet business.
    /// </remarks>
    public interface IDataSetDomain
    {
        /// <summary>
        /// This function permits to get all the DataSets
        /// </summary>
        /// <returns>IEnumerable</returns>
        Task<IEnumerable<DataSet>> Get();

        /// <summary>
        /// This function permits to get all the DataSets
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>DataSet</returns>
        Task<DataSet> Get(long id);

        /// <summary>
        /// This method permits to get an element in the base.
        /// </summary>
        /// <param name="name">Name</param>
        /// <returns>DataSet</returns>
        Task<DataSet> Get(string name);

        /// <summary>
        /// This function permits to add a new DataSet
        /// </summary>
        /// <param name="dataSet">DataSet</param>
        /// <returns>DataSet</returns>
        Task<DataSet> Add(DataSet dataSet);

        /// <summary>
        /// This function permits to update a DataSet
        /// </summary>
        /// <param name="dataSet">DataSet</param>
        /// <returns>DataSet</returns>
        Task<DataSet> Update(DataSet dataSet);

        /// <summary>
        /// This function permits to delete a DataSet
        /// </summary>
        /// <param name="dataSet">dataSet</param>
        /// <returns>Task</returns>
        Task Delete(DataSet dataSet);

        /// <summary>
        /// This function permits to delete a DataSet
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Task</returns>
        Task Delete(long id);

        /// <summary>
        /// Initialise les données d'un DataSet.
        /// </summary>
        /// <param name="dataset">DataSet</param>
        /// <returns>Nombre de mise à jour</returns>
        /// <remarks>Met à jour la colonne CurrentValue dans les lignes de la table ValueObject qui appartiennent au DataSet.</remarks>
        Task<int> InitializeData(DataSet dataset);

        /// <summary>
        /// Permet de générer la liste de valeur distinct d'un DataSet.
        /// </summary>
        /// <param name="dataSetId">Id du DataSet</param>
        /// <returns>Nombre d'enregistrement réussi</returns>
        /// <remarks>S'il existe déjà des données, elles sont effacées au profit des nouvelles.</remarks>
        Task<int> InitializeDistinctValue(long dataSetId);

        /// <summary>
        /// Ajout un nouveau DataSet.
        /// </summary>
        /// <param name="dataSet">Nouveau DataSet</param>
        /// <returns>Message de retour</returns>
        Task<DataSet> AddDataSet(DataSet dataSet);

        /// <summary>
        /// Ajoute un DataSetDimension à un DataSet.
        /// </summary>
        /// <param name="dataSetDimension">DataSetDimension</param>
        /// <param name="dataSetId">Id du DataSet cible</param>
        /// <param name="dimensionId">Id de la dimension à associer</param>
        /// <returns>Message de retour</returns>
        Task<DataSetDimension> AddDataSetDimension(DataSetDimension dataSetDimension, long dataSetId, long dimensionId);

        /// <summary>
        /// Récupére toutes les DistinctValue associées à un DataSet.
        /// </summary>
        /// <param name="dataSetId">Id du DataSet</param>
        /// <returns>Message de retour</returns>
        Task<IEnumerable<DistinctValue>> GetAllDistinctValue(long dataSetId);

        /// <summary>
        /// Récupére les DistinctValue associées à un DataSet d'une dimension
        /// </summary>
        /// <param name="dataSetId">Id du DataSet</param>
        /// <param name="dimensionId">Id de la Dimension</param>
        /// <returns>Message de retour</returns>
        Task<IEnumerable<DistinctValue>> GetDistinctValueByDimension(long dataSetId, long dimensionId);

        /// <summary>
        /// Lie une liste de ValueObject à un DataSet.
        /// </summary>
        /// <param name="dataSetId">Id du DataSet</param>
        /// <param name="valueObjectIds">Liste des ids des ValueObject à lier.</param>
        /// <returns>Message de retour</returns>
        Task<HttpResponseMessageResult> BindValueObjectToDataSet(long dataSetId, IEnumerable<long> valueObjectIds);


    }
}

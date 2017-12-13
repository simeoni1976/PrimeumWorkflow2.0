using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.Transverse.DTO;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces
{
    /// <summary>
    /// Interface d'adaptateur pour le controler DataSet.
    /// </summary>
    public interface IDataSetAdapter : IBaseAdapter<DataSet>
    {
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


        /// <summary>
        /// Permet de générer la liste de valeur distinct d'un DataSet.
        /// </summary>
        /// <param name="dataSetId">Id du DataSet</param>
        /// <returns>Message de retour</returns>
        /// <remarks>S'il existe déjà des données, elles sont effacées au profit des nouvelles.</remarks>
        Task<HttpResponseMessageResult> InitializeDistinctValue(long dataSetId);



    }
}

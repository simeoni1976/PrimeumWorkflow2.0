using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.DataLayer.Entities;

namespace Workflow.BusinessCore.BusinessLayer.Domains.Interfaces
{
    /// <summary>
    /// IDataSetDimension interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the DataSetDimension business.
    /// </remarks>
    public interface IDataSetDimensionDomain
    {
        /// <summary>
        /// This function permits to get all the DataSetDimensions
        /// </summary>
        /// <returns>IEnumerable</returns>
        Task<IEnumerable<DataSetDimension>> Get();

        /// <summary>
        /// This function permits to get all the DataSetDimensions by an id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>DataSetDimension</returns>
        Task<DataSetDimension> Get(long id);

        /// <summary>
        /// This function permits to get all the DataSetDimensions by an id
        /// </summary>
        /// <param name="dataSetDimension">DataSetDimension</param>
        /// <returns>DataSetDimension</returns>
        Task<DataSetDimension> Add(DataSetDimension dataSetDimension);

        /// <summary>
        /// This function permits to update DataSetDimension
        /// </summary>
        /// <param name="dataSetDimension">DataSetDimension</param>
        /// <returns>DataSetDimension</returns>
        Task<DataSetDimension> Update(DataSetDimension dataSetDimension);

        /// <summary>
        /// This function permits to delete DataSetDimension
        /// </summary>
        /// <param name="dataSetDimension">DataSetDimension</param>
        /// <returns>Task</returns>
        Task Delete(DataSetDimension dataSetDimension);

        /// <summary>
        /// This function permits to delete DataSetDimension
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>taTasksk</returns>
        Task Delete(long id);

        /// <summary>
        /// Donne un dictionnaire de DataSetDimension par rapport à un DataSet donné.
        /// </summary>
        /// <param name="dataSetId">ID du DataSet</param>
        /// <returns>Dictionnaire de DataSetDimension, avec comme clé l'Id de la dimension.</returns>
        Task<Dictionary<long, DataSetDimension>> GetDimensionColumns(long dataSetId);

    }
}

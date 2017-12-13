using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.DataLayer.Entities;

namespace Workflow.BusinessCore.BusinessLayer.Domains.Interfaces
{
    /// <summary>
    /// IDimensionScalarData interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the dimension scalar business.
    /// </remarks>
    public interface IDimensionScalarDataDomain
    {
        /// <summary>
        /// This function permits to get all the Dimensions
        /// </summary>
        /// <returns>IEnumerable</returns>
        Task<IEnumerable<DimensionScalarData>> Get();

        /// <summary>
        /// This function permits to get all the Dimensions by an id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Dimension</returns>
        Task<DimensionScalarData> Get(long id);

        /// <summary> 
        /// This function permits to add a new Dimension
        /// </summary>
        /// <param name="Dimension">Dimension</param>
        /// <returns>Dimension</returns>
        Task<DimensionScalarData> Add(DimensionScalarData Dimension);

        /// <summary>
        /// This function permits to update Dimension
        /// </summary>
        /// <param name="Dimension">Dimension</param>
        /// <returns>Dimension</returns>
        Task<DimensionScalarData> Update(DimensionScalarData Dimension);

        /// <summary>
        /// This function permits to delete Dimension
        /// </summary>
        /// <param name="Dimension">Dimension</param>
        /// <returns>Task</returns>
        Task Delete(DimensionScalarData Dimension);

        /// <summary>
        /// This function permits to delete Dimension
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Task</returns>
        Task Delete(long id);
    }
}

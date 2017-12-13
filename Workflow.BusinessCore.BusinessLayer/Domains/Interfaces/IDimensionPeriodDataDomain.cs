using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.DataLayer.Entities;

namespace Workflow.BusinessCore.BusinessLayer.Domains.Interfaces
{
    /// <summary>
    /// IDimension interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the dimension period business.
    /// </remarks>
    public interface IDimensionPeriodDataDomain
    {
        /// <summary>
        /// This function permits to get all the Dimensions
        /// </summary>
        /// <returns>IEnumerable</returns>
        Task<IEnumerable<DimensionPeriodData>> Get();

        /// <summary>
        /// This function permits to get all the Dimensions by an id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Dimension</returns>
        Task<DimensionPeriodData> Get(long id);

        /// <summary> 
        /// This function permits to add a new Dimension
        /// </summary>
        /// <param name="Dimension">Dimension</param>
        /// <returns>Dimension</returns>
        Task<DimensionPeriodData> Add(DimensionPeriodData Dimension);

        /// <summary>
        /// This function permits to update Dimension
        /// </summary>
        /// <param name="Dimension">Dimension</param>
        /// <returns>Dimension</returns>
        Task<DimensionPeriodData> Update(DimensionPeriodData Dimension);

        /// <summary>
        /// This function permits to delete Dimension
        /// </summary>
        /// <param name="Dimension">Dimension</param>
        /// <returns>Task</returns>
        Task Delete(DimensionPeriodData Dimension);

        /// <summary>
        /// This function permits to delete Dimension
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Task</returns>
        Task Delete(long id);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.DataLayer.Entities;

namespace Workflow.BusinessCore.BusinessLayer.Domains.Interfaces
{
    /// <summary>
    /// IDimensionGroupData interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the dimension groupdata business.
    /// </remarks>
    public interface IDimensionGroupDataDomain
    {
        /// <summary>
        /// This function permits to get all the Dimensions
        /// </summary>
        /// <returns>IEnumerable</returns>
        Task<IEnumerable<DimensionGroupData>> Get();

        /// <summary>
        /// This function permits to get all the Dimensions by an id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Dimension</returns>
        Task<DimensionGroupData> Get(long id);

        /// <summary> 
        /// This function permits to add a new Dimension
        /// </summary>
        /// <param name="Dimension">Dimension</param>
        /// <returns>Dimension</returns>
        Task<DimensionGroupData> Add(DimensionGroupData Dimension);

        /// <summary>
        /// This function permits to update Dimension
        /// </summary>
        /// <param name="Dimension">Dimension</param>
        /// <returns>Dimension</returns>
        Task<DimensionGroupData> Update(DimensionGroupData Dimension);

        /// <summary>
        /// This function permits to delete Dimension
        /// </summary>
        /// <param name="Dimension">Dimension</param>
        /// <returns>Task</returns>
        Task Delete(DimensionGroupData Dimension);

        /// <summary>
        /// This function permits to delete Dimension
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Task</returns>
        Task Delete(long id);
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.DataLayer.Entities;

namespace Workflow.BusinessCore.BusinessLayer.Domains.Interfaces
{
    /// <summary>
    /// IDimensionValues interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the dimension values business.
    /// </remarks>
    public interface IDimensionValuesDomain
    {
        /// <summary>
        /// This function permits to get all the Dimensions
        /// </summary>
        /// <returns>IEnumerable</returns>
        Task<IEnumerable<DimensionValues>> Get();

        /// <summary>
        /// This function permits to get all the Dimensions by an id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Dimension</returns>
        Task<DimensionValues> Get(long id);

        /// <summary> 
        /// This function permits to add a new Dimension
        /// </summary>
        /// <param name="Dimension">Dimension</param>
        /// <returns>Dimension</returns>
        Task<DimensionValues> Add(DimensionValues Dimension);

        /// <summary>
        /// This function permits to update Dimension
        /// </summary>
        /// <param name="Dimension">Dimension</param>
        /// <returns>Dimension</returns>
        Task<DimensionValues> Update(DimensionValues Dimension);

        /// <summary>
        /// This function permits to delete Dimension
        /// </summary>
        /// <param name="Dimension">Dimension</param>
        /// <returns>Task</returns>
        Task Delete(DimensionValues Dimension);

        /// <summary>
        /// This function permits to delete Dimension
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Task</returns>
        Task Delete(long id);
    }
}

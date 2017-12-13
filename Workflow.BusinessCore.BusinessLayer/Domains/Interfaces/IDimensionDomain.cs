using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.BusinessLayer.Domains.Interfaces
{
    /// <summary>
    /// IDimension interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the dimension business.
    /// </remarks>
    public interface IDimensionDomain
    {
        /// <summary>
        /// This function permits to get all the Dimensions
        /// </summary>
        /// <returns>IEnumerable</returns>
        Task<IEnumerable<Dimension>> Get();

        /// <summary>
        /// This function permits to get all the Dimensions by an id
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Dimension</returns>
        Task<Dimension> Get(long id);



        /// <summary>
        /// Ajoute une dimension.
        /// </summary>
        /// <param name="dimension">Nouvelle Dimension</param>
        /// <returns>Message de retour</returns>
        Task<Dimension> AddDimension(Dimension dimension);
    }
}

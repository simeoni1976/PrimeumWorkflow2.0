using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.Transverse.DTO.Common;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces
{
    /// <summary>
    ///  IBaseAdapter interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the adapters.
    /// </remarks>
    /// <typeparam name="T">DTO model</typeparam>
    public interface IBaseAdapter<TModel>
        where TModel : BaseDTO
    {

        /// <summary>
        /// Récupére toutes les entités DTO existantes.
        /// </summary>
        /// <returns>liste des entités</returns>
        Task<IEnumerable<TModel>> GetAll();

        /// <summary>
        /// Récupére l'entité désignée par l'id en paramétre.
        /// </summary>
        /// <param name="id">Id de l'entité</param>
        /// <returns>entité</returns>
        Task<TModel> GetById(long id);
    }
}
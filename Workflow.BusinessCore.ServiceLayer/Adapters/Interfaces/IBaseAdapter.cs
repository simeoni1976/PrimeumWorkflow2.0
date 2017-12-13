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
        /// R�cup�re toutes les entit�s DTO existantes.
        /// </summary>
        /// <returns>liste des entit�s</returns>
        Task<IEnumerable<TModel>> GetAll();

        /// <summary>
        /// R�cup�re l'entit� d�sign�e par l'id en param�tre.
        /// </summary>
        /// <param name="id">Id de l'entit�</param>
        /// <returns>entit�</returns>
        Task<TModel> GetById(long id);
    }
}
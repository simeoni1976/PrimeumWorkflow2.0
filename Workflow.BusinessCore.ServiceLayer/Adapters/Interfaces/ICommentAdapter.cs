using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.Transverse.Helpers;
using DTO = Workflow.Transverse.DTO;

namespace Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces
{
    /// <summary>
    ///  ICommentAdapter interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the Comment adapter.
    /// </remarks>
    public interface ICommentAdapter : IBaseAdapter<DTO.Comment>
    {
        /// <summary>
        /// Ajoute un commentaire en base
        /// </summary>
        /// <param name="comment">Commentaire</param>
        /// <returns>Message de retour</returns>
        Task<HttpResponseMessageResult> Post(DTO.Comment comment);
    }
}

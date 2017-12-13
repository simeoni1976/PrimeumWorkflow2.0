using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.DataLayer.Entities;

namespace Workflow.BusinessCore.BusinessLayer.Domains.Interfaces
{
    /// <summary>
    /// IComment interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the Comment business.
    /// </remarks>
    public interface ICommentDomain
    {
        /// <summary>
        /// This function permits to get all the Comment.
        /// </summary>
        /// <returns>Comment</returns>
        Task<Comment> Get(long id);

        /// <summary>
        /// This function permits to get all the Comment.
        /// </summary>
        /// <returns>IEnumerable</returns>
        Task<IEnumerable<Comment>> Get();

        /// <summary>
        /// This function permits to add a Comment item.
        /// </summary>
        /// <param name="Comment">Comment</param>
        /// <returns>Comment</returns>
        Task<Comment> Add(Comment Comment);

        /// <summary>
        /// This function permits to update a Comment item.
        /// </summary>
        /// <param name="Comment">Comment</param>
        /// <returns>Comment</returns>
        Task<Comment> Update(Comment Comment);

        /// <summary>
        /// This function permits to delete a Comment item.
        /// </summary>
        /// <param name="Comment">Comment</param>
        /// <returns>Task</returns>
        Task Delete(Comment Comment);

        /// <summary>
        /// This function permits to delete a Comment item.
        /// </summary>
        /// <param name="id">Id</param>
        /// <returns>Task</returns>
        Task Delete(long id);
    }
}

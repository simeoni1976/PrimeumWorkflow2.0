using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Common;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.BusinessLayer.UnitOfWork.Interfaces;
using Workflow.BusinessCore.DataLayer.Entities;

namespace Workflow.BusinessCore.BusinessLayer.Domains
{
    /// <summary>
    /// CommentDomain domain class
    /// </summary>
    public class CommentDomain :  AbstractDomain<Comment>, ICommentDomain
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="unitOfWork">Unit Of Work</param>
        public CommentDomain(IUnitOfWork unitOfWork) : base(unitOfWork.CommentRepository)
        {
        }
    }
}

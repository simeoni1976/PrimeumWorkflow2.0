using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using Workflow.Transverse.Helpers;
using DTO = Workflow.Transverse.DTO;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Workflow.BusinessCore.ServiceLayer.Controllers
{
    /// <summary>
    /// CommentController class.
    /// </summary>
    public class CommentController : BaseController<DTO.Comment, ICommentAdapter>
    {
        protected override ICommentAdapter Adapter
        {
            get
            {
                return _serviceProvider.GetService<ICommentAdapter>();
            }
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de services</param>
        public CommentController(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
        }
    }
}

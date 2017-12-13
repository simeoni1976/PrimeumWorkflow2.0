using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.ServiceLayer.Adapters.Interfaces;
using DTO = Workflow.Transverse.DTO;
using ENT = Workflow.BusinessCore.DataLayer.Entities;
using Workflow.Transverse.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Workflow.BusinessCore.ServiceLayer.Adapters
{
    /// <summary>
    /// CommentAdapter interface.
    /// </summary>
    /// <remarks>
    /// This interface permits to define all methods for the Comment adapter.
    /// </remarks>
    public class CommentAdapter : ICommentAdapter
    {
        private readonly IServiceProvider _serviceProvider = null;

        private ICommentDomain CommentDomain
        {
            get
            {
                return _serviceProvider?.GetService<ICommentDomain>();
            }
        }

        private IMapper Mapper
        {
            get
            {
                return _serviceProvider?.GetService<IMapper>();
            }
        }

        /// <summary>
        /// Constructeur
        /// </summary>
        /// <param name="serviceProvider">Fournisseur de services</param>
        public CommentAdapter(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }


        /// <summary>
        /// Récupére toutes les entités DTO existantes.
        /// </summary>
        /// <returns>Message de retour avec la liste en json</returns>
        public async Task<IEnumerable<DTO.Comment>> GetAll()
        {
            IEnumerable<ENT.Comment> comments = await CommentDomain.Get();

            IEnumerable<DTO.Comment> dtoComments = null;
            if (comments != null)
                dtoComments = Mapper.Map<IEnumerable<ENT.Comment>, IEnumerable<DTO.Comment>>(comments);
            else
                dtoComments = new List<DTO.Comment>();

            return dtoComments;
        }

        /// <summary>
        /// Récupére l'entité désignée par l'id en paramétre.
        /// </summary>
        /// <param name="id">Id de l'entité</param>
        /// <returns>Message de retour avec l'entité</returns>
        public async Task<DTO.Comment> GetById(long id)
        {
            ENT.Comment comment = await CommentDomain.Get(id);

            DTO.Comment dtoComment = null;
            if (comment != null)
                dtoComment = Mapper.Map<ENT.Comment, DTO.Comment>(comment);

            return dtoComment;
        }


        /// <summary>
        /// Ajoute un commentaire en base
        /// </summary>
        /// <param name="comment">Commentaire</param>
        /// <returns>Message de retour</returns>
        public async Task<HttpResponseMessageResult> Post(DTO.Comment comment)
        {
            ENT.Comment element = Mapper.Map<DTO.Comment, ENT.Comment>(comment);

            comment = Mapper.Map<ENT.Comment, DTO.Comment>(await CommentDomain.Add(element));

            HttpResponseMessageResult res = new HttpResponseMessageResult() { IsSuccess = true };
            res.Json = JsonConvert.SerializeObject(comment);

            return res;
        }

    }
}

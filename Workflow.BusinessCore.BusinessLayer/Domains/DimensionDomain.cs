using AutoMapper;
using System;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Common;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.BusinessLayer.Process.Exceptions;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.BusinessCore.BusinessLayer.UnitOfWork.Interfaces;
using Workflow.Transverse.Helpers;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace Workflow.BusinessCore.BusinessLayer.Domains
{
    /// <summary>
    /// Dimension domain class.
    /// </summary>
    public class DimensionDomain : AbstractDomain<Dimension>, IDimensionDomain
    {
        private readonly IServiceProvider _serviceProvider = null;

        private IUnitOfWork UnitOfWork
        {
            get
            {
                return _serviceProvider?.GetService<IUnitOfWork>();
            }
        }

        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="unitOfWork">Unit Of Work</param>
        /// <param name="mapper">Mapper</param>
        public DimensionDomain(IUnitOfWork unitOfWork, IServiceProvider serviceProvider) : base(unitOfWork.DimensionRepository)
        {
            _serviceProvider = serviceProvider;
        }


        /// <summary>
        /// Ajoute une dimension.
        /// </summary>
        /// <param name="dimension">Nouvelle Dimension</param>
        /// <returns>Message de retour</returns>
        public async Task<Dimension> AddDimension(Dimension dimension)
        {
            if (dimension == null)
                throw new WrongParameterException("DimensionDomain.AddDimension : Dimension is null.");
            if (string.IsNullOrWhiteSpace(dimension.Name))
                throw new WrongParameterException("DimensionDomain.AddDimension : Dimension's name is empty.");
            if (dimension.Id > 0)
                throw new WrongParameterException($"DimensionDomain.AddDimension : Dimension for add has already an Id ({dimension.Id}).");

            UnitOfWork.DimensionRepository.PrepareAddForObject(dimension);
            int nbr = await UnitOfWork.GetDbContext().SaveChangesAsync();

            if (nbr <= 0)
                throw new DatabaseException("DimensionDomain.AddDimension : impossible to save Dimension.");

            return dimension;
        }

    }
}

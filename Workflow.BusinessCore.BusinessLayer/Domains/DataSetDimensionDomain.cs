using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Workflow.BusinessCore.BusinessLayer.Common;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.BusinessCore.BusinessLayer.UnitOfWork.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Workflow.BusinessCore.BusinessLayer.Domains
{
    /// <summary>
    /// DataSetDimension domain class
    /// </summary>
    public class DataSetDimensionDomain : AbstractDomain<DataSetDimension>, IDataSetDimensionDomain
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
        public DataSetDimensionDomain(IUnitOfWork unitOfWork, IServiceProvider serviceProvider) : base(unitOfWork.DataSetDimensionRepository)
        {
            _serviceProvider = serviceProvider;
        }


        /// <summary>
        /// Donne un dictionnaire de DataSetDimension par rapport à un DataSet donné.
        /// </summary>
        /// <param name="dataSetId">ID du DataSet</param>
        /// <returns>Dictionnaire de DataSetDimension, avec comme clé l'Id de la dimension.</returns>
        public async Task<Dictionary<long, DataSetDimension>> GetDimensionColumns(long dataSetId)
        {
            List<DataSetDimension> dims = await UnitOfWork.GetDbContext().DataSetDimension
                .Include(dsd => dsd.DataSet)
                .Include(dsd => dsd.Dimension)
                .Where(dsd => dsd.DataSet.Id == dataSetId)
                .AsNoTracking()
                .ToAsyncEnumerable()
                .ToList();

            Dictionary<long, DataSetDimension> dimIds = dims.ToDictionary(elt => elt.Id, elt => elt);
            return dimIds;
        }

    }
}

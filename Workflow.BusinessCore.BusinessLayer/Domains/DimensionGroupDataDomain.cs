using System;
using System.Threading.Tasks;
using AutoMapper;
using Workflow.BusinessCore.BusinessLayer.Common;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.BusinessCore.BusinessLayer.UnitOfWork.Interfaces;

namespace Workflow.BusinessCore.BusinessLayer.Domains
{
    /// <summary>
    /// Dimension groupdata domain class.
    /// </summary>
    public class DimensionGroupDataDomain : AbstractDomain<DimensionGroupData>, IDimensionGroupDataDomain
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="unitOfWork">Unit Of Work</param>
        /// <param name="mapper">Mapper</param>
        public DimensionGroupDataDomain(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork.DimensionGroupDataRepository)
        {
        }
    }
}

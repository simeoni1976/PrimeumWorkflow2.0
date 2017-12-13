using AutoMapper;
using Workflow.BusinessCore.BusinessLayer.Common;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.BusinessCore.BusinessLayer.UnitOfWork.Interfaces;

namespace Workflow.BusinessCore.BusinessLayer.Domains
{
    /// <summary>
    /// Dimension values domain class.
    /// </summary>
    public class DimensionValuesDomain : AbstractDomain<DimensionValues>, IDimensionValuesDomain
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="unitOfWork">Unit Of Work</param>
        /// <param name="mapper">Mapper</param>
        public DimensionValuesDomain(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork.DimensionValuesRepository)
        {
        }
    }
}

using AutoMapper;
using Workflow.BusinessCore.BusinessLayer.Common;
using Workflow.BusinessCore.BusinessLayer.Domains.Interfaces;
using Workflow.BusinessCore.DataLayer.Entities;
using Workflow.BusinessCore.BusinessLayer.UnitOfWork.Interfaces;

namespace Workflow.BusinessCore.BusinessLayer.Domains
{
    /// <summary>
    /// Dimension tree domain class.
    /// </summary>
    public class DimensionTreeDataDomain : AbstractDomain<DimensionTreeData>, IDimensionTreeDataDomain
    {
        /// <summary>
        /// Class constructor.
        /// </summary>
        /// <param name="unitOfWork">Unit Of Work</param>
        /// <param name="mapper">Mapper</param>
        public DimensionTreeDataDomain(IUnitOfWork unitOfWork, IMapper mapper) : base(unitOfWork.DimensionTreeDataRepository)
        {
        }
    }
}

using System.Collections.Generic;
using Workflow.Transverse.DTO.Common;
using Workflow.Transverse.Helpers;

namespace Workflow.Transverse.DTO
{
    /// <summary>
    /// Dimension DTO class.
    /// </summary>
    public class Dimension : BaseDTO
    {
        /// <summary>
        /// Name property.
        /// </summary>
        /// <value>
        /// Gets or sets the Name value.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Criteria property.
        /// </summary>
        /// <value>
        /// Gets or sets the Criteria value.
        /// </value>
        public ICollection<Criteria> Criteria { get; set; }

        /// <summary>
        /// DataSetDimension property.
        /// </summary>
        /// <value>
        /// Gets or sets the DataSetDimension value.
        /// </value>
        public ICollection<DataSetDimension> DataSetDimension { get; set; }

        /// <summary>
        /// WorkflowDimension property.
        /// </summary>
        /// <value>
        /// Gets or sets the WorkflowDimension value.
        /// </value>
        public ICollection<WorkflowDimension> WorkflowDimension { get; set; }

        /// <summary>
        /// Type property.
        /// </summary>
        /// <value>
        /// Gets or sets the Type value.
        /// </value>
        public DimensionTypeEnum TypeDimension { get; set; }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public Dimension()
        {
        }
    }
}

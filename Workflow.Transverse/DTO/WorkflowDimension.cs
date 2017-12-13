
using Workflow.Transverse.DTO.Common;

namespace Workflow.Transverse.DTO
{
    /// <summary>
    /// WorkflowDimension DTO class.
    /// </summary>
    public class WorkflowDimension : BaseDTO
    {
        /// <summary>
        /// WorkflowConfig property.
        /// </summary>
        /// <value>
        /// Gets or sets the WorkflowConfig value.
        /// </value>
        public WorkflowConfig WorkflowConfig { get; set; }

        /// <summary>
        /// Dimension property.
        /// </summary>
        /// <value>
        /// Gets or sets the Dimension value.
        /// </value>
        public Dimension Dimension { get; set; }

        /// <summary>
        /// ColumnName property.
        /// </summary>
        /// <value>
        /// Gets or sets the ColumnName value.
        /// </value>
        public string ColumnName { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        public WorkflowDimension()
        {
        }
    }
}
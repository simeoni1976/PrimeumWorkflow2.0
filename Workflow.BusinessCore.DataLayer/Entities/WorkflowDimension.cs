using Workflow.BusinessCore.DataLayer.Common;

namespace Workflow.BusinessCore.DataLayer.Entities
{
    /// <summary>
    /// WorkflowDimension Entity class.
    /// </summary>
    public class WorkflowDimension : BaseEntity
    {
        /// <summary>
        /// WorkflowConfig property.
        /// </summary>
        /// <value>
        /// Gets or sets the WorkflowConfig value.
        /// </value>
        public virtual WorkflowConfig WorkflowConfig { get; set; }

        /// <summary>
        /// Dimension property.
        /// </summary>
        /// <value>
        /// Gets or sets the Dimension value.
        /// </value>
        public virtual Dimension Dimension { get; set; }

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
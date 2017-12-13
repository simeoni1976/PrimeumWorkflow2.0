
using Workflow.Transverse.DTO.Common;

namespace Workflow.Transverse.DTO
{
    /// <summary>
    /// WorkflowOption DTO class.
    /// </summary>
    public class WorkflowOption : BaseDTO
    {
        /// <summary>
        /// WorkflowId property.
        /// </summary>
        /// <value>
        /// Gets or sets the WorkflowId value.
        /// </value>
        public long WorkflowId { get; set; }

        /// <summary>
        /// OptionId property.
        /// </summary>
        /// <value>
        /// Gets or sets the OptionId value.
        /// </value>
        public long OptionId { get; set; }

        /// <summary>
        /// WorkflowOption property.
        /// </summary>
        /// <value>
        /// Gets or sets the WorkflowOption value.
        /// </value>
        public WorkflowOption Option { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        public WorkflowOption()
        {
        }
    }
}

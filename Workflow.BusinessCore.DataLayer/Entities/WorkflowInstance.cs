using System.Collections.Generic;
using Workflow.BusinessCore.DataLayer.Common;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.DataLayer.Entities
{
    /// <summary>
    /// WorkflowInstance class.
    /// </summary>
    /// <remarks>
    /// This class permits to instance the current Workflow.
    /// </remarks>
    public class WorkflowInstance : BaseEntity
    {
        /// <summary>
        /// Name property.
        /// </summary>
        /// <value>
        /// Gets or sets the Name value.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// WorkflowConfig property.
        /// </summary>
        /// <value>
        /// Gets or sets the WorkflowConfig value.
        /// </value>
        public virtual WorkflowConfig WorkflowConfig { get; set; }

        /// <summary>
        /// SelectorInstance property.
        /// </summary>
        /// <value>
        /// Gets or sets the SelectorInstance value.
        /// </value>
        public virtual ICollection<SelectorInstance> SelectorInstance { get; set; }

        /// <summary>
        /// DataSetId property.
        /// </summary>
        /// <value>
        /// Gets or sets the DataSetId value.
        /// </value>
        /// <remarks>
        /// WorkflowInstance reference for the [DataSet] prop. 
        /// Compulsory field for .NET Core one-to-zero-one relationship.
        /// </remarks>
        public long DataSetId { get; set; }


        /// <summary>
        /// Id du UserSet
        /// </summary>
        /// <value>
        /// Gets or sets the UserSetId value.
        /// </value>
        /// <remarks>
        /// Même remarque que le DataSetId.
        /// WorkflowInstance reference for the [DataSet] prop. 
        /// Compulsory field for .NET Core one-to-zero-one relationship.
        /// </remarks>
        public long UserSetId { get; set; }

        /// <summary>
        /// Status property.
        /// </summary>
        /// <value>
        /// Gets or sets the Status value.
        /// </value>
        public WorkflowStateEnum? Status { get; set; }

        /// <summary>
        /// ModifyUser property.
        /// </summary>
        /// <value>
        /// Gets or sets the ModifyUser value.
        /// </value>
        public User ModifyUser { get; set; }

        /// <summary>
        /// ValidateUser property.
        /// </summary>
        /// <value>
        /// Gets or sets the ValidateUser value.
        /// </value>
        public User ValidateUser { get; set; }

        /// <summary>
        /// Constraints property.
        /// </summary>
        /// <value>
        /// Gets or sets the Constraints value.
        /// </value>
        public string Constraints { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        public WorkflowInstance()
        {
            SelectorInstance = new HashSet<SelectorInstance>();
        }
    }
}

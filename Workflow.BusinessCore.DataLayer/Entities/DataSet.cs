using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Workflow.BusinessCore.DataLayer.Common;

namespace Workflow.BusinessCore.DataLayer.Entities
{
    /// <summary>
    /// DataSet class.
    /// </summary>
    public class DataSet : BaseEntity
    {
        /// <summary>
        /// Name property.
        /// </summary>
        /// <value>
        /// Gets or sets the Name value.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// ValueObjects property.
        /// </summary>
        /// <value>
        /// Gets or sets the ValueObjects value.
        /// </value>
        public virtual ICollection<ValueObject> ValueObjects { get; set; }

        /// <summary>
        /// DataSetDimension property.
        /// </summary>
        /// <value>
        /// Gets or sets the DataSetDimension value.
        /// </value>
        public virtual ICollection<DataSetDimension> DataSetDimensions { get; set; }

        /// <summary>
        /// WorkflowInstanceId property.
        /// </summary>
        /// <value>
        /// Gets or sets the WorkflowInstanceId value.
        /// </value>
        /// <remarks>
        /// One to zero-one relation
        /// WorkflowInstance reference for the [WorkflowInstance] prop.
        /// </remarks>
        public long? WorkflowInstanceId { get; set; }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public DataSet()
        {
            ValueObjects = new HashSet<ValueObject>();
            DataSetDimensions = new HashSet<DataSetDimension>();
        }
    }
}

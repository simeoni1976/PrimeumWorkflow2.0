using System;
using System.Collections.Generic;
using Workflow.Transverse.DTO.Common;

namespace Workflow.Transverse.DTO
{
    /// <summary>
    /// DataSet DTO class.
    /// </summary>
    public class DataSet : BaseDTO
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
        public ICollection<ValueObject> ValueObjects { get; set; }

        /// <summary>
        /// DataSetDimension property.
        /// </summary>
        /// <value>
        /// Gets or sets the DataSetDimension value.
        /// </value>
        public ICollection<DataSetDimension> DataSetDimensions { get; set; }

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
        }
    }
}

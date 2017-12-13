using System.Collections.Generic;
using Workflow.Transverse.DTO;
using Workflow.Transverse.DTO.Common;

namespace Workflow.Transverse.DTO
{
    /// <summary>
    /// DataSetDimension class.
    /// </summary>
    public class DataSetDimension : BaseDTO
    {
        /// <summary>
        /// Dimension property.
        /// </summary>
        /// <value>
        /// Gets or sets the Dimension value.
        /// </value>
        public string ColumnName { get; set; }

        /// <summary>
        /// Dimension property.
        /// </summary>
        /// <value>
        /// Gets or sets the Dimension value.
        /// </value>
        public Dimension Dimension { get; set; }

        /// <summary>
        /// DataSet property.
        /// </summary>
        /// <value>
        /// Gets or sets the DataSet value.
        /// </value>
        public DataSet DataSet { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        public DataSetDimension()
        {
        }
    }
}

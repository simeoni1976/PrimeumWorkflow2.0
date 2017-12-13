
using Workflow.BusinessCore.DataLayer.Common;

namespace Workflow.BusinessCore.DataLayer.Entities
{
    /// <summary>
    /// DataSetDimension class.
    /// </summary>
    public class DataSetDimension : BaseEntity
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
        public virtual Dimension Dimension { get; set; }

        /// <summary>
        /// DataSet property.
        /// </summary>
        /// <value>
        /// Gets or sets the DataSet value.
        /// </value>
        public virtual DataSet DataSet { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        public DataSetDimension()
        {
        }
    }
}

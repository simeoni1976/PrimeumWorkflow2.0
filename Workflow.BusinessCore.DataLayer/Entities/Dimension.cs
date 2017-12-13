using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Workflow.BusinessCore.DataLayer.Common;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.DataLayer.Entities
{
    /// <summary>
    /// Dimension class.
    /// </summary>
    public class Dimension : BaseEntity
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
        public virtual ICollection<Criteria> Criteria { get; set; }

        /// <summary>
        /// DataSetDimension property.
        /// </summary>
        /// <value>
        /// Gets or sets the DataSetDimension value.
        /// </value>
        public virtual ICollection<DataSetDimension> DataSetDimension { get; set; }

        /// <summary>
        /// WorkflowDimension property.
        /// </summary>
        /// <value>
        /// Gets or sets the WorkflowDimension value.
        /// </value>
        public virtual ICollection<WorkflowDimension> WorkflowDimension { get; set; }

        /// <summary>
        /// Type de dimension
        /// </summary>
        /// <value>Enumération du type de la dimension</value>
        public virtual DimensionTypeEnum TypeDimension { get; set; }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public Dimension()
        {
            Criteria = new HashSet<Criteria>();
            DataSetDimension = new HashSet<DataSetDimension>();
            WorkflowDimension = new HashSet<WorkflowDimension>();
        }
    }
}

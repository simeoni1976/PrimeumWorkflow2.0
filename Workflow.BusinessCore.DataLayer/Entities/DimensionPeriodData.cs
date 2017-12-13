using System.Collections.Generic;
using Workflow.BusinessCore.DataLayer.Common;

namespace Workflow.BusinessCore.DataLayer.Entities
{
    /// <summary>
    /// DimensionPeriodData class
    /// </summary>
    public class DimensionPeriodData : BaseEntity
    {
        /// <summary>
        /// Valeur clé de référence, celle que l'on retrouve dans les colonnes DimensionX de ValueObject
        /// </summary>
        public string ValueKey { get; set; }

        /// <summary>
        /// Dimensions property.
        /// </summary>
        /// <value>
        /// Gets or sets the Dimensions value.
        /// </value>
        public virtual ICollection<Dimension> Dimensions { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        public DimensionPeriodData()
        {
            Dimensions = new HashSet<Dimension>();
        }
    }
}

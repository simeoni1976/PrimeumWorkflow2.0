using System.Collections.Generic;
using Workflow.Transverse.DTO.Common;

namespace Workflow.Transverse.DTO
{
    /// <summary>
    /// DimensionGroupData class.
    /// </summary>
    public class DimensionGroupData : BaseDTO
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
        public DimensionGroupData()
        {
            Dimensions = new HashSet<Dimension>();
        }
    }
}

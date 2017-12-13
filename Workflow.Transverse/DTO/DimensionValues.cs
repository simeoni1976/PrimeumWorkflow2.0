using System;
using System.Collections.Generic;
using System.Text;
using Workflow.Transverse.DTO.Common;

namespace Workflow.Transverse.DTO
{
    /// <summary>
    /// DimensionValues class
    /// </summary>
    public class DimensionValues : BaseDTO
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
        public DimensionValues()
        {
            Dimensions = new HashSet<Dimension>();
        }
    }
}

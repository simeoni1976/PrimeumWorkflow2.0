using System;
using System.Collections.Generic;
using System.Text;
using Workflow.Transverse.DTO.Common;

namespace Workflow.Transverse.DTO
{
    public class GridConfig : BaseDTO
    {
        /// <summary>
        /// Nom de la config
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Dimensions en colonne (en haut de façon horizontales)
        /// </summary>
        public ICollection<GridDimensionConfig> ColumnDimensions { get; set; }

        /// <summary>
        /// Dimensions en ligne (à gauche de façon verticales)
        /// </summary>
        public ICollection<GridDimensionConfig> RowDimensions { get; set; }

        /// <summary>
        /// Dimension hors grid, dont une valeur est fixée.
        /// </summary>
        public ICollection<GridDimensionConfig> FixedDimensions { get; set; }

        /// <summary>
        /// WorkflowConfig lié.
        /// </summary>
        public WorkflowConfig WorkflowConfig { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using Workflow.BusinessCore.DataLayer.Common;

namespace Workflow.BusinessCore.DataLayer.Entities
{
    public class GridConfig : BaseEntity
    {
        /// <summary>
        /// Nom de la config
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Dimensions en colonne (en haut de façon horizontales)
        /// </summary>
        [InverseProperty("GridColumn")]
        public ICollection<GridDimensionConfig> ColumnDimensions { get; set; }

        /// <summary>
        /// Dimensions en ligne (à gauche de façon verticales)
        /// </summary>
        [InverseProperty("GridRow")]
        public ICollection<GridDimensionConfig> RowDimensions { get; set; }

        /// <summary>
        /// Dimension hors grid, dont une valeur est fixée.
        /// </summary>
        [InverseProperty("GridFixed")]
        public ICollection<GridDimensionConfig> FixedDimensions { get; set; }

        /// <summary>
        /// WorkflowConfig lié.
        /// </summary>
        public WorkflowConfig WorkflowConfig { get; set; }

        public GridConfig()
        {
            ColumnDimensions = new HashSet<GridDimensionConfig>();
            RowDimensions = new HashSet<GridDimensionConfig>();
            FixedDimensions = new HashSet<GridDimensionConfig>();
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Workflow.Frontal.BusinessLogic.Models
{
    public class GridConfigModel
    {
        /// <summary>
        /// ColumnDimensions property.
        /// </summary>
        /// <value>
        /// Gets or sets the ColumnDimensions value.
        /// </value>
        public List<GridDimensionModel> ColumnDimensions { get; set; }

        /// <summary>
        /// RowDimensions property.
        /// </summary>
        /// <value>
        /// Gets or sets the RowDimensions value.
        /// </value>
        public List<GridDimensionModel> RowDimensions { get; set; }

        /// <summary>
        /// ExcludedDimensions property.
        /// </summary>
        /// <value>
        /// Gets or sets the ExcludedDimensions value.
        /// </value>
        public List<GridDimensionModel> ExcludedDimensions { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        public GridConfigModel()
        {
            ColumnDimensions = new List<GridDimensionModel>();
            RowDimensions = new List<GridDimensionModel>();
            ExcludedDimensions = new List<GridDimensionModel>();
        }
    }
}

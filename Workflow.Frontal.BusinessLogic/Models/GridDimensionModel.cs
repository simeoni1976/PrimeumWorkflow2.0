using System;
using System.Collections.Generic;
using System.Text;

namespace Workflow.Frontal.BusinessLogic.Models
{
    public enum NumberDimension
    {
        TypeValue = 0,
        Dimension1 = 1,
        Dimension2 = 2,
        Dimension3 = 3,
        Dimension4 = 4,
        Dimension5 = 5,
        Dimension6 = 6,
        Dimension7 = 7,
        Dimension8 = 8,
        Dimension9 = 9,
        Dimension10 = 10
    }

    public class GridDimensionModel
    {
        /// <summary>
        /// Nom affiché de la dimension.
        /// </summary>
        /// <value>
        /// Gets or sets the Name value.
        /// </value>
        public string DisplayName { get; set; }

        /// <summary>
        /// Nom interne de la dimension
        /// </summary>
        public NumberDimension NumberDimension { get; set; }

        /// <summary>
        /// Order property.
        /// </summary>
        /// <value>
        /// Gets or sets the Order value.
        /// </value>
        public int Order { get; set; }

        /// <summary>
        /// Values property.
        /// </summary>
        /// <value>
        /// Gets or sets the Values value.
        /// </value>
        public Dictionary<int, string> Values { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        public GridDimensionModel()
        {
            Values = new Dictionary<int, string>();
        }

    }
}

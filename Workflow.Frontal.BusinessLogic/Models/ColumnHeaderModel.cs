using System;
using System.Collections.Generic;

namespace Workflow.Frontal.BusinessLogic.Models
{
    /// <summary>
    /// ColumnHeaderModel class.
    /// </summary>
    public class ColumnHeaderModel
    {
        /// <summary>
        /// caption property.
        /// </summary>
        /// <value>
        /// Gets or sets the caption value.
        /// </value>
        public string caption { get; set; }

        /// <summary>
        /// dataField property.
        /// </summary>
        /// <value>
        /// Gets or sets the dataField value.
        /// </value>
        public string dataField { get; set; }

        /// <summary>
        /// sortOrder property.
        /// </summary>
        /// <value>
        /// Gets or sets the sortOrder value.
        /// </value>
        public string sortOrder { get; set; }

        /// <summary>
        /// cellTemplate property.
        /// </summary>
        /// <value>
        /// Gets or sets the cellTemplate value.
        /// </value>
        public string cellTemplate { get; set; }

        /// <summary>
        /// allowEditing property.
        /// </summary>
        /// <value>
        /// Gets or sets the allowEditing value.
        /// </value>
        public bool allowEditing { get; set; }

        /// <summary>
        /// alignment property.
        /// </summary>
        /// <value>
        /// Gets or sets the alignment value.
        /// </value>
        public string alignment { get; set; }

        /// <summary>
        /// columns property.
        /// </summary>
        /// <value>
        /// Gets or sets the columns value.
        /// </value>
        public List<ColumnHeaderModel> columns { get; set; }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public ColumnHeaderModel()
        {
            allowEditing = true;
            columns = new List<ColumnHeaderModel>();
        }
    }
}
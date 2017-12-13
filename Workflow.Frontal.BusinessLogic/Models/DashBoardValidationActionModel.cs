using System;
using System.Collections.Generic;
using System.Text;

namespace Workflow.Frontal.BusinessLogic.Models
{
    /// <summary>
    /// DashBoardSectionModel configuration
    /// </summary>
    public class DashBoardValidationActionModel
    {
        /// <summary>
        /// NotSubmitedTotal property.
        /// </summary>
        /// <value>
        /// Gets or sets the NotSubmitedTotal value.
        /// </value>
        public long? NotSubmitedTotal { get; set; }

        /// <summary>
        /// ToBeValidatedTotal property.
        /// </summary>
        /// <value>
        /// Gets or sets the ToBeValidatedTotal value.
        /// </value>
        public long? ToBeValidatedTotal { get; set; }

        /// <summary>
        /// ValidatedTotal property.
        /// </summary>
        /// <value>
        /// Gets or sets the ValidatedTotal value.
        /// </value>
        public long? ValidatedTotal { get; set; }

        /// <summary>
        /// Activities property.
        /// </summary>
        /// <value>
        /// Gets or sets the Activities value.
        /// </value>
        public List<DashBoardValidationActivityModel> Activities { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        public DashBoardValidationActionModel()
        {
            Activities = new List<DashBoardValidationActivityModel>();
        }
    }
}
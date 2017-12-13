using System;
using System.Collections.Generic;
using System.Text;

namespace Workflow.Frontal.BusinessLogic.Models
{
    /// <summary>
    /// DashBoardSectionModel configuration
    /// </summary>
    public class DashBoardModificationActionModel
    {
        /// <summary>
        /// ToDoTotal property.
        /// </summary>
        /// <value>
        /// Gets or sets the ToDoTotal value.
        /// </value>
        public long? ToDoTotal { get; set; }

        /// <summary>
        /// PendingValidationTotal property.
        /// </summary>
        /// <value>
        /// Gets or sets the PendingValidationTotal value.
        /// </value>
        public long? PendingValidationTotal { get; set; }

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
        public List<DashBoardModificationActivityModel> Activities { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        public DashBoardModificationActionModel()
        {
            Activities = new List<DashBoardModificationActivityModel>();
        }
    }
}
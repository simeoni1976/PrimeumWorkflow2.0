

using System;
using System.Collections.Generic;
using Workflow.Transverse.DTO;

namespace Workflow.Frontal.BusinessLogic.Models
{
    /// <summary>
    /// DashBoardSelectorConfigModel class.
    /// </summary>
    public class DashBoardSectionModel
    {
        /// <summary>
        /// SelectorConfig property.
        /// </summary>
        /// <value>
        /// Gets or sets the SelectorConfig value.
        /// </value>
        public SelectorConfig SelectorConfig { get; set; }

        /// <summary>
        /// Description property.
        /// </summary>
        /// <value>
        /// Gets or sets the Description value.
        /// </value>
        public string Description { get; set; }

        /// <summary>
        /// DeadLine property.
        /// </summary>
        /// <value>
        /// Gets or sets the DeadLine value.
        /// </value>
        public DateTime? DeadLine { get; set; }

        /// <summary>
        /// Modification property.
        /// </summary>
        /// <value>
        /// Gets or sets the Modification value.
        /// </value>
        public DashBoardModificationActionModel Modification { get; set; }

        /// <summary>
        /// Validation property.
        /// </summary>
        /// <value>
        /// Gets or sets the Validation value.
        /// </value>
        public DashBoardValidationActionModel Validation { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        public DashBoardSectionModel()
        {
        }
    }
}
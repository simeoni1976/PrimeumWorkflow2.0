
using System;
using System.Collections.Generic;
using Workflow.Transverse.DTO;

namespace Workflow.Frontal.BusinessLogic.Models
{
    /// <summary>
    /// DashBoardWorkflowModel configuration
    /// </summary>
    public class DashBoardWorkflowModel
    {
        /// <summary>
        /// WorkflowInstanceId property.
        /// </summary>
        /// <value>
        /// Gets or sets the WorkflowInstanceId value.
        /// </value>
        public long WorkflowInstanceId { get; set; }

        /// <summary>
        /// Title property.
        /// </summary>
        /// <value> 
        /// Gets or sets the Title value.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// ModificationTotal property.
        /// </summary>
        /// <value>
        /// Gets or sets the ModificationTotal value.
        /// </value>
        public long ModificationTotal { get; set; }

        /// <summary>
        /// ValidationTotal property.
        /// </summary>
        /// <value>
        /// Gets or sets the ValidationTotal value.
        /// </value>
        public long ValidationTotal { get; set; }

        /// <summary>
        /// Sections property.
        /// </summary>
        /// <value>
        /// Gets or sets the Sections value.
        /// </value>
        public List<DashBoardSectionModel> Sections { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        public DashBoardWorkflowModel()
        {
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;
using Workflow.Transverse.DTO.Common;

namespace Workflow.Transverse.DTO
{
    public class ConditionnedCriteriaValues : BaseDTO
    {
        /// <summary>
        /// SelectorInstance property.
        /// </summary>
        /// <value>
        /// Gets or sets the SelectorInstance value.
        /// </value>
        public SelectorInstance SelectorInstance { get; set; }

        /// <summary>
        /// Criteria property.
        /// </summary>
        /// <value>
        /// Gets or sets the Criteria value.
        /// </value>
        public ConditionnedCriteria ConditionnedCriteria { get; set; }

        /// <summary>
        /// Value property.
        /// </summary>
        /// <value>
        /// Gets or sets the Value value.
        /// </value>
        public string Value { get; set; }

        public ConditionnedCriteriaValues()
        {
        }
    }
}

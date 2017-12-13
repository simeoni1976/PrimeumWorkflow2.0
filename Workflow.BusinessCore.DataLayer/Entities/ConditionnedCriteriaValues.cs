using System;
using System.Collections.Generic;
using System.Text;
using Workflow.BusinessCore.DataLayer.Common;

namespace Workflow.BusinessCore.DataLayer.Entities
{
    public class ConditionnedCriteriaValues : BaseEntity
    {
        /// <summary>
        /// SelectorInstance property.
        /// </summary>
        /// <value>
        /// Gets or sets the SelectorInstance value.
        /// </value>
        public virtual SelectorInstance SelectorInstance { get; set; }

        /// <summary>
        /// Criteria property.
        /// </summary>
        /// <value>
        /// Gets or sets the Criteria value.
        /// </value>
        public virtual ConditionnedCriteria ConditionnedCriteria { get; set; }

        /// <summary>
        /// Value property.
        /// </summary>
        /// <value>
        /// Gets or sets the Value value.
        /// </value>
        public string Value { get; set; }


        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public ConditionnedCriteriaValues()
        {
        }
    }
}

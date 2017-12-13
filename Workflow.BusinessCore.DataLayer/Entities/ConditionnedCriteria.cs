using System;
using System.Collections.Generic;
using System.Text;
using Workflow.BusinessCore.DataLayer.Common;

namespace Workflow.BusinessCore.DataLayer.Entities
{
    public class ConditionnedCriteria : BaseEntity
    {
        /// <summary>
        /// SelectorConfig property.
        /// </summary>
        /// <value>
        /// Gets or sets the SelectorConfig value.
        /// </value>
        public virtual SelectorConfig SelectorConfig { get; set; }

        /// <summary>
        /// Dimension property.
        /// </summary>
        /// <value>
        /// Gets or sets the Dimension value.
        /// </value>
        public virtual Dimension Dimension { get; set; }

        /// <summary>
        /// Formule à appliquer
        /// </summary>
        /// <value>Chaine complete de la formule à appliquer</value>
        public string Formula { get; set; }

        /// <summary>
        /// Value property.
        /// </summary>
        /// <value>
        /// Gets or sets the Value value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Ordre dans la liste des ConditionnedCriteria.
        /// </summary>
        public int Order { get; set; }

        public ConditionnedCriteria()
        {
        }

    }
}

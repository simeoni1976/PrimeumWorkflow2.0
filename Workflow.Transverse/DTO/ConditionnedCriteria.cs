using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Workflow.Transverse.DTO.Common;

namespace Workflow.Transverse.DTO
{
    public class ConditionnedCriteria : BaseDTO
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
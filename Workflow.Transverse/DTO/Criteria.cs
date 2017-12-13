using System;
using System.Collections.Generic;
using Workflow.Transverse.DTO.Common;

namespace Workflow.Transverse.DTO
{
    /// <summary>
    /// Criteria DTO class.
    /// </summary>
    public class Criteria : BaseDTO
    {
        /// <summary>
        /// SelectorConfig property.
        /// </summary>
        /// <value>
        /// Gets or sets the SelectorConfig value.
        /// </value>
        public SelectorConfig SelectorConfig { get; set; }

        /// <summary>
        /// SelectorConfig en tant que modificateur (peut être nul).
        /// </summary>
        public SelectorConfig SelectorConfigModifiers { get; set; }

        /// <summary>
        /// SelectorConfig en tant que validateur (peut être nul).
        /// </summary>
        public SelectorConfig SelectorConfigValidators { get; set; }

        /// <summary>
        /// SelectorConfig en tant que données pouvent être modifiées
        /// </summary>
        public virtual SelectorConfig SelectorConfigModifyData { get; set; }


        /// <summary>
        /// Dimension property.
        /// </summary>
        /// <value>
        /// Gets or sets the Dimension value.
        /// </value>
        public Dimension Dimension { get; set; }

        /// <summary>
        /// CriteriaValues property.
        /// </summary>
        /// <value>
        /// Gets or sets the CriteriaValues value.
        /// </value>
        public ICollection<CriteriaValues> CriteriaValues { get; set; }

        /// <summary>
        /// Value property.
        /// </summary>
        /// <value>
        /// Gets or sets the Value value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Ordre si le critere est dans une liste.
        /// </summary>
        /// <value>
        /// Zéro par défaut et zéro based.
        /// </value>
        public int Order { get; set; }

        /// <summary>
        /// Numéro de chaine de critère, si le critère est dans une chaine
        /// </summary>
        /// <value>
        /// Zéro par défaut et zéro based.
        /// </value>
        public int ChainNumber { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Criteria()
        {
        }
    }
}

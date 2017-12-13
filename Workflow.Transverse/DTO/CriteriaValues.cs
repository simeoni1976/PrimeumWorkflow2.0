
using Workflow.Transverse.DTO.Common;

namespace Workflow.Transverse.DTO
{
    /// <summary>
    /// CriteriaValues DTO class.
    /// </summary>
    public class CriteriaValues : BaseDTO
    {
        /// <summary>
        /// SelectorInstance property.
        /// </summary>
        /// <value>
        /// Gets or sets the SelectorInstance value.
        /// </value>
        public SelectorInstance SelectorInstance { get; set; }

        /// <summary>
        /// SelectorInstance lorsque le criteria est un modifier.
        /// </summary>
        public SelectorInstance SelectorInstanceModifier { get; set; }

        /// <summary>
        /// SelectorInstance lorsque le criteria est un validator.
        /// </summary>
        public SelectorInstance SelectorInstanceValidator { get; set; }

        /// <summary>
        /// Criteria property.
        /// </summary>
        /// <value>
        /// Gets or sets the Criteria value.
        /// </value>
        public Criteria Criteria { get; set; }

        /// <summary>
        /// Value property.
        /// </summary>
        /// <value>
        /// Gets or sets the Value value.
        /// </value>
        public string Value { get; set; }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public CriteriaValues()
        {
        }
    }
}
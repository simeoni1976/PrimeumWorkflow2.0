using Workflow.BusinessCore.DataLayer.Common;

namespace Workflow.BusinessCore.DataLayer.Entities
{
    /// <summary>
    /// CriteriaValues class.
    /// </summary>
    public class CriteriaValues : BaseEntity
    {
        /// <summary>
        /// SelectorInstance property.
        /// </summary>
        /// <value>
        /// Gets or sets the SelectorInstance value.
        /// </value>
        public virtual SelectorInstance SelectorInstance { get; set; }

        /// <summary>
        /// SelectorInstance lorsque le criteria est un modifier.
        /// </summary>
        public virtual SelectorInstance SelectorInstanceModifier { get; set; }

        /// <summary>
        /// SelectorInstance lorsque le criteria est un validator.
        /// </summary>
        public virtual SelectorInstance SelectorInstanceValidator { get; set; }

        /// <summary>
        /// Criteria property.
        /// </summary>
        /// <value>
        /// Gets or sets the Criteria value.
        /// </value>
        public virtual Criteria Criteria { get; set; }

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
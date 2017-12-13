using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Workflow.BusinessCore.DataLayer.Common;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.DataLayer.Entities
{
    /// <summary>
    /// SelectorInstance class.
    /// </summary>
    /// <remarks>
    /// This class permits to instance the current selector.
    /// </remarks>
    public class SelectorInstance : BaseEntity
    {
        /// <summary>
        /// Status property.
        /// </summary>
        /// <value>
        /// Gets or sets the Status value.
        /// </value>
        public SelectorStateEnum? Status { get; set; }

        /// <summary>
        /// PreviousStatus property.
        /// </summary>
        /// <value>
        /// Gets or sets the PreviousStatus value.
        /// </value>
        public SelectorStateEnum? PreviousStatus { get; set; }

        /// <summary>
        /// CriteriaValues property.
        /// </summary>
        /// <value>
        /// Gets or sets the CriteriaValues value.
        /// </value>
        [InverseProperty("SelectorInstance")]
        public virtual ICollection<CriteriaValues> CriteriaValues { get; set; }

        /// <summary>
        /// SelectorConfig property.
        /// </summary>
        /// <value>
        /// Gets or sets the SelectorConfig value.
        /// </value>
        public virtual SelectorConfig SelectorConfig { get; set; }

        /// <summary>
        /// WorkflowInstance property.
        /// </summary>
        /// <value>
        /// Gets or sets the WorkflowInstance value.
        /// </value>
        public virtual WorkflowInstance WorkflowInstance { get; set; }

        /// <summary>
        /// Valeurs des ConditionnedCriteria, pour selectionner le modificateur.
        /// </summary>
        [InverseProperty("SelectorInstanceModifier")]
        public virtual ICollection<CriteriaValues> ModifyCriteriasValues { get; set; }

        /// <summary>
        /// Numéro de la chaine des criteria.
        /// </summary>
        public int ChainNumberModifyer { get; set; }

        /// <summary>
        /// Id du User modificateur
        /// </summary>
        public long ModifyerId { get; set; }

        /// <summary>
        /// Valeurs des ConditionnedCriteria, pour selectionner le validateur.
        /// </summary>
        [InverseProperty("SelectorInstanceValidator")]
        public virtual ICollection<CriteriaValues> ValidatorCriteriasValues { get; set; }

        /// <summary>
        /// Id du SelectorInstance parent, s'il existe.
        /// </summary>
        public long ParentSelectorInstanceId { get; set; }

        /// <summary>
        /// Membre représentant la liaison avec le subset de ValueObject.
        /// </summary>
        public ICollection<SelectorInstanceValueObject> SelectorInstanceValueObject { get; set; }

        /// <summary>
        /// Liaison avec les utilisateurs (notament les validateurs)
        /// </summary>
        public ICollection<SelectorInstanceUser> SelectorInstanceUser { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        public SelectorInstance()
        {
            CriteriaValues = new HashSet<CriteriaValues>();
            ModifyCriteriasValues = new HashSet<CriteriaValues>();
            ValidatorCriteriasValues = new HashSet<CriteriaValues>();
            SelectorInstanceValueObject = new HashSet<SelectorInstanceValueObject>();
            SelectorInstanceUser = new HashSet<SelectorInstanceUser>();
        }
    }
}

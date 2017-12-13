using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Workflow.BusinessCore.DataLayer.Common;
using Workflow.Transverse.DTO;

namespace Workflow.BusinessCore.DataLayer.Entities
{
    /// <summary>
    /// SelectorConfig class.
    /// </summary>
    public class SelectorConfig : BaseEntity
    {
        /// <summary>
        /// Name property.
        /// </summary>
        /// <value>
        /// Gets or sets the Name value.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Description du Selectorinstance.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// PrevPropagateId property.
        /// </summary>
        /// <value>
        /// Gets or sets the PrevPropagateId value.
        /// </value>
        public long PrevPropagateId { get; set; }

        /// <summary>
        /// PropagateId property.
        /// </summary>
        /// <value>
        /// Gets or sets the PropagateId value.
        /// </value>
        public long PropagateId { get; set; }

        /// <summary>
        /// FailPropagateId property.
        /// </summary>
        /// <value>
        /// Gets or sets the FailPropagateId value.
        /// </value>
        public long FailPropagateId { get; set; }

        /// <summary>
        /// Criterias property.
        /// </summary>
        /// <value>
        /// Gets or sets the Criterias value.
        /// </value>
        [InverseProperty("SelectorConfig")]
        public virtual ICollection<Criteria> Criterias { get; set; }

        /// <summary>
        /// SelectorInstance property.
        /// </summary>
        /// <value>
        /// Gets or sets the SelectorInstance value.
        /// </value>
        public virtual ICollection<SelectorInstance> SelectorInstance { get; set; }

        /// <summary>
        /// WorkflowConfig property.
        /// </summary>
        /// <value>
        /// Gets or sets the WorkflowConfig value.
        /// </value>
        public virtual WorkflowConfig WorkflowConfig { get; set; }


        /// <summary>
        /// Criteria pour la selection des données modifiées
        /// </summary>
        /// <value>Liste des critéres conditionnés pour extraires les données modifiables.</value>
        [InverseProperty("SelectorConfigModifyData")]
        public virtual ICollection<Criteria> ModifyCriterias { get; set; }


        /// <summary>
        /// Liste ordonnée de la configuration des modificateurs
        /// </summary>
        [InverseProperty("SelectorConfigModifiers")]
        public virtual ICollection<Criteria> Modifiers { get; set; }

        /// <summary>
        /// Liste ordonnée de la configuration des validateurs
        /// </summary>
        [InverseProperty("SelectorConfigValidators")]
        public virtual ICollection<Criteria> Validators { get; set; }


        /// <summary>
        /// Référence de la séquence d'action à effectuer sur les SelectorInstance.
        /// </summary>
        public long ActionSequenceRef { get; set; }


        /// <summary>
        /// Référence de la séquence de contrainte à effectuer sur les SelectorInstance.
        /// </summary>
        public long ConstraintSequenceRef { get; set; }


        /// <summary>
        /// Class constructor.
        /// </summary>
        public SelectorConfig()
        {
            Criterias = new HashSet<Criteria>();
            SelectorInstance = new HashSet<SelectorInstance>();
            Modifiers = new HashSet<Criteria>();
            Validators = new HashSet<Criteria>();
            ModifyCriterias = new HashSet<Criteria>();
            ActionSequenceRef = -1;
            ConstraintSequenceRef = -1;
        }
    }
}
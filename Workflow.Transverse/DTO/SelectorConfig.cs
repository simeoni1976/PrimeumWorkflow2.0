using System;
using System.Collections.Generic;
using Workflow.Transverse.DTO;
using Workflow.Transverse.DTO.Common;

namespace Workflow.Transverse.DTO
{
    /// <summary>
    /// SelectorConfig DTO class.
    /// </summary>
    public class SelectorConfig : BaseDTO
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
        public ICollection<Criteria> Criterias { get; set; }

        /// <summary>
        /// SelectorInstance property.
        /// </summary>
        /// <value>
        /// Gets or sets the SelectorInstance value.
        /// </value>
        public ICollection<SelectorInstance> SelectorInstance { get; set; }

        /// <summary>
        /// Id du WorkflowConfig parent.
        /// </summary>
        /// <value>Donne ou régle l'id du WorkflowConfig parent</value>
        /// <remarks>Première utilisation pour les controles lors de l'ajout d'un SelectorConfig.</remarks>
        public long IdWorkflowConfig { get; set; }


        /// <summary>
        /// Criteria pour la selection des données modifiées
        /// </summary>
        /// <value>Liste des critéres conditionnés pour extraires les données modifiables.</value>
        public ICollection<Criteria> ModifyCriterias { get; set; }


        /// <summary>
        /// Liste ordonnée de la configuration des modificateurs
        /// </summary>
        public ICollection<Criteria> Modifiers { get; set; }

        /// <summary>
        /// Liste ordonnée de la configuration des validateurs
        /// </summary>
        public ICollection<Criteria> Validators { get; set; }

        /// <summary>
        /// Référence de la séquence d'action à effectuer par défaut sur les SelectorInstance.
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
        }
    }
}
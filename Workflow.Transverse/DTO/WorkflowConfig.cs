using System.Collections.Generic;
using Workflow.Transverse.DTO.Common;

namespace Workflow.Transverse.DTO
{
    /// <summary>
    /// WorkflowConfig DTO class.
    /// </summary>
    public class WorkflowConfig : BaseDTO
    {
        /// <summary>
        /// Name property.
        /// </summary>
        /// <value>
        /// Gets or sets the Name value.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Id du WorkflowConfig d'origine, si existant (nullable).
        /// </summary>
        /// <remarks>Lors d'une création de WorkflowInstance, une copie du WorkflowConfig est faite, afin de préserver les paramétres d'origine.
        /// On garde dans ce champ l'id du WorkflowConfig original.</remarks>
        public long? IdWorkflowConfigOriginal { get; set; }

        /// <summary>
        /// FirstSelectorConfigId property.
        /// </summary>
        /// <value>
        /// Gets or sets the FirstSelectorConfigId value.
        /// </value>
        public long FirstSelectorConfigId { get; set; }

        /// <summary>
        /// WorkflowDimensions property.
        /// </summary>
        /// <value>
        /// Gets or sets the WorkflowDimensions value.
        /// </value>
        public ICollection<WorkflowDimension> WorkflowDimension { get; set; }

        /// <summary>
        /// WorkflowInstance property.
        /// </summary>
        /// <value>
        /// Gets or sets the WorkflowInstance value.
        /// </value>
        public ICollection<WorkflowInstance> WorkflowInstance { get; set; }

        /// <summary>
        /// Référence de la séquence d'action à effectuer par défaut sur les SelectorInstance.
        /// </summary>
        public long ActionSequenceRef { get; set; }


        /// <summary>
        /// Référence de la séquence de contrainte à effectuer par défaut sur les SelectorInstance.
        /// </summary>
        public long ConstraintSequenceRef { get; set; }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public WorkflowConfig()
        {
        }
    }
}
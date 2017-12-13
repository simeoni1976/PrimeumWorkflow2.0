using System;
using System.Collections.Generic;
using Workflow.Transverse.DTO.Common;
using Workflow.Transverse.Helpers;

namespace Workflow.Transverse.DTO
{
    /// <summary>
    /// SelectorInstance DTO class.
    /// </summary>
    public class SelectorInstance : BaseDTO
    {
        /// <summary>
        /// WorkflowInstance property.
        /// </summary>
        /// <value>
        /// Gets or sets the WorkflowInstance value.
        /// </value>
        public WorkflowInstance WorkflowInstance { get; set; }

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
        public ICollection<CriteriaValues> CriteriaValues { get; set; }

        /// <summary>
        /// SelectorConfig property.
        /// </summary>
        /// <value>
        /// Gets or sets the SelectorConfig value.
        /// </value>
        public SelectorConfig SelectorConfig { get; set; }


        /// <summary>
        /// Valeurs des ConditionnedCriteria, pour selectionner le modificateur.
        /// </summary>
        public ICollection<CriteriaValues> ModifyCriteriasValues { get; set; }

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
        public ICollection<CriteriaValues> ValidatorCriteriasValues { get; set; }


        /// <summary>
        /// Nom complet du modificateur
        /// </summary>
        /// <remarks>Uniquement pour le DTO afin de faciliter l'accès à la donnée.</remarks>
        public string ModificatorName { get; set; }

        /// <summary>
        /// Noms des valdateurs
        /// </summary>
        /// <remarks>Utilisé uniquement pour le DTO, pour faciliter l'accès à la donnée.</remarks>
        public string ValidatorsNames { get; set; }


        /// <summary>
        /// Valeur caractèrisant le SelectorInstance, selon une dimension fixée pour l'instant (première dimension de type arborescente rencontrée)
        /// </summary>
        /// <remarks>Utilisé uniquement pour le DTO</remarks>
        public string DimensionValueImportant { get; set; }


        /// <summary>
        /// Class constructor
        /// </summary>
        public SelectorInstance()
        {
        }
    }
}

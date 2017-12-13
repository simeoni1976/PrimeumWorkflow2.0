using System.Collections.Generic;
using Workflow.Transverse.DTO.Common;
using Workflow.Transverse.Helpers;

namespace Workflow.Transverse.DTO
{
    /// <summary>
    /// ValueObject DTO class.
    /// </summary>
    public class ValueObject : BaseDTO
    {
        /// <summary>
        /// DataSet property.
        /// </summary>
        /// <value>
        /// Gets or sets the DataSet value.
        /// </value>
        public DataSet DataSet { get; set; }

        /// <summary>
        /// InitialValue property.
        /// </summary>
        /// <value>
        /// Gets or sets the InitialValue value.
        /// </value>
        public double? InitialValue { get; set; }

        /// <summary>
        /// CurrentValue property.
        /// </summary>
        /// <value>
        /// Gets or sets the CurrentValue value.
        /// </value> 
        public double? CurrentValue { get; set; }

        /// <summary>
        /// FutureValue property.
        /// </summary>
        /// <value>
        /// Gets or sets the FutureValue value.
        /// </value>
        public double? FutureValue { get; set; }

        /// <summary>
        /// Valeur volatile, non sauvée par l'utilisateur mais quand même enregistrée.
        /// </summary>
        public double? VolatileValue { get; set; }

        /// <summary>
        /// TypeValue de la ligne : réprésente le type de valeur (objectif, PKI, mois de mensualisation, etc...) donné par la ligne.
        /// </summary>
        public string TypeValue { get; set; }

        /// <summary>
        /// Dimension1 property.
        /// </summary>
        /// <value>
        /// Gets or sets the Dimension1 value.
        /// </value>
        public string Dimension1 { get; set; }

        /// <summary>
        /// Dimension2 property.
        /// </summary>
        /// <value>
        /// Gets or sets the Dimension2 value.
        /// </value>
        public string Dimension2 { get; set; }

        /// <summary>
        /// Dimension3 property.
        /// </summary>
        /// <value>
        /// Gets or sets the Dimension3 value.
        /// </value>
        public string Dimension3 { get; set; }

        /// <summary>
        /// Dimension4 property.
        /// </summary>
        /// <value>
        /// Gets or sets the Dimension4 value.
        /// </value>
        public string Dimension4 { get; set; }

        /// <summary>
        /// Dimension5 property.
        /// </summary>
        /// <value>
        /// Gets or sets the Dimension5 value.
        /// </value>
        public string Dimension5 { get; set; }

        /// <summary>
        /// Dimension6 property.
        /// </summary>
        /// <value>
        /// Gets or sets the Dimension6 value.
        /// </value>
        public string Dimension6 { get; set; }

        /// <summary>
        /// Dimension7 property.
        /// </summary>
        /// <value>
        /// Gets or sets the Dimension7 value.
        /// </value>
        public string Dimension7 { get; set; }

        /// <summary>
        /// Dimension8 property.
        /// </summary>
        /// <value>
        /// Gets or sets the Dimension8 value.
        /// </value>
        public string Dimension8 { get; set; }

        /// <summary>
        /// Dimension9 property.
        /// </summary>
        /// <value>
        /// Gets or sets the Dimension9 value.
        /// </value>
        public string Dimension9 { get; set; }

        /// <summary>
        /// Dimension10 property.
        /// </summary>
        /// <value>
        /// Gets or sets the Dimension10 value.
        /// </value>
        public string Dimension10 { get; set; }

        /// <summary>
        /// Status property.
        /// </summary>
        /// <value>
        /// Gets or sets the Status value.
        /// </value>
        public ValueObjectStatusEnum? Status { get; set; }

        /// <summary>
        /// SelectionInstance property.
        /// </summary>
        /// <value>
        /// Gets or sets the SelectionInstance value.
        /// </value>
        public SelectorInstance SelectionInstance { get; set; }

        /// <summary>
        /// Comment property.
        /// </summary>
        /// <value>
        /// Gets or sets the Comment value.
        /// </value>
        public virtual ICollection<Comment> Comment { get; set; }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public ValueObject()
        {
            Comment = new HashSet<Comment>();
        }
    }
}
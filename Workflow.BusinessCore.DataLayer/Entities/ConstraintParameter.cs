using System;
using System.Collections.Generic;
using System.Text;
using Workflow.BusinessCore.DataLayer.Common;

namespace Workflow.BusinessCore.DataLayer.Entities
{
    /// <summary>
    /// Paramètre pour une contrainte.
    /// </summary>
    public class ConstraintParameter : BaseEntity
    {
        /// <summary>
        /// Référence vers la séquence de la contrainte
        /// </summary>
        public long ReferenceSequence { get; set; }

        /// <summary>
        /// Numéro d'ordre de la contrainte dans la séquence.
        /// </summary>
        public int OrderSequence { get; set; }

        /// <summary>
        /// Nom du paramètre
        /// </summary>
        public string ParameterName { get; set; }

        /// <summary>
        /// Valeur du paramètre
        /// </summary>
        public string Value { get; set; }
    }
}

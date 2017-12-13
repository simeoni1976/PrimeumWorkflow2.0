using System;
using System.Collections.Generic;
using System.Text;
using Workflow.BusinessCore.DataLayer.Common;

namespace Workflow.BusinessCore.DataLayer.Entities
{
    /// <summary>
    /// Classe décrivant un paramétre d'action
    /// </summary>
    public class ActionParameter : BaseEntity
    {
        /// <summary>
        /// Référence vers la séquence d'action
        /// </summary>
        public long ReferenceSequence { get; set; }

        /// <summary>
        /// Numéro d'ordre de l'action dans la séquence.
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

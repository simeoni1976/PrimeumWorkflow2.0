using System;
using System.Collections.Generic;
using System.Text;
using Workflow.Transverse.DTO.Common;

namespace Workflow.Transverse.DTO
{
    public class ActionParameter : BaseDTO
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

        public ActionParameter()
        {
        }
    }
}

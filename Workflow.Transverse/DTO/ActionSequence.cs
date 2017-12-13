using System;
using System.Collections.Generic;
using System.Text;
using Workflow.Transverse.DTO.Common;

namespace Workflow.Transverse.DTO
{
    public class ActionSequence : BaseDTO
    {
        /// <summary>
        /// Référence de la séquence
        /// </summary>
        public long Reference { get; set; }

        /// <summary>
        /// Nom de la séquance
        /// </summary>
        public string SequenceName { get; set; }

        /// <summary>
        /// Ordre de l'action de la séquence
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Action
        /// </summary>
        public Action Action { get; set; }

        public ActionSequence()
        {
        }
    }
}

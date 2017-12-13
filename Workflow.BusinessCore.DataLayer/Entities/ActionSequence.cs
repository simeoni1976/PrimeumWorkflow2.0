using System;
using System.Collections.Generic;
using System.Text;
using Workflow.BusinessCore.DataLayer.Common;

namespace Workflow.BusinessCore.DataLayer.Entities
{
    /// <summary>
    /// Représentant les sequences d'actions possibles.
    /// </summary>
    public class ActionSequence : BaseEntity
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


    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Workflow.BusinessCore.DataLayer.Common;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.DataLayer.Entities
{
    /// <summary>
    /// Classe réprésentant les actions définies globalement.
    /// </summary>
    public class Action : BaseEntity
    {
        /// <summary>
        /// Nom de l'action
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type de l'action.
        /// </summary>
        /// <remarks>0 : en dur, 1 : dynamique</remarks>
        public ActionTypeEnum Type { get; set; }

        ///// <summary>
        ///// Source de l'action
        ///// </summary>
        //public string Source { get; set; }

    }
}

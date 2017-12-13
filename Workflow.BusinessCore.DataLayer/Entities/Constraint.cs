using System;
using System.Collections.Generic;
using System.Text;
using Workflow.BusinessCore.DataLayer.Common;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.DataLayer.Entities
{
    /// <summary>
    /// Classe représentant les contraintes définies globalement
    /// </summary>
    public class Constraint : BaseEntity
    {
        /// <summary>
        /// Nom de la contrainte
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Type de la contrainte
        /// </summary>
        /// <remarks>0 : en dur, 1 : dynamique</remarks>
        public ConstraintTypeEnum Type { get; set; }
    }
}

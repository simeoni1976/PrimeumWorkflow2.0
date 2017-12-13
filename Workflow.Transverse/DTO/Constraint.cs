using System;
using System.Collections.Generic;
using System.Text;
using Workflow.Transverse.DTO.Common;
using Workflow.Transverse.Helpers;

namespace Workflow.Transverse.DTO
{
    public class Constraint : BaseDTO
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

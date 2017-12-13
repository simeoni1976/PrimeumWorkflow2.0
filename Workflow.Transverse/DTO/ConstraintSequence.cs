﻿using System;
using System.Collections.Generic;
using System.Text;
using Workflow.Transverse.DTO.Common;
using Workflow.Transverse.Helpers;

namespace Workflow.Transverse.DTO
{
    public class ConstraintSequence : BaseDTO
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
        /// Ordre de la contrainte dans la séquence
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Précise l'importance de la contrainte en cas d'échec
        /// </summary>
        public ConstraintLevelEnum Level { get; set; }

        /// <summary>
        /// Contrainte
        /// </summary>
        public Constraint Constraint { get; set; }
    }
}

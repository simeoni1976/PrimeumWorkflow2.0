﻿using System;
using System.Collections.Generic;
using System.Text;
using Workflow.Transverse.DTO.Common;

namespace Workflow.Transverse.DTO
{
    /// <summary>
    /// Entité contenant les données supplémentaires pour les dimensions de type Scalar.
    /// </summary>
    public class DimensionScalarData : BaseDTO
    {
        /// <summary>
        /// Valeur clé de référence, celle que l'on retrouve dans les colonnes DimensionX de ValueObject
        /// </summary>
        public string ValueKey { get; set; }

        /// <summary>
        /// Dimensions property.
        /// </summary>
        /// <value>
        /// Gets or sets the Dimensions value.
        /// </value>
        public virtual ICollection<Dimension> Dimensions { get; set; }

        /// <summary>
        /// Constructeur par défaut
        /// </summary>
        public DimensionScalarData()
        {
            Dimensions = new HashSet<Dimension>();
        }
    }
}

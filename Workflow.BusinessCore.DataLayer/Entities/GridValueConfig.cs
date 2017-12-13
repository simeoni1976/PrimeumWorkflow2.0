using System;
using System.Collections.Generic;
using System.Text;
using Workflow.BusinessCore.DataLayer.Common;

namespace Workflow.BusinessCore.DataLayer.Entities
{
    /// <summary>
    /// Valeur affichée dans l'entête de colonne
    /// </summary>
    public class GridValueConfig : BaseEntity
    {
        /// <summary>
        /// Valeur affichée
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// Ordre dans lequel la valeur est affichée
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Lien vers le GridDimensionConfig parent.
        /// </summary>
        public GridDimensionConfig GridDimensionConfig { get; set; }
    }
}

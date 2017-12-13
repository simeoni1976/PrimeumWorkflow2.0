using System;
using System.Collections.Generic;
using System.Text;
using Workflow.Transverse.DTO.Common;

namespace Workflow.Transverse.DTO
{
    public class GridValueConfig : BaseDTO
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

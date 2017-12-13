using System;
using System.Collections.Generic;
using System.Text;
using Workflow.Transverse.DTO.Common;

namespace Workflow.Transverse.DTO
{
    public class DistinctValue : BaseDTO
    {
        /// <summary>
        /// Identifiant du DataSet associé
        /// </summary>
        public long DataSetId { get; set; }

        /// <summary>
        /// Identifiant de la dimension associée
        /// </summary>
        public long DimensionId { get; set; }

        /// <summary>
        /// Valeur unique dans la dimension
        /// </summary>
        public string Value { get; set; }
    }
}

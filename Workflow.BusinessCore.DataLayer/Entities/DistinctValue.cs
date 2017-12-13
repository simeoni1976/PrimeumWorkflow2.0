using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Workflow.BusinessCore.DataLayer.Common;

namespace Workflow.BusinessCore.DataLayer.Entities
{
    public class DistinctValue : BaseEntity
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


        /// <summary>
        /// Dans le cas d'un type de valeur plutôt qu'une dimension (ColumnName du DataSetDimension = "TypeValue"), il s'agit du format numérique des valeurs (facultatif).
        /// </summary>
        public string NumericalFormat { get; set; }

    }
}

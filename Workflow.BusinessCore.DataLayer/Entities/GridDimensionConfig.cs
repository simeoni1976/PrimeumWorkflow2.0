using System;
using System.Collections.Generic;
using System.Text;
using Workflow.BusinessCore.DataLayer.Common;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.DataLayer.Entities
{
    public class GridDimensionConfig : BaseEntity
    {
        /// <summary>
        /// Nom affiché de la dimension (utile lorsque la dimension est affichée verticalement à gauche de la grid).
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Nom interne de la dimension
        /// </summary>
        public InternalNameDimension InternalName { get; set; }

        /// <summary>
        /// Ordre d'affichage de la dimension
        /// </summary>
        public int Order { get; set; }

        /// <summary>
        /// Valeurs de la dimension
        /// </summary>
        public ICollection<GridValueConfig> Values { get; set; }

        /// <summary>
        /// Référent à la grid parente (en tant que colonne)
        /// </summary>
        public GridConfig GridColumn { get; set; }

        /// <summary>
        /// Référent à la grid parente (en tant que ligne)
        /// </summary>
        public GridConfig GridRow { get; set; }

        /// <summary>
        /// Référent à la grid parente (en tant que dimension fixed)
        /// </summary>
        public GridConfig GridFixed { get; set; }

        public GridDimensionConfig()
        {
            Values = new HashSet<GridValueConfig>();
        }

    }
}

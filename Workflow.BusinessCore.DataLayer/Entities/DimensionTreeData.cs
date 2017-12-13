using System;
using System.Collections.Generic;
using System.Text;
using Workflow.BusinessCore.DataLayer.Common;

namespace Workflow.BusinessCore.DataLayer.Entities
{
    /// <summary>
    /// Entité contenant les données supplémentaires pour les dimensions de type Tree.
    /// </summary>
    public class DimensionTreeData : BaseEntity
    {
        /// <summary>
        /// Valeur clé de référence, celle que l'on retrouve dans les colonnes DimensionX de ValueObject
        /// </summary>
        public string ValueKey { get; set; }

        /// <summary>
        /// Nom affiché du noeud
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// Nom du niveau du noeud
        /// </summary>
        public string LevelName { get; set; }

        /// <summary>
        /// Nom de l'arbre
        /// </summary>
        public string TreeName { get; set; }

        /// <summary>
        /// Poids du noeud
        /// </summary>
        public double Weight { get; set; }

        /// <summary>
        /// Dimensions property.
        /// </summary>
        /// <value>
        /// Gets or sets the Dimensions value.
        /// </value>
        public virtual ICollection<Dimension> Dimensions { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        public DimensionTreeData()
        {
            Dimensions = new HashSet<Dimension>();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Workflow.BusinessCore.DataLayer.Common;

namespace Workflow.BusinessCore.DataLayer.Entities
{
    /// <summary>
    /// Classe présentant la table de vairables globales de configuration du WF.
    /// </summary>
    public class ConfigVariable : BaseEntity
    {

        /// <summary>
        /// Nom de la variable
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Valeur de la variable
        /// </summary>
        public string Value { get; set; }


    }
}

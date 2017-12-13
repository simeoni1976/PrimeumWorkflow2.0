using System;
using System.Collections.Generic;
using System.Text;
using Workflow.Transverse.DTO.Common;

namespace Workflow.Transverse.DTO
{
    public class ConfigVariable : BaseDTO
    {
        /// <summary>
        /// Nom de la variable
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Valeur de la variable
        /// </summary>
        public string Value { get; set; }

        ///// <summary>
        ///// Format numérique global du WF
        ///// </summary>
        ///// <remarks>Voir la page descriptive du format en question.</remarks>
        ///// <see cref="http://confluence.primeum.com/pages/viewpage.action?pageId=2393994"/>
        //public string NumericFormat { get; set; }
    }
}

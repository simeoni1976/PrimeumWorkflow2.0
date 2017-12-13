using System;
using System.Collections.Generic;
using System.Text;
using Workflow.Transverse.DTO.Common;

namespace Workflow.Transverse.DTO
{
    /// <summary>
    /// Classe réprésentant les actions définies globalement.
    /// </summary>
    public class Action : BaseDTO
    {
        /// <summary>
        /// Nom de l'action
        /// </summary>
        public string Name { get; set; }
    }
}

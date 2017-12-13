using System;
using System.Collections.Generic;
using System.Text;
using Workflow.BusinessCore.DataLayer.Common;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.DataLayer.Entities
{
    /// <summary>
    /// Classe de lien entre les SelectorInstance et les User.
    /// </summary>
    public class SelectorInstanceUser : BaseEntity
    {
        /// <summary>
        /// Id du SelectorInstance
        /// </summary>
        public long SelectorInstanceId { get; set; }
        /// <summary>
        /// Référence vers le SelectorInstance
        /// </summary>
        public SelectorInstance SelectorInstance { get; set; }

        /// <summary>
        /// Id du User
        /// </summary>
        public long UserId { get; set; }
        /// <summary>
        /// Référence vers le user.
        /// </summary>
        public User User { get; set; }

        /// <summary>
        /// Droits de l'utilisateur
        /// </summary>
        public RightEnum Right { get; set; }

    }
}

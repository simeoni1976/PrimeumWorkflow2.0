using System;
using System.Collections.Generic;
using System.Text;
using Workflow.BusinessCore.DataLayer.Common;
using Workflow.Transverse.Helpers;

namespace Workflow.BusinessCore.DataLayer.Entities
{
    /// <summary>
    /// UserSet class.
    /// </summary>
    public class UserSet : BaseEntity
    {
        /// <summary>
        /// Nom du UserSet
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// UserSetUser property.
        /// </summary>
        /// <value>
        /// Gets or sets the UserSetUser value.
        /// </value>
        public ICollection<UserSetUser> UserSetUser { get; set; }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public UserSet()
        {
        }
    }
}

using System.Collections.Generic;
using Workflow.Transverse.DTO.Common;

namespace Workflow.Transverse.DTO
{
    /// <summary>
    /// Table désignant les rôles / noeuds / utilisateurs.
    /// </summary>
    public class UserSet : BaseDTO
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

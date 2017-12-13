using System.Collections.Generic;
using Workflow.Transverse.DTO.Common;
using Workflow.Transverse.Helpers;

namespace Workflow.Transverse.DTO
{
    /// <summary>
    /// UserSetUser class.
    /// </summary>>
    public class UserSetUser : BaseDTO
    {
        /// <summary>
        /// User property.
        /// </summary>
        /// <value>
        /// Gets or sets the User value.
        /// </value>
        public User User { get; set; }

        /// <summary>
        /// UserSet property.
        /// </summary>
        /// <value>
        /// Gets or sets the UserSet value.
        /// </value>
        public UserSet UserSet { get; set; }

        /// <summary>
        /// Droits de l'utilisateur
        /// </summary>
        /// <value>
        /// Gets or sets the Role value.
        /// </value>
        public RightEnum Right { get; set; }

        /// <summary>
        /// Position 1 : valeur sur la dimension 1
        /// </summary>
        public string Position1 { get; set; }

        /// <summary>
        /// Position 2 : valeur sur la dimension 2
        /// </summary>
        public string Position2 { get; set; }

        /// <summary>
        /// Position 3 : valeur sur la dimension 3
        /// </summary>
        public string Position3 { get; set; }

        /// <summary>
        /// Position 4 : valeur sur la dimension 4
        /// </summary>
        public string Position4 { get; set; }

        /// <summary>
        /// Position 5 : valeur sur la dimension 5
        /// </summary>
        public string Position5 { get; set; }

        /// <summary>
        /// Position 6 : valeur sur la dimension 6
        /// </summary>
        public string Position6 { get; set; }

        /// <summary>
        /// Position 7 : valeur sur la dimension 7
        /// </summary>
        public string Position7 { get; set; }

        /// <summary>
        /// Position 8 : valeur sur la dimension 8
        /// </summary>
        public string Position8 { get; set; }

        /// <summary>
        /// Position 9 : valeur sur la dimension 9
        /// </summary>
        public string Position9 { get; set; }

        /// <summary>
        /// Position 10 : valeur sur la dimension 10
        /// </summary>
        public string Position10 { get; set; }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public UserSetUser()
        {
        }
    }
}

using System.Collections.Generic;
using Workflow.Transverse.DTO.Common;

namespace Workflow.Transverse.DTO
{
    /// <summary>
    /// User DTO class.
    /// </summary>
    public class User : BaseDTO
    {
        /// <summary>
        /// Login property.
        /// </summary>
        /// <value>
        /// Gets or sets the Login value.
        /// </value>
        public string Login { get; set; }

        /// <summary>
        /// Password property.
        /// </summary>
        /// <value>
        /// Gets or sets the Password value.
        /// </value>
        public string Password { get; set; }

        /// <summary>
        /// Email property.
        /// </summary>
        /// <value>
        /// Gets or sets the Email value.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Firstname property.
        /// </summary>
        /// <value>
        /// Gets or sets the Firstname value.
        /// </value>
        public string Firstname { get; set; }

        /// <summary>
        /// Lastname property.
        /// </summary>
        /// <value>
        /// Gets or sets the Lastname value.
        /// </value>
        public string Lastname { get; set; }

        /// <summary>
        /// EmployeeID property.
        /// </summary>
        /// <value>
        /// Gets or sets the EmployeeID value.
        /// </value>
        /// <remarks>
        /// This property is an employee identification
        /// </remarks>
        public string EmployeeID { get; set; }

        /// <summary>
        /// Name property.
        /// </summary>
        /// <value>
        /// Gets or sets the Name value.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Role de l'utilisateur : label ou tag utilisé pour les recherches de modificateurs/validateurs, 
        /// donnant plus de flexibilité.
        /// </summary>
        public string Role { get; set; }

        /// <summary>
        /// UserSetUser property.
        /// </summary>
        /// <value>
        /// Gets or sets the UserSetUser value.
        /// </value>
        public ICollection<UserSetUser> UserSetUser { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        public User()
        {
        }
    }
}
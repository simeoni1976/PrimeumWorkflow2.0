using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workflow.Transverse.DTO.Common
{
    /// <summary>
    /// BaseEntity class.
    /// </summary>
    public class BaseDTO
    {
        /// <summary>
        /// Id property.
        /// </summary>
        /// <value>
        /// Gets or sets the Id value.
        /// </value>
        public Int64 Id { get; set; }

        /// <summary>
        /// AddedDate property.
        /// </summary>
        /// <value>
        /// Gets or sets the AddedDate value.
        /// </value>
        public DateTime AddedDate { get; set; }

        /// <summary>
        /// ModifiedDate property.
        /// </summary>
        /// <value>
        /// Gets or sets the ModifiedDate value.
        /// </value>
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Username property.
        /// </summary>
        /// <value>
        /// Gets or sets the Username value.
        /// </value>
        public string Username { get; set; }

        public BaseDTO()
        {
        }
    }
}

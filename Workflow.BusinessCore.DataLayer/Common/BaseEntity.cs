using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Workflow.BusinessCore.DataLayer.Common
{
    /// <summary>
    /// BaseEntity class.
    /// </summary>
    public partial class BaseEntity
    {
        /// <summary>
        /// Id property.
        /// </summary>
        /// <value>
        /// Gets or sets the Id value.
        /// </value>
        [Key]
        [Required]
        [StringLength(20)] 
        [Column("ID", Order = 1)]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 Id { get; set; }

        /// <summary>
        /// AddedDate property.
        /// </summary>
        /// <value>
        /// Gets or sets the AddedDate value.
        /// </value>
        [Column(Order = 200)]
        public DateTime AddedDate { get; set; }

        /// <summary>
        /// ModifiedDate property.
        /// </summary>
        /// <value>
        /// Gets or sets the ModifiedDate value.
        /// </value>
        [Column(Order = 201)]
        public DateTime ModifiedDate { get; set; }

        /// <summary>
        /// Username property.
        /// </summary>
        /// <value>
        /// Gets or sets the Username value.
        /// </value>
        [Column(Order = 202)]
        public string Username { get; set; }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public BaseEntity()
        {
        }
    }
}

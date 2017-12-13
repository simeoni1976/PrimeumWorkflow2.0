using System;
using Workflow.Transverse.DTO.Common;

namespace Workflow.Transverse.DTO
{
    public class Comment : BaseDTO
    {
        /// <summary>
        /// Author property.
        /// </summary>
        /// <value>
        /// Gets or sets the Author value.
        /// </value>
        public long Author { get; set; }

        /// <summary>
        /// Receiver property.
        /// </summary>
        /// <value>
        /// Gets or sets the Receiver value.
        /// </value>
        public long Receiver { get; set; }

        /// <summary>
        /// Previous property.
        /// </summary>
        /// <value>
        /// Gets or sets the Previous value.
        /// </value>
        public long Previous { get; set; }

        /// <summary>
        /// Next property.
        /// </summary>
        /// <value>
        /// Gets or sets the Next value.
        /// </value>
        public long Next { get; set; }

        /// <summary>
        /// Message property.
        /// </summary>
        /// <value>
        /// Gets or sets the Message value.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Date property.
        /// </summary>
        /// <value>
        /// Gets or sets the Date value.
        /// </value>
        public DateTime Date { get; set; }

        /// <summary>
        /// IsRead property.
        /// </summary>
        /// <value>
        /// Gets or sets the IsRead value.
        /// </value>
        public bool IsRead { get; set; }

        /// <summary>
        /// ValueObject property.
        /// </summary>
        /// <value>
        /// Gets or sets the ValueObject value.
        /// </value>
        public ValueObject ValueObject { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        public Comment()
        {
        }
    }
}

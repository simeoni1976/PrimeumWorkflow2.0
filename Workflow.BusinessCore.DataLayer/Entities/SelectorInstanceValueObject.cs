using System;
using System.Collections.Generic;
using System.Text;
using Workflow.BusinessCore.DataLayer.Common;

namespace Workflow.BusinessCore.DataLayer.Entities
{
    public class SelectorInstanceValueObject : BaseEntity
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
        /// Id du ValueObject
        /// </summary>
        public long ValueObjectId { get; set; }
        /// <summary>
        /// Référence du ValueObject
        /// </summary>
        public ValueObject ValueObject { get; set; }

        /// <summary>
        /// Permet de savoir si le ValueObject peut être modifier par le modificateur du SelectorInstance.
        /// </summary>
        public bool IsEditable { get; set; }

    }
}

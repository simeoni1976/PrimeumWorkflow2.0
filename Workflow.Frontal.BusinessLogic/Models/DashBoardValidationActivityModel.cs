
using System;
using System.Collections.Generic;
using System.Linq;

namespace Workflow.Frontal.BusinessLogic.Models
{
    /// <summary>
    /// SelectorConfigStatusEnum Enum
    /// </summary>
    public enum ValidationStatusEnum
    {
        None = 0,
        Validated = 1,
        ToBeValidated = 2,
        NotSubmited = 3
    }

    /// <summary>
    /// DashBoardActiviteSectionModel configuration
    /// </summary>
    public class DashBoardValidationActivityModel
    {
        private Dictionary<ValidationStatusEnum, Tuple<string, string>> _statusLabels = new Dictionary<ValidationStatusEnum, Tuple<string, string>>()
        {
            { ValidationStatusEnum.Validated, Tuple.Create("VALIDATED","Validated") },
            { ValidationStatusEnum.ToBeValidated,Tuple.Create("TO BE VALIDATED", "Saved") },
            { ValidationStatusEnum.NotSubmited,Tuple.Create("NOT SUBMITED", "Not submited") }
        };

        /// <summary>
        /// SelectorInstanceId property.
        /// </summary>
        /// <value>
        /// Gets or sets the SelectorInstanceId value.
        /// </value>
        public long SelectorInstanceId { get; set; }

        /// <summary>
        /// Node property.
        /// </summary>
        /// <value>
        /// Gets or sets the Node value.
        /// </value>
        public string Node { get; set; }

        /// <summary>
        /// Affected property.
        /// </summary>
        /// <value>
        /// Gets or sets the Affected value.
        /// </value>
        public string Affected { get; set; }

        /// <summary>
        /// Status property.
        /// </summary>
        /// <value>
        /// Gets or sets the Status value.
        /// </value>
        public ValidationStatusEnum Status { get; set; }


        /// <summary>
        /// StatusLabel property.
        /// </summary>
        /// <value>
        /// Gets or sets the StatusLabel value.
        /// </value>
        public string ButtonLabel
        {
            get
            {
                return _statusLabels.First(s => s.Key == Status).Value.Item1;
            }
        }

        /// <summary>
        /// LastAction property.
        /// </summary>
        /// <value>
        /// Gets or sets the LastAction value.
        /// </value>
        public string StatusLabel
        {
            get
            {
                return _statusLabels.First(s => s.Key == Status).Value.Item2;
            }
        }

        /// <summary>
        /// LastAction property.
        /// </summary>
        /// <value>
        /// Gets or sets the LastAction value.
        /// </value>
        public string LastAction { get; set; }

        /// <summary>
        /// CanRemindModifier property.
        /// </summary>
        /// <value>
        /// Gets or sets the CanRemindModifier value.
        /// </value>
        public bool CanRemindModifier { get; set; }

        /// <summary>
        /// CanModifyInsteadOfModifier property.
        /// </summary>
        /// <value>
        /// Gets or sets the CanModifyInsteadOfModifier value.
        /// </value>
        public bool CanModifyInsteadOfModifier { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        public DashBoardValidationActivityModel()
        {
        }
    }
}
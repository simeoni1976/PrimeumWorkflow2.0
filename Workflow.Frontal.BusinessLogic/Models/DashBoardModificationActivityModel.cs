
using System;
using System.Collections.Generic;
using System.Linq;

namespace Workflow.Frontal.BusinessLogic.Models
{
    /// <summary>
    /// ModificationStatusEnum Enum
    /// </summary>
    public enum ModificationStatusEnum
    {
        None = 0,
        ToDo = 1,
        PendingValidation = 2,
        Validated = 3
    }

    /// <summary>
    /// DashBoardActiviteSectionModel configuration
    /// </summary>
    public class DashBoardModificationActivityModel
    {
        private Dictionary<ModificationStatusEnum, Tuple<string, string>> _statusLabels = new Dictionary<ModificationStatusEnum, Tuple<string, string>>()
        {
            { ModificationStatusEnum.ToDo,  Tuple.Create("TO DO", "Pending") },
            { ModificationStatusEnum.PendingValidation,Tuple.Create("PENDING VALIDATION", "Saved")},
            { ModificationStatusEnum.Validated,Tuple.Create("VALIDATED", "Modified") }
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
        public ModificationStatusEnum Status { get; set; }


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
        /// CanModify property.
        /// </summary>
        /// <value>
        /// Gets or sets the CanModify value.
        /// </value>
        public bool CanModify { get; set; }

        /// <summary>
        /// CanRemindValidator property.
        /// </summary>
        /// <value>
        /// Gets or sets the CanRemindValidator value.
        /// </value>
        public bool CanRemindValidator { get; set; }

        /// <summary>
        /// CanView property.
        /// </summary>
        /// <value>
        /// Gets or sets the CanView value.
        /// </value>
        public bool CanView { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        public DashBoardModificationActivityModel()
        {
        }
    }
}
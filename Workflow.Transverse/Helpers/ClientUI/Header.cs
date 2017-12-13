using Newtonsoft.Json;
using System.Collections.Generic;

namespace Workflow.Transverse.Helpers.ClientUI
{
    /// <summary>
    /// Header class.
    /// </summary>
    public class Header
    {
        /// <summary>
        /// LogoImageSource property.
        /// </summary>
        /// <value>
        /// Gets or sets the LogoImageSource value.
        /// </value>
        public string LogoImageSource { get; set; }

        /// <summary>
        /// ToolTip property.
        /// </summary>
        /// <value>
        /// Gets or sets the ToolTip value.
        /// </value>
        public bool ToolTip { get; set; }

        /// <summary>
        /// LeftMenu property.
        /// </summary>
        /// <value>
        /// Gets or sets the LeftMenu value.
        /// </value>
        public List<HyperLink> LeftMenu { get; set; }

        /// <summary>
        /// RightMenu property.
        /// </summary>
        /// <value>
        /// Gets or sets the RightMenu value.
        /// </value>
        public RightMenu RightMenu { get; set; }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public Header()
        {
            LeftMenu = new List<HyperLink>();
        }
    }
}
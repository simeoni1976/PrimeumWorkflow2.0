using Newtonsoft.Json;
using Workflow.Transverse.Helpers.ClientUI;

namespace Workflow.Transverse.Helpers
{
    /// <summary>
    /// ClientSettings class.
    /// </summary>
    public class ClientSettings
    {
        /// <summary>
        /// HeaderSetting property.
        /// </summary>
        /// <value>
        /// Gets or sets the HeaderSetting value.
        /// </value>
        [JsonProperty(PropertyName = "Header")]
        public Header Header { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        public ClientSettings()
        {
        }
    }
}
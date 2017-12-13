namespace Workflow.Transverse.Helpers.ClientUI
{
    /// <summary>
    /// RightMenu class.
    /// </summary>
    public class RightMenu
    {
        /// <summary>
        /// DefaultLanguage property.
        /// </summary>
        /// <value>
        /// Gets or sets the DefaultLanguage value.
        /// </value>
        public string DefaultLanguage { get; set; }

        /// <summary>
        /// LanguageMenu property.
        /// </summary>
        /// <value>
        /// Gets or sets the LanguageMenu value.
        /// </value>
        public bool LanguageMenu { get; set; }

        /// <summary>
        /// ProfilMenu property.
        /// </summary>
        /// <value>
        /// Gets or sets the ProfilMenu value.
        /// </value>
        public bool ProfilMenu { get; set; }

        /// <summary>
        /// Class constructor.
        /// </summary>
        public RightMenu()
        {
            LanguageMenu = true;
            ProfilMenu = true;
        }
    }
}
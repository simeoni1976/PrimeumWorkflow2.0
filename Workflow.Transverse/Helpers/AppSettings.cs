namespace Workflow.Transverse.Helpers
{
    /// <summary>
    /// AppSettings class.
    /// </summary>
    public class AppSettings
    {
        /// <summary>
        /// Api property.
        /// </summary>
        /// <value>
        /// Gets or sets the Api value.
        /// </value>
        public string Api { get; set; }

        public AppSetting_Auth Auth { get; set; }

        public AppSetting_ConnectionStrings ConnectionStrings { get; set; }

        /// <summary>
        /// Class constructor
        /// </summary>
        public AppSettings()
        {
        }
    }

    public class AppSetting_Auth
    {
        public AppSetting_Auth_Consumer Consumer { get; set; }
        public string TokenServerUrl { get; set; }
        public string AuthorizeServerUrl { get; set; }
        public string LoginUrl { get; set; }
        public string PostLogoutUrl { get; set; }
        public string ScopeClaimsTypeRolePrefix { get; set; }
    }


    public class AppSetting_Auth_Consumer
    {
        public string ConsumerKey { get; set; }
        public string ConsumerSecret { get; set; }
    }

    public class AppSetting_ConnectionStrings
    {
        public string ServerConnection { get; set; }
    }
}
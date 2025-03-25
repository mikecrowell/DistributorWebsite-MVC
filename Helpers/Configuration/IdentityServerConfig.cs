using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace DistributorWebsite.MVC.WebUI.Helpers.Configuration
{
    public class IdentityServerConfig
    {
        public String ClientID { get; set; }
        public String RedirectUri { get; set; }
        public String PostLogoutRedirectUri { get; set; }
        public String Authority { get; set; }
        public String ROP { get; set; }
        public String Secret { get; set; }

        #region "PROPERTY: TokenEndpoint"
        /// <summary>
        /// Get the token endpoint for the current authority
        /// </summary>
        public String TokenEndpoint
        {
            get
            {
                return (String.Format(this.Authority + "/connect/token"));
            }
        }
        #endregion

        public Boolean IsValid
        {
            get
            {
                return ((!String.IsNullOrWhiteSpace(ClientID)) &&
                        (!String.IsNullOrWhiteSpace(RedirectUri)) &&
                        (!String.IsNullOrWhiteSpace(PostLogoutRedirectUri)) &&
                        (!String.IsNullOrWhiteSpace(Authority)));
            }
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DistributorWebsite.MVC.WebUI
{
    /// <summary>
    /// Custom authorization to allow local windows authentication 
    /// </summary>
    public class HCAuthorizeInternalMenuAttribute : HCAuthorizeMenuAttribute
    {
        #region "CONSTRUCTORS"

        #region "CONSTRUCTOR(SecurityItem)"
        /// <summary>
        /// Initialize the HC Authorize Menu attribute 
        /// </summary>
        public HCAuthorizeInternalMenuAttribute(eSecurityItem securityItem):base(securityItem)
        {
        }
        #endregion

        #region "CONSTRUCTOR:(SecurityItem,SkipRedirection)"
        /// <summary>
        /// Initialize the HC Authorize menu attribute
        /// </summary>
        /// <param name="securityItem"></param>
        /// <param name="skipRedirection"></param>
        public HCAuthorizeInternalMenuAttribute(eSecurityItem securityItem, Boolean skipRedirection):base(securityItem, skipRedirection)
        {
        }
        #endregion

        #endregion

        #region "PROCEDURE: AuthorizeCore"
        /// <summary>
        /// Check to see if the user is authorized
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            //-- MAKE SURE THIS IS THE INTERNAL VERSION OF THE WEBSITE --
            if (Helpers.Application.Instance.AuthorizationType != eWebAuthType.WINDOWS)
            {
                return false;
            }

            //-- MAKE SURE THE USER IS LOGGED INTO THE WEBSITE --
            return base.AuthorizeCore(httpContext);
        }
        #endregion                       
    }
}
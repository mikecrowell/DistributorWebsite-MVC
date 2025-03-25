using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DistributorWebsite.MVC.WebUI
{
    /// <summary>
    /// Custom authorization to allow local windows authentication and remote identity server authentication
    /// </summary>
    public class HCAuthorizeMenuAttribute : HCAuthorizeAttribute
    {
        #region "PROPERTY: SecurityItem"
        /// <summary>
        /// Get/set the security item to check the access for
        /// </summary>
        protected List<eSecurityItem> SecurityItems { get; set; } = new List<eSecurityItem>();
        #endregion

        #region "CONSTRUCTORS"

        #region "CONSTRUCTOR(SecurityItem)"
        /// <summary>
        /// Initialize the HC Authorize Menu attribute 
        /// </summary>
        public HCAuthorizeMenuAttribute(eSecurityItem securityItem):base()
        {
            this.SecurityItems.Add(securityItem);
        }
        #endregion

        #region "CONSTRUCTOR:(SecurityItem,SkipRedirection)"
        /// <summary>
        /// Initialize the HC Authorize menu attribute
        /// </summary>
        /// <param name="securityItem"></param>
        /// <param name="skipRedirection"></param>
        public HCAuthorizeMenuAttribute(eSecurityItem securityItem, Boolean skipRedirection):base(skipRedirection)
        {
            this.SecurityItems.Add(securityItem);
        }
        #endregion

        #region "CONSTRUCTOR(SecurityItem)"
        /// <summary>
        /// Initialize the HC Authorize Menu attribute 
        /// </summary>
        public HCAuthorizeMenuAttribute(params eSecurityItem[] securityItems) : base()
        {
            this.SecurityItems.AddRange(securityItems.ToList());
        }
        #endregion

        #region "CONSTRUCTOR:(SecurityItem,SkipRedirection)"
        /// <summary>
        /// Initialize the HC Authorize menu attribute
        /// </summary>
        /// <param name="securityItem"></param>
        /// <param name="skipRedirection"></param>
        public HCAuthorizeMenuAttribute(Boolean skipRedirection, params eSecurityItem[] securityItems) : base(skipRedirection)
        {
            this.SecurityItems.AddRange(securityItems.ToList());
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
            //-- MAKE SURE THE USER IS LOGGED INTO THE WEBSITE --
            bool isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                return false;
            }

            //-- CHECK THE USER'S ACCESS TO THE CURRENT SECURITY ITEM --
            foreach (var securityItem in this.SecurityItems)
            {
                if (!Helpers.Common.CanUserAccessSecurityItem(securityItem))
                {
                    isAuthorized = false;
                    break;
                }
            }

            return isAuthorized;
        }
        #endregion                       
    }
}
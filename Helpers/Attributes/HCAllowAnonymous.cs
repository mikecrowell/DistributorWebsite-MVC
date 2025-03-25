using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DistributorWebsite.MVC.WebUI
{
    public class HCAllowAnonymous : AuthorizeAttribute
    {
        #region "FUNCTION: AuthorizeCore"
        /// <summary>
        /// Always allow
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            return (true);
        }
        #endregion
    }
}
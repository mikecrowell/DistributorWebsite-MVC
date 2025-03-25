using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DistributorWebsite.MVC.WebUI.Controllers
{
    public class InCiteController : BaseController
    {
        #region "CONTROLLER: Login"
        /// <summary>
        /// Display the incite login page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuInciteLogin)]
        public async Task<ActionResult> Login()
        {
            string url = null;

            try
            {
                //-- UPDATE THE DEFAULT ENTITY FOR THE SALESFORCE LOGIN --
                using (var api = new Helpers.WebAPI(language: this.CurrentLanguage, existingToken: this.CurrentUser))
                {
                    await api.InitializeSalesForceEntity();
                }
            }
            catch (Exception ex)
            {
                TempData["errormessage"] = ex.ToString();

                return new RedirectToRouteResult(new RouteValueDictionary
                                                 {
                                                    { "action", "Index" },
                                                    { "controller", "Error" },
                                                    { "id", "NOHEADER" },
                                                    { "showHeader", false },
                                                    { "showLanguageLinks", false },
                                                    { "showReturnToLoginButton", false },
                                                    { "showLoginLink", false }
                                                 });
            }

            //-- REDIRECT TO INCITE --
            url = $"{Helpers.Application.Instance.InCiteURL}?oauth_token={this.CurrentAccessToken}";
            return new RedirectResult(url);
        }
        #endregion

        #region "CONTROLLER: Login/{id}"
        /// <summary>
        /// Redirect the user to the InCite (SalesForce) application
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuInciteLogin)]
        [Route("InCite/Login/{id:int}")]
        public async Task<ActionResult> Login(Int32 id)
        {
            string url = null;

            try
            {
                //-- UPDATE THE DEFAULT ENTITY FOR THE SALESFORCE LOGIN --
                using (var api = new Helpers.WebAPI(language: this.CurrentLanguage, existingToken: this.CurrentUser))
                {
                    await api.UpdateSalesForceDefaultEntity(id);
                }
            }
            catch (Exception ex)
            {
                TempData["errormessage"] = ex.ToString();

                return new RedirectToRouteResult(new RouteValueDictionary
                                                 {
                                                    { "action", "Index" },
                                                    { "controller", "Error" },
                                                    { "id", "NOHEADER" },
                                                    { "showHeader", false },
                                                    { "showLanguageLinks", false },
                                                    { "showReturnToLoginButton", false },
                                                    { "showLoginLink", false }
                                                 });
            }

            //-- REDIRECT TO INCITE --
            url = $"{Helpers.Application.Instance.InCiteURL}?oauth_token={this.CurrentAccessToken}";
            return new RedirectResult(url);
        }
        #endregion
    }
}
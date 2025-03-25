using DistributorWebsite.MVC.WebUI.Helpers.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace DistributorWebsite.MVC.WebUI.Controllers
{
    public class ErrorController : BaseControllerNoEntityChecks
    {
        #region "CONTROLLER: Index"
        /// <summary>
        /// Default error view - internal server error
        /// </summary>
        /// <param name="title"></param>
        /// <param name="desc"></param>
        /// <param name="message"></param>
        /// <param name="showHeader"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Index(String title = "", String desc = "", String message = "", Boolean showHeader = true, Boolean showLanguageLinks = true, Boolean showLoginLink = true)
        {
            return ErrorView(GetErrorInfo(System.Net.HttpStatusCode.InternalServerError, showHeader, showLanguageLinks, showLoginLink, title, desc, message));
        }
        #endregion

        #region "CONTROLLER: AccessProhibited"
        /// <summary>
        /// Display the access prohibited page
        /// </summary>
        /// <param name="title"></param>
        /// <param name="desc"></param>
        /// <param name="message"></param>
        /// <param name="showHeader"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult AccessProhibited(String title = "", String desc = "", String message = "", Boolean showHeader = false, Boolean showLanguageLinks = false, Boolean showLoginLink = false)
        {
            return Unauthorized(title, desc, message, showHeader, showLanguageLinks, showLoginLink);
        }
        #endregion

        #region "CONTROLLER: IdentityServerUnavailable"
        /// <summary>
        /// Generic identity server unavailabe response
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="desc"></param>
        /// <param name="message"></param>
        /// <param name="showHeader"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult IdentityServerUnavailable(String id = "NOHEADER", String title = null, String desc = null, String message = "", Boolean showHeader = false, Boolean showLanguageLinks = true, Boolean showLoginLink = true)
        {
            return ErrorView(GetErrorInfo(System.Net.HttpStatusCode.ServiceUnavailable, showHeader, showLanguageLinks, showLoginLink, title, desc, message, "IdentityServerUnavailable"));
        }
        #endregion

        #region "CONTROLLER: Unauthorized"
        /// <summary>
        /// Generic unauthorized response
        /// </summary>
        /// <param name="id"></param>
        /// <param name="title"></param>
        /// <param name="desc"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Unauthorized(String title = "", String desc = "", String message = "", Boolean showHeader = true, Boolean showLanguageLinks = true, Boolean showLoginLink = true)
        {
            HCExceptionInfo info = GetErrorInfo(System.Net.HttpStatusCode.Unauthorized, showHeader, showLanguageLinks, showLoginLink, title, desc, message);
            return ErrorView(info);
        }
        #endregion

        #region "CONTROLLER: Forbidden"
        //
        // GET: /Error/Forbidden
        [AllowAnonymous]
        public ActionResult Forbidden(String title = "", String desc = "", String message = "", Boolean showHeader = true, Boolean showLanguageLinks = true, Boolean showLoginLink = true)
        {
            return ErrorView(GetErrorInfo(System.Net.HttpStatusCode.Forbidden,showHeader, showLanguageLinks, showLoginLink, title, desc, message, "Unauthorized"));
        }
        #endregion

        public ActionResult BadRequest(String title = "", String desc = "", String message = "", Boolean showHeader = true, Boolean showLanguageLinks = true, Boolean showLoginLink = true)
        {
            return ErrorView(GetErrorInfo(System.Net.HttpStatusCode.BadRequest, showHeader, showLanguageLinks, showLoginLink, title, desc, message));
        }

        //
        // GET: /Error/NotFound
        [AllowAnonymous]
        public ActionResult DocumentNotFound()
        {
            return ErrorView(GetErrorInfo(System.Net.HttpStatusCode.NotFound, false, false, false, resourceKey: "DocumentNotFound"));
        }

        //
        // GET: /Error/NotFound
        [AllowAnonymous]
        public ActionResult NotFound(String title = "", String desc = "", String message = "", Boolean showHeader = true, Boolean showLanguageLinks = true, Boolean showLoginLink = true)
        {
            return ErrorView(GetErrorInfo(System.Net.HttpStatusCode.NotFound, showHeader, showLanguageLinks, showLoginLink, title, desc, message));
        }

        #region "FUNCTION: ErrorView"
        /// <summary>
        /// Return the appropriate error view
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        private ActionResult ErrorView(HCExceptionInfo info)
        {
            Boolean showHeader = true;

            //-- CHECK TO SEE WHETHER OR NOT TO DISPLAY THE NORMAL HEADER/MENU SYSTEM FOR THE CURRENT PAGE --
            if (!System.Web.HttpContext.Current.Request.IsAuthenticated)
                showHeader = false;
            else if (!info.ShowErrorHeader)
                showHeader = false;

            //-- DETERMINE WHETHER OR NOT TO SHOT THE RETURN TO LOGIN BUTTON --
            ViewBag.SHOWRETURNTOLOGIN = info.ShowReturnToLoginButton;

            //-- DETERMINE WHETHER OR NOT THE SHOW THE LANGUAGE LINKS --
            ViewBag.SHOWLANGUAGELINKS = info.ShowLanguageLinks;

            if (showHeader)
                return View("Index", info);
            else
                return View("IndexNH", info);
        }
        #endregion

        #region "FUNCTION: GetErrorInfo"
        /// <summary>
        /// Build the error info object for the current exception
        /// </summary>
        /// <param name="status"></param>
        /// <param name="title"></param>
        /// <param name="desc"></param>
        /// <returns></returns>
        private HCExceptionInfo GetErrorInfo(System.Net.HttpStatusCode status = System.Net.HttpStatusCode.InternalServerError, Boolean showHeader = false, Boolean showLanguageButtons = true, Boolean showLoginLinkButton = true, String title = null, String desc = null, String message = null, String resourceKey = null)
        {
            HCExceptionInfo info = new HCExceptionInfo();
            Tuple<string, string> errorInfo;

            try
            {
                //-- GET THE LOCALIZED ERROR TITLE AND DESCRIPTION FOR THE STATUS CODE --
                errorInfo = GetStatusCodeText(status, resourceKey);
                
                //-- BUILD THE ERROR INFO OBJECT --
                info.PageTitle = String.Format("{0} - {1}", (Int32)status, errorInfo.Item1);
                info.ErrorCode = ((Int32)status).ToString();
                info.ErrorTitle = String.IsNullOrWhiteSpace(title) ? errorInfo.Item1 : title;
                info.ErrorDescription = String.IsNullOrWhiteSpace(desc) ? errorInfo.Item2 : desc;

                //-- ADD THE ERROR DETAILS --
                if (Helpers.Application.Instance.ShowErrorDetails)
                {
                    if (String.IsNullOrWhiteSpace(message))
                    {
                        if (TempData.ContainsKey("errormessage"))
                            message = TempData["errormessage"].ToString();
                    }

                    if (!String.IsNullOrWhiteSpace(message))
                    {
                        info.ShowErrorDetails = true;
                        info.ErrorDetails = message;
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                info = new HCExceptionInfo();
                info.PageTitle = String.Format("{0} - {1}", (Int32)status, LocalResources.HCResources.InternalServerError);
                info.ErrorCode = ((Int32)status).ToString();
                info.ErrorTitle = LocalResources.HCResources.InternalServerError;
                info.ErrorDescription = LocalResources.HCResources.InternalServerErrorDescription;
            }

            info.ShowErrorHeader = showHeader;
            info.ShowLanguageLinks = showLanguageButtons;
            info.ShowReturnToLoginButton = showLoginLinkButton;

            return (info);
        }
        #endregion

        #region "FUNCTION: GetStatusCodeText"
        /// <summary>
        /// Get the localized status code text for the specified status code
        /// </summary>
        /// <param name="status"></param>
        /// <returns></returns>
        private Tuple<String, String> GetStatusCodeText(System.Net.HttpStatusCode status, String resourceKey = null)
        {
            string title = null;
            string description;

            //-- SET THE ERROR TITLE --
            title = LocalResources.HCResources.ResourceManager.GetString(resourceKey ?? status.ToString()) ?? String.Format("{0}-{1}", status, LocalResources.HCResources.InternalServerError);
            description = LocalResources.HCResources.ResourceManager.GetString(String.Format("{0}Description", resourceKey ?? status.ToString())) ?? LocalResources.HCResources.InternalServerErrorDescription;

            return new Tuple<string, string>(title, description);
        }
        #endregion
    }
}
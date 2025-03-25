using DistributorWebsite.MVC.WebUI.Helpers.Security;
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
    public class HCAuthorizeAttribute : AuthorizeAttribute
    {
        #region "ENUM: eAuthError"
        /// <summary>
        /// List of reasons a user can be denied access
        /// </summary>
        protected enum eAuthError
        {
            /// <summary>
            /// Authorization status is unknown
            /// </summary>
            Unknown,

            /// <summary>
            /// User is not authorized
            /// </summary>
            NotAuthorized,

            /// <summary>
            /// The access token is missing or invalid
            /// </summary>
            MissingOrInvalidAccessToken,

            /// <summary>
            /// The entity is missing or invalid
            /// </summary>
            MissingOrInvalidEntity,

            /// <summary>
            /// Access is forbidden for the current user
            /// </summary>
            Forbidden
        }
        #endregion

        #region "PROPERTY: AuthorizationStatus"
        /// <summary>
        /// Get/set the current authorization status
        /// </summary>
        protected eAuthError AuthorizationStatus { get; set; }
        #endregion

        #region "PROPERTY: SkipRedirectionToEntitySelectionPage"
        /// <summary>
        /// Skip the redirection to the entity selection page
        /// </summary>
        private bool SkipRedirectionToEntitySelectionPage { get; set; }
        #endregion

        #region "FUNCTION: CanAccessSite"
        /// <summary>
        /// Get/set whether or not the current user can access the site
        /// </summary>
        protected bool CanAccessSite { get; set; }
        #endregion

        #region "FUNCTION: RedirectToEntitySelectionPage"
        /// <summary>
        /// Get/set whether or not the user should be redirected to the entity selection page
        /// </summary>
        private bool RedirectToEntitySelectionPage { get; set; }
        #endregion

        #region "CONSTRUCTORS"

        #region "CONSTRUCTOR"
        public HCAuthorizeAttribute():base()
        {
            this.SkipRedirectionToEntitySelectionPage = false;
            this.AuthorizationStatus = eAuthError.Unknown;
        }
        #endregion

        #region "CONSTRUCTOR(skipRedirection)"
        /// <summary>
        /// Initialize the HC Authorize attribute and skip redirection to the entity selection page
        /// </summary>
        /// <param name="skipRedirection"></param>
        public HCAuthorizeAttribute(bool skipRedirection):this()
        {
            this.SkipRedirectionToEntitySelectionPage = skipRedirection;
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
            Logger.LogControllerAndAction();

            System.Diagnostics.Trace.TraceInformation(String.Format("AuthorizeCore: AUTHORIZED = {0}", httpContext.User.Identity.IsAuthenticated));
            this.CanAccessSite = false;

            var test = DependencyResolver.Current.GetService<IUserAccessToken>();
            if (test != null)
            {
                System.Diagnostics.Trace.TraceInformation(test.ToString());
            }

            //-- MAKE SURE THE USER IS LOGGED INTO THE WEBSITE --
            bool isAuthorized = base.AuthorizeCore(httpContext);
            if (!isAuthorized)
            {
                AuthorizationStatus = eAuthError.NotAuthorized;
                this.CanAccessSite = false;
                return false;
            }

            switch (Helpers.Application.Instance.AuthorizationType)
            {
                case eWebAuthType.WINDOWS:

                    //-- CHECK THE WINDOWS AUTHENTICATION --
                    if (!AuthorizeWindowsUser(httpContext))
                    {
                        AuthorizationStatus = eAuthError.NotAuthorized;
                        this.CanAccessSite = false;
                        return (false);
                    }
                    break;

                case eWebAuthType.IDENTITYSERVER:

                    //-- MAKE SURE THE USER HAS A VALID ACCESS TOKEN --
                    if (String.IsNullOrWhiteSpace(Helpers.CookieHelper.AccessToken))
                    {
                        this.AuthorizationStatus = eAuthError.MissingOrInvalidAccessToken;
                        return (false);
                    }

                    //-- MAKE SURE A CURRENT ENTITY IS SELECTED --
                    if (String.IsNullOrEmpty(Helpers.CookieHelper.CurrentEntity))
                    {
                        this.AuthorizationStatus = eAuthError.MissingOrInvalidEntity;
                        return (false);
                    }
                    break;
            }

            //-- THE USER CAN ACCESS THE SITE --
            this.CanAccessSite = true;

            if (this.SkipRedirectionToEntitySelectionPage)
                RedirectToEntitySelectionPage = false;
            else
            {
                switch (Helpers.Application.Instance.AuthorizationType)
                {
                    case eWebAuthType.WINDOWS:
                        RedirectToEntitySelectionPage = String.IsNullOrWhiteSpace(Helpers.CookieHelper.AccessToken) || 
                                                        String.IsNullOrWhiteSpace(Helpers.CookieHelper.CurrentEntity) || 
                                                        Helpers.CookieHelper.AccessTokenType == eAccessTokenType.InternalCore;
                        break;

                    default:
                        RedirectToEntitySelectionPage = false;
                        break;
                }
            }

            return true;
        }
        #endregion
        
        #region "PROCEDURE: OnAuthorization"
        /// <summary>
        /// Redirect the user to the login page if he/she is using windows authentication
        /// </summary>
        /// <param name="filterContext"></param>
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            System.Diagnostics.Trace.TraceInformation(String.Format("OnAuthorization START: AUTHORIZED = {0}", filterContext.HttpContext.User.Identity.IsAuthenticated));

            var skipAuthorization = filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                                    filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                                    filterContext.ActionDescriptor.IsDefined(typeof(HCAllowAnonymous), true) ||
                                    filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(HCAllowAnonymous), true);

            if (!skipAuthorization)
            {
                switch (Helpers.Application.Instance.AuthorizationType)
                {
                    case eWebAuthType.WINDOWS:
                        base.OnAuthorization(filterContext);
                        break;

                    default:
                        base.OnAuthorization(filterContext);
                        break;
                }

                if (RedirectToEntitySelectionPage)
                {
                    //--------------------------------------------------------------------
                    //-- FOR WINDOWS AUTHENTICATION - REDIRECT TO THE UNAUTHORIZED PAGE --
                    //--------------------------------------------------------------------
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                                                                 {
                                                                   { "action", "SelectEntity" },
                                                                   { "controller", "Internal" }
                                                                 });
                }
            }

            System.Diagnostics.Trace.TraceInformation(String.Format("OnAuthorization FINISHED: AUTHORIZED = {0}", filterContext.HttpContext.User.Identity.IsAuthenticated));
        }
        #endregion

        #region "PROCEDURE: HandleUnauthorizedRequest"
        /// <summary>
        /// Handle the unauthorized request
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            switch (Helpers.Application.Instance.AuthorizationType)
            {
                case eWebAuthType.WINDOWS:

                    //--------------------------------------------------------------------
                    //-- FOR WINDOWS AUTHENTICATION - REDIRECT TO THE UNAUTHORIZED PAGE --
                    //--------------------------------------------------------------------
                    filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                                                             {
                                                               { "action", "AccessProhibited" },
                                                               { "controller", "Error" }
                                                             });
                    break;

                case eWebAuthType.IDENTITYSERVER:

                    //------------------------------------------------------------------------------------------------
                    //-- FOR IDENTITY SERVER AUTHENTICATION - REDIRECT TO THE UNAUTHORIZED PAGE FOR SPECIFIC ERRORS --
                    //------------------------------------------------------------------------------------------------
                    switch (this.AuthorizationStatus)
                    {
                        case eAuthError.MissingOrInvalidAccessToken:
                        case eAuthError.MissingOrInvalidEntity:
                        case eAuthError.Forbidden:
                            filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary
                                                             {
                                                               { "action", "AccessProhibited" },
                                                               { "controller", "Error" }
                                                             });
                            break;

                        default:

                            //----------------------------
                            //-- USE DEFAULT PROCESSING --
                            //----------------------------
                            base.HandleUnauthorizedRequest(filterContext);
                            break;
                    }
                    break;

                default:

                    //----------------------------
                    //-- USE DEFAULT PROCESSING --
                    //----------------------------
                    base.HandleUnauthorizedRequest(filterContext);
                    break;
            }

        }
        #endregion

        #region "PROCEDURE: AuthorizeWindowsUser"
        /// <summary>
        /// Authorize the current windows user
        /// </summary>
        /// <param name="httpContext"></param>
        /// <returns></returns>
        private bool AuthorizeWindowsUser(HttpContextBase httpContext)
        {
            Boolean retVal = false;

            try
            {
                //-- CHECK THE AUTHORIZATION COOKIE --
                if (!Helpers.CookieHelper.LastWindowsAuthCheckTime.HasValue)
                {
                    foreach (String group in Helpers.Application.Instance.WindowsAuthRoles)
                    {
                        if (httpContext.User.IsInRole(group))
                        {
                            //-- USER IS IN ONE OF THE SPECIFIED ROLES - ALLOW ACCESS --
                            Helpers.CookieHelper.LastWindowsAuthCheckTime = DateTime.Now;
                            return (true);
                        }
                    }
                }
                else
                {
                    //-- AUTHORIZATION COOKIE IS STILL VALID --
                    retVal = true;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
                retVal = false;
            }

            return (retVal);
        }
        #endregion
    }
}
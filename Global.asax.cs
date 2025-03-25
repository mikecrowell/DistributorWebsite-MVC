using DistributorWebsite.MVC.WebUI.Helpers.Exceptions;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Serilog;

namespace DistributorWebsite.MVC.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            System.Diagnostics.Trace.TraceInformation("************************************************");
            System.Diagnostics.Trace.TraceInformation("GLOBAL_ASAX.Application_Start");
            System.Diagnostics.Trace.TraceInformation("************************************************");

            //-- INITIALIZE THE GLOBAL APPLICATION SETTINGS OBJECT --
            Helpers.Application.Instance.Initialize();

            //-- LOCALIZATION HELPER --
            ControllerBuilder.Current.SetControllerFactory(new DefaultControllerFactory(new CultureAwareControllerActivator()));

            //-- OTHER MVC INITIALIZATION --
            AreaRegistration.RegisterAllAreas();
            UnityConfig.RegisterComponents();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            //BundleTable.EnableOptimizations = true;
        }

        #region "PROCEDURE: Session_Start"
        /// <summary>
        /// Initialize the session
        /// </summary>
        protected void Session_Start()
        {
            //--------------------------------------
            //-- UPDATE THE USER DISPLAY SETTINGS --
            //--------------------------------------
            Helpers.CookieHelper.DateFormat = "MM/dd/yyyy";     //TODO: PULL FROM DATABASE

            //--------------------------------------------------------------------------------------
            //-- ATTEMPT TO LOG THE USER OUT IF HE IS CURRENTLY LOGGED IN FROM A PREVIOUS SESSION --
            //--------------------------------------------------------------------------------------
            try
            {
                var context = Request.GetOwinContext();

                if ((context != null) && (context.Authentication != null) && (context.Authentication.User != null) && (context.Authentication.User.Identity != null) && (context.Authentication.User.Identity.IsAuthenticated))
                {
                    System.Diagnostics.Debug.WriteLine("Logging out stuck user session");

                    context.Authentication.SignOut(new AuthenticationProperties()
                    {
                        RedirectUri = Helpers.Application.Instance.IdentityServer.RedirectUri
                    });
                }
            }
            catch (Exception)
            {
            }

        }
        #endregion

        #region "PROCEDURE: Application_Error"
        /// <summary>
        /// Global exception handler
        /// </summary>
        protected void Application_Error()
        {
            Exception exception;
            HCExceptionInfo info;

            try
            {
                exception = Server.GetLastError();

                //-- LOG THE EXCEPTION --
                if (exception != null)
                    Logger.LogError(exception.ToString());

                info = ExceptionHelpers.ParseException(exception);

                switch (info.ErrorType)
                {
                    case eCustomErrorType.CanNotConnectToIdentityServer:
                    case eCustomErrorType.AccessIsForbidden:

                        //-- PROBLEM CONNECTING TO IDENTITY SERVER --
                        Response.Clear();
                        Server.ClearError();
                        Response.Redirect(info.RedirectURL);
                        break;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
            }
        }
        #endregion
    }
}

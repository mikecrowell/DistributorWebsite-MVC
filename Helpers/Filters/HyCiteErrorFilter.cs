using System.Web.Mvc;
using System.Web;
using System.Net;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Web.Routing;

namespace DistributorWebsite.MVC
{
    /// <summary>
    /// Custom error handler processing
    /// </summary>
    public class HyciteErrorAttribute : FilterAttribute, IExceptionFilter
    {
        #region "PROCEDURE: OnException"
        /// <summary>
        /// Process the unhandled exception
        /// </summary>
        /// <param name="filterContext"></param>
        public void OnException(ExceptionContext filterContext)
        {
            Int32 statusCode;
            ActionResult result;


            //-- CHECK TO SEE IF THE EXCEPTION HAS BEEN HANDLED --
            if (filterContext.ExceptionHandled || !filterContext.HttpContext.IsCustomErrorEnabled)
                return;

            //-- GET THE STATUS CODE --
            statusCode = (Int32)HttpStatusCode.InternalServerError;

            if (filterContext.Exception is HttpException)
            {
                statusCode = (filterContext.Exception as HttpException).GetHttpCode();
            }
            else if (filterContext.Exception is UnauthorizedAccessException)
            {
                statusCode = (Int32)HttpStatusCode.Forbidden;
            }

            //-- TODO: LOG THE ERROR SOMEWHERE --
            //if ((filterContext.Exception != null) && (filterContext.Exception.Message.ToLower().Contains("see the inner exception for details")) && (filterContext.Exception.InnerException != null))
            //    DistributorWebsite.WebUI.Helpers.ExceptionManager.HandleError(filterContext.HttpContext, filterContext.Exception.InnerException);
            //else
            //    DistributorWebsite.WebUI.Helpers.ExceptionManager.HandleError(filterContext.HttpContext, filterContext.Exception);

            //-- DETERMINE WHAT ACTION TO TAKE FOR THE CURRENT EXCEPTION --
            result = CreateActionResult(filterContext, statusCode);
            filterContext.Result = result;
            filterContext.ExceptionHandled = true;

            //-- PREPARE THE RESPONSE --
            //filterContext.ExceptionHandled = true;
            //filterContext.HttpContext.Response.Clear();
            //filterContext.HttpContext.Response.StatusCode = statusCode;
            //filterContext.HttpContext.Response.TrySkipIisCustomErrors = true;
        }
        #endregion

        #region "PROCEDURE: CreateActionResult"
        /// <summary>
        /// Create the action to be taken for the current error
        /// </summary>
        /// <param name="filterContext"></param>
        /// <param name="statusCode"></param>
        /// <returns></returns>
        protected virtual ActionResult CreateActionResult(ExceptionContext filterContext, int statusCode)
        {
            String actionName;
            Boolean showHeader = true;
            Boolean isAntiforgeryException = false;

            var ctx = new ControllerContext(filterContext.RequestContext, filterContext.Controller);
            var statusCodeName = ((HttpStatusCode)statusCode).ToString();

            showHeader = (filterContext.Exception == null ? true : (filterContext.Exception.HelpLink == "NOHEADER" ? false : true));
            if (showHeader && (filterContext.RouteData.Values["action"].ToString() == "ViewStaticFile"))
                showHeader = false;

            isAntiforgeryException = (filterContext.Exception != null && filterContext.Exception is HttpAntiForgeryException);

            if (isAntiforgeryException)
                statusCode = 403;

            switch (statusCode)
            {
                case 404:
                    actionName = "NotFound";
                    break;

                case 403:
                    actionName = "Forbidden";
                    break;

                case 401:
                    actionName = "Unauthorized";
                    break;

                default:
                    actionName = "Index";
                    break;
            }

            return new RedirectToRouteResult(new RouteValueDictionary()
                                       {
                                         { "controller", "Error" },
                                         { "action", actionName },
                                         { "showHeader", showHeader },
                                         { "id", (showHeader ? "HEADER" : "NOHEADER")}
                                       });
        }
        #endregion
    }

}
using System;
using System.Web;
using System.Web.Mvc;
using System.Linq;
using Serilog;

namespace DistributorWebsite.MVC.WebUI
{
    public class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            ReflectedControllerDescriptor descriptor;
            ActionDescriptor action;

            try
            {
                //-- CHECK TO SEE IF THE ACTION IS DECORATED WITH THE SkipNocacheFilter ATTRIBUTE --
                descriptor = new ReflectedControllerDescriptor(filterContext.Controller.GetType());
                action = descriptor.FindAction(filterContext.Controller.ControllerContext, filterContext.RequestContext.RouteData.GetRequiredString("action"));

                if (!action.GetCustomAttributes(typeof(SkipNocacheFilterAttribute), true).Any())
                {
                    //-- ADD THE NO-CACHE HEADERS --
                    filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
                    filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
                    filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                    filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                    filterContext.HttpContext.Response.Cache.SetNoStore();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                base.OnResultExecuting(filterContext);
            }

            base.OnResultExecuting(filterContext);
        }
    }

}
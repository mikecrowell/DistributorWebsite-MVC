using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DistributorWebsite.MVC.WebUI
{
    public class CultureAwareControllerActivator : IControllerActivator
    {
        public IController Create(RequestContext requestContext, Type controllerType)
        {
            String currentLanguage;
            String currentCulture;

            try
            {
                //---------------------------------------------
                //-- UPDATE THE LANGUAGE AND CULTURE COOKIES --
                //---------------------------------------------
                currentCulture = Helpers.CultureHelper.InitializeCurrentCulture();
                currentLanguage = Helpers.CultureHelper.GetLanguageFromCulture(currentCulture);

                //-- MODIFY THE CURRENT THREAD'S CULTURE --            
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(currentCulture);
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(currentLanguage);

                //-- SET THE CULTURE ON THE RESOURCE FILE --
                DistributorWebsite.MVC.LocalResources.HCResources.Culture = System.Threading.Thread.CurrentThread.CurrentUICulture;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }

            return DependencyResolver.Current.GetService(controllerType) as IController;
        }
    }
}
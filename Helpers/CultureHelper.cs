using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Serilog;

namespace DistributorWebsite.MVC.WebUI.Helpers
{
    public static class CultureHelper
    {
        #region "LANGUAGE AND CULTURE HELPERS"

        #region "FUNCTION: InitializeCurrentCulture"
        /// <summary>
        /// Get the current culture
        /// </summary>
        /// <returns></returns>
        public static String InitializeCurrentCulture()
        {

            String currentCulture = GetCurrentRequestCulture();

            //-- UPDATE THE CURRENT CULTURE COOKIE --
            Helpers.CookieHelper.CurrentCulture = currentCulture;

            return currentCulture;
        }
        #endregion

        #region "FUNCTION: GetLanguageFromCulture"
        /// <summary>
        /// Get the current language
        /// </summary>
        /// <returns></returns>
        public static String GetLanguageFromCulture(String currentCulture)
        {
            String currentLanguage = "";

            try
            {
                currentLanguage = currentCulture.Substring(0, 2).ToLower();

                if (!("|en|es|pt|".Contains(currentLanguage)))
                    currentLanguage = "en";
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("DEFAULTING LANGUAGE TO en");
                currentLanguage = "en";
            }

            //-- GET THE CURRENT LANGUAGE --
            return currentLanguage;
        }
        #endregion

        #region "FUNCTION: GetCurrentRequestCulture"
        /// <summary>
        /// Get the culture to use for the current request
        /// </summary>
        /// <returns></returns>
        public static string GetCurrentRequestCulture()
        {
            String currentCulture = null;
            String supportedCulture = null;

            //-- CHECK THE CURRENT QUERYSTRING IF IS IS AVAILABLE --
            try
            {
                var queryString = HttpContext.Current?.Request?.QueryString;
                if (queryString != null && queryString.AllKeys.Contains("changelang"))
                {
                    currentCulture = queryString["changelang"];

                    if (!String.IsNullOrWhiteSpace(currentCulture))
                    {
                        switch (currentCulture.ToUpper())
                        {
                            case "EN":
                                currentCulture = "en-US";
                                break;

                            case "ES":
                                currentCulture = "es-MX";
                                break;

                            case "PT":
                                currentCulture = "pt-BR";
                                break;
                        }
                    }

                    supportedCulture = Helpers.Application.Instance.SupportedCultures.FirstOrDefault(o => o.ToUpper() == currentCulture.ToUpper());
                    if (!String.IsNullOrWhiteSpace(supportedCulture))
                        return (supportedCulture);
                }
            }
            catch (Exception)
            {
            }

            //-- CHECK THE LANGUAGE COOKIE NEXT - DEFAULT TO en-US IF NOTHING IS FOUND --
            try
            {
                if (!String.IsNullOrWhiteSpace(Helpers.CookieHelper.CurrentCulture))
                    currentCulture = Helpers.CookieHelper.CurrentCulture;
                else
                    currentCulture = HttpContext.Current.Request.UserLanguages[0];

                supportedCulture = Helpers.Application.Instance.SupportedCultures.FirstOrDefault(o => o.ToUpper() == currentCulture.ToUpper());
                if (!String.IsNullOrWhiteSpace(supportedCulture))
                    return (supportedCulture);
            }
            catch (Exception)
            {
                System.Diagnostics.Debug.WriteLine("DEFAULTING CULTURE TO en-US");
            }

            //-- DEFAULT TO US ENGLISH --
            return ("en-US");
        }
        #endregion

        #endregion
    }
}
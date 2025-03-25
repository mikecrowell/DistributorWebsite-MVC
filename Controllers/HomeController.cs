using DistributorWebsite.MVC.WebUI.Helpers.Structures;
using Microsoft.Owin.Security;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Serilog;
using System.Web.Routing;
using DistributorWebsite.MVC.WebUI.Models;
using DistributorWebsite.MVC.LocalResources;

namespace DistributorWebsite.MVC.WebUI.Controllers
{
    public class HomeController : BaseController
    {
        #region "CONTROLLER: ReloadSecurityItems"
        /// <summary>
        /// Reload the security items
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [HCAllowAnonymous]
        public async Task<ActionResult> ReloadSecurityItems(string key)
        {
            //-- VALIDATE THE KEY --
            if (key != System.Configuration.ConfigurationManager.AppSettings["SECRELOAD.KEY"])
                return new HttpStatusCodeResult(System.Net.HttpStatusCode.Forbidden);

            //-- RELOAD THE SECURITY ITEMS --
            await Helpers.Navigation.RefreshSecurityItemsFromAPI();

            //-- RETURN THE OK RESPONSE --
            return new HttpStatusCodeResult(System.Net.HttpStatusCode.OK);
        }
        #endregion

        #region "CONTROLLER: VerifyEmail"
        /// <summary>
        /// Verify the email verification token
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HCAuthorize(skipRedirection: true)]
        [HCAllowAnonymous]
        public async Task<ActionResult> VerifyEmail(string token)
        {
            EmailVerificationResultDTO model;
            String language;

            //-- DON'T BUILD OR SHOW THE MENU ON THE ENTITY SELECTION PAGE --
            ViewBag.USEEMPTYCONTAINER = true;
            ViewBag.HIDELANGUAGELINKS = true;
            ViewBag.CurrentBrand = "";
            ViewBag.ShowSelectEntities = false;

            //-- TRY TO VERIFY THE EMAIL --
            try
            {
                //-- PARSE THE LANGUAGE OUT OF THE TOKEN --
                if (String.IsNullOrWhiteSpace(token))
                    language = this.CurrentLanguage;
                else if (!token.EndsWith("-EN") && !token.EndsWith("-ES") && !token.EndsWith("-PT"))
                    language = this.CurrentLanguage;
                else
                {
                    language = token.Substring(token.Length - 2, 2);
                    token = token.Substring(0, token.Length - 3);
                }

                using (Helpers.WebAPI oAPI = new Helpers.WebAPI(type: eAccessTokenType.InternalCore, language: language))
                {
                    model = await oAPI.VerifyEmail(token);
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                model = new EmailVerificationResultDTO();
                model.PanelCSS = "panel-danger";
                model.Header = LocalResources.HCResources.InternalServerError;
                model.Message = LocalResources.HCResources.InternalServerErrorDescription;
                model.StatusCode = System.Net.HttpStatusCode.InternalServerError;
            }
 
            return View(model);
        }
        #endregion

        #region "CONTROLLER: Index"
        /// <summary>
        /// Default page for all entities
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.DistributorWebsite)]
        public async Task<ActionResult> Index()
        {
            LandingPageInfoDTO info;
            String defaultChartTab = "";

            try
            {
                using (Helpers.WebAPI oAPI = new Helpers.WebAPI(language: this.CurrentLanguage, existingToken: this.CurrentUser))
                {
                    if (this.CurrentUser.IsDistributor)
                    {
                        info = await oAPI.GetDistributorLandingPageInfo(this.CurrentUser.EntityCode);

                        if (info.PriceLevelChart != null)
                        {
                            //-- ADD THE PRICE LEVEL CHART --
                            info.Charts.Add(new PerformanceChartDTO()
                            {
                                AmountDisplayType = "DECIMAL",
                                Color = "0,255,0",
                                DaysInPeriod = 12,
                                TitleXAxis = "",
                                TitleYAxis = HCResources.CompanyPurchases,
                                PercentOfGoal = info.PriceLevelChart.CompanyPurchases,
                                PerformanceChartType = "PRICELEVEL",
                                Tab = "PRICELEVEL",
                                TabItemSequence = 1,
                                TitlePreviousPeriod = HCResources.Level + " 3",
                                TitleTarget = HCResources.Level + " 2",
                                TitleCurrentPeriod = HCResources.Level + " 1",
                                TitleLegend = HCResources.Level,
                                TitleLegendGoal = HCResources.Salesperson,
                                TitleMain = info.PriceLevelChart.ChartTitle,
                                TitlePercentOfGoal = info.PriceLevelChart.OnTrackFor,
                                TotalLastPeriod = info.PriceLevelChart.Level3Required,
                                TotalTarget = info.PriceLevelChart.Level2Required,
                                TotalCurrentPeriod = info.PriceLevelChart.Level1Required,
                                TotalType = "DECIMAL",
                                TotalValues = info.PriceLevelChart.CompanyPurchaseValues,
                                TitleSub = info.PriceLevelChart.OnTrackFor,
                                TitleSubBGColor = info.PriceLevelChart.OnTrackForBackgroundColor,
                                TitleSubFGColor = info.PriceLevelChart.OnTrackForForegroundColor
                            });
                        }
                    }
                    else
                        info = await oAPI.GetSalespersonLandingPageInfo(this.CurrentUser.EntityCode);
                }

                //-- CLEAR THE PRIVATE MESSAGE COUNT IF USER CAN'T ACCESS --
                if (!Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.mnuPrivateMessages, this.CurrentUser))
                    info.TotalUnreadPrivateMessages = 0;

                //-- UPDATE THE BRAND --
                info.UpdateBrand(this.CurrentUser.Brand);

                //-- INITIALIZE THE HOME PAGE LINKS --
                info.Links = new HomePageLinks(this.CurrentUser, this.CurrentLanguage);

                //-- CHECK FOR AVAILABLE LEADS TO PURCHASE --
                using (Helpers.WebAPI oAPI = new Helpers.WebAPI(language: this.CurrentLanguage, existingToken: this.CurrentUser))
                {
                    var leads = await oAPI.GetPossibleLeads();
                    bool possibleLeads = false;

                    if (leads != null)
                    {
                        possibleLeads = leads.Count() > 0;                       
                    }
                    else
                    {
                        possibleLeads = false;
                    }

                    DistributorWebsite.MVC.WebUI.Helpers.CookieHelper.HasPossibleLeads = possibleLeads;
                    ViewBag.HASLEADS = possibleLeads;
                }

            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                info = null;
            }

            if (info == null)
                info = new LandingPageInfoDTO();

            //-- SET THE DEFAULT PERFORMANCE CHART TAB --
            if (info.HasPerformanceCharts)
            {
                if (info.HasVolumeCharts)
                    defaultChartTab = "VOLUME";
                else if (info.HasIcebreakerCharts)
                    defaultChartTab = "ICEBREAKER";
            }

            //-- WHETHER OR NOT TO SHOW THE SALEPERSON SETUP FORM --
            if (!Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opUpdateSalespersonProfile))
                info.ShowSPSetupForm = false;

            DistributorWebsite.MVC.WebUI.Helpers.CookieHelper.SetPopupTracker(this.CurrentUser.EntityCode);

            ViewBag.DEFAULTCHARTTAB = defaultChartTab;
            ViewBag.REFRESHALERTS = true;

            return View(info);
        }
        #endregion

        #region "PROCEDURE: SwitchEntity"
        /// <summary>
        /// Switch to a different entity
        /// </summary>
        /// <returns></returns>
        public async Task<ActionResult> SwitchEntity(string entitycode = null)
        {
            Helpers.Structures.TokenRefreshResponse result = null;

            try
            {
                //-- MAKE SURE THE ENTITY CODE WAS SUPPLIED --
                if (string.IsNullOrWhiteSpace(entitycode))
                {
                    return new RedirectToRouteResult(new RouteValueDictionary
                                                 {
                                                    { "action", "Forbidden" },
                                                    { "controller", "Error" }
                                                 });
                }

                //-- MAKE SURE THE ENTITY CODE IS VALID --
                var entities = (Dictionary<String, String>)ViewBag.UserEntities;
                if (entities == null || !entities.ContainsKey(entitycode))
                {
                    return new RedirectToRouteResult(new RouteValueDictionary
                                                 {
                                                    { "action", "Forbidden" },
                                                    { "controller", "Error" }
                                                 });
                }

                //-- TRY TO REFRESH THE TOKEN --
                result = await Helpers.EndpointAndTokenHelper.CheckAndPossiblyRefreshToken(eAccessTokenType.Undefined,
                                                                                           System.Web.HttpContext.Current.User.Identity as ClaimsIdentity,
                                                                                           this.User.Identity.Name,
                                                                                           this.CurrentFullName,
                                                                                           entitycode,
                                                                                           Helpers.CookieHelper.CurrentUserBeingImpersonated,
                                                                                           forceRefresh: true);
                if (result.Status == eTokenRefreshResponse.FailedToRefreshAccessToken)
                {
                    //-- LINK MIGHT BE DISABLED ON THE SERVER OR THERE WAS A PROBLEM CONNECTING TO THE IDENTITY SERVER --
                    return new RedirectToRouteResult(new RouteValueDictionary
                                                 {
                                                    { "action", "Forbidden" },
                                                    { "controller", "Error" }
                                                 });
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                result = null;
            }

            if (result == null || result.Status != eTokenRefreshResponse.TokenWasRefreshed)
            {
                throw new System.Exception("Failed to refresh token for entity code " + entitycode);
            }

            //-- CLEAR THE COOKIES --
            Helpers.CookieHelper.Total0to30Accounts = 0;
            Helpers.CookieHelper.TotalRepurchaseLetterAccounts = 0;

            //-- REDIRECT TO THE INDEX PAGE --
            return new RedirectToRouteResult(new RouteValueDictionary
            {
                { "action", "Index" },
                { "controller" , "Home" }
            });
        }
        #endregion

        #region "CONTROLLER: Settings"
        /// <summary>
        /// Allow the current user to change his/her settings
        /// </summary>
        public ActionResult Settings()
        {
            return View();
        }
        #endregion

        #region "CONTROLLER: TermsOfUse"
        /// <summary>
        /// Terms Of Use
        /// </summary>
        public ActionResult TermsOfUse()
        {
            return View();
        }
        #endregion

        #region "CONTROLLER: PrivacyPolicy"
        /// <summary>
        /// Privacy Policy
        /// </summary>
        public ActionResult PrivacyPolicy()
        {
            ViewBag.Client = this.CurrentUser.DefaultClient;
            return View();
        }
        #endregion

        #region "PROCEDURE: Signout"
        /// <summary>
        /// Log the current user out
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        public ActionResult Signout()
        {
            Request.GetOwinContext().Authentication.SignOut(new AuthenticationProperties()
            {
                RedirectUri = Helpers.Application.Instance.IdentityServer.RedirectUri
            });
            Session.Abandon();
            Helpers.CookieHelper.AccessToken = "";
            Helpers.CookieHelper.CurrentEntity = "";
            Helpers.CookieHelper.CurrentEntityName = "";
            Helpers.CookieHelper.CurrentSalespersonCode = "";
            Helpers.CookieHelper.DefaultClient = "";
            Helpers.CookieHelper.UserEntities = null;
            Helpers.CookieHelper.ClearPopupTracker();

            return Redirect("~/");
        }
        #endregion   
    }
}
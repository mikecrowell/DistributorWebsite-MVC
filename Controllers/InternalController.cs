using DistributorWebsite.MVC.WebUI.Helpers.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using static IdentityModel.Client.TokenClientExtensions;

namespace DistributorWebsite.MVC.WebUI.Controllers
{
    [HCAuthorize(skipRedirection: true)]
    public class InternalController : BaseController
    {
        #region "CONSTRUCTOR"
        /// <summary>
        /// Make sure the 
        /// </summary>
        public InternalController()
        {
        }
        #endregion

        #region "CONTROLLER: SelectEntity"
        /// <summary>
        /// Allow the user to select an entity
        /// </summary>
        [HttpGet]
        public async Task<ActionResult> SelectEntity()
        {
            Models.InputModels.InternalLoginModel model;

            //-- CHECK AND REFRESH THE CORE ACCESS TOKEN --
            if (!await CheckAndRefreshCoreAccessToken())
            {
                return new RedirectToRouteResult(new RouteValueDictionary
                                                 {
                                                    { "action", "IdentityServerUnavailable" },
                                                    { "controller", "Error" }
                                                 });
            }

            //-- CREATE A GENERIC MODEL --
            model = new Models.InputModels.InternalLoginModel();
            model.Username = User.Identity.Name;

            //-- DON'T BUILD OR SHOW THE MENU ON THE ENTITY SELECTION PAGE --
            ViewBag.USEEMPTYCONTAINER = true;
            ViewBag.CurrentBrand = "";
            ViewBag.IMPERSONATEUSERNOTE = String.Format(DistributorWebsite.MVC.LocalResources.HCResources.ImpersonateUserNote, this.User.Identity.Name);

            //-- SAVE THE RECENTLY USED USER LIST --
            var userList = Helpers.CookieHelper.RecentlyUsedUsers;
            if (userList != null && userList.Count > 0)
            {
                ViewBag.RECENTUSERLIST = userList;
                ViewBag.HASRECENTUSERS = true;
            }
            else
                ViewBag.HASRECENTUSERS = false;

            //-- CLEAR THE POPUP TRACKER --
            Helpers.CookieHelper.ClearPopupTracker();

            return View(model);
        }
        #endregion

        #region "PROCEDURE: POST: SelectEntity"
        /// <summary>
        /// Attempt to log the internal user into the selected entity
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ActionResult> SelectEntity(Models.InputModels.InternalLoginModel model)
        {
            String detailMessage = "";
            String entityName = "";
            String brand = "HC";

            TokenRefreshResponse tokenResponse;

            if (ModelState.IsValid)
            {
                try
                {
                    tokenResponse = await LoginInternalUser(model.EntityCode.ToUpper(), model.UserToImpersonate);

                    //-- MAKE SURE THE RESPONSE WAS VALID --
                    if (tokenResponse == null || tokenResponse.Status != eTokenRefreshResponse.TokenWasRefreshed)
                    {
                        switch (tokenResponse.Status)
                        {
                            case eTokenRefreshResponse.CustomMessage:
                                ModelState.AddModelError("", tokenResponse.ErrorMessage);
                                break;

                            default:
                                throw new System.Exception(tokenResponse.ErrorMessage);
                        }
                    }
                    else
                    {
                        //-- SET THE CURRENT ENTITY VALUES --
                        Helpers.CookieHelper.CurrentEntity = model.EntityCode.ToUpper();

                        //-- PARSE THE ACCESS TOKEN --
                        var token = Helpers.Common.ParseEntityAccessToken(tokenResponse.NewAccessToken);
                        if (token != null)
                        {
                            entityName = token.EntityName;
                            brand = token.Brand;
                        }

                        Helpers.CookieHelper.CurrentEntityBrand = brand;
                        Helpers.CookieHelper.CurrentEntityName = entityName;
                        Helpers.CookieHelper.CurrentUserBeingImpersonated = model.UserToImpersonate ?? "";
                        Helpers.CookieHelper.CurrentSalespersonCode = token.SalespersonCode;
                        Helpers.CookieHelper.DefaultClient = token.DefaultClient;

                        //-- ADD THE USER TO THE RECENTLY USED USER COOKIE --
                        Helpers.CookieHelper.AddRecentUser(model.EntityCode.ToUpper(), entityName, brand, model.UserToImpersonate);

                        //-- REDIRECT THE USER TO THE HOME PAGE --
                        return new RedirectToRouteResult(new RouteValueDictionary
                                                         {
                                                           { "action", "Index" },
                                                           { "controller", "Home" }
                                                         });
                    }
                }
                catch (Exception ex)
                {
                    //-- DISPLAY THE DETAIL MESSAGE TO THE USER --
                    if (Helpers.Application.Instance.ShowErrorDetails)
                        detailMessage = ex.Message;

                    //-- ADD THE ERROR MESSAGE --
                    ModelState.AddModelError("",
                                                String.Format(LocalResources.HCResources.ErrorFailedToSelectEntityInternal,
                                                model.EntityCode.Length == 5 ? LocalResources.HCResources.Distributor : LocalResources.HCResources.Salesperson, model.EntityCode) + (String.IsNullOrWhiteSpace(detailMessage) ? "" : " (" + detailMessage + ")"));
                }
             }

            //-- DON'T BUILD OR SHOW THE MENU ON THE ENTITY SELECTION PAGE --
            ViewBag.USEEMPTYCONTAINER = true;
            ViewBag.CurrentBrand = "";
            ViewBag.IMPERSONATEUSERNOTE = String.Format(DistributorWebsite.MVC.LocalResources.HCResources.ImpersonateUserNote, this.User.Identity.Name);

            //-- SAVE THE RECENTLY USED USER LIST --
            var userList = Helpers.CookieHelper.RecentlyUsedUsers;
            if (userList != null && userList.Count > 0)
            {
                ViewBag.RECENTUSERLIST = userList;
                ViewBag.HASRECENTUSERS = true;
            }
            else
                ViewBag.HASRECENTUSERS = false;

            return View(model);
        }
        #endregion

        #region "FUNCTION: LoginInternalUser"
        /// <summary>
        /// Attempt to log the internal user into a specific entity
        /// </summary>
        /// <returns></returns>
        private async Task<TokenRefreshResponse> LoginInternalUser(string entityCode, string userToImpersonate = null)
        {
            TokenRefreshResponse tokenResponse;
            Models.UserStatusDTO userStatus;

            try
            {
                //-- CHECK THE CURRENT USER STATUS WHEN IMPERSONATING A USER --
                if (!String.IsNullOrWhiteSpace(userToImpersonate))
                {
                    try
                    {
                        using (Helpers.WebAPI oAPI = new Helpers.WebAPI(type: eAccessTokenType.InternalCore))
                        {
                            userStatus = await oAPI.GetImpersonatedUserStatus(userToImpersonate);
                        }

                        if (userStatus != null && userStatus.Enabled != "Y")
                        {
                            switch (userStatus.Enabled)
                            {
                                case "V":
                                    return(new TokenRefreshResponse() { ErrorMessage = LocalResources.HCResources.UserDisabledUntilVerification, Status = eTokenRefreshResponse.CustomMessage });

                                case "N":
                                    if (!String.IsNullOrWhiteSpace(userStatus.DisableReason))
                                        return (new TokenRefreshResponse() { ErrorMessage = userStatus.DisableReason, Status = eTokenRefreshResponse.CustomMessage });
                                    else
                                        return (new TokenRefreshResponse() { ErrorMessage = LocalResources.HCResources.UserDisabled, Status = eTokenRefreshResponse.CustomMessage });
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Trace.TraceError(ex.ToString());
                    }
                }

                //-- REFRESH THE CURRENT ACCESS TOKEN --
                tokenResponse = await Helpers.EndpointAndTokenHelper.CheckAndPossiblyRefreshToken(eAccessTokenType.InternalUser,
                                                                                  username: this.User.Identity.Name,
                                                                                  fullName: this.CurrentFullName,
                                                                                  entityCode: entityCode,
                                                                                  impersonateUser: userToImpersonate);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
                tokenResponse = new TokenRefreshResponse();
                tokenResponse.Status = eTokenRefreshResponse.FailedToRefreshAccessToken;
                tokenResponse.Error = "System.Exception";
                tokenResponse.ErrorMessage = ex.Message;
            }

            return (tokenResponse);
        }
        #endregion

        #region "HELPER FUNCTIONS"

        #region "FUNCTION: CheckAndRefreshCoreAccessToken"
        /// <summary>
        /// Check and possibly refresh the core access token(s)
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CheckAndRefreshCoreAccessToken()
        {
            String funcName = "CheckAndRefreshCoreAccessToken";
            TokenRefreshResponse response;
            bool retVal = true;

            try
            {
                Logger.LogVerbose(funcName + ": starting");

                //-- CLEAR THE CURRENT ENTITY COOKIES --
                Helpers.CookieHelper.CurrentEntity = "";
                Helpers.CookieHelper.CurrentEntityName = "";
                Helpers.CookieHelper.CurrentEntityBrand = "";
                Helpers.CookieHelper.CurrentUserBeingImpersonated = "";
                Helpers.CookieHelper.CurrentSalespersonCode = "";
                Helpers.CookieHelper.DefaultClient = "";

                //-- CHECK THE CORE ACCESS TOKEN - IF EXPIRED GET A NEW ONE AND UPDATE THE CURRENT USER'S ACCESS TOKEN COOKIES --
                response = await Helpers.EndpointAndTokenHelper.CheckAndPossiblyRefreshToken(eAccessTokenType.InternalCore);
                if (response.Status == eTokenRefreshResponse.FailedToRefreshAccessToken)
                    return false;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                retVal = false;
            }

            return (retVal); 
        }
        #endregion

        #endregion
    }
}
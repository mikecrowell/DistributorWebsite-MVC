using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using System.Web;
using DistributorWebsite.MVC.WebUI.Helpers.Structures;
using Serilog;
using IdentityModel.Client;
using IdentityModel;
using System.Collections.Generic;

namespace DistributorWebsite.MVC.WebUI.Helpers
{
    public static class EndpointAndTokenHelper
    {
        #region "CONSTANTS"

        /// <summary>
        /// Identity server scope used for internal employee logins
        /// </summary>
        private const string InternalLoginScope = "distwebhcemp";

        #endregion

        #region "FUNCTION: DecodeAndWrite"
        /// <summary>
        /// Attempt to decode and write out the JWT token details
        /// </summary>
        /// <param name="token"></param>
        /// <param name="tokenType"></param>
        public static void DecodeAndWrite(String token, String tokenType = "TOKEN")
        {
            try
            {
                var parts = token.Split('.');
                var part = Encoding.UTF8.GetString(Base64Url.Decode(parts[1]));

                var jwt = JObject.Parse(part);

                System.Diagnostics.Trace.TraceInformation("");
                System.Diagnostics.Trace.TraceInformation("=========================================");
                System.Diagnostics.Trace.TraceInformation(tokenType + " RECEIVED FROM IDENTITY SERVER");
                System.Diagnostics.Trace.TraceInformation(jwt.ToString());
                System.Diagnostics.Trace.TraceInformation("=========================================");
                System.Diagnostics.Trace.TraceInformation("");
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceInformation("");
                System.Diagnostics.Trace.TraceInformation("=========================================");
                System.Diagnostics.Trace.TraceInformation("FAILED TO DECODE " + tokenType + " TOKEN");
                System.Diagnostics.Trace.TraceInformation(ex.Message);
                System.Diagnostics.Trace.TraceInformation("=========================================");
                System.Diagnostics.Trace.TraceInformation("");
            }

        }
        #endregion

        #region "FUNCTION: CheckAndPossiblyRefreshToken"
        /// <summary>
        /// Check to see if the current access token is expired - get a new one if it has using the refresh token
        /// </summary>
        /// <param name="id"></param>
        public static async Task<TokenRefreshResponse> CheckAndPossiblyRefreshToken(eAccessTokenType tokenType = eAccessTokenType.Undefined, ClaimsIdentity id = null, String username = null, String fullName = null, String entityCode = null, String impersonateUser = null, Boolean forceRefresh = false)
        {
            TokenRefreshResponse response = new TokenRefreshResponse();

            try
            {
                //-- GET THE TOKEN TYPE FROM THE COOKIE IF IT WASN'T SUPPLIED --
                if (tokenType == eAccessTokenType.Undefined) tokenType = Helpers.CookieHelper.AccessTokenType;

                switch (tokenType)
                {
                    case eAccessTokenType.InternalCore:

                        //-- PERFORM THE CORE REFRESH IF NECESSARY --
                        Logger.LogDebug("CheckAndPossiblyRefreshToken: CHECKING CORE TOKEN...");
                        return await CheckAndPossiblyRefreshInternalToken(forceRefresh: forceRefresh);

                    case eAccessTokenType.InternalUser:

                        //-- PERFORM THE INTERNAL USER REFRESH IF NECESSARY --
                        Logger.LogDebug("CheckAndPossiblyRefreshToken: CHECKING CORE ENTITY TOKEN...");
                        return await CheckAndPossiblyRefreshInternalUserToken(username, fullName, entityCode, impersonateUser, forceRefresh);

                    case eAccessTokenType.ExternalUser:

                        //-- PERFORM THE EXTERNAL USER REFRESH IF NECESSARY --
                        Logger.LogDebug("CheckAndPossiblyRefreshToken: CHECKING EXTERNAL USER TOKEN...");
                        return await CheckAndPossibleRefreshExternalToken(entityCode, forceRefresh);

                    default:
                        throw new NotImplementedException(String.Format("ACCESS TOKEN TYPE {0} IS NOT SUPPORTED FOR REFRESHING", Helpers.CookieHelper.AccessTokenType));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                response.Status = eTokenRefreshResponse.FailedToRefreshAccessToken;
            }

            return response;
        }
        #endregion

        #region "PROCEDURE: UpdateExternalAccessTokenCookies"
        /// <summary>
        /// Update the user cookies bases on the new access token information
        /// </summary>
        /// <param name="accessToken"></param>
        public static void UpdateExternalAccessTokenCookies(String accessToken)
        {
            try
            {
                //-- PARSE THE ACCESS TOKEN --
                var token = Helpers.Common.ParseEntityAccessToken(accessToken);
                if (token == null)
                    throw new System.Exception("FAILED TO PARSE THE EXTERNAL ACCESS TOKEN");

                //-- SET THE CURRENT ENTITY VALUES --
                Helpers.CookieHelper.CurrentEntity = token.EntityCode;
                Helpers.CookieHelper.CurrentEntityName = token.EntityName;
                Helpers.CookieHelper.CurrentEntityBrand = token.Brand;
                Helpers.CookieHelper.CurrentSalespersonCode = token.SalespersonCode;
                Helpers.CookieHelper.DefaultClient = token.DefaultClient;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
                throw;
            }
        }
        #endregion

        #region "HELPER FUNCTIONS"

        #region "FUNCTION: CheckAndPossibleRefreshHybridToken"
        /// <summary>
        /// Check and possibly refresh an external identity server token
        /// </summary>
        /// <returns></returns>
        private static async Task<TokenRefreshResponse> CheckAndPossibleRefreshExternalToken(string entityCode, bool forceRefresh = false)
        {
            TokenRefreshResponse response = new TokenRefreshResponse();
            TokenResponse tokenEndpointResponse;
            Int32 retryCount = 0;
            Int32 maxRetries = 3;

            System.Diagnostics.Debug.WriteLine("*********************************");
            System.Diagnostics.Debug.WriteLine("CHECKING FOR EXPIRED ACCESS TOKEN");
            System.Diagnostics.Debug.WriteLine("*********************************");

            Helpers.CookieHelper.AccessTokenType = eAccessTokenType.ExternalUser;

            if ((forceRefresh) || (DateTime.Now.ToUniversalTime() >= (Helpers.CookieHelper.AccessTokenExpiration)))
            {
                //---------------------------------------------
                //-- ACCESS TOKEN IS EXPIRED - GET A NEW ONE --
                //---------------------------------------------
                System.Diagnostics.Debug.WriteLine("ACCESS TOKEN IS EXPIRED - RETRIEVING NEW ACCESS TOKEN FROM IDENTITY SERVER...");

                if (String.IsNullOrWhiteSpace(entityCode))
                    entityCode = Helpers.CookieHelper.CurrentEntity;

                var tokenEndpointClient = new TokenClient(Helpers.Application.Instance.IdentityServer.TokenEndpoint + "?ECODE=" + entityCode,
                                                     Helpers.Application.Instance.IdentityServer.ClientID,
                                                     Helpers.Application.Instance.IdentityServer.Secret);

                tokenEndpointResponse = await tokenEndpointClient.RequestRefreshTokenAsync(Helpers.CookieHelper.RefreshToken);

                while ((tokenEndpointResponse == null || tokenEndpointResponse.IsError) && (retryCount++ <= maxRetries))
                {
                    System.Diagnostics.Trace.TraceInformation("SLEEPING FOR 1 SECONDS BEFORE TRYING ACCESS TOKEN...");
                    System.Threading.Thread.Sleep(1000);
                    tokenEndpointResponse = await tokenEndpointClient.RequestRefreshTokenAsync(Helpers.CookieHelper.RefreshToken);
                }

                if (!tokenEndpointResponse.IsError)
                {
                    Helpers.CookieHelper.AccessToken = tokenEndpointResponse.AccessToken;
                    Helpers.CookieHelper.RefreshToken = tokenEndpointResponse.RefreshToken;
                    Helpers.CookieHelper.AccessTokenExpiration = DateTime.Now.AddSeconds(tokenEndpointResponse.ExpiresIn).ToLocalTime();

                    UpdateExternalAccessTokenCookies(tokenEndpointResponse.AccessToken);

                    response.Status = eTokenRefreshResponse.TokenWasRefreshed;
                    response.NewAccessToken = tokenEndpointResponse.AccessToken;

                    System.Diagnostics.Debug.WriteLine("A NEW ACCESS TOKEN WAS RECEIVED FROM THE IDENTITY SERVER");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("FAILED TO RETRIEVE NEW ACCESS TOKEN FROM SERVER");

                    response.Status = eTokenRefreshResponse.FailedToRefreshAccessToken;
                }
            }

            else
            {
                //-------------------------------
                //-- THE ACCESS TOKEN IS VALID --
                //-------------------------------
                System.Diagnostics.Debug.WriteLine("THE ACCESS TOKEN IS VALID");
                response.Status = eTokenRefreshResponse.TokenIsValid;
            }

            return (response);
        }
        #endregion

        #region "FUNCTION: CheckAndPossiblyRefreshInternalToken"
        /// <summary>
        /// Check to see if the global internal access token is expired - get a new one if it is
        /// </summary>
        /// <returns></returns>
        private static async Task<TokenRefreshResponse> CheckAndPossiblyRefreshInternalToken(bool setCookies = true, bool forceRefresh = false)
        {
            string funcName = "CheckAndPossiblyRefreshInternalToken";
            TokenRefreshResponse response = new TokenRefreshResponse();

            try
            {
                Logger.LogVerbose(funcName + ": starting");
                Logger.LogVerbose(funcName + ": attempting to retrieve new access token from identity server");

                if ((forceRefresh) || (Helpers.Application.Instance.InternalAccessTokenExpirationDate <= DateTime.Now.AddSeconds(-10)))
                {
                    Logger.LogVerbose(funcName + ": updating missing or expired core access token");

                    var tokenEndpointClient = new TokenClient(Helpers.Application.Instance.IdentityServerInternal.TokenEndpoint,
                                                                        Helpers.Application.Instance.IdentityServerInternal.ClientID,
                                                                        Helpers.Application.Instance.IdentityServerInternal.Secret);

                    var tokenEndpointResponse = await tokenEndpointClient.RequestClientCredentialsAsync(InternalLoginScope);

                    if (!tokenEndpointResponse.IsError)
                    {
                        response.NewAccessToken = tokenEndpointResponse.AccessToken;
                        response.ExpirationDate = DateTime.Now.AddSeconds(tokenEndpointResponse.ExpiresIn);
                        response.Status = eTokenRefreshResponse.TokenWasRefreshed;
                        Logger.LogVerbose(funcName + ": new access token retrieved from identity server");

                        //-- UPDATE THE CORE APPLICATION OBJECT WITH THE NEW ACCESS TOKEN --
                        Helpers.Application.Instance.UpdateCoreAccessToken(response.NewAccessToken, response.ExpirationDate.Value);
                    }
                    else
                    {
                        Logger.LogError(funcName + ": failed to retrieve access token from identity server");
                        Logger.LogError(funcName + ": " + tokenEndpointResponse.HttpErrorReason);
                        response.Status = eTokenRefreshResponse.FailedToRefreshAccessToken;
                    }
                }
                else
                {
                    Logger.LogVerbose(funcName + ": access token is valid");
                    response.Status = eTokenRefreshResponse.TokenIsValid;
                }

                //-- SET THE USER COOKIES IF APPLICABLE --
                if ((setCookies) && (System.Web.HttpContext.Current != null && Helpers.CookieHelper.AccessToken != Helpers.Application.Instance.InternalAccessToken))
                {
                    Logger.LogVerbose(funcName + ": setting user cookies for internal access token");

                    Helpers.CookieHelper.AccessTokenType = eAccessTokenType.InternalCore;
                    Helpers.CookieHelper.AccessToken = Helpers.Application.Instance.InternalAccessToken;
                    Helpers.CookieHelper.AccessTokenExpiration = Helpers.Application.Instance.InternalAccessTokenExpirationDate;
                    Helpers.CookieHelper.RefreshToken = "";
                }

                Logger.LogVerbose(funcName + ": finished processing");
            }
            catch (Exception ex)
            {
                Logger.LogVerbose(funcName + ": failed - " + ex.Message);
                Logger.LogError(ex.ToString());
                throw;
            }

            return response;
        }
        #endregion

        #region "FUNCTION: CheckAndPossiblyRefreshInternalUserToken"
        /// <summary>
        /// Check to see if the global internal access token is expired - get a new one if it is
        /// </summary>
        /// <returns></returns>
        private static async Task<TokenRefreshResponse> CheckAndPossiblyRefreshInternalUserToken(String username, String fullName, String entityCode, String userToImpersonate = null, bool forceRefresh = false)
        {
            string funcName = "CheckAndPossiblyRefreshInternalEntityToken";
            IdentityModel.Client.TokenResponse response;
            IdentityModel.Client.TokenClient client;
            TokenRefreshResponse retVal = new TokenRefreshResponse();

            try
            {
                Logger.LogVerbose(funcName + ": starting");
                Logger.LogVerbose(funcName + ": attempting to retrieve new access token from identity server");

                if ((forceRefresh) || (String.IsNullOrWhiteSpace(Helpers.CookieHelper.CurrentEntity) || Helpers.CookieHelper.AccessTokenExpiration <= DateTime.Now.AddSeconds(-5) || Helpers.CookieHelper.CurrentEntity != entityCode || Helpers.CookieHelper.CurrentUserBeingImpersonated != userToImpersonate))
                {
                    //-- NEED TO CHECK THE INTERNAL ACCESS TOKEN AND REFRESH IT IF IT HAS EXPIRED OR IS ABOUT TO EXPIRE --
                    await CheckAndPossiblyRefreshInternalToken(false);

                    //-- SET THE LIST OF REQUIRED ACR VALUES --
                    var optional = new
                    {
                        acr_values = String.Format("tenant:DistributorWebsite WUU:{0} WUN:{1} WAT:{2} WIMP:{3} WENT:{4}",
                                                   System.Web.HttpUtility.UrlEncode(username),
                                                   System.Web.HttpUtility.UrlEncode(fullName),
                                                   Helpers.Application.Instance.InternalAccessToken,
                                                   System.Web.HttpUtility.UrlEncode(userToImpersonate ?? ""),
                                                   System.Web.HttpUtility.UrlEncode(entityCode ?? ""))
                    };

                    //-- INITIALIZE THE OAUTH2 CLIENT --
                    client = new IdentityModel.Client.TokenClient(Helpers.Application.Instance.IdentityServerInternal.Authority + "/connect/token",
                                                                                Helpers.Application.Instance.IdentityServerInternalEnt.ClientID,
                                                                                Helpers.Application.Instance.IdentityServerInternalEnt.Secret);

                    response = await client.RequestResourceOwnerPasswordAsync(username, Helpers.Application.Instance.IdentityServerInternalEnt.ROP, Helpers.Application.Instance.IdentityScopes, optional);
                    if (response != null && !response.IsError && !String.IsNullOrWhiteSpace(response.AccessToken))
                    {
                        Helpers.CookieHelper.AccessTokenType = eAccessTokenType.InternalUser;
                        Helpers.CookieHelper.AccessToken = response.AccessToken;
                        Helpers.CookieHelper.RefreshToken = "";
                        Helpers.CookieHelper.AccessTokenExpiration = DateTime.Now.AddSeconds(response.ExpiresIn).ToLocalTime();

                        retVal.NewAccessToken = response.AccessToken;
                        retVal.ExpirationDate = DateTime.Now.AddSeconds(response.ExpiresIn);
                        retVal.Status = eTokenRefreshResponse.TokenWasRefreshed;
                        Logger.LogVerbose(funcName + ": new access token retrieved from identity server");
                    }
                    else
                    {
                        Logger.LogError(funcName + ": failed to retrieve access token from identity server");
                        Logger.LogError(funcName + ": " + response.Raw);
                        retVal.Status = eTokenRefreshResponse.FailedToRefreshAccessToken;
                        retVal.Error = response.Error;
                        retVal.ErrorMessage = response.ErrorDescription;
                    }
                }
                else
                    retVal.Status = eTokenRefreshResponse.TokenIsValid;

                Logger.LogVerbose(funcName + ": finished processing");
            }
            catch (Exception ex)
            {
                Logger.LogVerbose(funcName + ": failed - " + ex.Message);
                Logger.LogError(ex.ToString());
                retVal.Error = "System.Exception";
                retVal.ErrorMessage = ex.Message;
                throw;
            }

            return retVal;
        }
        #endregion

        #endregion
    }
}
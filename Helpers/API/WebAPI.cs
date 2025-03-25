using DistributorWebsite.MVC.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Script.Serialization;
using System.Diagnostics;
using DistributorWebsite.MVC.WebUI.Helpers.Models;
using DistributorWebsite.MVC.WebUI.Models.JuniorDistributor;

namespace DistributorWebsite.MVC.WebUI.Helpers
{
    public class WebAPI : IDisposable
    {
        #region "ENUM: eCreditReportType"
        /// <summary>
        /// Define the various credit report types
        /// </summary>
        public enum eCreditReportType
        {
            Customer,
            Cosignor
        }
        #endregion

        #region "ENUM: eSubmissionMethod"
        /// <summary>
        /// Get/set the submission method
        /// </summary>
        public enum eSubmissionMethod
        {
            POST,
            GET
        }
        #endregion

        #region "PROPERTY: ExistingToken"
        /// <summary>
        /// Get/set the existing access token
        /// </summary>
        private Helpers.Security.AccessToken ExistingToken { get; set; }
        #endregion

        #region "PROPERTY: BearerToken"
        /// <summary>
        /// Get/set the bearer token to use for this request
        /// </summary>
        private string BearerToken { get; set; }
        #endregion

        #region "PROPERTY: CurrentUsername"
        /// <summary>
        /// Get the current username from the existing access token
        /// </summary>
        private string CurrentUsername
        {
            get
            {
                if (this.ExistingToken == null)
                    return null;
                else
                    return this.ExistingToken.UserName;
            }
        }
        #endregion

        #region "PROPERTY: CurrentFullName"
        /// <summary>
        /// Get the current full name from the existing access token
        /// </summary>
        private string CurrentFullName
        {
            get
            {
                if (this.ExistingToken == null)
                    return null;
                else
                    return this.ExistingToken.Name;
            }
        }
        #endregion

        #region "PROPERTY: CurrentEntityCode"
        /// <summary>
        /// Get the current entity code from the existing access token
        /// </summary>
        private string CurrentEntityCode
        {
            get
            {
                if (this.ExistingToken == null)
                    return null;
                else
                    return this.ExistingToken.EntityCode;
            }
        }
        #endregion

        #region "PROPERTY: CurrentImpersonatedUser"
        /// <summary>
        /// Get the current impersonated user
        /// </summary>
        private string CurrentImpersonatedUser
        {
            get
            {
                return Helpers.CookieHelper.CurrentUserBeingImpersonated;
            }
        }
        #endregion

        #region "PROPERTY: APIClient"
        /// <summary>
        /// Get/set the link to the API Client
        /// </summary>
        private HttpClient APIClient { get; set; }
        #endregion

        #region "CTOR"
        /// <summary>
        /// Initialize the Web API Helper class
        /// </summary>
        public WebAPI(String requestedVersion = null, String language = "en", eAccessTokenType type = eAccessTokenType.ExternalUser, Helpers.Security.AccessToken existingToken = null)
        {
            if (!DistributorWebsite.MVC.WebUI.Helpers.Application.Instance.IsRunningLocal)
                System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;

            this.APIClient = new HttpClient();
            this.ExistingToken = existingToken;

            switch (type)
            {
                case eAccessTokenType.InternalCore:

                    //-- SEND THE INTERNAL CORE ACCESS TOKEN --
                    this.SetBearerToken(Helpers.Application.Instance.InternalAccessToken);
                    break;

                default:

                    //-- SEND THE ACCESS TOKEN TO THE API --
                    var token = Helpers.CookieHelper.AccessToken;
                    if (!String.IsNullOrWhiteSpace(token))
                    {
                        this.SetBearerToken(token);
                    }
                    break;
            }

            //-- INITIALIZE THE API --
            this.APIClient.BaseAddress = new Uri(DistributorWebsite.MVC.WebUI.Helpers.Application.Instance.APIS.DistributorWebAPI);
            this.APIClient.DefaultRequestHeaders.Accept.Clear();
            this.APIClient.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
            this.APIClient.DefaultRequestHeaders.AcceptLanguage.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue(language));

        }
        #endregion

        #region "PROCEDURE: SetBearerToken"
        /// <summary>
        /// Set the bearer token to be used for the current request
        /// </summary>
        private void SetBearerToken(String token)
        {
            //-- UPDATE THE BEARER TOKEN PROPERTY --
            this.BearerToken = token;

            //-- UPDATE THE API CLIENT --
            this.APIClient.SetBearerToken(token);
        }
        #endregion

        #region "FUNCTION: GetTotalOrdersPendingApproval"
        /// <summary>
        /// Get the total number of orders pending approval
        /// </summary>
        /// <returns></returns>
        public async Task<Int32> GetTotaOrdersPendingApproval()
        {
            String url;
            url = String.Format("MobileOrders/Default.TotalPendingApproval");
            return (await ParseSingleResult<ODataSingleResult<Int32>>(url)).value;
        }
        #endregion

        #region "FUNCTION: GetAlerts"
        /// <summary>
        /// Get any current account alerts for the current user
        /// </summary>
        /// <returns></returns>
        public async Task<String> GetAlerts()
        {
            String url;
            url = String.Format("Users/Default.GetAlerts");
            return await ParseSingleResult<String>(url);
        }
        #endregion

        #region "FUNCTION: GetDownloadCenterCategoriesAsync"
        /// <summary>
        /// Get the list of accessible download center categories
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<DocumentCategoryDTO>> GetDownloadCenterCategoriesAsync()
        {
            String url;
            url = $"Documents/Default.DownloadCenterCategories";
            return await ParseResultList<DocumentCategoryDTO>(url);
        }
        #endregion

        #region "FUNCTION: GetTrainingCategoriesAsync"
        /// <summary>
        /// Get the list of accessible download center categories
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<DocumentCategoryDTO>> GetTrainingCategoriesAsync()
        {
            String url;
            url = $"Documents/Default.TrainingCategories";
            return await ParseResultList<DocumentCategoryDTO>(url);
        }
        #endregion

        #region "FUNCTION: GetMagazineCategoriesAsync"
        /// <summary>
        /// Get the list of available magazine categories
        /// </summary>
        /// <returns></returns>
        public async Task<MagazineLeadCategoriesDTO> GetMagazineCategoriesAsync()
        {
            String url;
            url = $"MagazineCustomers/Default.Categories";
            return await ParseSingleResult<MagazineLeadCategoriesDTO>(url);
        }
        #endregion

        #region "FUNCTION: GetSecurityItems"
        /// <summary>
        /// Get the list of security items
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<SecurityItemDTO>> GetSecurityItems()
        {
            return await ParseResultList<SecurityItemDTO>("SecurityItems");
        }
        #endregion

        #region "FUNCTION: VerifyEmail"
        /// <summary>
        /// Verify the email
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<EmailVerificationResultDTO> VerifyEmail(String token)
        {
            EmailVerificationResultDTO result = new EmailVerificationResultDTO();
            HttpResponseMessage response = null;
            String url;
            String json;
            Int32 retryCount = 0;
            dynamic jObject;

            try
            {
                //-- INITIALIZE THE RESULT --
                result.PanelCSS = "panel-danger";
                result.Header = LocalResources.HCResources.InternalServerError;
                result.Message = LocalResources.HCResources.InternalServerErrorDescription;
                result.StatusCode = System.Net.HttpStatusCode.InternalServerError;

                //-- SET THE URL --
                url = this.APIClient.BaseAddress + $"VerifyEmail(token='{token}')";

                while (retryCount <= 1)
                {
                    //-- SEND THE INTERNAL CORE ACCESS TOKEN --
                    this.SetBearerToken(Helpers.Application.Instance.InternalAccessToken);

                    //-- CALL THE API --
                    response = await this.APIClient.PostAsync(url, new StringContent(""));
                    result.StatusCode = response.StatusCode;

                    if (response.IsSuccessStatusCode)
                    {
                        json = await response.Content.ReadAsStringAsync();

                        //-- EMAIL WAS VERIFIED SUCCESSFULLY --
                        jObject = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                        result.StatusCode = System.Net.HttpStatusCode.OK;
                        result.PanelCSS = "panel-success";
                        result.Header = jObject.ResponseHeader;
                        result.Message = jObject.ResponseHTML;
                        result.ShowDocusignMessage = jObject.DocusignSent;
                        result.DocusignMessage = jObject.DocusignMessage;
                        return (result);
                    }
                    else if ((response.StatusCode == System.Net.HttpStatusCode.Unauthorized) && (retryCount < 1))
                    {
                        Trace.TraceInformation("Attempting to refresh expired access token");
                        retryCount += 1;

                        try
                        {
                            var tokenResponse = await Helpers.EndpointAndTokenHelper.CheckAndPossiblyRefreshToken(eAccessTokenType.InternalCore);

                            if ((tokenResponse == null) || ((tokenResponse.Status != Structures.eTokenRefreshResponse.TokenWasRefreshed) && (tokenResponse.Status != Structures.eTokenRefreshResponse.TokenIsValid)))
                            {
                                System.Diagnostics.Debug.WriteLine("Failed to refresh the access token: " + tokenResponse.Status.ToString());

                                //-- UNAUTHORIZED TO CALL THE API --
                                result.PanelCSS = "panel-danger";
                                result.Header = LocalResources.HCResources.Unauthorized;
                                result.Message = LocalResources.HCResources.UnauthorizedDescription;
                                return (result);
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Failed to refresh the access token: " + ex.ToString());
                            throw new HttpException((Int32)response.StatusCode, response.ReasonPhrase);
                        }
                    }
                    else if (response.StatusCode == System.Net.HttpStatusCode.Unauthorized)
                    {
                        //-- UNAUTHORIZED TO CALL THE API --
                        result.PanelCSS = "panel-danger";
                        result.Header = LocalResources.HCResources.Unauthorized;
                        result.Message = LocalResources.HCResources.UnauthorizedDescription;
                        return (result);
                    }
                    else
                    {
                        //-- TRY TO PARSE THE ERROR RESPONSE --
                        json = await response.Content.ReadAsStringAsync();
                        jObject = Newtonsoft.Json.JsonConvert.DeserializeObject(json);
                        result.PanelCSS = "panel-danger";
                        result.Header = jObject.UserError.Title;
                        result.Message = jObject.UserError.Message;
                        return (result);
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                result.PanelCSS = "panel-danger";
                result.Header = LocalResources.HCResources.InternalServerError;
                result.Message = LocalResources.HCResources.InternalServerErrorDescription;
            }

            return (result);
        }
        #endregion

        #region "FUNCTION: UpdateAndGetPaymentStatus"
        /// <summary>
        /// Refresh the payment status from Worldpay and retrieve the current status
        /// </summary>
        /// <param name="paymentID"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public async Task<PaymentStatusDTO> UpdateAndGetPaymentStatus(Int32 paymentID)
        {
            try
            {
                return await ParseSingleResult<PaymentStatusDTO>(String.Format("RefreshPaymentStatus(id={0})", paymentID), eSubmissionMethod.POST);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region "FUNCTION: UpdateSalesForceDefaultEntity"
        /// <summary>
        /// Update the default entity for the sales force login
        /// </summary>
        /// <param name="entityID"></param>
        /// <returns></returns>
        public async Task UpdateSalesForceDefaultEntity(Int32 entityID)
        {
            try
            {
                await Post($"SalesForce.UpdateLoginEntity(id={entityID})");
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region "FUNCTION: InitializeSalesforceEntity"
        /// <summary>
        /// Update the default entity for the sales force login
        /// </summary>
        /// <param name="entityID"></param>
        /// <returns></returns>
        public async Task InitializeSalesForceEntity()
        {
            try
            {
                await Post($"SalesForce.InitializeLoginEntity");
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region "FUNCTION: GetDistributorLandingPageInfo"
        /// <summary>
        /// Get the landing page details
        /// </summary>
        /// <returns></returns>
        public async Task<LandingPageInfoDTO> GetDistributorLandingPageInfo(String distributorNo)
        {
            String url;
            url = String.Format("Distributors/Default.LandingPage(InitialVisit={0})", (!Helpers.CookieHelper.HasDisplayedPopupsForEntity(distributorNo)).ToString().ToLower());
            return await ParseSingleResult<LandingPageInfoDTO>(url);
        }
        #endregion

        #region "FUNCTION: GetSalespersonLandingPageInfo"
        /// <summary>
        /// Get the landing page details
        /// </summary>
        /// <returns></returns>
        public async Task<LandingPageInfoDTO> GetSalespersonLandingPageInfo(String salespersonCode)
        {
            String url;
            url = String.Format("Salesperson/Default.LandingPage(InitialVisit={0})", (!Helpers.CookieHelper.HasDisplayedPopupsForEntity(salespersonCode)).ToString().ToLower());
            return await ParseSingleResult<LandingPageInfoDTO>(url);
        }
        #endregion

        #region "FUNCTION: GetUserInfo"
        /// <summary>
        /// Get the landing page details
        /// </summary>
        /// <returns></returns>
        public async Task<Helpers.Security.UserInfoClaim> GetUserInfo(String accessToken)
        {
            String url = "Users/Default.GetUserInfo";
            String json;
            ODataResponse<String> odataResponse;
            Helpers.Security.UserInfoClaim retVal = null;
            HttpResponseMessage response = null;

            try
            {
                //-- CONNECT TO THE WEB API AND GET THE RESPONSE --
                this.SetBearerToken(accessToken);
                response = this.APIClient.GetAsync(this.APIClient.BaseAddress + url).Result;

                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    odataResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<ODataResponse<String>>(json);

                    //-- DESERIALIZE THE CONTENT INTO AN OBJECT OF TYPE T --
                    retVal = Helpers.Security.UserInfoClaim.ParseBase64String(odataResponse.Value);
                }
                else
                {
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.Forbidden:
                            throw new HCForbiddenException($"Status Code: {response.StatusCode} - Reason: {response.ReasonPhrase}");

                        default:
                            throw new HttpException((Int32)response.StatusCode, response.ReasonPhrase);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw;
            }
            finally
            {
                if (response != null)
                {
                    try
                    {
                        response.Dispose();
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return retVal;
        }
        #endregion

        #region "FUNCTION: GetImpersonatedUserStatus"
        /// <summary>
        /// Get the impersonated user status
        /// </summary>
        /// <returns></returns>
        public async Task<UserStatusDTO> GetImpersonatedUserStatus(string userName)
        {
            String url = $"Users/Default.GetImpersonatedUserStatus(username='{userName}')";
            String json;
            UserStatusDTO status;
            HttpResponseMessage response = null;

            try
            {
                //-- CONNECT TO THE WEB API AND GET THE RESPONSE --
                response = this.APIClient.GetAsync(this.APIClient.BaseAddress + url).Result;

                if (response.IsSuccessStatusCode)
                {
                    json = await response.Content.ReadAsStringAsync();
                    status = Newtonsoft.Json.JsonConvert.DeserializeObject<UserStatusDTO>(json);
                }
                else
                {
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.Forbidden:
                            throw new HCForbiddenException($"Status Code: {response.StatusCode} - Reason: {response.ReasonPhrase}");

                        default:
                            throw new HttpException((Int32)response.StatusCode, response.ReasonPhrase);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw;
            }
            finally
            {
                if (response != null)
                {
                    try
                    {
                        response.Dispose();
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return status;
        }
        #endregion

        #region "FUNCTION: GetUSStates"
        /// <summary>
        /// Get the list of US States
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<StateDTO>> GetUSStates()
        {
            return await ParseResultList<StateDTO>("States/Default.USStates?$orderby=StateName&$pagesize=999");
        }


        #endregion

        #region "FUNCTION: GetUSStates"
        /// <summary>
        /// Get the list of US States
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<StateDTO>> GetStates(String countryCode)
        {
            return await ParseResultList<StateDTO>($"States/Default.States(countryCode='{countryCode}')?$orderby=StateName&$pagesize=999");
        }


        #endregion


        #region "FUNCTION: GetEsignClients"
        /// <summary>
        /// Get the list of US States
        /// </summary>
        /// <returns></returns>
        public async Task<eSignClientDTO> GetEsignClients()
        {
            
            String url = "MobileOrders/Default.ESignClients";
            String json;
            eSignClientDTO clients;
            HttpResponseMessage response = null;

            try
            {
                //-- CONNECT TO THE WEB API AND GET THE RESPONSE --
                response = this.APIClient.GetAsync(this.APIClient.BaseAddress + url).Result;
                json = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode)
                {                   
                    clients = Newtonsoft.Json.JsonConvert.DeserializeObject<eSignClientDTO>(json);
                }
                else
                {
                    switch (response.StatusCode)
                    {
                        case System.Net.HttpStatusCode.Forbidden:
                            throw new HCForbiddenException($"Status Code: {response.StatusCode} - Reason: {response.ReasonPhrase}");

                        default:
                            throw new HttpException((Int32)response.StatusCode, json);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw;
            }
            finally
            {
                if (response != null)
                {
                    try
                    {
                        response.Dispose();
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return clients;
        }
        #endregion

        #region "FUNCTION: GetUSStatesWithSalesTaxRates"
        /// <summary>
        /// Get the list of US States
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<StateDTO>> GetUSStatesWithSalesTaxRates()
        {
            return await ParseResultList<StateDTO>("States/Default.USStates?$filter=(HasSalesTaxRates eq true)&$orderby=StateName&$pagesize=999");
        }
        #endregion

        #region "FUNCTION: GetJDRegions"
        /// <summary>
        /// Get the list of regions
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<RegionDTO>> GetJDRegions()
        {
            return await ParseResultList<RegionDTO>("GetJDRegions");
        }
        #endregion

        #region "FUNCTION: GetFile"
        /// <summary>
        /// Get the list of announcements
        /// </summary>
        /// <returns></returns>
        public async Task<HCFileInfo> GetFile(String url)
        {
            return await ParseSingleResult<HCFileInfo>(url);
        }
        #endregion

        #region "FUNCTION: GenerateSSRSReport"
        /// <summary>
        /// Generate the specified SSRS report
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<ReportDTO> GenerateSSRSReport(String url, String content, String etag = null)
        {
            return await ParseSSRSReportResponse(url, content, etag);
        }
        #endregion

        #region "FUNCTION: GetCreditReport"
        /// <summary>
        /// Get the specified credit bureau report
        /// </summary>
        /// <param name="orderNo"></param>
        /// <param name="client"></param>
        /// <param name="reportID"></param>
        /// <returns></returns>
        public async Task<ODataResponse<byte[]>> GetCreditReport(Int32 orderNo, String client, eCreditReportType which)
        {
            switch (which)
            {
                case eCreditReportType.Cosignor:
                    return await ParseSingleResult<ODataResponse<byte[]>>(String.Format(@"CreditReports(OrderNo={0},Client='{1}')/Default.Cosignor", orderNo, client));

                case eCreditReportType.Customer:
                    return await ParseSingleResult<ODataResponse<byte[]>>(String.Format(@"CreditReports(OrderNo={0},Client='{1}')/Default.Customer", orderNo, client));

                default:
                    throw new NotImplementedException();
            }
        }
        #endregion

        #region "FUNCTION: ParseResultList"
        /// <summary>
        /// Attempt to deserialize the http response into the specified object type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task<IEnumerable<T>> ParseResultList<T>(String url, eSubmissionMethod method = eSubmissionMethod.GET, String jsonContent = null, String contentType = "application/x-www-form-urlencoded", String accessTokenOverride = null)
        {
            String json;
            IEnumerable<T> retVal = null;

            try
            {
                //-- GET THE JSON RESPONSE FROM THE API --
                json = await this.GetAPIResponseAsync(url, method, jsonContent, contentType, accessTokenOverride);

                //-- DESERIALIZE THE CONTENT INTO THE IEnumerable<T> --
                retVal = Newtonsoft.Json.JsonConvert.DeserializeObject<ODataResult<T>>(json).value;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                throw;
            }

            return retVal;
        }
        #endregion

        #region "FUNCTION: ParseSSRSReportReponse"
        /// <summary>
        /// Generate the SSRS Report and return the formatted response
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        private async Task<ReportDTO> ParseSSRSReportResponse(String url, String content, String etag = null)
        {
            if (content != null)
                return await ParseSingleResult<ReportDTO>(url, eSubmissionMethod.POST, content, etag: etag);
            else
                return await ParseSingleResult<ReportDTO>(url, eSubmissionMethod.POST, null, etag: etag);
        }
        #endregion

        #region "FUNCTION: ParseSingleResult"
        /// <summary>
        /// Attempt to deserialize the http response into the specified object type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task<T> ParseSingleResult<T>(String url, eSubmissionMethod method = eSubmissionMethod.GET, String jsonContent = null, String contentType = "application/x-www-form-urlencoded", String accessTokenOverride = null, String etag = null)
        {
            String json;
            T retVal = default(T);

            try
            {
                //-- GET THE JSON RESPONSE FROM THE API --
                json = await this.GetAPIResponseAsync(url, method, jsonContent, contentType, accessTokenOverride, etag);

                //-- DESERIALIZE THE CONTENT INTO AN OBJECT OF TYPE T --
                retVal = Newtonsoft.Json.JsonConvert.DeserializeObject<T>(json);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw;
            }

            return retVal;
        }
        #endregion

        #region "FUNCTION: GetAPIResponse"
        /// <summary>
        /// Attempt to deserialize the http response into the specified object type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="response"></param>
        /// <returns></returns>
        private async Task<String> GetAPIResponseAsync(String url, eSubmissionMethod method = eSubmissionMethod.GET, String jsonContent = null, String contentType = "application/x-www-form-urlencoded", String accessTokenOverride = null, String etag = null)
        {
            String json = null;
            HttpResponseMessage response = null;
            Int16 retryCount = 0;
            StringContent content;
            String accessToken = accessTokenOverride ?? this.BearerToken;

            try
            {
                while (retryCount <= 1)
                {
                    //-- CONNECT TO THE WEB API AND GET THE RESPONSE --
                    this.SetBearerToken(accessToken);

                    //-- ADD THE ETAG IF APPLICABLE --
                    if (!String.IsNullOrWhiteSpace(etag))
                        this.APIClient.DefaultRequestHeaders.Add("If-Match", etag);

                    switch (method)
                    {
                        case eSubmissionMethod.GET:
                            response = this.APIClient.GetAsync(this.APIClient.BaseAddress + url).Result;
                            break;

                        case eSubmissionMethod.POST:

                            if (String.IsNullOrWhiteSpace(jsonContent))
                                content = new StringContent(String.Empty);
                            else
                                content = new StringContent(jsonContent, System.Text.Encoding.UTF8, contentType);

                            response = this.APIClient.PostAsync(this.APIClient.BaseAddress + url, content).Result;
                            break;
                    }

                    if (response.IsSuccessStatusCode)
                    {
                        //-- READ THE JSON CONTENT --
                        json = await response.Content.ReadAsStringAsync();
                        break;
                    }
                    else if ((response.StatusCode == System.Net.HttpStatusCode.Unauthorized) && (retryCount < 1))
                    {
                        Trace.TraceInformation("Attempting to refresh expired access token");
                        retryCount += 1;

                        try
                        {
                            var result = await Helpers.EndpointAndTokenHelper.CheckAndPossiblyRefreshToken(eAccessTokenType.Undefined, System.Web.HttpContext.Current.User.Identity as ClaimsIdentity, username: this.CurrentUsername, fullName: this.CurrentFullName, entityCode: this.CurrentEntityCode, impersonateUser: this.CurrentImpersonatedUser, forceRefresh: true);     //TODO: FIX THIS
                            if ((result == null) || ((result.Status != Structures.eTokenRefreshResponse.TokenWasRefreshed) && (result.Status != Structures.eTokenRefreshResponse.TokenIsValid)))
                            {
                                System.Diagnostics.Debug.WriteLine("Failed to refresh the access token: " + result.Status.ToString());
                                throw new HttpException((Int32)response.StatusCode, response.ReasonPhrase);
                            }
                            else
                            {
                                //-- USE THE NEW ACCESS TOKEN ON THE NEXT ATTEMPT --
                                accessToken = result.NewAccessToken;
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Failed to refresh the access token: " + ex.ToString());
                            throw new HttpException((Int32)response.StatusCode, response.ReasonPhrase);
                        }

                    }
                    else
                    {
                        throw new HttpException((Int32)response.StatusCode, response.ReasonPhrase);
                    }
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw;
            }
            finally
            {
                if (response != null)
                {
                    try
                    {
                        response.Dispose();
                    }
                    catch (Exception)
                    {
                    }
                }
            }

            return json;
        }
        #endregion

        #region "FUNCTION: GetTelemarketerSettings"
        /// <summary>
        /// Get the telemarketer settings
        /// </summary>
        /// <param name="distributorNo"></param>
        /// <returns></returns>
        public async Task<TelemarketerSetupInfoDTO> GetTelemarketerSettings(string distributorNo)
        {
            return await ParseSingleResult<TelemarketerSetupInfoDTO>("DistributorTelemarketers/Default.GetTelemarketerSettings");
        }
        #endregion

        #region "FUNCTION: GetPossibleLeads"
        /// <summary>
        /// Get the list of available leads
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<PossibleLeadDTO>> GetPossibleLeads()
        {
            String url;
            url = $"PossibleLeads";

            try
            {
                return await ParseResultList<PossibleLeadDTO>(url);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                return new List<PossibleLeadDTO>();
            }
        }
        #endregion

        #region "PROCEDURE: Post"
        /// <summary>
        /// Attempt to deserialize the http response into the specified object type
        /// </summary>
        /// <returns></returns>
        private async Task Post(String url)
        {
            HttpResponseMessage response = null;
            Int16 retryCount = 0;
            String accessToken = this.BearerToken;

            try
            {
                while (retryCount <= 1)
                {
                    //-- CONNECT TO THE WEB API AND GET THE RESPONSE --
                    this.SetBearerToken(accessToken);
                    var content = new StringContent(String.Empty);
                    response = this.APIClient.PostAsync(this.APIClient.BaseAddress + url, content).Result;

                    if (response.IsSuccessStatusCode)
                    {
                        //-- THE REQUEST WAS SUCCESSFULL --
                        return;
                    }
                    else if ((response.StatusCode == System.Net.HttpStatusCode.Unauthorized) && (retryCount < 1))
                    {
                        Trace.TraceInformation("Attempting to refresh expired access token");
                        retryCount += 1;

                        try
                        {
                            var result = await Helpers.EndpointAndTokenHelper.CheckAndPossiblyRefreshToken(eAccessTokenType.Undefined, System.Web.HttpContext.Current.User.Identity as ClaimsIdentity, this.CurrentUsername, this.CurrentFullName, this.CurrentEntityCode, this.CurrentImpersonatedUser, true);
                            if ((result == null) || ((result.Status != Structures.eTokenRefreshResponse.TokenWasRefreshed) && (result.Status != Structures.eTokenRefreshResponse.TokenIsValid)))
                            {
                                System.Diagnostics.Debug.WriteLine("Failed to refresh the access token: " + result.Status.ToString());
                                throw new HttpException((Int32)response.StatusCode, response.ReasonPhrase);
                            }
                            else
                            {
                                accessToken = result.NewAccessToken;
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine("Failed to refresh the access token: " + ex.ToString());
                            throw new HttpException((Int32)response.StatusCode, response.ReasonPhrase);
                        }

                    }
                    else
                    {
                        throw new HttpException((Int32)response.StatusCode, response.ReasonPhrase);
                    }
                }

                throw new HttpException(500, "Failed to post request: " + this.APIClient.BaseAddress + url);
            }
            catch (Exception ex)
            {
                Trace.TraceError(ex.ToString());
                throw;
            }
            finally
            {
                if (response != null)
                {
                    try
                    {
                        response.Dispose();
                    }
                    catch (Exception)
                    {
                    }
                }
            }
        }
        #endregion

        #region "PROCEDURE: Dispose"
        /// <summary>
        /// Clean up the current class and make sure the http connection is closed
        /// </summary>
        public void Dispose()
        {
            if (this.APIClient != null)
            {
                try
                {
                    this.APIClient.Dispose();
                }
                catch (Exception)
                {
                }
            }
        }
        #endregion
    }
}
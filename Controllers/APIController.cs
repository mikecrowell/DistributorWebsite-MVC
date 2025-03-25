using DistributorWebsite.MVC.WebUI.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DistributorWebsite.MVC.WebUI.Controllers
{
    [HCAuthorize(skipRedirection: true)]
    public class APIController : BaseController
    {
        #region "PROCEDURE: CheckTokens"
        /// <summary>
        /// Check the access token and try to refresh it if it is expired
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SwitchEntity(string entitycode = null)
        {
            Helpers.Structures.TokenRefreshResponse result = null;

            try
            {
                //-- MAKE SURE THE ENTITY CODE WAS SUPPLIED --
                if (string.IsNullOrWhiteSpace(entitycode))
                    throw new System.Exception("entitycode was not supplied");

                //-- MAKE SURE THE ENTITY CODE IS VALID --
                var entities = (Dictionary<String, String>)ViewBag.UserEntities;
                if (entities == null || !entities.ContainsKey(entitycode))
                    throw new System.Exception(String.Format("{0} is an invalid entity code"));

                result = await Helpers.EndpointAndTokenHelper.CheckAndPossiblyRefreshToken(eAccessTokenType.Undefined,
                                                                                           System.Web.HttpContext.Current.User.Identity as ClaimsIdentity,
                                                                                           this.User.Identity.Name,
                                                                                           this.CurrentFullName,
                                                                                           entitycode,
                                                                                           Helpers.CookieHelper.CurrentUserBeingImpersonated,
                                                                                           forceRefresh: true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw;
            }

            return Json(Newtonsoft.Json.JsonConvert.SerializeObject(result));
        }
        #endregion 

        #region "PROCEDURE: CheckTokens"
        /// <summary>
        /// Check the access token and try to refresh it if it is expired
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CheckTokens()
        {
            Helpers.Structures.TokenRefreshResponse result = null;

            try
            {
                result = await Helpers.EndpointAndTokenHelper.CheckAndPossiblyRefreshToken(eAccessTokenType.Undefined,
                                                                                           System.Web.HttpContext.Current.User.Identity as ClaimsIdentity,
                                                                                           this.User.Identity.Name,
                                                                                           this.CurrentFullName,
                                                                                           Helpers.CookieHelper.CurrentEntity,
                                                                                           Helpers.CookieHelper.CurrentUserBeingImpersonated,
                                                                                           forceRefresh: true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                throw;
            }

            return Json(Newtonsoft.Json.JsonConvert.SerializeObject(result));
        }
        #endregion 

        #region "PROCEDURE: GetErrorObject"
        [Authorize]
        [ValidateInput(false)]
        [AcceptVerbs(HttpVerbs.Post)]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> GetErrorObject(String error)
        {
            ErrorInfo retVal = null;
            JsonSerializerSettings settings;

            try
            {
                //-- CREATE THE ERROR OBJECT FROM THE ERROR --
                if (!String.IsNullOrWhiteSpace(error))
                {
                    settings = new JsonSerializerSettings();
                    settings.Error = HandleDeserializationError;

                    retVal = Newtonsoft.Json.JsonConvert.DeserializeObject<ErrorInfo>(error, settings);
                }
                else
                    retVal = new ErrorInfo();

                retVal.ShowDetails = Helpers.Application.Instance.ShowErrorDetails;
                retVal.ShowLogoutButton = false;

                //-- BUILD THE ERROR OBJECT FROM THE SERIALIZED JSON ERROR OBJECT --
                if (String.IsNullOrWhiteSpace(retVal.StatusCode)) retVal.StatusCode = "500";
                if (String.IsNullOrWhiteSpace(retVal.StatusDesc)) retVal.StatusDesc = LocalResources.HCResources.InternalServerError;

                if (retVal.StatusMessages == null)
                    retVal.StatusMessages = new ErrorInfoData();

                if (String.IsNullOrEmpty(retVal.StatusMessages.Message))
                {
                    if ((retVal.StatusMessages.Error != null) && (!String.IsNullOrWhiteSpace(retVal.StatusMessages.Error.message)))
                        retVal.StatusMessages.Message = retVal.StatusMessages.Error.message.Replace(Environment.NewLine, @"<br/>");
                    else
                        retVal.StatusMessages.Message = LocalResources.HCResources.InternalServerErrorDescription;
                }

                if (String.IsNullOrEmpty(retVal.StatusMessages.MessageDetail))
                {
                    if ((retVal.StatusMessages.Error.innererror != null))
                        retVal.StatusMessages.MessageDetail = retVal.StatusMessages.Error.innererror.ToString().Replace(Environment.NewLine, @"<br/>");
                }

                //-- OVERIDE FOR KNOWN ERROR TYPES --
                switch (retVal.StatusCode)
                {
                    case "401":
                        retVal.ShowLogoutButton = true;
                        break;

                    case "503":
                        retVal.StatusDesc = LocalResources.HCResources.ServiceUnavailable;
                        retVal.StatusMessages.Message = LocalResources.HCResources.ServiceUnavailableDesc;
                        break;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());

                retVal = new ErrorInfo();
                retVal.StatusCode = "500";
                retVal.StatusDesc = LocalResources.HCResources.InternalServerError;
                retVal.StatusMessages = new ErrorInfoData();
                retVal.StatusMessages.Message = LocalResources.HCResources.InternalServerErrorDescription;
                retVal.StatusMessages.MessageDetail = ex.ToString();
            }

            if (!retVal.ShowDetails)
                retVal.StatusMessages.MessageDetail = "";

            //-- HANDLE THE PLEASE TRY AGAIN MESSAGE --
            if (retVal.StatusMessages.Error.target == "InvalidModelState")
                retVal.ShowPleaseTryAgainMessage = false;

            //-- REMOVE THE ERROR SO IT ISN'T DUPLICATED WHEN BEING SENT BACK TO THE CLIENT --
            retVal.StatusMessages.Error = null;

            return Json(Newtonsoft.Json.JsonConvert.SerializeObject(retVal));
        }
        #endregion

        #region "PROCEDURE: HandleDesrilizationError"
        /// <summary>
        /// Ignore any deserialization errors when trying to create an object from JSON string
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="errorArgs"></param>
        public void HandleDeserializationError(object sender, ErrorEventArgs errorArgs)
        {
            var currentError = errorArgs.ErrorContext.Error.Message;
            errorArgs.ErrorContext.Handled = true;
        }
        #endregion
    }
}
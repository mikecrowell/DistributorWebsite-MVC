using DistributorWebsite.MVC.WebUI.Controllers;
using DistributorWebsite.MVC.WebUI.Models;
using DistributorWebsite.MVC.WebUI.Models.Shared;
using IdentityModel;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace DistributorWebsite.MVC.WebUI.Helpers
{
    public static partial class Common
    {
        #region "FUNCTION: GetAntiForgeryToken"
        /// <summary>
        /// Get the anti forgery token values to be used in Angular js
        /// </summary>
        /// <returns></returns>
        public static string GetAntiForgeryToken()
        {
            string cookieToken, formToken;
            AntiForgery.GetTokens(null, out cookieToken, out formToken);
            return cookieToken + ":" + formToken;
        }
        #endregion

        #region "PROPERTY: UseInlineDownload"
        /// <summary>
        /// Determine if we should use inline content disposition
        /// </summary>
        public static Boolean UseInlineDownload
        {
            get
            {
                eDeviceType type;
                Boolean? useInlineDownloads;

                //-- CHECK TO SEE IF THE USER SAVED A PREFERENCE ON THEIR CURRENT DEVICE --
                useInlineDownloads = Helpers.CookieHelper.UseInlineDownload;

                if (useInlineDownloads.HasValue)
                {
                    //-- USE THE VALUE SET BY THE USER --
                    return (useInlineDownloads.Value);
                }
                else
                {
                    type = Helpers.Common.CurrentDeviceType;

                    switch (type)
                    {
                        case eDeviceType.Android:
                        case eDeviceType.WindowsPhone:
                        case eDeviceType.OtherMobileDevice:
                            return (false);

                        default:
                            return (true);
                    }
                }
            }
        }
        #endregion

        #region "FUNCTION: GetDeviceType"
        /// <summary>
        /// Get the current device type
        /// </summary>
        public static eDeviceType CurrentDeviceType
        {
            get
            {
                String curBrowserString;
                eDeviceType retVal = eDeviceType.Unknown;

                try
                {
                    curBrowserString = System.Web.HttpContext.Current.Request.UserAgent;
                    if (!String.IsNullOrWhiteSpace(curBrowserString))
                    {
                        if (curBrowserString.ToLower().Contains("android"))
                        {
                            retVal = eDeviceType.Android;
                        }
                        else if (curBrowserString.ToLower().Contains("windows phone"))
                        {
                            retVal = eDeviceType.WindowsPhone;
                        }
                        else if (curBrowserString.ToLower().Contains("ipod") || curBrowserString.ToLower().Contains("iphone"))
                        {
                            retVal = eDeviceType.IPhone;
                        }
                        else if (curBrowserString.ToLower().Contains("ipad"))
                        {
                            retVal = eDeviceType.IPAD;
                        }
                        else if (System.Web.HttpContext.Current.Request.Browser.IsMobileDevice)
                        {
                            retVal = eDeviceType.OtherMobileDevice;
                        }
                    }
                }
                catch (Exception)
                {
                    retVal = eDeviceType.Unknown;
                }

                return (retVal);
            }
        }
        #endregion

        #region "FUNCTION: GetCurrentUserName"
        /// <summary>
        /// Get the distributor token of the currently logged in user
        /// </summary>
        /// <returns></returns>
        public static String GetCurrentUserName(System.Web.Mvc.Controller controller)
        {
            ClaimsIdentity id;

            try
            {
                //-- GET THE CURRENT USER --
                if (controller != null)
                    id = controller.User.Identity as ClaimsIdentity;
                else
                    id = System.Web.HttpContext.Current.User.Identity as ClaimsIdentity;

                if (id != null)
                {
                    if (id.HasClaim(o => o.Type == "hcusername"))
                    {
                        return id.FindFirst("hcusername").Value;
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.ToString());
            }

            return "";
        }
        #endregion

        #region "FUNCTION: GetUserAddress"
        /// <summary>
        /// Get the user's remote address
        /// </summary>
        /// <returns></returns>
        public static String GetUserAddress()
        {
            String retVal;

            try
            {
                retVal = System.Web.HttpContext.Current.Request.UserHostAddress;
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                retVal = "UNKNOWN";
            }

            return (retVal);
        }
        #endregion

        #region "FUNCTION: DecodeAndWrite"
        /// <summary>
        /// Attempt to decode and write out the JWT token details
        /// </summary>
        /// <param name="token"></param>
        /// <param name="tokenType"></param>
        public static Helpers.Security.AccessToken ParseEntityAccessToken(String token)
        {
            Helpers.Security.AccessToken entityToken;

            try
            {
                var parts = token.Split('.');
                var part = Encoding.UTF8.GetString(Base64Url.Decode(parts[1]));
                var jwt = JObject.Parse(part);

                entityToken = Helpers.Security.AccessToken.Parse(jwt.GetValue("hcdistwebapi").ToString());
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                entityToken = null;
            }

            return (entityToken);
        }
        #endregion

        #region "FUNCTION: GetCurrentEntityToken"
        /// <summary>
        /// Get the current user's entity token
        /// </summary>
        /// <returns></returns>
        public static Helpers.Security.AccessToken GetCurrentEntityToken()
        {
            try
            {
                //-- PARSE THE ACCESS TOKEN --
                var token = Helpers.Common.ParseEntityAccessToken(Helpers.CookieHelper.AccessToken);
                if (token == null)
                {
                    return null;
                }

                return (token);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.Message, ex);
                return (null);
            }
        }
        #endregion

        #region "FUNCTION: CanUserAccessSecurityItem"
        /// <summary>
        /// Check to see if the current user can access the specified security item
        /// </summary>
        /// <param name="securityItem"></param>
        /// <returns></returns>
        public static Boolean CanUserAccessSecurityItem(eSecurityItem securityItem, Helpers.Security.AccessToken currentToken = null)
        {
            SecurityItemDTO securityItemDef;
            Helpers.Security.AccessToken token;
            String securityString;

            try
            {
                //-- GET THE CURRENT ENTITY ACCESS TOKEN --
                if (currentToken != null)
                    token = currentToken;
                else
                    token = GetCurrentEntityToken();

                if (token == null || String.IsNullOrWhiteSpace(token.SecurityString) || token.SecurityString.Length != 64)
                    return (false);

                securityString = token.SecurityString;

                //-- MAKE SURE WE HAVE A VALID COMPRESSED OR DECOMPRESSED SECURITY STRING --
                if (securityString.Length == 64)
                    securityString = Helpers.Security.AccessToken.DecompressUserSecurityString(securityString);

                if (securityString.Length != 256)
                    return (false);

                //-- GET THE SECURITY ITEM --
                securityItemDef = Helpers.Application.Instance.SecurityItems.Where(o => o.Value.SecurityItemName.ToUpper() == securityItem.ToString().ToUpper()).FirstOrDefault().Value;
                if ((securityItemDef == null) || (securityItemDef.SecurityIndex < 0))
                    return (false);

                //-- MAKE SURE THE SECURITY ITEM IS ENABLED --
                if (!securityItemDef.Enabled.HasValue || securityItemDef.Enabled.Value == false)
                    return (false);

                //-- MAKE SURE THE SECURITY ITEM IS VISIBLE --
                if (!String.IsNullOrWhiteSpace(securityItemDef.Visibility))
                {
                    switch (securityItemDef.Visibility.ToUpper())
                    {
                        case "I":
                            //-- INTERNAL ONLY --
                            if (!token.IsInternal)
                                return (false);
                            break;

                        case "E":
                            //-- EXTERNAL ONLY --
                            if ((token.IsInternal) && (token.UserID == Helpers.Constants.INTERNALUSERID))
                                return (false);
                            break;
                    }
                }

                //-- CHECK TO SEE IF THE USER CAN ACCESS THE ITEM --
                return "[Y][1]".Contains(securityString.Substring(securityItemDef.SecurityIndex, 1).ToUpper());
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
                return (false);
            }
        }
        #endregion

        #region "FUNCTION: NoRecordsFoundHTML"
        /// <summary>
        /// Build the custom no records found HTML
        /// </summary>
        public static string NoRecordsFoundHTML(string entityName, string header = null, string message = null, string faicon = null)
        {
            return "<div class='row'>" +
                   "  <div class='col-xs-12'>" +
                   "    <div class='norecordsfound panel panel-info'>" +
                   "      <div class='panel-body'>" +
                   "        <div class='row'>" +
                   "          <div class='col-xs-12 col-lg-offset-1 col-lg-10'>" +
                   "             <div><span class='fa " + (faicon ?? "fa-search") + " fa-5x'></span></div>" +
                   "             <div>" +
     String.Format("               <h2>{0}</h2>", (header ?? String.Format(LocalResources.HCResources.NoMatchingEntitiesFound, entityName)).ToUpper()) +
     String.Format("               <p>{0}</p>", message ?? String.Format(LocalResources.HCResources.NoMathingEntitiesText, entityName)) +
                   "               <br />" +
                   "             </div>" +
                   "          </div>" +
                   "        </div>" +
                   "     </div>" +
                   "   </div>" +
                   "</div>";
        }
        #endregion

        #region "PROCEDURE: InitializePaymentFromModel"
        /// <summary>
        /// Initialize the payment form objects
        /// </summary>
        public static void InitializePaymentFromModel(ePaymentPageHost host, BaseController controller, Boolean? allowEmailPayments = null, Boolean? freezeDistributorNo = null, Boolean? freezeSalesperson = null, Boolean? freezeClient = null, Boolean? freezeOrderNo = null, eSecurityItem? securityItem = eSecurityItem.mnuCreditCardProcessing, bool autoInitHistoryGrid = true, bool? showPaymentEntryHeader = null, bool allowTokenization = false, bool allowOnFilePayments = false, bool allowEmailReceipt = true, bool allowDebitOrCredit = true, String paymentSource = "DistributorWebsite")
        {
            CreditCardPaymentDTO model = new CreditCardPaymentDTO();

            String apiURL = "";
            String paymentType;
            Boolean freezeDistributor = false;
            Boolean freezeSP = false;
            Boolean showHeader = showPaymentEntryHeader.HasValue ? showPaymentEntryHeader.Value : true;
            Boolean freezeCustomerName = false;
            Boolean showCardType = true;
            Boolean showExistingAccounts = false;
            Boolean showCustomerTable = false;
            Boolean showCustomerSearch = false;

            Helpers.Security.AccessToken token = GetCurrentEntityToken();

            //-- BASE API URL --
            switch (host)
            {
                case ePaymentPageHost.ListPayments:
                    apiURL = Helpers.Application.Instance.APIS.DistributorWebAPI + "SubmitListPayment";
                    freezeClient = false;
                    freezeDistributor = true;
                    freezeOrderNo = true;
                    freezeSP = true;
                    freezeCustomerName = true;
                    showHeader = false;
                    showCardType = false;
                    showExistingAccounts = true;
                    showCustomerTable = true;
                    paymentType = "LISTPAYMENT";
                    break;

                case ePaymentPageHost.CustomerPayments:
                    apiURL = Helpers.Application.Instance.APIS.DistributorWebAPI + "InitializePayment";
                    freezeClient = false;
                    freezeDistributor = true;
                    freezeOrderNo = true;
                    freezeSP = true;
                    freezeCustomerName = true;
                    showHeader = false;
                    showCardType = false;
                    showExistingAccounts = false;
                    showCustomerSearch = true;
                    paymentType = "CUSTPAYMENT";
                    break;

                default:
                    showExistingAccounts = allowOnFilePayments;
                    showCardType = false;
                    apiURL = Helpers.Application.Instance.APIS.DistributorWebAPI + "InitializePayment";
                    paymentType = "ORDERPAYMENT";
                    break;
            }

            switch (token.EntityType)
            {
                case eEntityType.Distributor:
                    freezeDistributor = true;
                    model.DistributorNo = token.EntityCode;
                    break;

                case eEntityType.Salesperson:
                    freezeSP = true;
                    model.SalespersonCode = token.EntityCode;
                    break;
            }

            //-- APPLY OVERRIDES --
            if (freezeDistributorNo.HasValue) freezeDistributor = freezeDistributorNo.Value;
            if (freezeSalesperson.HasValue) freezeSP = freezeSalesperson.Value;
            if (!freezeClient.HasValue) freezeClient = false;
            if (!freezeOrderNo.HasValue) freezeOrderNo = false;

            //-- DEBIT/CREDIT SELECTION LIST --
            List<SelectListItem> dcSelectList = new List<SelectListItem>();
            dcSelectList.Add(new SelectListItem() { Value = "", Text = "" });
            dcSelectList.Add(new SelectListItem() { Value = "credit", Text = LocalResources.HCResources.Credit });
            dcSelectList.Add(new SelectListItem() { Value = "debit", Text = LocalResources.HCResources.Debit });

            //-- SET THE VIEWBAG PROPERTIES --
            controller.ViewBag.CCPaymentEntry_FreezeDistributorNo = freezeDistributor;
            controller.ViewBag.CCPaymentEntry_FreezeSalesperson = freezeSP;
            controller.ViewBag.CCPaymentEntry_FreezeClient = freezeClient;
            controller.ViewBag.CCPaymentEntry_FreezeOrderNo = freezeOrderNo;
            controller.ViewBag.CCPaymentEntry_SecurityItem = securityItem;
            controller.ViewBag.CCPaymentEntry_AutoInitGrid = autoInitHistoryGrid;
            controller.ViewBag.CCPaymentEntry_APIURL = apiURL;
            controller.ViewBag.CCPaymentEntry_AllowEmailPayments = allowEmailPayments ?? false;
            controller.ViewBag.CCPaymentEntry_ShowHeader = showHeader;
            controller.ViewBag.CCPaymentEntry_FreezeCustomerName = freezeCustomerName;
            controller.ViewBag.CCPaymentEntry_ShowCardTypes = showCardType;
            controller.ViewBag.CCPaymentEntry_ShowExistingAccounts = showExistingAccounts;
            controller.ViewBag.CCPaymentEntry_ShowCustomerTable = showCustomerTable;
            controller.ViewBag.CCPaymentEntry_ShowCustomerSearch = showCustomerSearch;
            controller.ViewBag.CCPaymentEntry_Type = paymentType;
            controller.ViewBag.CCPaymentEntry_ShowDebitCredit = allowDebitOrCredit;
            controller.ViewBag.CCPaymentEntry_ShowPaymentReceipt = allowEmailReceipt;
            controller.ViewBag.CCPaymentEntry_DCSelectList = new SelectList(dcSelectList, "Value", "Text");
            controller.ViewBag.CCPaymentEntry_Source = paymentSource;
            controller.ViewBag.CCPaymentEntry = model;            
        }
        #endregion

        #region "FUNCTION: GetLanguagePreferenceSelectList"
        /// <summary>
        /// Get the language preference select list
        /// </summary>
        /// <param name="includeBlankOption"></param>
        /// <param name="blankOptionValue"></param>
        /// <returns></returns>
        public static SelectList GetLanguagePreferenceSelectList(bool includeBlankOption = true, string blankOptionValue = "")
        {
            List<SelectListItem> languages = new List<SelectListItem>();

            if (includeBlankOption)
                languages.Add(new SelectListItem() { Value = blankOptionValue, Text = "" });

            languages.Add(new SelectListItem() { Value = "en", Text = "English" });
            languages.Add(new SelectListItem() { Value = "es", Text = "Español" });
            languages.Add(new SelectListItem() { Value = "pt", Text = "Português" });

            return new SelectList(languages, "Value", "Text");
        }
        #endregion

        #region "FUNCTION: GetTimeZoneSelectList"
        /// <summary>
        /// Get the time zone select list
        /// </summary>
        /// <param name="includeBlankOption"></param>
        /// <param name="blankOptionValue"></param>
        /// <returns></returns>
        public static SelectList GetTimeZoneSelectList(bool includeBlankOption = true, string blankOptionValue = "")
        {
            List<SelectListItem> timeZones = new List<SelectListItem>();

            if (includeBlankOption)
                timeZones.Add(new SelectListItem() { Value = blankOptionValue, Text = "" });

            timeZones.Add(new SelectListItem() { Value = "Pacific/Kiritimati", Text = "(GMT+14:00) Line Islands Time (Pacific/Kiritimati)" });
            timeZones.Add(new SelectListItem() { Value = "Pacific/Chatham", Text = "(GMT+13:45) Chatham Daylight Time (Pacific/Chatham)" });
            timeZones.Add(new SelectListItem() { Value = "Pacific/Auckland", Text = "(GMT+13:00) New Zealand Daylight Time (Pacific/Auckland)" });
            timeZones.Add(new SelectListItem() { Value = "Pacific/Enderbury", Text = "(GMT+13:00) Phoenix Islands Time (Pacific/Enderbury)" });
            timeZones.Add(new SelectListItem() { Value = "Pacific/Fiji", Text = "(GMT+13:00) Fiji Summer Time (Pacific/Fiji)" });
            timeZones.Add(new SelectListItem() { Value = "Pacific/Tongatapu", Text = "(GMT+13:00) Tonga Time (Pacific/Tongatapu)" });
            timeZones.Add(new SelectListItem() { Value = "Asia/Kamchatka", Text = "(GMT+12:00) Magadan Time (Asia/Kamchatka)" });
            timeZones.Add(new SelectListItem() { Value = "Pacific/Norfolk", Text = "(GMT+11:30) Norfolk Islands Time (Pacific/Norfolk)*" });
            timeZones.Add(new SelectListItem() { Value = "Australia/Lord_Howe", Text = "(GMT+11:00) Lord Howe Daylight Time (Australia/Lord_Howe)" });
            timeZones.Add(new SelectListItem() { Value = "Australia/Sydney", Text = "(GMT+11:00) Australian Eastern Daylight Time (Australia/Sydney)" });
            timeZones.Add(new SelectListItem() { Value = "Pacific/Guadalcanal", Text = "(GMT+11:00) Solomon Islands Time (Pacific/Guadalcanal)" });
            timeZones.Add(new SelectListItem() { Value = "Australia/Adelaide", Text = "(GMT+10:30) Australian Central Daylight Time (Australia/Adelaide)" });
            timeZones.Add(new SelectListItem() { Value = "Australia/Darwin", Text = "(GMT+09:30) Australian Central Standard Time (Australia/Darwin)" });
            timeZones.Add(new SelectListItem() { Value = "Asia/Seoul", Text = "(GMT+09:00) Korean Standard Time (Asia/Seoul)" });
            timeZones.Add(new SelectListItem() { Value = "Asia/Tokyo", Text = "(GMT+09:00) Japan Standard Time (Asia/Tokyo)" });
            timeZones.Add(new SelectListItem() { Value = "Asia/Hong_Kong", Text = "(GMT+08:00) Hong Kong Time (Asia/Hong_Kong)" });
            timeZones.Add(new SelectListItem() { Value = "Asia/Kuala_Lumpur", Text = "(GMT+08:00) Malaysia Time (Asia/Kuala_Lumpur)" });
            timeZones.Add(new SelectListItem() { Value = "Asia/Manila", Text = "(GMT+08:00) Philippine Time (Asia/Manila)" });
            timeZones.Add(new SelectListItem() { Value = "Asia/Shanghai", Text = "(GMT+08:00) China Standard Time (Asia/Shanghai)" });
            timeZones.Add(new SelectListItem() { Value = "Asia/Singapore", Text = "(GMT+08:00) Singapore Standard Time (Asia/Singapore)" });
            timeZones.Add(new SelectListItem() { Value = "Asia/Taipei", Text = "(GMT+08:00) Taipei Standard Time (Asia/Taipei)" });
            timeZones.Add(new SelectListItem() { Value = "Australia/Perth", Text = "(GMT+08:00) Australian Western Standard Time (Australia/Perth)" });
            timeZones.Add(new SelectListItem() { Value = "Asia/Bangkok", Text = "(GMT+07:00) Indochina Time (Asia/Bangkok)" });
            timeZones.Add(new SelectListItem() { Value = "Asia/Ho_Chi_Minh", Text = "(GMT+07:00) Indochina Time (Asia/Ho_Chi_Minh)" });
            timeZones.Add(new SelectListItem() { Value = "Asia/Jakarta", Text = "(GMT+07:00) Western Indonesia Time (Asia/Jakarta)" });
            timeZones.Add(new SelectListItem() { Value = "Asia/Rangoon", Text = "(GMT+06:30) Myanmar Time (Asia/Rangoon)" });
            timeZones.Add(new SelectListItem() { Value = "Asia/Dhaka", Text = "(GMT+06:00) Bangladesh Time (Asia/Dhaka)" });
            timeZones.Add(new SelectListItem() { Value = "Asia/Yekaterinburg", Text = "(GMT+06:00) Yekaterinburg Time (Asia/Yekaterinburg)" });
            timeZones.Add(new SelectListItem() { Value = "Asia/Kathmandu", Text = "(GMT+05:45) Nepal Time (Asia/Kathmandu)" });
            timeZones.Add(new SelectListItem() { Value = "Asia/Colombo", Text = "(GMT+05:30) India Standard Time (Asia/Colombo)" });
            timeZones.Add(new SelectListItem() { Value = "Asia/Kolkata", Text = "(GMT+05:30) India Standard Time (Asia/Kolkata)" });
            timeZones.Add(new SelectListItem() { Value = "Asia/Karachi", Text = "(GMT+05:00) Pakistan Time (Asia/Karachi)" });
            timeZones.Add(new SelectListItem() { Value = "Asia/Tashkent", Text = "(GMT+05:00) Uzbekistan Time (Asia/Tashkent)" });
            timeZones.Add(new SelectListItem() { Value = "Asia/Kabul", Text = "(GMT+04:30) Afghanistan Time (Asia/Kabul)" });
            timeZones.Add(new SelectListItem() { Value = "Asia/Dubai", Text = "(GMT+04:00) Gulf Standard Time (Asia/Dubai)" });
            timeZones.Add(new SelectListItem() { Value = "Asia/Tbilisi", Text = "(GMT+04:00) Georgia Time (Asia/Tbilisi)" });
            timeZones.Add(new SelectListItem() { Value = "Europe/Moscow", Text = "(GMT+04:00) Moscow Standard Time (Europe/Moscow)" });
            timeZones.Add(new SelectListItem() { Value = "Asia/Tehran", Text = "(GMT+03:30) Iran Standard Time (Asia/Tehran)" });
            timeZones.Add(new SelectListItem() { Value = "Africa/Nairobi", Text = "(GMT+03:00) East Africa Time (Africa/Nairobi)" });
            timeZones.Add(new SelectListItem() { Value = "Asia/Baghdad", Text = "(GMT+03:00) Arabian Standard Time (Asia/Baghdad)" });
            timeZones.Add(new SelectListItem() { Value = "Asia/Kuwait", Text = "(GMT+03:00) Arabian Standard Time (Asia/Kuwait)" });
            timeZones.Add(new SelectListItem() { Value = "Asia/Riyadh", Text = "(GMT+03:00) Arabian Standard Time (Asia/Riyadh)" });
            timeZones.Add(new SelectListItem() { Value = "Europe/Minsk", Text = "(GMT+03:00) Further-eastern European Time (Europe/Minsk)" });
            timeZones.Add(new SelectListItem() { Value = "Africa/Cairo", Text = "(GMT+02:00) Eastern European Time (Africa/Cairo)" });
            timeZones.Add(new SelectListItem() { Value = "Africa/Johannesburg", Text = "(GMT+02:00) South Africa Standard Time (Africa/Johannesburg)" });
            timeZones.Add(new SelectListItem() { Value = "Asia/Jerusalem", Text = "(GMT+02:00) Israel Standard Time (Asia/Jerusalem)" });
            timeZones.Add(new SelectListItem() { Value = "Europe/Athens", Text = "(GMT+02:00) Eastern European Time (Europe/Athens)" });
            timeZones.Add(new SelectListItem() { Value = "Europe/Bucharest", Text = "(GMT+02:00) Eastern European Time (Europe/Bucharest)" });
            timeZones.Add(new SelectListItem() { Value = "Europe/Helsinki", Text = "(GMT+02:00) Eastern European Time (Europe/Helsinki)" });
            timeZones.Add(new SelectListItem() { Value = "Europe/Istanbul", Text = "(GMT+02:00) Eastern European Time (Europe/Istanbul)" });
            timeZones.Add(new SelectListItem() { Value = "Africa/Algiers", Text = "(GMT+01:00) Central European Time (Africa/Algiers)" });
            timeZones.Add(new SelectListItem() { Value = "Europe/Amsterdam", Text = "(GMT+01:00) Central European Time (Europe/Amsterdam)" });
            timeZones.Add(new SelectListItem() { Value = "Europe/Berlin", Text = "(GMT+01:00) Central European Time (Europe/Berlin)" });
            timeZones.Add(new SelectListItem() { Value = "Europe/Brussels", Text = "(GMT+01:00) Central European Time (Europe/Brussels)" });
            timeZones.Add(new SelectListItem() { Value = "Europe/Paris", Text = "(GMT+01:00) Central European Time (Europe/Paris)" });
            timeZones.Add(new SelectListItem() { Value = "Europe/Prague", Text = "(GMT+01:00) Central European Time (Europe/Prague)" });
            timeZones.Add(new SelectListItem() { Value = "Europe/Rome", Text = "(GMT+01:00) Central European Time (Europe/Rome)" });
            timeZones.Add(new SelectListItem() { Value = "Europe/Dublin", Text = "(GMT+00:00) Greenwich Mean Time (Europe/Dublin)" });
            timeZones.Add(new SelectListItem() { Value = "Europe/Lisbon", Text = "(GMT+00:00) Western European Time (Europe/Lisbon)" });
            timeZones.Add(new SelectListItem() { Value = "Europe/London", Text = "(GMT+00:00) Greenwich Mean Time (Europe/London)" });
            timeZones.Add(new SelectListItem() { Value = "GMT", Text = "(GMT+00:00) Greenwich Mean Time (GMT)" });
            timeZones.Add(new SelectListItem() { Value = "Atlantic/Cape_Verde", Text = "(GMT-01:00) Cape Verde Time (Atlantic/Cape_Verde)" });
            timeZones.Add(new SelectListItem() { Value = "America/Sao_Paulo", Text = "(GMT-02:00) Brasilia Summer Time (America/Sao_Paulo)" });
            timeZones.Add(new SelectListItem() { Value = "Atlantic/South_Georgia", Text = "(GMT-02:00) South Georgia Time (Atlantic/South_Georgia)" });
            timeZones.Add(new SelectListItem() { Value = "America/Argentina/Buenos_Aires", Text = "(GMT-03:00) Argentina Time (America/Argentina/Buenos_Aires)" });
            timeZones.Add(new SelectListItem() { Value = "America/Santiago", Text = "(GMT-03:00) Chile Summer Time (America/Santiago)" });
            timeZones.Add(new SelectListItem() { Value = "America/St_Johns", Text = "(GMT-03:30) Newfoundland Standard Time (America/St_Johns)" });
            timeZones.Add(new SelectListItem() { Value = "America/Halifax", Text = "(GMT-04:00) Atlantic Standard Time (America/Halifax)" });
            timeZones.Add(new SelectListItem() { Value = "America/Puerto_Rico", Text = "(GMT-04:00) Atlantic Standard Time (America/Puerto_Rico)" });
            timeZones.Add(new SelectListItem() { Value = "Atlantic/Bermuda", Text = "(GMT-04:00) Atlantic Standard Time (Atlantic/Bermuda)" });
            timeZones.Add(new SelectListItem() { Value = "America/Caracas", Text = "(GMT-04:30) Venezuela Time (America/Caracas)" });
            timeZones.Add(new SelectListItem() { Value = "America/Bogota", Text = "(GMT-05:00) Colombia Time (America/Bogota)" });
            timeZones.Add(new SelectListItem() { Value = "America/Indiana/Indianapolis", Text = "(GMT-05:00) Eastern Standard Time (America/Indiana/Indianapolis)" });
            timeZones.Add(new SelectListItem() { Value = "America/Lima", Text = "(GMT-05:00) Peru Time (America/Lima)" });
            timeZones.Add(new SelectListItem() { Value = "America/New_York", Text = "(GMT-05:00) Eastern Standard Time (America/New_York)" });
            timeZones.Add(new SelectListItem() { Value = "America/Panama", Text = "(GMT-05:00) Eastern Standard Time (America/Panama)" });
            timeZones.Add(new SelectListItem() { Value = "America/Chicago", Text = "(GMT-06:00) Central Standard Time (America/Chicago)" });
            timeZones.Add(new SelectListItem() { Value = "America/El_Salvador", Text = "(GMT-06:00) Central Standard Time (America/El_Salvador)" });
            timeZones.Add(new SelectListItem() { Value = "America/Mexico_City", Text = "(GMT-06:00) Central Standard Time (America/Mexico_City)" });
            timeZones.Add(new SelectListItem() { Value = "America/Denver", Text = "(GMT-07:00) Mountain Standard Time (America/Denver)****America/Denver" });
            timeZones.Add(new SelectListItem() { Value = "America/Phoenix", Text = "(GMT-07:00) Mountain Standard Time (America/Phoenix)" });
            timeZones.Add(new SelectListItem() { Value = "America/Los_Angeles", Text = "(GMT-08:00) Pacific Standard Time (America/Los_Angeles)" });
            timeZones.Add(new SelectListItem() { Value = "America/Tijuana", Text = "(GMT-08:00) Pacific Standard Time (America/Tijuana)" });
            timeZones.Add(new SelectListItem() { Value = "America/Anchorage", Text = "(GMT-09:00) Alaska Standard Time (America/Anchorage)" });
            timeZones.Add(new SelectListItem() { Value = "Pacific/Honolulu", Text = "(GMT-10:00) Hawaii-Aleutian Standard Time (Pacific/Honolulu)" });
            timeZones.Add(new SelectListItem() { Value = "Pacific/Niue", Text = "(GMT-11:00) Niue Time (Pacific/Niue)" });
            timeZones.Add(new SelectListItem() { Value = "Pacific/Pago_Pago", Text = "(GMT-11:00) Samoa Standard Time (Pacific/Pago_Pago)" });

            return new SelectList(timeZones, "Value", "Text");
        }
        #endregion

        #region "FUNCTION: GetCurrentEntityClients"
        /// <summary>
        /// Get the list of clients the current entity has access to
        /// </summary>
        /// <returns></returns>
        public static SelectList GetCurrentEntityClients(bool includeBlankOption = true, string blankOptionValue = "", bool useAllForBlankLabel = false)
        {
            List<SelectListItem> clients = new List<SelectListItem>();
            Helpers.Security.AccessToken token = GetCurrentEntityToken();
            String clientDescription;

            if (includeBlankOption)
                clients.Add(new SelectListItem() { Value = blankOptionValue, Text = (useAllForBlankLabel ? LocalResources.HCResources.All : "") });

            foreach (var client in token.Clients)
            {
                //-- INITIALIZE THE CLIENT DESCRIPTION FROM THE RESOURCE FILE --
                try
                {
                    clientDescription = LocalResources.HCResources.ResourceManager.GetString("Client" + client.ToUpper());
                }
                catch (Exception)
                {
                    clientDescription = null;
                }
                if (String.IsNullOrWhiteSpace(clientDescription))
                    clientDescription = client.ToUpper();

                clients.Add(new SelectListItem() { Value = client, Text = clientDescription, Selected = client == token.DefaultClient });
            }

            return new SelectList(clients, "Value", "Text");
        }
        #endregion
        
        #region "FUNCTION: DoesSecurityItemSupportClient"
        /// <summary>
        /// Check to see if the security item is supported in the specified client
        /// </summary>
        /// <param name="securityItem"></param>
        /// <param name="client"></param>
        /// <returns></returns>
        public static Boolean DoesSecurityItemSupportClient(eSecurityItem securityItem, string client)
        {
            eClientBits clientBit;
          
            if (!Enum.TryParse<eClientBits>(client, true, out clientBit))
                return (false);

            var item = Helpers.Application.Instance.SecurityItems.Values.Where(o => o.SecurityIndex == (Int32)securityItem).FirstOrDefault();
            if (item == null)
                return (false);

            if (!item.RestrictByClientBits.HasValue || item.RestrictByClientBits.Value == 0)
                return (true);

            if ((item.RestrictByClientBits.Value & (Int32)clientBit) == (Int32)clientBit)
                return (true);

            return (false);
        }
        #endregion

        #region "FUNCTION: GetUSStateWithSalesTaxRatesSelectionList"
        /// <summary>
        /// Get the US State selection list
        /// </summary>
        /// <returns></returns>
        public static async Task<SelectList> GetUSStateWithSalesTaxRatesSelectionList(Helpers.Security.AccessToken token)
        {
            List<SelectListItem> stateList = new List<SelectListItem>();

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewSalesTaxRatesReport))
            {
                using (var api = new WebAPI(existingToken: token))
                {
                    var states = await api.GetUSStatesWithSalesTaxRates();
                    foreach (var state in states)
                    {
                        stateList.Add(new SelectListItem() { Value = state.StateAbbreviation, Text = state.StateName });
                    }
                }
            }

            return new SelectList(stateList, "Value", "Text");
        }
        #endregion

        #region "FUNCTION: GetPeriodSelectList"
        /// <summary>
        /// Get the period selection list
        /// </summary>
        /// <param name="includeCurrentPeriod"></param>
        /// <returns></returns>
        public static SelectList GetPeriodSelectList(String culture, bool includeCurrentPeriod = true, Int32 months = 36, DateTime? dateFrom = null)
        {
            List<SelectListItem> period = new List<SelectListItem>();
            DateTime fromDate;
            DateTime toDate;

            //-- FORMAT THE CULTURE --
            switch (culture.ToLower().Substring(0,2))
            {
                case "es":
                    culture = "es";
                    break;

                case "pt":
                    culture = "pt";
                    break;

                default:
                    culture = "en";
                    break;
            }

            toDate = DateTime.ParseExact((includeCurrentPeriod ? DateTime.Now : DateTime.Now.AddMonths(-1)).ToString("yyyyMM") + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);

            if (dateFrom.HasValue)
                fromDate = dateFrom.Value;
            else
                fromDate = toDate.AddMonths(months * -1);

            while (toDate >= fromDate)
            {
                period.Add(new SelectListItem() { Value = toDate.ToString("yyyyMM"), Text = toDate.ToString("MMMM yyyy", System.Globalization.CultureInfo.CreateSpecificCulture(culture)) });
                toDate = toDate.AddMonths(-1);
            }

            return new SelectList(period, "Value", "Text");
        }
        #endregion

        #region "FUNCTION: GetWeekSelectionList"
        /// <summary>
        /// Get the weekly selection list
        /// </summary>
        /// <param name="includeCurrentPeriod"></param>
        /// <returns></returns>
        public static SelectList GetWeekSelectionList(String culture)
        {
            List<SelectListItem> period = new List<SelectListItem>();
            DateTime curDate;
            int delta;
            DateTime curMonday;
            int curWeek;

            //-- FORMAT THE CULTURE --
            switch (culture.ToLower().Substring(0, 2))
            {
                case "es":
                    culture = "es";
                    break;

                case "pt":
                    culture = "pt";
                    break;

                default:
                    culture = "en";
                    break;
            }

            //-- GET THE FIRST MONDAY OF THE CURRENT WEEK --
            delta = DayOfWeek.Monday - DateTime.Now.DayOfWeek;
            if (delta > 0)
                delta -= 7;
            curMonday = DateTime.Now.AddDays(delta);
            curDate = curMonday;

            //-- ADD THE WEEKS --
            for (Int32 i = 1; i <= 53; i++)
            {
                curDate = curDate.AddDays(-7);
                curWeek = System.Globalization.CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(curDate, System.Globalization.CalendarWeekRule.FirstDay, DayOfWeek.Monday);

                period.Add(new SelectListItem()
                {
                    Value = String.Format("{0}{1}", curDate.ToString("MM/dd/yyyy"), curDate.AddDays(6).ToString("MM/dd/yyyy")),
                    Text = String.Format("{0} {1} {2} ({3} - {4})",
                                         curDate.Year,
                                         LocalResources.HCResources.Week,
                                         curWeek,
                                         curDate.ToString("MM/dd/yyyy"),
                                         curDate.AddDays(6).ToString("MM/dd/yyyy"))
                });

            }

            return new SelectList(period, "Value", "Text");
        }
        #endregion


        #region "FUNCTION: GetWeekendingSelectionList"
        /// <summary>
        /// Get the weekly ending selection list
        /// </summary>
        public static SelectList GetWeekendingSelectionList(String culture)
        {
            List<SelectListItem> period = new List<SelectListItem>();
            DateTime curDate;
           

            //-- FORMAT THE CULTURE --
            switch (culture.ToLower().Substring(0, 2))
            {
                case "es":
                    culture = "es";
                    break;

                case "pt":
                    culture = "pt";
                    break;

                default:
                    culture = "en";
                    break;
            }

            //-- GET THE PREV SATURDAY
            curDate = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek).AddSeconds(-1);            

            //-- ADD THE WEEKS --
            for (Int32 i = 1; i <= 31; i++)
            {
                period.Add(new SelectListItem()
                {
                    Value = String.Format("{0}", curDate.ToString(new CultureInfo("en-US"))),
                    Text = String.Format("{0}", curDate.ToString(CultureInfo.GetCultureInfo(culture)))
                });

                curDate = curDate.AddDays(-7);
            }

            return new SelectList(period, "Value", "Text");
        }
        #endregion

        #region "FUNCTION: GetGenericSelectList"
        /// <summary>
        /// Get the generic selection list
        /// </summary>
        /// <returns></returns>
        public static SelectList GetGenericSelectList()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Value = "", Text = "" });
            return new SelectList(items, "Value", "Text");
        }
        #endregion

        #region "FUNCTION: GetColombiaPaymentAccountSelections"
        /// <summary>
        /// Get the generic selection list
        /// </summary>
        /// <returns></returns>
        public static SelectList GetColombiaPaymentAccountSelections()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Value = "", Text = "" });
            items.Add(new SelectListItem() { Text = "Efecty", Value = "Efecty" });
            items.Add(new SelectListItem() { Text = "Daviplata", Value = "Daviplata" });

            if (Helpers.Application.Instance.AllowBankAccountsInColombia)
            {
                items.Add(new SelectListItem() { Text = LocalResources.HCResources.BankAccountSavings, Value = "ACHS" });
                items.Add(new SelectListItem() { Text = LocalResources.HCResources.BankAccountChecking, Value = "ACHC" });
            }

            return new SelectList(items, "Value", "Text");
        }
        #endregion

        #region "FUNCTION: GetMonthSelectList"
        /// <summary>
        /// Get the month selection list
        /// </summary>
        /// <returns></returns>
        public static SelectList GetMonthSelectList(bool includeEmpty = true)
        {
            List<SelectListItem> period = new List<SelectListItem>();

            if (includeEmpty)
                period.Add(new SelectListItem() { Value = "", Text = "" });

            period.Add(new SelectListItem() { Value = "01", Text = LocalResources.HCResources.January });
            period.Add(new SelectListItem() { Value = "02", Text = LocalResources.HCResources.February });
            period.Add(new SelectListItem() { Value = "03", Text = LocalResources.HCResources.March });
            period.Add(new SelectListItem() { Value = "04", Text = LocalResources.HCResources.April });
            period.Add(new SelectListItem() { Value = "05", Text = LocalResources.HCResources.May });
            period.Add(new SelectListItem() { Value = "06", Text = LocalResources.HCResources.June });
            period.Add(new SelectListItem() { Value = "07", Text = LocalResources.HCResources.July });
            period.Add(new SelectListItem() { Value = "08", Text = LocalResources.HCResources.August });
            period.Add(new SelectListItem() { Value = "09", Text = LocalResources.HCResources.September });
            period.Add(new SelectListItem() { Value = "10", Text = LocalResources.HCResources.October });
            period.Add(new SelectListItem() { Value = "11", Text = LocalResources.HCResources.November });
            period.Add(new SelectListItem() { Value = "12", Text = LocalResources.HCResources.December });

            return new SelectList(period, "Value", "Text");
        }
        #endregion
        
        #region "FUNCTION: GetNumericSelectList"
        /// <summary>
        /// Get a numeric select list
        /// </summary>
        /// <returns></returns>
        public static SelectList GetNumericSelectList(Int32 min, Int32 max, Int32? padto = null, bool includeEmpty = true, bool reverseOrder = false)
        {
            List<SelectListItem> period = new List<SelectListItem>();

            if (includeEmpty)
                period.Add(new SelectListItem() { Value = "", Text = "" });

            if (reverseOrder)
            {
                for (var i = max; i >= min; i--)
                {
                    if (padto != null)
                        period.Add(new SelectListItem() { Value = i.ToString().PadLeft(padto.Value, '0'), Text = i.ToString() });
                    else
                        period.Add(new SelectListItem() { Value = i.ToString(), Text = i.ToString() });
                }
            }
            else
            {
                for (var i = min; i <= max; i++)
                {
                    if (padto != null)
                        period.Add(new SelectListItem() { Value = i.ToString().PadLeft(padto.Value, '0'), Text = i.ToString() });
                    else
                        period.Add(new SelectListItem() { Value = i.ToString(), Text = i.ToString() });
                }
            }

            return new SelectList(period, "Value", "Text");
        }
        #endregion

        #region "FUNCTION: GetSecurityItemClients"
        /// <summary>
        /// Get the list of clients enabled for the specified security item
        /// </summary>
        /// <returns></returns>
        public static SelectList GetSecurityItemClients(eSecurityItem securityItem, bool includeBlankOption = true)
        {
            List<SelectListItem> clients = new List<SelectListItem>();
            Helpers.Security.AccessToken token = GetCurrentEntityToken();
            String clientDescription;

            foreach (var client in token.Clients)
            {
                if (DoesSecurityItemSupportClient(securityItem, client))
                {                    
                    //-- INITIALIZE THE CLIENT DESCRIPTION FROM THE RESOURCE FILE --
                    try
                    {
                        clientDescription = LocalResources.HCResources.ResourceManager.GetString("Client" + client.ToUpper());
                    }
                    catch (Exception)
                    {
                        clientDescription = null;
                    }
                    if (String.IsNullOrWhiteSpace(clientDescription))
                        clientDescription = client.ToUpper();

                    if (client == Helpers.CookieHelper.DefaultClient)
                        clients.Insert(0, new SelectListItem() { Value = client, Text = clientDescription, Selected = client == token.DefaultClient });
                    else
                        clients.Add(new SelectListItem() { Value = client, Text = clientDescription, Selected = client == token.DefaultClient });
                }
            }

            if (includeBlankOption)
                clients.Insert(0, new SelectListItem() { Value = "", Text = "" });

            return new SelectList(clients, "Value", "Text");
        }
        #endregion

        #region "FUNCTION: GetSecurityItemClientString"
        /// <summary>
        /// Get the list of clients enabled for the specified security item
        /// </summary>
        /// <returns></returns>
        public static String GetSecurityItemClientString(eSecurityItem securityItem)
        {
            Helpers.Security.AccessToken token = GetCurrentEntityToken();
            String clientDescription;
            String clientString = "";

            foreach (var client in token.Clients)
            {
                if (DoesSecurityItemSupportClient(securityItem, client))
                {
                    //-- INITIALIZE THE CLIENT DESCRIPTION FROM THE RESOURCE FILE --
                    try
                    {
                        clientDescription = LocalResources.HCResources.ResourceManager.GetString("Client" + client.ToUpper());
                    }
                    catch (Exception)
                    {
                        clientDescription = null;
                    }
                    if (String.IsNullOrWhiteSpace(clientDescription))
                        clientDescription = client.ToUpper();

                    if (client == Helpers.CookieHelper.DefaultClient)
                        clientString = String.Format("{0}^{1}", client, clientDescription) + (clientString == "" ? "" : "|" + clientString);
                    else
                        clientString = (clientString == "" ? "" : clientString + "|") + String.Format("{0}^{1}", client, clientDescription);
                }
            }

            return (clientString);
        }
        #endregion

        #region "FUNCTION: FormatCurrency"
        /// <summary>
        /// Format the currency value
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="currency"></param>
        /// <returns></returns>
        public static string FormatCurrency(Decimal amount, string client, Boolean showCurrencyCode = false, Boolean colorize = false)
        {
            string amtColor = "";
            string amtDisplay = "";
            string curCulture = "";
            string curCurrency;

            Models.ClientInfo clientInfo;

            //-- GET THE CLIENT INFO --
            clientInfo = GetClientInfo(client);
            curCurrency = clientInfo.Currency.ToUpper();

            //-- CHECK FOR COLOR FORMATTING --
            if ((colorize) && (amount != 0))
            {
                if (amount < 0)
                    amtColor = "color:#990000;";
                else
                    amtColor = "color:#009900;";
            }

            //-- CREATE THE OPENING SPAN TAG --
            amtDisplay = "<span data-currency=\"NAT\" style=\"" + amtColor + "\">";

            switch (curCurrency)
            {
                case "CAD":
                    curCulture = "en-CA";
                    break;

                case "COP":
                    curCulture = "es-CO";
                    break;

                case "BRL":
                    curCulture = "pt-BR";
                    curCurrency = "";
                    break;

                case "MXN":
                    curCulture = "es-MX";
                    break;

                case "CLP":
                    curCulture = "es-CL";
                    break;

                case "ARS":
                    curCulture = "es-AR";
                    break;

                case "DOP":
                    curCulture = "es-DO";
                    curCurrency = "";
                    break;

                case "PEN":
                    curCulture = "es-PE";
                    curCurrency = "";
                    break;

                case "PHP":
                    curCulture = "en-PT";
                    curCurrency = "";
                    break;

                case "USD":
                    curCulture = "en-US";
                    break;

                default:
                    curCulture = "en-US";
                    break;
            }

            amtDisplay += amount.ToString("C2", new System.Globalization.CultureInfo(curCulture));
            if (showCurrencyCode && !String.IsNullOrWhiteSpace(curCurrency))
                amtDisplay += "&nbsp;<span class=\"currencycode\">" + curCurrency + "</span>";

            amtDisplay += "</span>";

            return (amtDisplay);
        }
        #endregion

        #region "FUNCTION: UpdateSecurityBit"
        /// <summary>
        /// Set/unset the specified security bit
        /// </summary>
        /// <param name="ss"></param>
        /// <param name="securityIndex"></param>
        /// <param name="type"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        public static String UpdateSecurityBit(String securityString, Int32 securityIndex, Char status)
        {
            System.Text.StringBuilder sb;

            try
            {
                //-- CHECK TO SEE IF THE USER HAS ACCESS --
                if (securityIndex > (securityString.Length - 1))
                    return securityString;
                else if ((securityString.Substring(securityIndex, 1) == status.ToString()))
                    return securityString;
                else
                {
                    sb = new StringBuilder(securityString);
                    sb[securityIndex] = status;
                    return sb.ToString();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.Write(ex.ToString());
            }

            return securityString;
        }
        #endregion

        #region "FUNCTION: GetCashReceiptsOrderByList"
        /// <summary>
        /// Get the cash receipts order by string for the report select list
        /// </summary>
        /// <returns></returns>
        public static string GetCashReceiptsOrderByList()
        {
            return "LOAN^" + LocalResources.HCResources.CustomerNo + "|" +
                    "PAYDATE^" + LocalResources.HCResources.PaymentDate + "|" +
                    "FIRST_NAME^" + LocalResources.HCResources.FirstName + "|" +
                    "LAST_NAME^" + LocalResources.HCResources.LastName + "|" +
                    "AMOUNT^" + LocalResources.HCResources.PaymentAmount;
        }
        #endregion

        #region "FUNCTION: GetRegions"
        /// <summary>
        /// Return regions.
        /// </summary>
        /// <returns></returns>
        public static async Task<SelectList> GetRegions(Helpers.Security.AccessToken token)
        {

            List<SelectListItem> regions = new List<SelectListItem>();

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.mnuJDSuccessReport))
            {
                using (var api = new WebAPI(existingToken: token))
                {
                    var jdRegions = await api.GetJDRegions();
                    foreach (var region in jdRegions)
                    {
                        regions.Add(new SelectListItem() { Value = region.Region, Text = region.Description });
                    }

                }
            }

            return new SelectList(regions, "Value", "Text");

        }    
        

        #endregion

        #region "FUNCTION: GetJDReportOrderBy"
        /// <summary>
        /// Return regions.
        /// </summary>
        /// <returns></returns>
        public static string GetJDReportOrderByList()
        {
            return "SPONSOR^Sponsor|NETWORK^Territory";
        }

        #endregion
    }
}
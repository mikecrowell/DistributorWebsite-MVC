using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Serilog;
using System.Text.RegularExpressions;
using DistributorWebsite.MVC.WebUI.Models;

namespace DistributorWebsite.MVC.WebUI.Helpers
{
    public static class CookieHelper
    {
        private const String COOKIEPREFIX = "HCEDISTWEB-";
        public const String ACCESSTOKENEXPIRATIONKEY = COOKIEPREFIX + "ACTEXP";
        public const String ACCESSTOKENKEY = COOKIEPREFIX + "ACT";
        public const String REFRESHTOKENKEY = COOKIEPREFIX + "RFT";

        private const String DATEFMTREGEX = @"^((MM[\/\.\- ]{1}dd[\/\.\- ]{1}yyyy)|(yyyy[\/\.\- ]{1}MM[\/\.\- ]{1}dd))$";
        private const String DATEFMTDEFAULT = @"MM/dd/yyyy";
        private const String COOKIEDATEFORMATSTRING = "yyyy-MM-dd HH:mm:ss.fffff";

        #region "PROPERTY: AccessToken"
        /// <summary>
        /// Get/set the user's current language
        /// </summary>
        public static String AccessToken
        {
            get
            {
                return (GetCookieObject("ACT", ""));
            }
            set
            {
                SetCookie("ACT", value, false, 0);
            }
        }
        #endregion

        #region "PROPERTY: AccessTokenType"
        /// <summary>
        /// Get/set the user's current language
        /// </summary>
        public static eAccessTokenType AccessTokenType
        {
            get
            {
                eAccessTokenType type;
                Int32 value = 0;

                if (!Int32.TryParse(GetCookieObject("ACTT", "0"), out value))
                    value = 0;

                if (!Enum.IsDefined(typeof(eAccessTokenType), value))
                    type = eAccessTokenType.Undefined;
                else
                    type = (eAccessTokenType)value;

                return (type);
            }
            set
            {
                SetCookie("ACTT", ((Int32)value).ToString(), true, 0);
            }
        }
        #endregion

        #region "PROPERTY: AccessTokenExpiration"
        /// <summary>
        /// Get/set the access token expiration date/time
        /// </summary>
        public static DateTime? AccessTokenExpiration
        {
            get
            {
                string value;
                DateTime dateValue;
                DateTime? retVal = null;

                value = GetCookieObject("ACTEXP", "");
                if (DateTime.TryParseExact(value, "yyyy-MM-dd HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeLocal, out dateValue))
                    retVal = dateValue;

                return (retVal);
            }
            set
            {
                SetCookie("ACTEXP", value.Value.ToString("yyyy-MM-dd HH:mm:ss"), true, 0);
            }
        }
        #endregion

        #region "PROPERTY: RefreshToken"
        /// <summary>
        /// Get/set the user's current language
        /// </summary>
        public static String RefreshToken
        {
            get
            {
                return (GetCookieObject("RFT", ""));
            }
            set
            {
                SetCookie("RFT", value, true, 0);
            }
        }
        #endregion

        #region "PROPERTY: ShowLicenseError"
        /// <summary>
        /// Get/set whether or not to return the license error details to the client
        /// </summary>
        public static bool ShowLicenseError
        {
            get
            {
                return (Boolean.Parse(GetCookieObject("SLE", "false")));
            }
            set
            {
                SetCookie("SLE", value.ToString(), true, 0);
            }
        }
        #endregion

        #region "PROPERTY: LastLicenseError"
        /// <summary>
        /// Get/set the last license error
        /// </summary>
        public static string LastLicenseError
        {
            get
            {
                return (GetCookieObject("SLETEXT", ""));
            }
            set
            {
                SetCookie("SLETEXT", value.ToString(), true, 0, 5);
            }
        }
        #endregion

        #region "PROPERTY: UserAlerts"
        /// <summary>
        /// Get/set the user alert cookie
        /// </summary>
        public static String UserAlerts
        {
            get
            {
                return (GetCookieObject("ALERTS", null));
            }
            set
            {
                SetCookie("ALERTS", value, false, 0, Helpers.Application.Instance.UserAlertRefreshMinutes);
            }
        }
        #endregion

        #region "PROPERTY: TotalRepurchaseLetterAccounts"
        /// <summary>
        /// Get/set the total number of repurchase letter accounts
        /// </summary>
        public static Int32 TotalRepurchaseLetterAccounts
        {
            get { return (Int32.Parse(GetCookieObject("RLT", "0"))); }
            set { SetCookie("RLT", value.ToString(), false); }
        }
        #endregion

        #region "PROPERTY: Total0to30Accounts"
        /// <summary>
        /// Get/set the total number of accounts 0 to 30 days delinquent
        /// </summary>
        public static Int32 Total0to30Accounts
        {
            get { return (Int32.Parse(GetCookieObject("ZTH", "0"))); }
            set { SetCookie("ZTH", value.ToString(), false); }
        }
        #endregion

        #region "PROPERTY: TotalAlerts"
        /// <summary>
        /// Get the total number of alerts
        /// </summary>
        public static Int32 TotalAlerts
        {
            get
            {
                string alertJson;

                //-- GET THE CURRENT ALERT COOKIE VALUE --
                alertJson = UserAlerts;
                if (String.IsNullOrWhiteSpace(alertJson))
                    return (0);

                try
                {
                    //-- PARSE THE USER ALERT JSON STRING --
                    var alerts = Newtonsoft.Json.JsonConvert.DeserializeObject<UserAlertsDTO>(HttpUtility.UrlDecode(alertJson));
                    if (alerts == null || alerts.Total <= 0)
                        return 0;

                    //-- RETURN THE CURRENT NUMBER OF ALERTS --
                    return (alerts.Total);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.ToString());
                }

                return (0);
            }
        }
        #endregion

        #region "PROPERTY: CurrentCulture"
        /// <summary>
        /// Get/set the culture
        /// </summary>
        public static String CurrentCulture
        {
            get
            {
                return (GetCookieObject("culture", ""));
            }
            set
            {
                SetCookie("culture", value, false, 30);
            }
        }
        #endregion

        #region "PROPERTY: CurrentEntity"
        /// <summary>
        /// Get/set the culture
        /// </summary>
        public static String CurrentEntity
        {
            get
            {
                return (GetCookieObject("entitycode", ""));
            }
            set
            {
                SetCookie("entitycode", value, true, 0);
            }
        }
        #endregion

        #region "PROPERTY: CurrentSalespersonCode"
        /// <summary>
        /// Get/set the culture
        /// </summary>
        public static String CurrentSalespersonCode
        {
            get
            {
                return (GetCookieObject("spcode", ""));
            }
            set
            {
                SetCookie("spcode", value, true, 0);
            }
        }
        #endregion

        #region "PROPERTY: DefaultClient"
        /// <summary>
        /// Get/set the default client
        /// </summary>
        public static String DefaultClient
        {
            get
            {
                return (GetCookieObject("defcl", ""));
            }
            set
            {
                SetCookie("defcl", value, true, 0);
            }
        }
        #endregion

        #region "PROPERTY: CurrentEntityName"
        /// <summary>
        /// Get/set the culture
        /// </summary>
        public static String CurrentEntityName
        {
            get
            {
                return (GetCookieObject("entityname", ""));
            }
            set
            {
                SetCookie("entityname", value, true, 0);
            }
        }
        #endregion

        #region "PROPERTY: CurrentUserBeingImpersonated"
        /// <summary>
        /// Get/set the culture
        /// </summary>
        public static String CurrentUserBeingImpersonated
        {
            get
            {
                return (GetCookieObject("uimp", ""));
            }
            set
            {
                SetCookie("uimp", value, true, 0);
            }
        }
        #endregion

        #region "PROPERTY: CurrentEntityBrand"
        /// <summary>
        /// Get/set the culture
        /// </summary>
        public static String CurrentEntityBrand
        {
            get
            {
                return (GetCookieObject("entitybrand", ""));
            }
            set
            {
                SetCookie("entitybrand", value, true, 0);
            }
        }
        #endregion

        #region "PROPERTY: CurrentFullName"
        /// <summary>
        /// Get/set the user's full name
        /// </summary>
        public static String CurrentFullName
        {
            get
            {
                return (GetCookieObject("ufn", ""));
            }
            set
            {
                SetCookie("ufn", value, false, 0);
            }
        }
        #endregion

        #region "PROPERTY: DateFormat"
        /// <summary>
        /// Get/set the user's preferred date format
        /// </summary>
        public static String DateFormat
        {
            get
            {
                String dateFormat;

                try
                {
                    dateFormat = GetCookieObject("userdateformat", "");
                }
                catch (Exception)
                {
                    dateFormat = DATEFMTDEFAULT;
                }

                if (!Regex.IsMatch(dateFormat, DATEFMTREGEX))
                    dateFormat = DATEFMTDEFAULT;

                return (dateFormat);
            }
            set
            {
                if (!Regex.IsMatch(value, DATEFMTREGEX))
                    value = DATEFMTDEFAULT;

                SetCookie("userdateformat", value, false, 300);
            }
        }
        #endregion

        #region "PROPERTY: UseInlineDownload"
        /// <summary>
        /// Get/set whether or not to use inline type for file attachments for the current user/device
        /// </summary>
        public static Boolean? UseInlineDownload
        {
            get
            {
                Boolean? retVal = null;
                Boolean testVal;

                try
                {
                    if (Boolean.TryParse(GetCookieObject("useinlinedl", ""), out testVal))
                        retVal = testVal;
                }
                catch (Exception)
                {
                    retVal = null;
                }

                return (retVal);
            }
            set
            {
                SetCookie("useinlinedl", value.ToString());
            }
        }
        #endregion

        #region "PROPERTY: LastWindowsAuthCheckTime"
        /// <summary>
        /// Get/set the last time the current user's role membership was checked
        /// </summary>
        public static DateTime? LastWindowsAuthCheckTime
        {
            get
            {
                String cookieValue;
                DateTime dtValue;

                cookieValue = GetCookieObject("LWACT", "");
                if (String.IsNullOrWhiteSpace(cookieValue))
                    return null;

                if (!DateTime.TryParseExact(cookieValue, COOKIEDATEFORMATSTRING, System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.AssumeLocal, out dtValue))
                    return null;

                return (dtValue);
            }
            set
            {
                SetShortLivedCookie("LWACT", value.Value.ToString(COOKIEDATEFORMATSTRING), true, Helpers.Application.Instance.WindowsAuthCheckMinutes);
            }
        }
        #endregion

        #region "PROPERTY: UserEntities"
        /// <summary>
        /// Set the user entities retrieved from the identity server userinfo endpoint
        /// </summary>
        public static Helpers.Security.UserInfoClaim UserEntities
        {
            get
            {
                var currentValue = GetCookieObject("uents", "");
                if (String.IsNullOrWhiteSpace(currentValue))
                    return null;

                return Helpers.Security.UserInfoClaim.ParseBase64String(currentValue);
            }
            set
            {
                if (value == null)
                    SetCookie("uents", String.Empty, true, -1);
                else
                    SetCookie("uents", value.ToBase64String(), true, 0);
            }
        }
        #endregion

        #region "PROPERTY: AntiForgeryToken"
        /// <summary>
        /// Get the latest anti forgery token value stored in the cookie
        /// </summary>
        public static String AntiForgeryToken
        {
            get
            {
                String key;
                String retVal = null;

                try
                {
                    if (System.Web.HttpContext.Current != null)
                    {
                        if ((System.Web.HttpContext.Current.Request != null) && (System.Web.HttpContext.Current.Request.Cookies != null))
                        {
                            key = System.Web.HttpContext.Current.Request.Cookies.AllKeys.Where(o => o.StartsWith("__RequestVerificationToken", StringComparison.CurrentCultureIgnoreCase)).First();
                            retVal = System.Web.HttpContext.Current.Request.Cookies[key].Value;
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.Write(ex.ToString());
                }

                if (String.IsNullOrWhiteSpace(retVal))
                    return ("");
                else
                    return retVal;
            }
        }
        #endregion

        #region "PROPERTY: AddRecentUser"
        /// <summary>
        /// Add a new user to the recently used user cookie
        /// </summary>
        /// <param name="entityCode"></param>
        /// <param name="entityName"></param>
        /// <param name="brand"></param>
        public static void AddRecentUser (String entityCode, String entityName, String brand, String userToImpersonate)
        {
            List<Models.RecentUser> userList;
            Models.RecentUser user;

            try
            {
                //-- GET THE RECENTLY USED USERS CURRENT COOKIE VALUE --
                userList = RecentlyUsedUsers;

                //-- UPDATE THE CURRENT VALUE IF IT ALREADY EXISTS --
                user = userList.FirstOrDefault(o => o.EntityCode == entityCode && o.EntityName == entityName && o.UsernameToImpersonate == userToImpersonate);
                if (user != null)
                {
                    //-- UPDATE THE EXISTING RECORD --
                    user.Brand = brand;
                    user.LastLoginDate = DateTime.Now;
                }
                else
                {
                    //-- ADD THE NEW RECORD --
                    userList.Add(new Models.RecentUser() { Brand = brand, EntityCode = entityCode, EntityName = entityName, UsernameToImpersonate = userToImpersonate, LastLoginDate = DateTime.Now });
                }

                //-- SAVE THE UPDATED COOKIE --
                SetCookie("intrecent",
                          Newtonsoft.Json.JsonConvert.SerializeObject(userList.OrderByDescending(o => o.LastLoginDate).Take(5)),
                          true,
                          30);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
            }
        }
        #endregion

        #region "PROPERTY: RecentlyUsedUserList"
        /// <summary>
        /// Get/set the recently used user list
        /// </summary>
        public static List<Models.RecentUser> RecentlyUsedUsers
        {           
            get
            {
                List<Models.RecentUser> userList = null;

                try
                {
                    //-- GET THE CURRENT RECENT USER COOKIE --
                    String recentUserCookie = GetCookieObject("intrecent", "");

                    //-- MAKE SURE WE HAVE AT LEAST ONE USERS --
                    if (String.IsNullOrWhiteSpace(recentUserCookie))
                        return new List<Models.RecentUser>();

                    //-- PARSE THE RECENTLY USED USER LIST INTO AN ARRAY --
                    userList = Newtonsoft.Json.JsonConvert.DeserializeObject<List<Models.RecentUser>>(recentUserCookie);
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.ToString());
                    userList = null;
                }

                //-- RETURN AN EMPTY LIST IF NO LIST EXISTS --
                if (userList == null)
                    return new List<Models.RecentUser>();

                //-- RETURN THE SORTED LIST --
                return userList.OrderByDescending(o => o.LastLoginDate).ToList();
            }
        }
        #endregion

        #region "PROPERTY: HasPossibleLeads"
        /// <summary>
        /// Get/set whether or not to display alert for possible leads
        /// </summary>
        public static Boolean HasPossibleLeads
        {
            get
            {
                Boolean retVal = false;
                Boolean testVal;

                try
                {
                    if (Boolean.TryParse(GetCookieObject("hasleads", ""), out testVal))
                        retVal = testVal;
                }
                catch (Exception)
                {
                    retVal = false;
                }

                return (retVal);
            }
            set
            {
                SetCookie("hasleads", value.ToString());
            }
        }
        #endregion

        #region "FUNCTION: HasDisplayedPopupsForEntity"
        /// <summary>
        /// Check to see whether or not popups were already displayed for the current entity during the current login session
        /// </summary>
        /// <param name="entityCode"></param>
        /// <returns></returns>
        public static Boolean HasDisplayedPopupsForEntity(String entityCode)
        {
            Boolean retVal = false;
            String key = String.Format("{0}", entityCode.ToUpper());

            try
            {
                String cookie = GetCookieObject("POPUPSBYENTITY");
                retVal = (!String.IsNullOrWhiteSpace(cookie) && (cookie.Contains("[" + key + "]")));
            }
            catch (Exception ex)
            {
                retVal = false;
                Logger.LogError(ex.Message);
            }

            return (retVal);
        }
        #endregion

        #region "FUNCTION: SetPopupTracker"
        /// <summary>
        /// Set the popup tracking cookie
        /// </summary>
        /// <param name="entityCode"></param>
        public static void SetPopupTracker(String entityCode)
        {
            String key = String.Format("{0}", entityCode.ToUpper());
            String cookie = GetCookieObject("POPUPSBYENTITY");

            if (!String.IsNullOrWhiteSpace(cookie))
            {
                if (!cookie.Contains("[" + key + "]"))
                    cookie += "[" + key + "]";
            }
            else
                cookie = "[" + key + "]";

            SetCookie("POPUPSBYENTITY", cookie, true, 0);
        }
        #endregion

        #region "PROCEDURE: ClearPopupTracker"
        /// <summary>
        /// Clear the popup tracker cookie
        /// </summary>
        public static void ClearPopupTracker()
        {
            SetCookie("POPUPSBYENTITY", "", true, 0);
            SetCookie("POPUPIDS", "", false, 0);
            SetCookie("ALERTS", "", false, -1);
        }
        #endregion

        #region "FUNCTION: SetCookie"
        /// <summary>
        /// Create a cookie on the user's computer
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="httpOnly"></param>
        /// <param name="numberOfDaysToKeep"></param>
        private static void SetCookie(String key, String value, Boolean httpOnly = true, Int32 numberOfDaysToKeep = 7, Int32? numberOfMinutesToKeep = null)
        {
            if (HttpContext.Current != null && HttpContext.Current.Response != null && HttpContext.Current.Response.Cookies != null)
            {
                key = COOKIEPREFIX + key;
                HttpCookie newCookie = new HttpCookie(key);
                newCookie.HttpOnly = httpOnly;
                newCookie.Secure = HttpContext.Current.Request.IsSecureConnection;
                if (numberOfDaysToKeep != 0)
                    newCookie.Expires = DateTime.Now.AddDays(numberOfDaysToKeep);
                else if (numberOfMinutesToKeep.HasValue && numberOfMinutesToKeep.Value > 0)
                    newCookie.Expires = DateTime.Now.AddMinutes(numberOfMinutesToKeep.Value);

                newCookie.Value = value;
                HttpContext.Current.Response.Cookies.Add(newCookie);
            }
            else
            {
                Logger.LogInformation("SetCookie: NO HTTP CONTEXT AVAILABLE");
            }
        }
        #endregion

        #region "FUNCTION: SetShortLivedCookie"
        /// <summary>
        /// Create a cookie on the user's computer
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="httpOnly"></param>
        /// <param name="numberOfDaysToKeep"></param>
        private static void SetShortLivedCookie(String key, String value, Boolean httpOnly = true, Int32 numberOfMinutesToKeep = 30)
        {
            key = COOKIEPREFIX + key;
            HttpCookie newCookie = new HttpCookie(key);
            newCookie.HttpOnly = httpOnly;
            newCookie.Secure = HttpContext.Current.Request.IsSecureConnection;
            if (numberOfMinutesToKeep != 0)
                newCookie.Expires = DateTime.Now.AddMinutes(numberOfMinutesToKeep);
            newCookie.Value = value;
            HttpContext.Current.Response.Cookies.Add(newCookie);
        }
        #endregion

        #region "FUNCTION: GetCookieObject"
        /// <summary>
        /// Retrieve an object from the current session
        /// </summary>
        /// <param name="key"></param>
        /// <param name="?"></param>
        /// <returns></returns>
        private static String GetCookieObject(String key, String defaultValue = "")
        {
            String retVal = defaultValue;

            try
            {
                key = COOKIEPREFIX + key;

                if (System.Web.HttpContext.Current != null)
                {
                    if ((System.Web.HttpContext.Current.Request != null) && (System.Web.HttpContext.Current.Request.Cookies != null))
                    {
                        if (System.Web.HttpContext.Current.Request.Cookies[key] != null)
                        {
                            if (!String.IsNullOrWhiteSpace(System.Web.HttpContext.Current.Request.Cookies[key].Value.ToString()))
                                retVal = System.Web.HttpContext.Current.Request.Cookies[key].Value.ToString();
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                retVal = defaultValue;
            }

            return (retVal);
        }
        #endregion
    }
}
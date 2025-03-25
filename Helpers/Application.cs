using DistributorWebsite.MVC.WebUI.Helpers.Configuration;
using DistributorWebsite.MVC.WebUI.Models;
using HyCite.Utility.DomainHelper;
using Serilog.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributorWebsite.MVC.WebUI.Helpers
{
    /// <summary>
    /// Singleton class for global objects used throughout the entier IIS app domain for this project
    /// </summary>
    public sealed class Application
    {
        public struct WebAPIS
        {
            public string DistributorWebAPI { get; set; }
        }

        /// <summary>
        /// Static instance of this class to implement the Singleton pattern
        /// </summary>
        private static volatile Application m_instance;

        /// <summary>
        /// Locking object to allow this to be loaded in a multi-threaded environment
        /// </summary>
        private static object syncRoot = new Object();

        #region "PROPERTY: ResetPasswordLink"
        /// <summary>
        /// Reset password link
        /// </summary>
        public string ResetPasswordLink { get; private set; }
        #endregion

        #region "PROPERTY: SupportedCultures"
        /// <summary>
        /// Get/set the list of supported cultures
        /// </summary>
        public String[] SupportedCultures
        {
            get;
            private set;
        }
        #endregion

        #region "PROPERTY: InCiteURL"
        /// <summary>
        /// Get/set the InCite URL
        /// </summary>
        public string InCiteURL { get; private set; }
        #endregion

        #region "PROPERTY: RPUniversityURL"
        /// <summary>
        /// Get/set the RP University base URL
        /// </summary>
        public string RPUniversityURL { get; private set; }
        #endregion

        #region "PROPERTY: WindowsDomain"
        /// <summary>
        /// Get/set the current windows domain
        /// </summary>
        public String WindowsDomain
        {
            get;
            private set;
        }
        #endregion

        #region "PROPERTY: ShowErrorDetails"
        /// <summary>
        /// Get/set whether or not to return detailed errors to the client
        /// </summary>
        public bool ShowErrorDetails { get; private set; }
        #endregion

        #region "PROPERTY: IsRunningLocal"
        /// <summary>
        /// Get/set whether or not the website is running locally
        /// </summary>
        public bool IsRunningLocal { get; private set; }
        #endregion

        #region "PROPERTY: PaymentReceiptEmailOverride"
        /// <summary>
        /// Get/set the payment receipt email override
        /// </summary>
        public string PaymentReceiptEmailOverride { get; private set; }
        #endregion

        #region "PROPERTY: RequestDocuCiteIdentityScopes"
        /// <summary>
        /// Whether or not to request additonal scopes to allow user to interact with remote docucite API in Azure
        /// </summary>
        public bool RequestDocuCiteIdentityScopes { get; private set; }
        #endregion

        #region "PROPERTY: WebAPIS"
        /// <summary>
        /// Get/set the list of web APIS
        /// </summary>
        public WebAPIS APIS { get; private set; }
        #endregion

        #region "PROPERTY: LogToTrace"
        /// <summary>
        /// Whether or not to log messages to the trace output
        /// </summary>
        public Boolean LogToTrace { get; private set; }
        #endregion

        #region "PROPERTY: AllowBankAccountsInColombia"
        /// <summary>
        /// Allow bank accounts in Colombia
        /// </summary>
        public bool AllowBankAccountsInColombia { get; private set; }
        #endregion

        #region "PROPERTY: LogToTraceMinimumLevel"
        /// <summary>
        /// Minimum level to write the trace information to the log
        /// </summary>
        public Serilog.Events.LogEventLevel LogToTraceMinimumLevel { get; private set; }
        #endregion

        #region "PROPERTY: LogLevel"
        /// <summary>
        /// Minimum level to be logged
        /// </summary>
        public Serilog.Core.LoggingLevelSwitch LogLevel { get; private set; }
        #endregion

        #region "PROPERTY: IdentityServer"
        /// <summary>
        /// Get/set the identity server configuration information
        /// </summary>
        public IdentityServerConfig IdentityServer
        {
            get;
            private set;
        }
        #endregion

        #region "PROPERTY: IdentityServerInternal"
        /// <summary>
        /// Get/set the internal identity server for local logins to the distributor website
        /// </summary>
        public IdentityServerConfig IdentityServerInternal
        {
            get;
            private set;
        }
        #endregion

        #region "PROPERTY: IdentityServerInternalEnt"
        /// <summary>
        /// Get/set the internal identity server for local logins to the distributor website
        /// </summary>
        public IdentityServerConfig IdentityServerInternalEnt
        {
            get;
            private set;
        }
        #endregion

        #region "PROPERTY: IdentityScopes"
        /// <summary>
        /// Get the list of scopes to request from identity server
        /// </summary>
        public string IdentityScopes
        {
            get
            {
                if (this.RequestDocuCiteIdentityScopes)
                    return "openid profile roles offline_access hcdistwebuser hcdistwebapi hcmobileapi hcsalespersoncode";
                else
                    return "openid profile roles offline_access hcdistwebuser hcdistwebapi";
            }
        }
        #endregion

        #region "PROPERTY: UserAlertRefreshMinutes"
        /// <summary>
        /// Get/set the number of minute to wait before automatically refreshing the user alert info
        /// </summary>
        public Int32 UserAlertRefreshMinutes { get; private set; }
        #endregion

        #region "PROPERTY: AuthorizationType"
        /// <summary>
        /// Get/set the authorization type
        /// </summary>
        public eWebAuthType AuthorizationType { get; private set; }
        #endregion

        #region "PROPERTY: WindowsAuthCheckMinutes"
        /// <summary>
        /// Get/set the number of mintues before the windows auth token expired
        /// </summary>
        public Int32 WindowsAuthCheckMinutes { get; private set; }
        #endregion

        #region "PROPERTY: WindowsAuthRoles"
        /// <summary>
        /// Get/set the roles that are allowed to use the distributor website internally
        /// </summary>
        public String[] WindowsAuthRoles { get; private set; }
        #endregion

        #region "PROPERTY: InternalAccessTokenExpirationDate"
        /// <summary>
        /// Get/set the internal access token expiration date
        /// </summary>
        public DateTime? InternalAccessTokenExpirationDate { get; private set; }
        #endregion

        #region "PROPERTY: InternalAccessToken"
        /// <summary>
        /// Get/set the internal access token
        /// </summary>
        public String InternalAccessToken { get; private set; }
        #endregion
       
        #region "PROPERTY: NestedSecurityItems"
        public Dictionary<Int32, SecurityItemDTO> SecurityItems { get; private set; }
        #endregion

        #region "PROPERTY: CaptchaPublicKey"
        /// <summary>
        /// Get/set the Google recaptcha Site key
        /// </summary>
        public string CaptchaPublicKey { get; private set; }
        #endregion

        #region "PROPERTY: ACHCutoffTime"
        /// <summary>
        /// Get/set the ACH Cutoff Time
        /// </summary>
        public DateTime ACHCutoffTime { get; private set; }
        #endregion

        #region "PROPERTY: AppInsightsKey"
        /// <summary>
        /// Get/set the application insights key
        /// </summary>
        public string AppInsightsKey { get; private set; }
        #endregion

        public bool PCIEnabled { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        private Application()
        {
        }

        /// <summary>
        /// Singleton initializer/reference
        /// </summary>
        public static Application Instance
        {
            get
            {
                if (m_instance == null)
                {
                    lock (syncRoot)
                    {
                        if (m_instance == null)
                            m_instance = new Application();
                    }
                }

                return (m_instance);
            }
        }


        #region "PROCEDURE: Initialize"
        /// <summary>
        /// Initialize the properties in the singleton application object
        /// </summary>
        public void Initialize()
        {
            Boolean bValue;
            DateTime dtValue;            
            String appConfigType;
            Serilog.Events.LogEventLevel logLevel;
            eWebAuthType authType;

            try
            {
                lock (syncRoot)
                {
                    //-- INITIALIZE THE DOMAIN NAME --
                    this.WindowsDomain = HyCite.Utility.DomainHelper.DomainUtils.GetWindowsDomainName();

                    //-- ACH CUTOFF TIME --
                    if (!DateTime.TryParseExact(System.Configuration.ConfigurationManager.AppSettings["ACHCutoffTime"], "HH:mm", System.Globalization.CultureInfo.InvariantCulture, System.Globalization.DateTimeStyles.None, out dtValue))
                        dtValue = DateTime.ParseExact("16:00", "HH:mm", System.Globalization.CultureInfo.InvariantCulture);
                    this.ACHCutoffTime = dtValue;

                    //-- SET THE LIST OF SUPPORTED CULTURES --
                    this.SupportedCultures = System.Configuration.ConfigurationManager.AppSettings["SupportedCultures"].Split(',');

                    //-- SET THE IsRunningLocally FLAG --
                    this.IsRunningLocal = Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings["IsRunningLocal"]);

                    //-- SHOW ERROR DETAILS --
                    this.ShowErrorDetails = Boolean.Parse(System.Configuration.ConfigurationManager.AppSettings["ShowErrorDetails"]);

                    //-- USER ALERT REFRESH MINUTES --
                    this.UserAlertRefreshMinutes = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["UserAlertRefreshMinutes"]);

                    //-- INITIALIZE THE WEB API URLS --
                    InitializeWebAPIS();

                    //-- INITIALIZE THE IDENTITY SERVER CONNECTION --
                    InitializeIdentityServer();

                    //-- ALLOW BANK ACCOUNTS IN COLOMBIA --
                    if (!Boolean.TryParse(System.Configuration.ConfigurationManager.AppSettings["CO.AllowBankAccounts"], out bValue))
                    {
                        if (!Boolean.TryParse(System.Configuration.ConfigurationManager.AppSettings[$"CO.AllowBankAccounts.{this.WindowsDomain.ToUpper()}"], out bValue))
                            bValue = false;
                    }
                    this.AllowBankAccountsInColombia = bValue;

                    //-- INITIALIZE THE INCITE URL --
                    InCiteURL = System.Configuration.ConfigurationManager.AppSettings["INCITE.URL"];
                    if (String.IsNullOrWhiteSpace(InCiteURL))
                        InCiteURL = System.Configuration.ConfigurationManager.AppSettings[$"INCITE.URL.{this.WindowsDomain.ToUpper()}"];

                    //-- INITIALIZE THE AUTHORIZATION TYPE --
                    if (!Enum.TryParse<eWebAuthType>(System.Configuration.ConfigurationManager.AppSettings["AuthType"], out authType))
                        authType = eWebAuthType.IDENTITYSERVER;
                    this.AuthorizationType = authType;

                    if (this.AuthorizationType != eWebAuthType.WINDOWS)
                    {
                        //-- NOT USING WINDOWS AUTHENTICATION --
                        this.WindowsAuthCheckMinutes = 0;
                        this.WindowsAuthRoles = Array.Empty<String>();
                        this.InternalAccessToken = null;
                        this.InternalAccessTokenExpirationDate = null;
                        this.IdentityServerInternal = null;
                        this.IdentityServerInternalEnt = null;
                    }
                    else
                    {
                        //-- SET THE WINDOWS AUTH CONFIGURATION VALUES --
                        this.WindowsAuthCheckMinutes = Int32.Parse(System.Configuration.ConfigurationManager.AppSettings["WindowsAuth.CheckMinutes"]);
                        if (this.WindowsAuthCheckMinutes <= 0)
                            this.WindowsAuthCheckMinutes = 30;
                        this.WindowsAuthRoles = System.Configuration.ConfigurationManager.AppSettings[$"WindowsAuth.AllowedRoles.{this.WindowsDomain}"].Split(',');

                        ////-- INITIALIZE THE INTERNAL IDENTITY SERVER --
                        //this.InitializeIdentityServerInternal();

                        //-- INITIALIZE THE INTERNAL ENTITY ACCESS IDENTITY SERVER --
                        this.InitializeIdentityServerInternalEnt();
                    }

                    //-- INITIALIZE THE INTERNAL IDENTITY SERVER --
                    this.InitializeIdentityServerInternal();

                    //-- INITIALIZE THE GOOGLE CAPTCHA API --
                    this.InitializeCaptcha();

                    //-- INITIALIZE APPLICATION INSIGHTS --
                    this.AppInsightsKey = System.Configuration.ConfigurationManager.AppSettings["AppInsights"];
                    if (String.IsNullOrWhiteSpace(this.AppInsightsKey))
                    {
                        this.AppInsightsKey = System.Configuration.ConfigurationManager.AppSettings[$"AppInsights.{this.WindowsDomain}"];
                    }
                    Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration.Active.InstrumentationKey = this.AppInsightsKey;

                    //-- PASSWORD RESET LINK --
                    this.ResetPasswordLink = System.Configuration.ConfigurationManager.AppSettings[$"ResetPasswordLink.{this.WindowsDomain}"];
                    if (String.IsNullOrWhiteSpace(this.ResetPasswordLink))
                        this.ResetPasswordLink = System.Configuration.ConfigurationManager.AppSettings[$"ResetPasswordLink"];

                    //-- PAYMENT RECEIPT EMAIL OVERRIDE --
                    appConfigType = (this.IsRunningLocal ? "DEBUG" : this.WindowsDomain);
                    this.PaymentReceiptEmailOverride = System.Configuration.ConfigurationManager.AppSettings[$"PaymentProcessing.ReceiptEmailOverride.{appConfigType}"] ?? "";
                    if (String.IsNullOrWhiteSpace(this.PaymentReceiptEmailOverride))
                        this.PaymentReceiptEmailOverride = System.Configuration.ConfigurationManager.AppSettings[$"PaymentProcessing.ReceiptEmailOverride"];

                    //-- RP UNIVERSITY URL --
                    this.RPUniversityURL = System.Configuration.ConfigurationManager.AppSettings[$"LINK.RPUNIVERSITY.{this.WindowsDomain}"];
                    if (String.IsNullOrWhiteSpace(this.RPUniversityURL))
                        this.RPUniversityURL = System.Configuration.ConfigurationManager.AppSettings[$"LINK.RPUNIVERSITY"];

                    //-- INITIALIZE THE LOGGING --
                    appConfigType = (this.IsRunningLocal ? "DEBUG" : this.WindowsDomain);
                    if (!Enum.TryParse<Serilog.Events.LogEventLevel>(System.Configuration.ConfigurationManager.AppSettings[String.Format("LOGGER.{0}.Level", appConfigType)], out logLevel))
                        logLevel = Serilog.Events.LogEventLevel.Information;
                    this.LogLevel = new Serilog.Core.LoggingLevelSwitch(logLevel);

                    //-- LOG TO TRACE CONFIGURATION --
                    if (!Boolean.TryParse(System.Configuration.ConfigurationManager.AppSettings[String.Format("LOGGER.{0}.WriteToTrace", appConfigType)], out bValue))
                        bValue = false;
                    this.LogToTrace = bValue;

                    if (!Enum.TryParse<Serilog.Events.LogEventLevel>(System.Configuration.ConfigurationManager.AppSettings[String.Format("LOGGER.{0}.WriteToTraceLevel", appConfigType)], out logLevel))
                        logLevel = this.LogLevel.MinimumLevel;
                    this.LogToTraceMinimumLevel = logLevel;

                    //-- REQUEST DOCUCITE IDENTITY SCOPES --
                    if (!Boolean.TryParse(System.Configuration.ConfigurationManager.AppSettings["RequestDocuCiteIdentityScopes"], out bValue))
                    {
                        if (!Boolean.TryParse(System.Configuration.ConfigurationManager.AppSettings[$"RequestDocuCiteIdentityScopes-{this.WindowsDomain.ToUpper()}"], out bValue))
                        {
                            bValue = false;
                        }
                    }
                    this.RequestDocuCiteIdentityScopes = bValue;

                    if (!Boolean.TryParse(System.Configuration.ConfigurationManager.AppSettings["PCIEnabled"], out bValue))
                        bValue = false;

                    this.PCIEnabled = bValue;

                    //-- CONFIGURE THE LOGGING --
                    LoggerConfig.RegisterLogger();

                    //-- LOG THE CURRENT APPLICATION SETTINGS --
                    Logger.LogInformation(this.ToString());
                }

                //-- GET THE CORE ACCESS TOKEN --
                //if (this.AuthorizationType == eWebAuthType.WINDOWS)
                //{
                this.InternalAccessToken = "";
                this.InternalAccessTokenExpirationDate = DateTime.MinValue;
                System.Threading.Tasks.Task.Run(() => Helpers.EndpointAndTokenHelper.CheckAndPossiblyRefreshToken(eAccessTokenType.InternalCore)).GetAwaiter().GetResult();
                //}

                //-- INITIALIZE THE SECURITY ITEM MENU --
                if (!System.Threading.Tasks.Task.Run(() => Helpers.Navigation.RefreshSecurityItemsFromAPI()).GetAwaiter().GetResult())
                {
                    throw new System.Exception("FAILED TO LOAD SECURITY ITEMS FROM IDENTITY SERVER DATABASE");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
                throw new System.Exception("FAILED TO INITIALIZE APPLICATION:" + Environment.NewLine + ex.ToString() + this.ToString());
            }
        }
        #endregion

        #region "PROCEDURE: InitializeSecurityItemMenu"
        /// <summary>
        /// Initialize the security items menu from the API
        /// </summary>
        public bool RefreshSecurityItems(Dictionary<Int32, SecurityItemDTO> securityItemList)
        {
            Boolean retVal = true;

            try
            {
                lock (syncRoot)
                {
                    this.SecurityItems = securityItemList;
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                retVal = false;
            }

            return (retVal);
        }
        #endregion

        #region "PROCEDURE: InitializeIdentityServer"
        /// <summary>
        /// Initialize the identity server information
        /// </summary>
        private void InitializeIdentityServer()
        {
            String configType;
            IdentityServerConfig config;

            //-- GET THE CONFIGURATION TYPE --
            configType = System.Configuration.ConfigurationManager.AppSettings["WHICHIDENTITYSERVER"];
            if (String.IsNullOrWhiteSpace(configType))
                throw new ArgumentNullException("WHICHIDENTITYSERVER");

            //-- MAKE SURE THE CONFIG TYPE IS VALID --
            if (!"|DEV|TEST|PROD|DEBUG|DOMAIN|".Contains(String.Format("|{0}|", configType.ToUpper())))
                throw new ArgumentException(String.Format("{0} is an invalid WHICHIDENTITYSERVER value", configType), "WHICHIDENTITYSERVER");

            //-- GET THE KEY TO SEARCH FOR --
            if (configType.ToUpper() == "DOMAIN")
            {
                configType = WindowsDomain;
            }
            else
            {
                configType = configType.ToUpper();
            }

            //-- BUILD THE CONFIGURATION --
            config = new IdentityServerConfig();

            System.Diagnostics.Trace.TraceInformation($"InitializeIdentityServer: configType = {configType} DOMAIN = {WindowsDomain}");

            config.ClientID = GetConfigValue($"IDENTITYSERVER-{configType}.ClientID");
            config.Authority = GetConfigValue($"IDENTITYSERVER-{configType}.Authority");
            config.RedirectUri = GetConfigValue($"IDENTITYSERVER-{configType}.RedirectURI");
            config.PostLogoutRedirectUri = GetConfigValue($"IDENTITYSERVER-{configType}.PostLogoutRedirectURI");
            config.Secret = GetConfigValue($"IDENTITYSERVER-{configType}.Secret");

            this.IdentityServer = config;
        }
        #endregion

        #region "PROCEDURE: UpdateCoreAccessToken"
        /// <summary>
        /// Update the core access token information
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="expirationDate"></param>
        public void UpdateCoreAccessToken(String accessToken, DateTime expirationDate)
        {
            if ((this.InternalAccessToken != accessToken || this.InternalAccessTokenExpirationDate != expirationDate))
            {
                lock (syncRoot)
                {
                    //-- UPDATE THE NEW ACCESS TOKEN VALUES --
                    this.InternalAccessToken = accessToken;
                    this.InternalAccessTokenExpirationDate = expirationDate;
                }
            }
        }
        #endregion

        #region "PROCEDURE: InitializeIdentityServerInternalEnt"
        /// <summary>
        /// Initialize the identity server information for internal entities
        /// </summary>
        private void InitializeIdentityServerInternalEnt()
        {
            String configType;
            IdentityServerConfig config;

            //-- GET THE CONFIGURATION TYPE --
            configType = System.Configuration.ConfigurationManager.AppSettings["WHICHIDENTITYSERVER"];
            if (String.IsNullOrWhiteSpace(configType))
                throw new ArgumentNullException("WHICHIDENTITYSERVER");

            //-- MAKE SURE THE CONFIG TYPE IS VALID --
            if (!"|DEV|TEST|PROD|DEBUG|DOMAIN|".Contains(String.Format("|{0}|", configType.ToUpper())))
                throw new ArgumentException(String.Format("{0} is an invalid WHICHIDENTITYSERVER value", configType), "WHICHIDENTITYSERVER");

            //-- GET THE KEY TO SEARCH FOR --
            if (configType.ToUpper() == "DOMAIN")
            {
                configType = WindowsDomain;
            }
            else
            {
                configType = configType.ToUpper();
            }

            //-- BUILD THE CONFIGURATION --
            config = new IdentityServerConfig();

            config.ClientID = DomainUtils.GetDomainSpecificConfigValue<String>("IDENTITYSERVERINTENT-{0}.ClientID", "", configType);
            config.Authority = DomainUtils.GetDomainSpecificConfigValue<String>("IDENTITYSERVERINTENT-{0}.Authority", "", configType);
            config.Secret = DomainUtils.GetDomainSpecificConfigValue<String>("IDENTITYSERVERINTENT-{0}.Secret", "", configType);
            config.ROP = DomainUtils.GetDomainSpecificConfigValue<String>("IDENTITYSERVERINTENT-{0}.ROP", "", configType);

            this.IdentityServerInternalEnt = config;
        }
        #endregion

        #region "PROCEDURE: InitializeIdentityServerInternal"
        /// <summary>
        /// Initialize the identity server information
        /// </summary>
        private void InitializeIdentityServerInternal()
        {
            String configType;
            IdentityServerConfig config;

            //-- GET THE CONFIGURATION TYPE --
            configType = System.Configuration.ConfigurationManager.AppSettings["WHICHIDENTITYSERVER"];
            if (String.IsNullOrWhiteSpace(configType))
                throw new ArgumentNullException("WHICHIDENTITYSERVER");

            //-- MAKE SURE THE CONFIG TYPE IS VALID --
            if (!"|DEV|TEST|PROD|DEBUG|DOMAIN|".Contains(String.Format("|{0}|", configType.ToUpper())))
                throw new ArgumentException(String.Format("{0} is an invalid WHICHIDENTITYSERVER value", configType), "WHICHIDENTITYSERVER");

            //-- GET THE KEY TO SEARCH FOR --
            if (configType.ToUpper() == "DOMAIN")
            {
                configType = WindowsDomain;
            }
            else
            {
                configType = configType.ToUpper();
            }

            //-- BUILD THE CONFIGURATION --
            config = new IdentityServerConfig();

            config.ClientID = DomainUtils.GetDomainSpecificConfigValue<String>("IDENTITYSERVERINT-{0}.ClientID", "", configType);
            config.Authority = DomainUtils.GetDomainSpecificConfigValue<String>("IDENTITYSERVERINT-{0}.Authority", "", configType);
            config.Secret = DomainUtils.GetDomainSpecificConfigValue<String>("IDENTITYSERVERINT-{0}.Secret", "", configType);

            this.IdentityServerInternal = config;
        }
        #endregion

        #region "PROCEDURE: InitializeWebAPIS"
        /// <summary>
        /// Initialize the Web API URL's from the config file
        /// </summary>
        private void InitializeWebAPIS()
        {
            WebAPIS apiInfo = new WebAPIS();

            //-- PAYMENT PROCESSING API --
            apiInfo.DistributorWebAPI = GetAPIConfigValue("DistributorWeb");

            this.APIS = apiInfo;
        }
        #endregion

        #region "PROCEDURE: InitializeCaptcha"
        /// <summary>
        /// Initialize the Googla captcha keys
        /// </summary>
        private void InitializeCaptcha()
        {
            String version;

            //-- GET THE VERSION TO USE --
            version = System.Configuration.ConfigurationManager.AppSettings["CAPTCHA.Version"];
            this.CaptchaPublicKey = System.Configuration.ConfigurationManager.AppSettings[String.Format("CAPTCHA.{0}.Public", version)];

            if (String.IsNullOrWhiteSpace(this.CaptchaPublicKey))
                throw new ArgumentException("Unable to parse CAPTCHA. information from web.config file");
        }
        #endregion

        #region "FUNCTION: GetAPIConfigValue"
        /// <summary>
        /// Get the API Configuration file value
        /// </summary>
        /// <param name="apiConfigName"></param>
        /// <returns></returns>
        private String GetAPIConfigValue(string apiConfigName)
        {
            string configType;
            string retVal;

            if (!this.IsRunningLocal)
                configType = "DOMAIN";
            else
            {
                configType = System.Configuration.ConfigurationManager.AppSettings[String.Format("WEBAPI.{0}.CONFIGTYPE", apiConfigName)];
                if (String.IsNullOrWhiteSpace(configType) || (configType.ToUpper() != "DOMAIN" && configType.ToUpper() != "DEBUG"))
                    configType = "DOMAIN";
            }

            if (configType == "DOMAIN")
                configType = this.WindowsDomain.ToUpper();

            //-- TRY TO GET THE INDIVIDUAL CONFIG VALUE --
            retVal = System.Configuration.ConfigurationManager.AppSettings[String.Format("WEBAPI.{0}-{1}", apiConfigName, configType)];
            if (String.IsNullOrWhiteSpace(retVal))
                retVal = System.Configuration.ConfigurationManager.AppSettings[String.Format("WEBAPI.{0}", apiConfigName)];

            return (retVal);
        }
        #endregion

        #region "FUNCTION: GetConfigValue"
        /// <summary>
        /// Get the config value
        /// </summary>
        /// <param name="key"></param>
        /// <param name="configType"></param>
        /// <returns></returns>
        private string GetConfigValue(String key)
        {
            string retVal;

            try
            {
                System.Diagnostics.Trace.TraceInformation($"GETTING CONFIG VALUE: {key}");
                retVal = System.Configuration.ConfigurationManager.AppSettings[key];
                if (String.IsNullOrWhiteSpace(retVal))
                    throw new Exception($"could not find config setting for key {key}");
                else
                    System.Diagnostics.Trace.TraceInformation($"RETRIEVED VALUE {retVal}");
            }
            catch (Exception)
            {
                throw;
            }

            return (retVal);
        }
        #endregion

        #region "FUNCTION: ToString"
        /// <summary>
        /// Create a string representation of the current application settings
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Format("==========================================" + Environment.NewLine) +
                   String.Format("APPLICATION SETTINGS" + Environment.NewLine) +
                   String.Format("==========================================" + Environment.NewLine) +
                   String.Format("WindowsDomain: {0}" + Environment.NewLine, this.WindowsDomain) +
                   String.Format("IsRunningLocally: {0}" + Environment.NewLine, this.IsRunningLocal) +
                   String.Format("LogToTrace: {0}" + Environment.NewLine, this.LogToTrace) +
                   String.Format("LogToTraceMinimumLevel: {0}" + Environment.NewLine, this.LogToTraceMinimumLevel) +
                   String.Format("LogLevel: {0}" + Environment.NewLine, this.LogLevel) +
                   String.Format("IdentityServer: {0}" + Environment.NewLine, this.IdentityServer == null ? "" : this.IdentityServer.Authority) +
                   String.Format("RequestDocuCiteScopes: {0}" + Environment.NewLine, this.RequestDocuCiteIdentityScopes) +
                   String.Format("WEBAP: DistWeb: {0}" + Environment.NewLine, this.APIS.DistributorWebAPI);
        }
        #endregion
    }
}
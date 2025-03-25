using DistributorWebsite.MVC.WebUI.Helpers.Models;
using DistributorWebsite.MVC.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DistributorWebsite.MVC.WebUI.Controllers
{
    [HCAuthorize]
    public class ProfileController : BaseWorkflowController
    {
        #region "CONTROLLER: Users"
        /// <summary>
        /// Get the user management form
        /// </summary>
        /// <returns></returns>
        // GET: Profile/Users
        [HCAuthorizeMenu(eSecurityItem.mnuManageUsers)]
        public ActionResult Users()
        {
            //-- SET THE WEB ADMIN API URL --
            ViewBag.WEBAPIURL = Helpers.Application.Instance.APIS.DistributorWebAPI + "Users";

            //-- SET THE USER SPECIFIC API URL --
            ViewBag.DETAILURL = "Users(" + this.CurrentUser.UserID.ToString() + ")/Default.DistributorSalespersonUser(userid={UserID},spcode='{SalespersonCode}')";

            //-- CREATE THE LANGUAGE PREFERENCE LIST BOX --
            ViewBag.SELECTLIST_LanguagePrefs = GetLanguagePreferences();

            //-- SET THE USERNAME TYPE (SALESPERSONCODE or EMAILADDRESS) --
            ViewBag.CONFIG_USERNAME_TYPE = "SALESPERSONCODE";
            
            return View();
        }
        #endregion

        #region "CONTROLLER: GET: AdminUsers"
        /// <summary>
        /// Get the list of admin users
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuChildUsers)]
        public ActionResult AdminUsers()
        {
            //-- SET THE WEB ADMIN API URL --
            ViewBag.WEBAPIURL = Helpers.Application.Instance.APIS.DistributorWebAPI + "AdminUsers";

            //-- SET THE CONFIGURATION VALUES --
            ViewBag.CONFIG_USERNAME_TYPE = "EMAIL";

            //-- CREATE THE LANGUAGE PREFERENCE LIST BOX --
            ViewBag.SELECTLIST_LanguagePrefs = GetLanguagePreferences();

            //-- SET THE CURRENT USER'S SALESPERSON CODE --
            ViewBag.SALESPERSONCODE = this.CurrentUser.SalespersonCode;

            return View();
        }
        #endregion

        #region "CONTROLLER: UserProfile"
        /// <summary>
        /// Get the user profile form
        /// </summary>
        /// <returns></returns>
        // GET: Profile/Users
        [HCAuthorizeMenu(eSecurityItem.mnuViewProfile)]
        public ActionResult UserProfile()
        {
            //-- SET THE WEB ADMIN API URL --
            ViewBag.WEBAPIURL = Helpers.Application.Instance.APIS.DistributorWebAPI + "Users";

            //-- SET THE ENTITY PATH --
            ViewBag.ENTITYPATH = "(" + this.CurrentUser.UserID.ToString() + ")/Default.Profile";

            //-- SET THE UPDATE API PATH --
            ViewBag.UPDATEAPI = Helpers.Application.Instance.APIS.DistributorWebAPI + "Users(" + this.CurrentUser.UserID.ToString() + ")/Default.UpdateProfile";

            //-- CREATE THE LANGUAGE PREFERENCE LIST BOX --
            ViewBag.SELECTLIST_LanguagePrefs = GetLanguagePreferences();

            //-- SET THE USERNAME TYPE (SALESPERSONCODE or EMAILADDRESS) --
            ViewBag.CONFIG_USERNAME_TYPE = "SALESPERSONCODE";

            return View();
        }
        #endregion

        #region "CONTROLLER: DistributorProfile"
        /// <summary>
        /// Get the user profile form
        /// </summary>
        /// <returns></returns>
        // GET: Profile/Users
        [HCAuthorizeMenu(eSecurityItem.mnuViewDistributorProfile)]
        public ActionResult DistributorProfile()
        {
            //-- SET THE WEB ADMIN API URL --
            ViewBag.WEBAPIURL = Helpers.Application.Instance.APIS.DistributorWebAPI + "Distributors";

            return View();
        }
        #endregion

        #region "CONTROLLER: SalespersonProfile"
        /// <summary>
        /// Get the user profile form
        /// </summary>
        /// <returns></returns>
        // GET: Profile/SalespersonProfile
        [HCAuthorizeMenu(eSecurityItem.mnuSalespersonProfile)]
        public ActionResult SalespersonProfile()
        {
            //-- SET THE WEB ADMIN API URL --
            ViewBag.WEBAPIURL = Helpers.Application.Instance.APIS.DistributorWebAPI + "Salesperson";

            //-- SET WHETHER OR NOT THE USER CAN EDIT THE PROFILE INFORMATION --
            ViewBag.CANEDITPROFILE = !this.CurrentUser.IsDistributor && Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opUpdateSalespersonProfile);
            ViewBag.ALLOWPROFILEEDIT = this.CurrentUser.IsInternal;
            ViewBag.CANVIEWPROFILE = !this.CurrentUser.IsDistributor;
            ViewBag.SPPROFILECANCELBUTTONTEXT = LocalResources.HCResources.Cancel;
            ViewBag.CanViewColombiaRUT = this.EntityAccessToken.IsInternal && Helpers.Application.Instance.AuthorizationType == eWebAuthType.WINDOWS;
            ViewBag.CanViewColombiaBANKCERT = this.EntityAccessToken.IsInternal && Helpers.Application.Instance.AuthorizationType == eWebAuthType.WINDOWS;
            ViewBag.CanViewImmigrationPermit = ViewBag.CanViewColombiaRUT;

            return View();
        }
        #endregion

        #region "CONTROLLER: Telemarketers"
        /// <summary>
        /// Get/set the telemarketer info form
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuTelemarketers)]
        public async Task<ActionResult> Telemarketers()
        {
            List<SelectListItem> enableOptionsNotVerified = new List<SelectListItem>();
            List<SelectListItem> enableOptionsVerified = new List<SelectListItem>();
            List<SelectListItem> languagePreferences = new List<SelectListItem>();
            List<SelectListItem> countries = new List<SelectListItem>();

            TelemarketerSetupInfoDTO settings;

            //-- GET THE FORM SETTINGS --
            using (var api = new Helpers.WebAPI(language: this.CurrentLanguage, existingToken: this.CurrentUser))
            {
                settings = await api.GetTelemarketerSettings(this.CurrentUser.DistributorNo);
            }

            ViewBag.DEFAULTCOUNTRY = settings.CountryCode;
            ViewBag.DEFAULTCLIENT = settings.Client;

            //-- INITIALIZE THE LANGUAGE PREFERENCES --
            languagePreferences = new List<SelectListItem>();
            languagePreferences.Add(new SelectListItem() { Value = "", Text = "" });
            foreach (var pref in settings.LanguagePreferences)
            {
                languagePreferences.Add(new SelectListItem() { Value = pref.Value, Text = pref.Text });
            }
            ViewBag.LANGUAGEPREFERENCES = new SelectList(languagePreferences, "Value", "Text");

            //-- INITIALIZE THE COUNTRIES --
            countries = new List<SelectListItem>();
            countries.Add(new SelectListItem() { Value = "", Text = "" });
            foreach (var country in settings.Countries)
            {
                countries.Add(new SelectListItem() { Value = country.Value, Text = country.Text });
            }
            ViewBag.COUNTRIES = new SelectList(countries, "Value", "Text");

            //-- INITIALIZE THE TIME ZONES --
            ViewBag.TIMEZONES = Helpers.Common.GetTimeZoneSelectList(true);

            //-- INITIALIZE THE URL --
            ViewBag.WEBAPIURL = Helpers.Application.Instance.APIS.DistributorWebAPI + "DistributorTelemarketers";

            //-- BUILD THE ENABLED OPTIONS FOR UN-VERIFIED USERS --
            enableOptionsNotVerified.Add(new SelectListItem() { Value = "", Text = "" });
            enableOptionsNotVerified.Add(new SelectListItem() { Value = "V", Text = LocalResources.HCResources.EnableWhenValidated });
            enableOptionsNotVerified.Add(new SelectListItem() { Value = "N", Text = LocalResources.HCResources.No });

            //-- BUILD THE ENABLED OPTIONS FOR VERFIED USERS --
            enableOptionsVerified.Add(new SelectListItem() { Value = "", Text = "" });
            enableOptionsVerified.Add(new SelectListItem() { Value = "Y", Text = LocalResources.HCResources.Yes });
            enableOptionsVerified.Add(new SelectListItem() { Value = "N", Text = LocalResources.HCResources.No });

            ViewBag.ENABLEOPTIONSUNVALIDATED = new SelectList(enableOptionsNotVerified, "Value", "Text");
            ViewBag.ENABLEOPTIONSVALIDATED = new SelectList(enableOptionsVerified, "Value", "Text");

            return View();
        }
        #endregion

        #region "CONTROLLER: GET: Profile/SalespersonProfile/Documents/WithholdingCertificate"
        /// <summary>
        /// Get the document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns> 
        [Route("Profile/SalespersonProfile/Documents/WithholdingCertificate/{id:int}")]
        [HCAuthorizeMenu(eSecurityItem.mnuSalespersonProfile)]
        public async Task<ActionResult> GetWithholdingCertificateAsync(Int32 id)
        {
            return await GetDocument($"Salesperson/Default.GetWithholdingCertificate(id={id})");
        }
        #endregion

        #region "CONTROLLER: GET: Profile/SalespersonProfile/Documents/ColombiaRUT"
        /// <summary>
        /// Get the document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns> 
        [Route("Profile/SalespersonProfile/Documents/ColombiaRUT")]
        [HCAuthorizeInternalMenu(eSecurityItem.mnuSalespersonProfile)]
        public async Task<ActionResult> GetSalespersonColombiaRUT()
        {
            return await GetDocument($"Salesperson/Default.ColombiaRUT");
        }
        #endregion

        #region "CONTROLLER: GET: Profile/SalespersonProfile/Documents/ColombiaBANKCERT"
        /// <summary>
        /// Get the document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns> 
        [Route("Profile/SalespersonProfile/Documents/ColombiaBANKCERT")]
        [HCAuthorizeInternalMenu(eSecurityItem.mnuSalespersonProfile)]
        public async Task<ActionResult> GetSalespersonColombiaBANKCERT()
        {
            return await GetDocument($"Salesperson/Default.ColombiaBANKCERT");
        }
        #endregion

        #region "CONTROLLER: GET: Profile/SalespersonProfile/Documents/ImmigrationPermit"
        /// <summary>
        /// Get the document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns> 
        [Route("Profile/SalespersonProfile/Documents/ImmigrationPermit")]
        [HCAuthorizeInternalMenu(eSecurityItem.mnuSalespersonProfile)]
        public async Task<ActionResult> GetSalespersonImmigrationPermit()
        {
            return await GetDocument($"Salesperson/Default.ImmigrationPermit");
        }
        #endregion

        #region "CONTROLLER: Messages"
        // GET: /Profile/Messages
        [HCAuthorizeMenu(eSecurityItem.mnuPrivateMessages)]
        public ActionResult Messages()
        {
            //-- SET THE ENTITY PATH --
            ViewBag.ENTITYPATH = $"({this.CurrentUser.UserID})/Default.Messages";

            //-- ONLY EXTERNAL USERS CAN MANAGE MESSAGES --
            ViewBag.CANMANAGEMESSAGES = !this.CurrentUser.IsInternal;

            return View();
        }
        #endregion

        #region "CONTROLLER: PaymentAccounts"
        /// <summary>
        /// Manage distributor payment accounts
        /// </summary>
        /// <returns></returns>
        // GET: Profile/PaymentAccounts
        [HCAuthorizeMenu(eSecurityItem.mnuPaymentAccounts)]
        public ActionResult PaymentAccounts()
        {
            //-- ACTIVE/INACTIVE STATUSES --
            List<SelectListItem> statusList = new List<SelectListItem>();
            statusList.Add(new SelectListItem() { Text = "", Value = "" });
            statusList.Add(new SelectListItem() { Text = LocalResources.HCResources.Active, Value = "Active" });
            statusList.Add(new SelectListItem() { Text = LocalResources.HCResources.Inactive, Value = "Inactive" });
            ViewBag.STATUSLIST = new SelectList(statusList, "Value", "Text");

            //-- INITIALIZE API URLS --
            ViewBag.APINEWCC = $"{Helpers.Application.Instance.APIS.DistributorWebAPI}InitializePaymentAccount";
            ViewBag.APIUPDATECC = $"{Helpers.Application.Instance.APIS.DistributorWebAPI}UpdatePaymentAccount";
            ViewBag.APIDELETECC = $"{Helpers.Application.Instance.APIS.DistributorWebAPI}DeletePaymentAccount";
            ViewBag.APICREATEDC = $"{Helpers.Application.Instance.APIS.DistributorWebAPI}CreateACHAccount";
            ViewBag.APIUPDATEDC = $"{Helpers.Application.Instance.APIS.DistributorWebAPI}UpdateACHAccount";

            //-- SECURITY SETTINGS --
            ViewBag.CanCreateAccounts = ((Environment.UserDomainName == "DEV") || (Helpers.Application.Instance.IsRunningLocal) || (!this.EntityAccessToken.IsInternal));
            ViewBag.CanPurgeAccounts = ((Environment.UserDomainName == "DEV") || (Helpers.Application.Instance.IsRunningLocal) || (!this.EntityAccessToken.IsInternal));

            //-- GET THE INITIAL CLIENT TO FILTER THE ACCOUNTS BY --
            ViewBag.FILTERCLIENT = Helpers.Common.GetSecurityItemClients(eSecurityItem.mnuPaymentAccounts, false).First().Value;

            return View();
        }
        #endregion

        #region "CONTROLLER: GET: Profile/DistributorProfile/Documents/WithholdingCertificate"
        /// <summary>
        /// Get the document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns> 
        [Route("Profile/DistributorProfile/Documents/WithholdingCertificate/{id:int}")]
        [HCAuthorizeMenu(eSecurityItem.mnuViewDistributorProfile)]
        public async Task<ActionResult> GetDistributorWithholdingCertificateAsync(Int32 id)
        {
            return await GetDocument($"Distributors/Default.GetWithholdingCertificate(id={id})");
        }
        #endregion

        #region "HELPER PROCEDURES AND FUNCTIONS"

        #region "PROCEDURE: GetLanguagePreferences"
        /// <summary>
        /// Get the list of available language preferences
        /// </summary>
        /// <returns></returns>
        public static SelectList GetLanguagePreferences()
        {
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem() { Value = "EN", Text = "English" });
            items.Add(new SelectListItem() { Value = "ES", Text = "Español" });
            items.Add(new SelectListItem() { Value = "PT", Text = "Português" });

            return new SelectList(items, "Value", "Text");
        }
        #endregion

        #endregion
    }
}
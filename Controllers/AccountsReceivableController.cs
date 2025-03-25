using DistributorWebsite.MVC.WebUI.Helpers.Models;
using DistributorWebsite.MVC.WebUI.Models;
using DistributorWebsite.MVC.WebUI.Models.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DistributorWebsite.MVC.WebUI.Controllers
{
    [HCAuthorize]
    public class AccountsReceivableController : BaseController
    {
        #region "CONTROLLER: Customers"
        /// <summary>
        /// Get the territory purchases
        /// </summary>
        /// <returns></returns>
        // GET: Purchases/Territory
        [HCAuthorizeMenu(eSecurityItem.mnuAccountSearch)]
        public ActionResult Customers(string customerno = "", string client = "", string defaultCategory = "")
        {
            SelectList accountSearchTypes;
            String defaultValue;
            String apiController;
            String category;

            //-- GET THE ACCOUNT SEARCH TYPES --
            accountSearchTypes = GetAccountSearchTypes();
            if (accountSearchTypes == null || accountSearchTypes.Count() <= 0)
            {
                return new RedirectToRouteResult(new RouteValueDictionary
                                                 {
                                                    { "action", "Forbidden" },
                                                    { "controller", "Error" }
                                                 });
            }

            ViewBag.ACCOUNTSEARCHTYPES = accountSearchTypes;

            defaultValue = accountSearchTypes.First().Value;
            if (!String.IsNullOrWhiteSpace(defaultValue))
            {
                apiController = "Customers";
                category = defaultValue;

                ViewBag.DEFAULTSEARCHCAT = category;
                ViewBag.DEFAULTSEARCHTYPE = String.Format("Default.{0}(cat='{1}')", apiController, category);
            }
            else
            {
                ViewBag.DEFAULTSEARCHCAT = "AllCustomers";
                ViewBag.DEFAULTSEARCHTYPE = "Default.Customers(cat='AllCustomers')";
            }
            ViewBag.INITIALCUSTOMERNO = customerno;
            ViewBag.INITIALCUSTOMERCLIENT = client;
            ViewBag.INITIALSORT = "";

            //-- USE THE DEFAULT VALUE IF ONE WAS PROVIDED --
            if (!String.IsNullOrWhiteSpace(defaultCategory) && accountSearchTypes.Any(o => o.Value == defaultCategory))
            {
                ViewBag.DEFAULTSEARCHCAT = defaultCategory;
                ViewBag.DEFAULTSEARCHTYPE = $"Default.Customers(cat='{defaultCategory}')";
                ViewBag.INITIALSORT = "CurrentBalance desc";
                ViewBag.INITIALCUSTOMERNO = "";
                ViewBag.INITIALCUSTOMERCLIENT = "";
            }

            ViewBag.ACCOUNTTYPE = "";
            ViewBag.CANAPPLYPAYMENTS = this.CanApplyPayments;
            ViewBag.CANSENDPAYMENTLINKS = this.CanSendPaymentLinks;

            return View("Customers");
        }
        #endregion

        #region "CONTROLLER: RepurchaseLetters"
        /// <summary>
        /// Get the repurchase letter customers
        /// </summary>
        /// <returns></returns>
        // GET: Purchases/Territory
        [HCAuthorizeMenu(eSecurityItem.opViewRepurchaseLetterAccounts)]
        public ActionResult RepurchaseLetters()
        {
            return Customers(defaultCategory: "RepurchaseLetter");
        }
        #endregion

        #region "CONTROLLER: Delinquent0to30"
        /// <summary>
        /// Get the repurchase letter customers
        /// </summary>
        /// <returns></returns>
        // GET: Purchases/Territory
        [HCAuthorizeMenu(eSecurityItem.opViewDelinquent0to30)]
        public ActionResult Delinquent0to30()
        {
            return Customers(defaultCategory: "Delinquent0to30");
        }
        #endregion

        #region "CONTROLLER: BalanceSummary"
        /// <summary>
        /// Display the balance summary page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuBalanceSummary)]
        public ActionResult BalanceSummary()
        {
            ViewBag.DISTRIBUTORNO = Helpers.CookieHelper.CurrentEntity;
            ViewBag.SHOWSAMPLE = Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewSampleBalanceDetails);
            return View();
        }
        #endregion

        #region "CONTROLLER: CollectorMessages"
        /// <summary>
        /// Display the balance summary page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuCollectorMessages)]
        public ActionResult CollectorMessages()
        {
            return View();
        }
        #endregion

        #region "CONTROLLER: ListPayments"
        /// <summary>
        /// Display the list payments page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuListPayments)]
        public ActionResult ListPayments()
        {
            //-- HANDLE THE STATUS MESSAGE (NOT APPLICABLE FOR LIST PAYMENTS) --
            ViewBag.ACHCUTOFFNOTE = GetACHMessage();

            //-- INITIALIZE THE CREDIT CARD MODEL --
            Helpers.Common.InitializePaymentFromModel(ePaymentPageHost.ListPayments, this, false, securityItem: eSecurityItem.mnuListPayments);

            return View();
        }
        #endregion

        #region "CONTROLLER: ReturnsEOriginals"
        /// <summary>
        /// Display the Returned E-Originals page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuReturnsEOriginals)]
        public ActionResult ReturnsEOriginals()
        {
            
            return View();
        }
        #endregion

        #region "CONTROLLER: AddonAccountSearch"
        /// <summary>
        /// Display the addon account search page
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [HCAuthorizeMenu(eSecurityItem.mnuAddonAccountSearch)]
        public ActionResult AddonCustomers()
        {
            ViewBag.ACCOUNTTYPE = "ADDON";
            ViewBag.CANAPPLYPAYMENTS = false;
            ViewBag.CANSENDPAYMENTLINKS = false;
            return View();
        }
        #endregion

        #region "CONTROLLER: CustomerPayments"
        /// <summary>
        /// Get the customer payments
        /// </summary>
        /// <returns></returns>
        // GET: AccountsReceivable/CustomerPayments
        [HCAuthorizeMenu(eSecurityItem.mnuCustomerPayments)]
        public ActionResult CustomerPayments(string id = "", string client = "")
        {
            bool canApplyPayments = this.CanApplyPayments;
            bool canSendPaymentLinks = this.CanSendPaymentLinks;

            //-- PAYMENT ACCOUNT TYPE --
            List<SelectListItem> paymentAccountTypes = new List<SelectListItem>();
            paymentAccountTypes.Add(new SelectListItem() { Text = "", Value = "" });
            paymentAccountTypes.Add(new SelectListItem() { Text = LocalResources.HCResources.PaymentAccountTypeNew, Value = "NEW" });
            paymentAccountTypes.Add(new SelectListItem() { Text = LocalResources.HCResources.PaymentAccountTypeExisting, Value = "ONFILE" });
            ViewBag.PAYMENTACCOUNTTYPE = new SelectList(paymentAccountTypes, "Value", "Text");

            //-- PAYMENT OPTIONS --
            List<SelectListItem> paymentOptions = new List<SelectListItem>();
            paymentOptions.Add(new SelectListItem() { Text = "", Value = "" });
            if (canApplyPayments) paymentOptions.Add(new SelectListItem() { Text = LocalResources.HCResources.ApplyPayment, Value = "APPLY" });
            if (canSendPaymentLinks)
            {
                paymentOptions.Add(new SelectListItem() { Text = LocalResources.HCResources.EmailPaymentLink, Value = "EMAIL" });
                paymentOptions.Add(new SelectListItem() { Text = LocalResources.HCResources.GeneratePaymentLink, Value = "GENERATE" });
            }
            ViewBag.PAYMENTOPTIONS = new SelectList(paymentOptions, "Value", "Text");

            //-- INITIALIZE API URLS --
            ViewBag.APINEWCC = $"{Helpers.Application.Instance.APIS.DistributorWebAPI}InitializeCustomerPayment";
            ViewBag.APISUBMITPAYMENT = $"{Helpers.Application.Instance.APIS.DistributorWebAPI}SubmitCustomerPayment";

            //-- DEFAULT CUSTOMER AND CLIENT --
            ViewBag.LOADCUSTOMER = id;
            ViewBag.LOADCLIENT = client;

            //-- SECURITY SETTINGS --
            ViewBag.CanApplyPayments = canApplyPayments;
            ViewBag.CanSendPaymentLinks = canSendPaymentLinks;

            return View();
        }
        #endregion

        #region "HELPER FUNCTIONS"    

        #region "FUNCTION: GetAccountSearchTypes"
        /// <summary>
        /// Get the list of account search types allowed for the current user
        /// </summary>
        /// <returns></returns>
        private SelectList GetAccountSearchTypes()
        {
            List<SelectListItem> types = new List<SelectListItem>();
            var token = Helpers.Common.GetCurrentEntityToken();
            var searchOptions = new Dictionary<eSecurityItem, Tuple<String, String>>();

            if (token != null)
            {
                //-- BUILD THE LIST OF SECURITY OPTIONS TO BE CHECKED --
                searchOptions.Add(eSecurityItem.opViewAllAccounts, new Tuple<string, string>("AllAccounts", LocalResources.HCResources.AccountSearchAllCustomers));
                searchOptions.Add(eSecurityItem.opViewZeroPayAccounts, new Tuple<string, string>("ZeroPay", LocalResources.HCResources.AccountSearchZeroPay));
                searchOptions.Add(eSecurityItem.opViewAccountsWithBalances, new Tuple<string, string>("CarryingABalance", LocalResources.HCResources.AccountSearchCarryingABalance));
                searchOptions.Add(eSecurityItem.opViewRepurchaseLetterAccounts, new Tuple<string, string>("RepurchaseLetter", LocalResources.HCResources.AccountSearchRepurchaseLetter));
                searchOptions.Add(eSecurityItem.opViewPaidInFullAccounts, new Tuple<string, string>("PaidInFull", LocalResources.HCResources.AccountSearchPaidInFull));
                searchOptions.Add(eSecurityItem.opViewRepurchasedAccounts, new Tuple<string, string>("Repurchased", LocalResources.HCResources.AccountSearchRepurchased));
                searchOptions.Add(eSecurityItem.opViewInactiveDistributorAddons, new Tuple<string, string>("AddonCustomers", LocalResources.HCResources.AccountSearchInactive));
                searchOptions.Add(eSecurityItem.opViewDelinquent0to30, new Tuple<string, string>("Delinquent0to30", LocalResources.HCResources.AccountSearchDelinquent0to30));
                searchOptions.Add(eSecurityItem.opViewDelinquent31to60, new Tuple<string, string>("Delinquent31to60", LocalResources.HCResources.AccountSearchDelinquent31to60));
                searchOptions.Add(eSecurityItem.opViewDelinquent61to90, new Tuple<string, string>("Delinquent61to90", LocalResources.HCResources.AccountSearchDelinquent61to90));
                searchOptions.Add(eSecurityItem.opViewDelinquentOver90, new Tuple<string, string>("DelinquentOver90", LocalResources.HCResources.AccountSearchDelinquentOver90));

                //-- BUILD THE SELECT LIST FOR ACCESSIBLE ITEMS --
                foreach (eSecurityItem sec in searchOptions.Keys)
                {
                    if (Helpers.Common.CanUserAccessSecurityItem(sec, token))
                    {
                        types.Add(new SelectListItem() { Value = searchOptions[sec].Item1, Text = searchOptions[sec].Item2 });
                    }
                }
            }

            return new SelectList(types, "Value", "Text");
        }
        #endregion

        #region "FUNCTION: GetACHMessage"
        /// <summary>
        /// Get the ACH cutoff message string
        /// </summary>
        /// <returns></returns>
        private String GetACHMessage()
        {
            TimeZone tz = TimeZone.CurrentTimeZone;
            String cutoffString = String.Format("{0} {1} ({2} UTC)",
                                                Helpers.Application.Instance.ACHCutoffTime.ToString("h:mm tt"),
                                                (tz.IsDaylightSavingTime(DateTime.Now) ? "CDT" : "CST"),
                                                tz.GetUtcOffset(DateTime.Now).Hours);

            return String.Format(LocalResources.HCResources.ACHCutoffNote, cutoffString);
        }
        #endregion

        #region "PROPERTY: CanApplyPayments"
        /// <summary>
        /// Determine whether or not the current user can apply payments for customers
        /// </summary>
        private bool CanApplyPayments
        {
            get
            {
                return false;

                //if (!Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.mnuCustomerPayments))
                //    return false;
                //else if (Helpers.Application.Instance.IsRunningLocal)
                //    return true;
                //else if (Environment.UserDomainName.ToUpper() == "DEV")
                //    return true;
                //else if (Helpers.Application.Instance.AuthorizationType == eWebAuthType.IDENTITYSERVER)
                //    return true;
                //else
                //    return false;
            }
        }
        #endregion

        #region "PROPERTY: CanSendPaymentLinks"
        /// <summary>
        /// Determine whether or not the current user can apply payments for customers
        /// </summary>
        private bool CanSendPaymentLinks
        {
            get
            {
                if (!Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.mnuCustomerPayments))
                    return false;
                else
                    return true;
            }
        }
        #endregion

        #endregion
    }
}
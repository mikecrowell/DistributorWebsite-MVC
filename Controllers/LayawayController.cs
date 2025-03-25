using DistributorWebsite.MVC.WebUI.Helpers.Models;
using DistributorWebsite.MVC.WebUI.Models;
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
    public class LayawayController : BaseWorkflowController
    {
        #region "CONTROLLER: Workflow"

        #region "CONTROLLER: Workflow"
        /// <summary>
        /// Get the customer workflow
        /// </summary>
        /// <returns></returns>
        // GET: Workflow/Customer
        [HCAuthorizeMenu(eSecurityItem.mnuLWWorkflow)]
        public ActionResult Workflow(Int32? orderno = null, String client = null, bool showStatusMessage = false, bool showPayments = false)
        {
            return WorkflowBase("Workflow", eSecurityItem.opManageLayawayWorkflow, eSecurityItem.opMakeLayawayWorkflowPayments, orderno, client, showStatusMessage, showPayments);
        }
        #endregion

        #endregion

        #region "CONTROLLER: AccountSearch"
        /// <summary>
        /// Get the account search
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuLWAccountSearch)]
        public ActionResult AccountSearch(string customerno = "", string client = "")
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
                ViewBag.DEFAULTSEARCHCAT = "DFAllCustomers";
                ViewBag.DEFAULTSEARCHTYPE = "Default.Customers(cat='DFAllCustomers')";
            }

            ViewBag.INITIALCUSTOMERNO = customerno;
            ViewBag.INITIALCUSTOMERCLIENT = client;
            ViewBag.ACCOUNTTYPE = "LW";

            return View();
        }
        #endregion

        #region "CONTROLLER: EligibleAccounts"
        /// <summary>
        /// Get the eligible accounts
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuLWEligibleAccounts)]
        public ActionResult EligibleAccounts()
        {
            return View();
        }
        #endregion

        #region "CONTROLLER: BalanceSummary"
        /// <summary>
        /// Get the balance summary
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuLWBalanceSummary)]
        public ActionResult BalanceSummary()
        {
            ViewBag.DISTRIBUTORNO = Helpers.CookieHelper.CurrentEntity;
            return View();
        }
        #endregion

        #region "CONTROLLER: Reports"
        /// <summary>
        /// Get the reports
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuLWReports)]
        public ActionResult Reports()
        {
            ViewBag.ReportPeriods = Helpers.Common.GetPeriodSelectList(this.CurrentLanguage, true, 36);
            ViewBag.ReportWeeks = new SelectList(new List<SelectListItem>(), "Value", "Text");
            ViewBag.ReportUSStates = new SelectList(new List<SelectListItem>(), "Value", "Text");
            ViewBag.CurrentState = this.CurrentUser.State;
            ViewBag.GenericSelectList = Helpers.Common.GetGenericSelectList();

            return View(GetAccessibleReports());
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
                searchOptions.Add(eSecurityItem.opLWViewAllAccounts, new Tuple<string, string>("LWAllAccounts", LocalResources.HCResources.AccountSearchAllCustomers));
                searchOptions.Add(eSecurityItem.opLWUntilMerchandisePriceIsMet, new Tuple<string, string>("LWMerch", LocalResources.HCResources.AccountSearchLayawayMerch));
                searchOptions.Add(eSecurityItem.opLWUntilRetailPriceIsMet, new Tuple<string, string>("LWRetail", LocalResources.HCResources.AccountSearchLayawayRetail));
                searchOptions.Add(eSecurityItem.opLWViewZeroPayAccounts, new Tuple<string, string>("LWZeroPay", LocalResources.HCResources.AccountSearchZeroPay));
                searchOptions.Add(eSecurityItem.opLWViewDelinquent0to30, new Tuple<string, string>("LWDelinquent0to30", LocalResources.HCResources.AccountSearchDelinquent0to30));
                searchOptions.Add(eSecurityItem.opLWViewDelinquent31to60, new Tuple<string, string>("LWDelinquent31to60", LocalResources.HCResources.AccountSearchDelinquent31to60));
                searchOptions.Add(eSecurityItem.opLWViewDelinquent61to90, new Tuple<string, string>("LWDelinquent61to90", LocalResources.HCResources.AccountSearchDelinquent61to90));
                searchOptions.Add(eSecurityItem.opLWViewDelinquentOver90, new Tuple<string, string>("LWDelinquentOver90", LocalResources.HCResources.AccountSearchDelinquentOver90));

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

        #region "FUNCTION: GetAccessibleReports"
        /// <summary>
        /// Build the list of reports that the current user can see
        /// </summary>
        /// <returns></returns>
        private List<ReportSectionDTO> GetAccessibleReports()
        {
            List<ReportSectionDTO> sections = new List<ReportSectionDTO>();
            ReportSectionDTO currentSection;

            //-- DAILY REPORTS --
            currentSection = new ReportSectionDTO() { SectionName = LocalResources.HCResources.DailyReports, SectionID = "daily" };

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewLayawayCashReceipts))
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "LayawayCashReceipts", SecurityItem = eSecurityItem.mnuLWReports, ReportID = "LAYAWAYCASHRECEIPTS", ReportName = LocalResources.HCResources.CashReceiptsReport, ClientParameterName = "client", ReportDateParameterName = "fromdate", OrderByParameterName = "sortby", SelectItemList = Helpers.Common.GetCashReceiptsOrderByList() });

            if (currentSection.TotalReports > 0)
                sections.Add(currentSection);

            //-- MONTHLY REPORTS --
            currentSection = new ReportSectionDTO() { SectionName = LocalResources.HCResources.MonthlyReports, SectionID = "monthly" };

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewLayawayCashReceipts))
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "LayawayMonthlyCashReceipts", SecurityItem = eSecurityItem.opViewDFCashReceipts, ReportID = "LAYAWAYMONTHLYCASHRECEIPTS", ReportName = LocalResources.HCResources.CashReceiptsMonthlyReport, ClientParameterName = "client", ReportPeriodParameterName = "period", IncludeCurrentPeriod = true, OrderByParameterName = "sortby", SelectItemList = Helpers.Common.GetCashReceiptsOrderByList() });

            if (currentSection.TotalReports > 0)
                sections.Add(currentSection);

            return (sections);
        }
        #endregion

        #endregion
    }
}
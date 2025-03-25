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
    public class DistributorFinancingController : BaseController
    {
        #region "CONTROLLER: BalanceSummary"
        /// <summary>
        /// Display the Balance Summary page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuDFBalanceSummary)]
        public ActionResult BalanceSummary()
        {
            ViewBag.DISTRIBUTORNO = Helpers.CookieHelper.CurrentEntity;
            return View();
        }
        #endregion

        #region "CONTROLLER: AccountSearch"
        /// <summary>
        /// Display the Accopunt Search page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuDFAccountSearch)]
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
            ViewBag.ACCOUNTTYPE = "DF";

            return View();
        }
        #endregion

        #region "CONTROLLER: Reports"
        /// <summary>
        /// Display the Reports page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuDFReports)]
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

        #region "CONTROLLER: EligibleAccounts"
        /// <summary>
        /// Display the Eligible Accounts page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuDFEligibleAccounts)]
        public ActionResult EligibleAccounts()
        {
            return View();
        }
        #endregion

        #region "CONTROLLER: Payments"
        /// <summary>
        /// Display the Payments page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuDFPaymentPosting)]
        public ActionResult Payments()
        {
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
                searchOptions.Add(eSecurityItem.opDFViewAllAccounts, new Tuple<string, string>("DFAllAccounts", LocalResources.HCResources.AccountSearchAllCustomers));
                searchOptions.Add(eSecurityItem.opDFViewConsecutivePaymentAccounts, new Tuple<string, string>("DFEligible", LocalResources.HCResources.AccountSearchEligible));
                searchOptions.Add(eSecurityItem.opDFViewZeroPayAccounts, new Tuple<string, string>("DFZeroPay", LocalResources.HCResources.AccountSearchZeroPay));
                searchOptions.Add(eSecurityItem.opDFViewAccountsWithBalances, new Tuple<string, string>("DFCarryingABalance", LocalResources.HCResources.AccountSearchCarryingABalance));
                searchOptions.Add(eSecurityItem.opDFViewPaidInFullAccounts, new Tuple<string, string>("DFPaidInFull", LocalResources.HCResources.AccountSearchPaidInFull));
                searchOptions.Add(eSecurityItem.opDFViewAccountsToBePurged, new Tuple<string, string>("DFToBePurged", LocalResources.HCResources.AccountSearchToBePurged));
                searchOptions.Add(eSecurityItem.opDFViewPurgedAccounts, new Tuple<string, string>("DFPurged", LocalResources.HCResources.AccountSearchPurged));
                searchOptions.Add(eSecurityItem.opDFViewDelinquent0to30, new Tuple<string, string>("DFDelinquent0to30", LocalResources.HCResources.AccountSearchDelinquent0to30));
                searchOptions.Add(eSecurityItem.opDFViewDelinquent31to60, new Tuple<string, string>("DFDelinquent31to60", LocalResources.HCResources.AccountSearchDelinquent31to60));
                searchOptions.Add(eSecurityItem.opDFViewDelinquent61to90, new Tuple<string, string>("DFDelinquent61to90", LocalResources.HCResources.AccountSearchDelinquent61to90));
                searchOptions.Add(eSecurityItem.opDFViewDelinquentOver90, new Tuple<string, string>("DFDelinquentOver90", LocalResources.HCResources.AccountSearchDelinquentOver90));

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

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewDFCashReceipts))
            {
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "DFCashReceipts", SecurityItem = eSecurityItem.opViewDFCashReceipts, ReportID = "DFCASHRECEIPTS", ReportName = LocalResources.HCResources.CashReceiptsReport, ClientParameterName = "client", ReportDateParameterName = "fromdate", OrderByParameterName = "sortby", SelectItemList = Helpers.Common.GetCashReceiptsOrderByList() });
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "DFRepoCashReceipts", SecurityItem = eSecurityItem.opViewDFCashReceipts, ReportID = "DFREPOCASHRECEIPTS", ReportName = LocalResources.HCResources.RepoCashReceipts, ClientParameterName = "client", ReportDateParameterName = "fromdate", OrderByParameterName = "sortby", SelectItemList = Helpers.Common.GetCashReceiptsOrderByList() });
            }

            if (currentSection.TotalReports > 0)
                sections.Add(currentSection);

            //-- MONTHLY REPORTS --
            currentSection = new ReportSectionDTO() { SectionName = LocalResources.HCResources.MonthlyReports, SectionID = "monthly" };

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewDFAccountSummaryReport))
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "DFAccountSummary", SecurityItem = eSecurityItem.opViewDFAccountSummaryReport, ReportID = "DFACCOUNTSUMMARY", ReportName = LocalResources.HCResources.AccountSummaryReport, ReportPeriodParameterName = "period", IncludeCurrentPeriod = true });

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewDFCashReceipts))
            {
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "DFMonthlyCashReceipts", SecurityItem = eSecurityItem.opViewDFCashReceipts, ReportID = "DFMONTHLYCASHRECEIPTS", ReportName = LocalResources.HCResources.CashReceiptsMonthlyReport, ClientParameterName = "client", ReportPeriodParameterName = "period", IncludeCurrentPeriod = true, OrderByParameterName = "sortby", SelectItemList = Helpers.Common.GetCashReceiptsOrderByList() });
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "DFRepoMonthlyCashReceipts", SecurityItem = eSecurityItem.opViewDFCashReceipts, ReportID = "DFREPOMONTHLYCASHRECEIPTS", ReportName = LocalResources.HCResources.RepoMonthlyCashReceipts, ClientParameterName = "client", ReportPeriodParameterName = "period", IncludeCurrentPeriod = true, OrderByParameterName = "sortby", SelectItemList = Helpers.Common.GetCashReceiptsOrderByList() });
            }

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewDFIVAPaymentsReport))
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "DFIVAOnPayments", SecurityItem = eSecurityItem.opViewDFIVAPaymentsReport, ReportID = "DFIVAONPAYMENTS", ReportName = LocalResources.HCResources.IVAPaymentsOnDFAccounts, ClientParameterName = "client", ReportPeriodParameterName = "period", IncludeCurrentPeriod = true });

            if (currentSection.TotalReports > 0)
                sections.Add(currentSection);

            return (sections);
        }
        #endregion

        #endregion
    }
}
using DistributorWebsite.MVC.WebUI.Helpers.Models;
using DistributorWebsite.MVC.WebUI.Helpers.Security;
using DistributorWebsite.MVC.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace DistributorWebsite.MVC.WebUI.Controllers
{
    public class CompanyController : BaseController
    {
        ///// <summary>
        ///// Injectable constructor
        ///// </summary>
        ///// <param name="token"></param>
        ////[InjectionConstructor]
        //public CompanyController(IUserAccessToken token)
        //{
        //    System.Diagnostics.Trace.TraceInformation(token.ToString());
        //}

        #region "ANNOUNCEMENTS"

        #region "CONTROLLER: Announcements"
        // GET: /Company/Announcements
        [HCAuthorizeMenu(eSecurityItem.mnuAnnouncements)]
        public ActionResult Announcements()
        {
            return View();
        }
        #endregion

        #region "CONTROLLER: GET: Announcement"
        /// <summary>
        /// Get a file linked to an individual announcement
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuAnnouncements)]
        // GET: /Company/Memos
        public async Task<ActionResult> Announcement(Int32 id)
        {
            return await GetDocument(String.Format("Announcements({0})/Default.GetDocument(language='{1}')", id, this.CurrentLanguage));
        }
        #endregion

        #endregion

        #region "CONTROLLER: Reports"
        /// <summary>
        /// Display the balance summary page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuReports)]
        public async Task<ActionResult> Reports()
        {
            ViewBag.ReportPeriods = Helpers.Common.GetPeriodSelectList(this.CurrentLanguage, true, 36, DateTime.ParseExact("01/01/2001", "MM/dd/yyyy", System.Globalization.CultureInfo.InvariantCulture));
            ViewBag.ReportWeeks = Helpers.Common.GetWeekSelectionList(this.CurrentLanguage);
            ViewBag.ReportUSStates = await Helpers.Common.GetUSStateWithSalesTaxRatesSelectionList(this.CurrentUser);
            ViewBag.CurrentState = this.CurrentUser.State;
            ViewBag.GenericSelectList = Helpers.Common.GetGenericSelectList();
            return View(GetAccessibleReports());
        }
        #endregion

        #region "CONTROLLER: DistributorDirectory"
        // GET: /Company/DistributorDirectory
        [HCAuthorizeMenu(eSecurityItem.mnuDistributorDirectory)]
        public ActionResult DistributorDirectory()
        {
            ViewBag.CANVIEWDISTRDETAILS = Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewDistributorDetails);
            return View();
        }
        #endregion

        #region "MEMOS"

        #region "CONTROLLER: Memos"
        // GET: /Company/Memos
        [HCAuthorizeMenu(eSecurityItem.mnuMemos)]
        public ActionResult Memos()
        {
            //-- DISPLAY THE MAIN MEMO LISTING --
            return View();
       }
        #endregion

        #region "CONTROLLER: GET: Memo"
        /// <summary>
        /// Get the document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuMemos)]
        // GET: /Company/Memo
        public async Task<ActionResult> Memo(Int32 id)
        {
            return await GetDocument(String.Format("Documents/Default.GetMemo(id='{0}')", id));
        }
        #endregion

        #endregion

        #region "DOCUMENTS AND FORMS"

        #region "CONTROLLER: Documents"
        // GET: /Company/Documents
        [HCAuthorizeMenu(eSecurityItem.mnuDocumentsAndForms)]
        public ActionResult Documents()
        {
            //-- DISPLAY THE MAIN MEMO LISTING --
            return View();
        }
        #endregion

        #region "CONTROLLER: GET: Document"
        /// <summary>
        /// Get the document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuDocumentsAndForms)]
        // GET: /Company/Document
        public async Task<ActionResult> Document(Int32 id)
        {
            return await GetDocument(String.Format("Documents/Default.GetDocumentOrForm(id='{0}')", id));
        }
        #endregion

        #endregion

        #region "CONTROLLER: ContactUs"
        // GET: /Company/ContactUs
        [HCAuthorizeMenu(eSecurityItem.mnuContactUs)]
        public ActionResult ContactUs()
        {
            ViewBag.CanViewEmployees = true; // this.CurrentUser.CanAccess(eSecurityItem.subEmployeeDirectory);

            return View();
        }
        #endregion

        #region "CONTROLLER: EmployeeDirectory"
        // GET: /Company/EmployeeDirectory
        [HCAuthorizeMenu(eSecurityItem.mnuEmployeeDirectory)]
        public ActionResult EmployeeDirectory()
        {
            return View();
        }
        #endregion

        #region "CONTROLLER: Ethics"

        #region "CONTROLLER: Ethics"
        // GET: /Company/Ethics
        [HCAuthorizeMenu(eSecurityItem.mnuEthicsTraining)]
        public ActionResult Ethics()
        {
            //-- DISPLAY THE MAIN MEMO LISTING --
            return View();
        }
        #endregion

        #region "CONTROLLER: GET: EthicsTraining"
        /// <summary>
        /// Get the document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuEthicsTraining)]
        // GET: /Company/Memo
        public async Task<ActionResult> EthicsTraining(Int32 id)
        {
            return await GetDocument(String.Format("Documents/Default.GetEthicsMemo(id='{0}')", id));
        }
        #endregion

        #endregion

        #region "HELPER FUNCTIONS"

        #region "FUNCTION: GetAccessibleReports"
        /// <summary>
        /// Build the list of reports that the current user can see
        /// </summary>
        /// <returns></returns>
        private List<ReportSectionDTO> GetAccessibleReports()
        {
            List<ReportSectionDTO> sections = new List<ReportSectionDTO>();
            ReportSectionDTO currentSection;

            //-- MISCELLANEOUS REPORTS --
            currentSection = new ReportSectionDTO() { SectionName = LocalResources.HCResources.MiscellaneousReports, SectionID = "misc" };

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewSalespersonAgreementStatuses))
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "SPAgreementStatuses", SecurityItem = eSecurityItem.opViewSalespersonAgreementStatuses, ReportID = "SPAGREEMENTSTATUSES", ReportName = LocalResources.HCResources.SalespersonAgreementStatusReport });

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewCustomerCardReport))
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "RPCustCards", SecurityItem = eSecurityItem.opViewCustomerCardReport, ReportID = "RPCUSTCARDS", ReportName = LocalResources.HCResources.RPCustomerCardReport, ReportMonthParameterName = "period", SelectItemList = String.Format("THISMONTH^{0}|LASTMONTH^{1}|LAST3^{2}|LAST6^{3}|LASTYEAR^{4}^ALL^{5}", LocalResources.HCResources.CardsSentThisMonth, LocalResources.HCResources.CardsSentLastMonth, LocalResources.HCResources.CardsSentLast3, LocalResources.HCResources.CardsSentLast6, LocalResources.HCResources.CardsSentLastYear, LocalResources.HCResources.CardsSentAnytime) });

            if (currentSection.TotalReports > 0)
                sections.Add(currentSection);

            //-- DAILY REPORTS --
            currentSection = new ReportSectionDTO() { SectionName = LocalResources.HCResources.DailyReports, SectionID = "daily" };

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewCashReceiptsReport))
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "CashReceipts", SecurityItem = eSecurityItem.opViewCashReceiptsReport, ReportID = "CASHRECEIPTS", ReportName = LocalResources.HCResources.CashReceiptsReport, ClientParameterName = "client", ReportDateParameterName = "fromdate", OrderByParameterName = "sortby", SelectItemList = Helpers.Common.GetCashReceiptsOrderByList() });

            if (currentSection.TotalReports > 0)
                sections.Add(currentSection);

            //-- WEEKLY REPORTS --
            currentSection = new ReportSectionDTO() { SectionName = LocalResources.HCResources.WeeklyReports, SectionID = "weekly" };

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewServiceChargeReport))
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "ServiceCharges", SecurityItem = eSecurityItem.opViewServiceChargeReport, ReportID = "SERVICECHARGES", ReportName = LocalResources.HCResources.ServiceChargeReport, ClientParameterName = "client", ReportWeekParameterName = "week" });

            if (currentSection.TotalReports > 0)
                sections.Add(currentSection);

            //-- MONTHLY REPORTS --
            currentSection = new ReportSectionDTO() { SectionName = LocalResources.HCResources.MonthlyReports, SectionID = "monthly" };

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewAccountSummaryReport))
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "AccountSummary", SecurityItem = eSecurityItem.opViewAccountSummaryReport, ReportID = "ACCOUNTSUMMARY", ReportName = LocalResources.HCResources.AccountSummaryReport, ReportPeriodParameterName = "period", IncludeCurrentPeriod = true });

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewCartridgeLetterReport))
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "CARPLetters", SecurityItem = eSecurityItem.opViewCartridgeLetterReport, ReportID = "CARPLETTERS", ReportName = LocalResources.HCResources.CartridgeReplacementLetterReport, ReportPeriodParameterName = "period", IncludeCurrentPeriod = false });

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewCustomerBirthdayReport))
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "CustBirthdays", SecurityItem = eSecurityItem.opViewCustomerBirthdayReport, ReportID = "CUSTBIRTHDAYS", ReportName = LocalResources.HCResources.CustomerBirthdaysReport, ReportMonthParameterName = "date", SelectItemList = String.Format("CURRENTMONTH^{0}|NEXTMONTH^{1}|NEXT3MONTHS^{2}|NEXT6MONTHS^{3}|ALL^{4}", LocalResources.HCResources.ReportViewCurrentMonth, LocalResources.HCResources.ReportViewThroughNextMonth, LocalResources.HCResources.ReportViewNext3Months, LocalResources.HCResources.ReportViewNext6Months, LocalResources.HCResources.ReportViewAllNextYear) });

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewInactiveCarpAddons))
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "CARPAddons", SecurityItem = eSecurityItem.opViewInactiveCarpAddons, ReportID = "INACTIVECARPS", ReportName = LocalResources.HCResources.InactiveCarpAddonReport });

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewCashReceiptsReport))
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "MonthlyCashReceipts", SecurityItem = eSecurityItem.opViewCashReceiptsReport, ReportID = "MONTHLYCASHRECEIPTS", ReportName = LocalResources.HCResources.CashReceiptsMonthlyReport, ClientParameterName = "client", ReportPeriodParameterName = "period", IncludeCurrentPeriod = true, OrderByParameterName = "sortby", SelectItemList = Helpers.Common.GetCashReceiptsOrderByList() });

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewReserveSummaryReport))
            {
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "ReserveSummary", SecurityItem = eSecurityItem.opViewReserveSummaryReport, ReportID = "RESERVESUMMARY", ReportName = LocalResources.HCResources.ReserveSummaryReport, ClientParameterName = "client", ReportPeriodParameterName = "period", IncludeCurrentPeriod = true });
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "HighRiskReserveSummary", SecurityItem = eSecurityItem.opViewReserveSummaryReport, ReportID = "HIGHRISKRESERVESUMMARY", ReportName = LocalResources.HCResources.HighRiskReserveSummaryReport, ClientParameterName = "client", ReportPeriodParameterName = "period", IncludeCurrentPeriod = true });
            }

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewTerritoryAgingReport))
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "TerritoryAging", SecurityItem = eSecurityItem.opViewTerritoryAgingReport, ReportID = "TERRITORYAGING", ReportName = LocalResources.HCResources.TerritoryAgingReport, ReportPeriodParameterName = "period", IncludeCurrentPeriod = true });

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewTDMeetingFundReport))
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "TDMeetingFund", SecurityItem = eSecurityItem.opViewTDMeetingFundReport, ReportID = "TDMEETINGFUND", ReportName = LocalResources.HCResources.TerritoryDirectorMeetingFundReport, ReportPeriodParameterName = "period", IncludeCurrentPeriod = true, ClientParameterName = "client" });

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewMonthlyExchangeRates))
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "ExchangeRates", SecurityItem = eSecurityItem.opViewMonthlyExchangeRates, ReportID = "EXCHANGERATES", ReportName = LocalResources.HCResources.MonthlyExchangeRatesReport });

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opTerritoryDirectorVolumeSummary))
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "TDDirectVolume", SecurityItem = eSecurityItem.opTerritoryDirectorVolumeSummary, ReportID = "TDDirectVolume", ReportName = LocalResources.HCResources.TDDirectVolumeReport, ReportPeriodParameterName = "period", IncludeCurrentPeriod = true });

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opMasterTerritoryVolumeSummary))
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "MasterDirectVolume", SecurityItem = eSecurityItem.opMasterTerritoryVolumeSummary, ReportID = "MasterDirectVolume", ReportName = LocalResources.HCResources.MasterDirectVolumeReport, ReportPeriodParameterName = "period", IncludeCurrentPeriod = true });

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opTerritoryDirectorPurchaseSummaryAmerican))
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "TerritoryDirectorPurchaseSummaryAmerican", SecurityItem = eSecurityItem.opTerritoryDirectorPurchaseSummaryAmerican, ReportID = "TerritoryDirectorPurchaseSummaryAmericanReport", ReportName = LocalResources.HCResources.TerritoryDirectorPurchaseSummaryAmerican, ReportPeriodParameterName = "period", IncludeCurrentPeriod = true });

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opMasterNetworkSplit))
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "MasterNetworkSplit", SecurityItem = eSecurityItem.opMasterNetworkSplit, ReportID = "MasterNetworkSplit", ReportName = LocalResources.HCResources.MasterNetworkSplitReport, ReportPeriodParameterName = "period", IncludeCurrentPeriod = true });

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opTDVolumeMXSummary))
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "TDVolumeMXSummary", SecurityItem = eSecurityItem.opTDVolumeMXSummary, ReportID = "TDVolumeMXSummaryReport", ReportName = LocalResources.HCResources.TDVolumeMXSummary, ReportPeriodParameterName = "period", IncludeCurrentPeriod = true });

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opTDVolumeWorldReport))
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "TDVolumeWorld", SecurityItem = eSecurityItem.opTDVolumeWorldReport, ReportID = "TDVolumeWorldReport", ReportName = LocalResources.HCResources.TDVolumeWorld, ReportPeriodParameterName = "period", IncludeCurrentPeriod = true });
            
            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.mnuCOTaxFactors))
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "COTaxFactor", SecurityItem = eSecurityItem.mnuCOTaxFactors, ReportID = "COTaxFactorsReport", ReportName = LocalResources.HCResources.COTaxFactor, ReportDateFromParameterName = "fromdate", ReportDatetoParameterName = "todate" });

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.mnuDistributorCOTaxFactor))
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "JDCOTaxFactor", SecurityItem = eSecurityItem.mnuDistributorCOTaxFactor, ReportID = "JDCOTaxFactorsReport", ReportName = LocalResources.HCResources.JDCOTaxFactor, ReportDateFromParameterName = "fromdate", ReportDatetoParameterName = "todate" });

            if (currentSection.TotalReports > 0)
                sections.Add(currentSection);

            return (sections);
        }
        #endregion

        #endregion

    }
}
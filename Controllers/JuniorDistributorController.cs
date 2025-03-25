using DistributorWebsite.MVC.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace DistributorWebsite.MVC.WebUI.Controllers
{
    public class JuniorDistributorController : BaseController
    {
        // GET: JuniorDistributor
        public ActionResult Index()
        {
            return View();
        }

        [HCAuthorizeMenu(eSecurityItem.mnuJDSuccessReport)]
        public async Task<ActionResult> Reports()
        {
            try
            {
                ViewBag.ReportPeriods = new SelectList(new List<SelectListItem>(), "Value", "Text");
                ViewBag.ReportWeeks = new SelectList(new List<SelectListItem>(), "Value", "Text");
                ViewBag.ReportUSStates = new SelectList(new List<SelectListItem>(), "Value", "Text");
                ViewBag.CurrentState = new SelectList(new List<SelectListItem>(), "Value", "Text");
                ViewBag.GenericSelectList = new SelectList(new List<SelectListItem>(), "Value", "Text");

                ViewBag.Region = await Helpers.Common.GetRegions(this.CurrentUser);
                ViewBag.Weekending = Helpers.Common.GetWeekendingSelectionList(this.CurrentLanguage);
                ViewBag.GenericSelectList = Helpers.Common.GetGenericSelectList();
                return View(GetReport());
            }
             catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return new RedirectToRouteResult(new RouteValueDictionary
                                                 {
                                                    { "action", "Index" },
                                                    { "controller", "Error" }
                                                 });
            }
        }


        private List<ReportSectionDTO> GetReport()
        {
            List<ReportSectionDTO> sections = new List<ReportSectionDTO>();

            ReportSectionDTO currentSection;

            //-- MISCELLANEOUS REPORTS --
            currentSection = new ReportSectionDTO() { SectionName = LocalResources.HCResources.MiscellaneousReports, SectionID = "misc" };

            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewJDSuccessAnalysisForTerritory))
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "JDSuccessAnalysisForTerritory", SecurityItem = eSecurityItem.opViewJDSuccessAnalysisForTerritory, ReportID = "JDSUCCESSANALYSISFORTERRITORY", ReportName = LocalResources.HCResources.JDSuccessReportForTerritory, RegionParameterName = "region", WeekendingParameterName = "weekending", OrderByParameterName = "orderby", SelectItemList = Helpers.Common.GetJDReportOrderByList() });
            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewJDSuccessAnalysisForSponsor))
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "JDSuccessAnalysisForSponsor", SecurityItem = eSecurityItem.opViewJDSuccessAnalysisForSponsor, ReportID = "JDSUCCESSANALYSISFORSPONSOR", ReportName = LocalResources.HCResources.JDSuccessReportForSponsor, RegionParameterName = "region", WeekendingParameterName = "weekending", OrderByParameterName = "orderby", SelectItemList = Helpers.Common.GetJDReportOrderByList() });
            if (Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewJDSuccessAnalysisForCompany))
                currentSection.Reports.Add(new ReportSectionItemDTO() { MVCAction = "JDSuccessAnalysisForCompany", SecurityItem = eSecurityItem.opViewJDSuccessAnalysisForCompany, ReportID = "JDSUCCESSANALYSISFORCOMPANY", ReportName = LocalResources.HCResources.JDSuccessReportForCompany, RegionParameterName = "region", WeekendingParameterName = "weekending", OrderByParameterName = "orderby", SelectItemList = Helpers.Common.GetJDReportOrderByList() });

            sections.Add(currentSection);

            return sections;
        }

    }
}
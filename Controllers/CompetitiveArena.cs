using DistributorWebsite.MVC.WebUI.Helpers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DistributorWebsite.MVC.WebUI.Controllers
{
    [HCAuthorize]
    public class CompetitiveArenaController : BaseController
    {
        #region "CONTROLLER: TerritoryVolume"
        /// <summary>
        /// Get the territory volume
        /// </summary>
        /// <returns></returns>
        // GET: CompetitiveArena/TerritoryVolume
        [HCAuthorizeMenu(eSecurityItem.mnuTerritoryVolume)]
        public ActionResult TerritoryVolume()
        {
            ViewBag.APIENTITYPATH = "Purchases/Default.TerritoryVolume(period='[PERIOD]',range='[TYPE]',from='[FROMDATE]',to='[TODATE]',datetype='[DATETYPE]',carp='[CARP]',sample='[SAMPLE]',bulk='[BULK]',zerobal='[ZEROBAL]')";
            ViewBag.EXPORTACTION = "TerritoryVolume";
            ViewBag.DISTORSP = LocalResources.HCResources.Distributor;
            ViewBag.SHOWZEROBALOPTION = true;
            return View("Volume");
        }
        #endregion

        #region "CONTROLLER: CompanyVolume"
        /// <summary>
        /// Get the company volume
        /// </summary>
        /// <returns></returns>
        // GET: CompetitiveArena/CompanyVolume
        [HCAuthorizeMenu(eSecurityItem.mnuCompanyVolume)]
        public ActionResult CompanyVolume()
        {
            ViewBag.APIENTITYPATH = "Purchases/Default.CompanyVolume(period='[PERIOD]',range='[TYPE]',from='[FROMDATE]',to='[TODATE]',datetype='[DATETYPE]',carp='[CARP]',sample='[SAMPLE]',bulk='[BULK]',zerobal='[ZEROBAL]')";
            ViewBag.EXPORTACTION = "CompanyVolume";
            ViewBag.DISTORSP = LocalResources.HCResources.Distributor;
            ViewBag.SHOWZEROBALOPTION = true;
            return View("Volume");
        }
        #endregion

        #region "CONTROLLER: JVVolume"
        /// <summary>
        /// Get the JV Volume
        /// </summary>
        /// <returns></returns>
        // GET: CompetitiveArena/JVVolume
        [HCAuthorizeMenu(eSecurityItem.mnuJVVolume)]
        public ActionResult JVVolume()
        {
            ViewBag.APIENTITYPATH = "Purchases/Default.JVVolume(period='[PERIOD]',range='[TYPE]',from='[FROMDATE]',to='[TODATE]',datetype='[DATETYPE]',carp='[CARP]',sample='[SAMPLE]',bulk='[BULK]',zerobal='[ZEROBAL]')";
            ViewBag.EXPORTACTION = "JVVolume";
            ViewBag.DISTORSP = LocalResources.HCResources.Distributor;
            ViewBag.SHOWZEROBALOPTION = true;
            return View("Volume");
        }
        #endregion

        #region "CONTROLLER: Icebreakers"
        /// <summary>
        /// Display the icebreaker form
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuIcebreakers)]
        public ActionResult Icebreakers()
        {
            List<SelectListItem> emptyList = new List<SelectListItem>();
            List<SelectListItem> yesNoList = new List<SelectListItem>();
            emptyList.Add(new SelectListItem() { Text = "", Value = "" });

            //-- YES/NO OPTIONS --
            yesNoList.Add(new SelectListItem() { Text = "", Value = "" });
            yesNoList.Add(new SelectListItem() { Text = LocalResources.HCResources.Yes, Value = "Y" });
            yesNoList.Add(new SelectListItem() { Text = LocalResources.HCResources.No, Value = "N" });

            //-- SET THE WEB ADMIN API URL --
            ViewBag.WEBAPIURL = Helpers.Application.Instance.APIS.DistributorWebAPI + "Icebreakers";

            //-- SET THE DEFAULT DISTRIBUTOR INFO --
            if (this.EntityAccessToken.IsDistributor)
            {
                ViewBag.DefaultDistributorNo = this.EntityAccessToken.EntityCode;
                ViewBag.DefaultDistributorName = this.EntityAccessToken.EntityName;
            }
            else
            {
                ViewBag.DefaultDistributorNo = "";
                ViewBag.DefaultDistributorName = "";
            }

            ViewBag.MonthSelectionList = Helpers.Common.GetMonthSelectList();
            ViewBag.DaySelectionList = Helpers.Common.GetNumericSelectList(1, 31, 2);
            ViewBag.BirthYearSelectionList = Helpers.Common.GetNumericSelectList(DateTime.Now.AddYears(-100).Year, DateTime.Now.AddYears(-15).Year, reverseOrder: true);

            ViewBag.CountrySelectionList = new SelectList(emptyList, "Value", "Text");
            ViewBag.StateSelectionList = new SelectList(emptyList, "Value", "Text");
            ViewBag.RecruitingSourceList = new SelectList(emptyList, "Value", "Text");
            ViewBag.NationalityList = new SelectList(emptyList, "Value", "Text");
            ViewBag.RecruiterList = new SelectList(emptyList, "Value", "Text");
            ViewBag.YesNoList = new SelectList(yesNoList, "Value", "Text");
            ViewBag.BankList = new SelectList(emptyList, "Value", "Text");

            if (this.EntityAccessToken.Brand == "NE")
                ViewBag.WorkForRPLabel = LocalResources.HCResources.HasWorkedForRPorNEBefore;
            else
                ViewBag.WorkForRPLabel = LocalResources.HCResources.HasWorkedForRPBefore;

            ViewBag.CanViewDigitalID = this.EntityAccessToken.IsInternal && Helpers.Application.Instance.AuthorizationType == eWebAuthType.WINDOWS;
            ViewBag.CanViewDigitalPhoto = true;
            ViewBag.CanViewSPContract = true;
            ViewBag.CanViewColombiaRUT = this.EntityAccessToken.IsInternal && Helpers.Application.Instance.AuthorizationType == eWebAuthType.WINDOWS;
            ViewBag.CanViewColombiaBANKCERT = this.EntityAccessToken.IsInternal && Helpers.Application.Instance.AuthorizationType == eWebAuthType.WINDOWS;
            ViewBag.CanViewImmigrationPermit = this.EntityAccessToken.IsInternal && Helpers.Application.Instance.AuthorizationType == eWebAuthType.WINDOWS;
            ViewBag.CanEditIcebreaker = Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opEditIcebreaker);

            ViewBag.CurrentDistributorNo = this.EntityAccessToken.DistributorNo;
            return (View());
        }
        #endregion

        #region "CONTROLLER: GET: Icebreaker/id/Documents/DigitalID"
        /// <summary>
        /// Get the document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns> 
        [Route("CompetitiveArena/Icebreakers/{id:int}/Documents/DigitalID")]
        [HCAuthorizeInternalMenu(eSecurityItem.mnuIcebreakers)]
        public async Task<ActionResult> GetIcebreakerDigitalID(Int32 id)
        {
            return await GetDocument($"Icebreakers({id})/Default.DigitalID");
        }
        #endregion

        #region "CONTROLLER: GET: Icebreaker/id/Documents/DigitalPhoto"
        /// <summary>
        /// Get the document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns> 
        [Route("CompetitiveArena/Icebreakers/{id:int}/Documents/DigitalPhoto")]
        [HCAuthorizeMenu(eSecurityItem.mnuIcebreakers)]
        public async Task<ActionResult> GetIcebreakerDigitalPhoto(Int32 id)
        {
            return await GetDocument($"Icebreakers({id})/Default.DigitalPhoto");
        }
        #endregion

        #region "CONTROLLER: GET: Icebreaker/id/Documents/SPContract"
        /// <summary>
        /// Get the document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns> 
        [Route("CompetitiveArena/Icebreakers/{id:int}/Documents/SPContract")]
        [HCAuthorizeMenu(eSecurityItem.mnuIcebreakers)]
        public async Task<ActionResult> GetIcebreakerSPContract(Int32 id)
        {
            return await GetDocument($"Icebreakers({id})/Default.SPContract");
        }
        #endregion

        #region "CONTROLLER: GET: Icebreaker/id/Documents/ColombiaRUT"
        /// <summary>
        /// Get the document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns> 
        [Route("CompetitiveArena/Icebreakers/{id:int}/Documents/ColombiaRUT")]
        [HCAuthorizeInternalMenu(eSecurityItem.mnuIcebreakers)]
        public async Task<ActionResult> GetIcebreakerColombiaRUT(Int32 id)
        {
            return await GetDocument($"Icebreakers({id})/Default.ColombiaRUT");
        }
        #endregion

        #region "CONTROLLER: GET: Icebreaker/id/Documents/ColombiaBANKCERT"
        /// <summary>
        /// Get the document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns> 
        [Route("CompetitiveArena/Icebreakers/{id:int}/Documents/ColombiaBANKCERT")]
        [HCAuthorizeInternalMenu(eSecurityItem.mnuIcebreakers)]
        public async Task<ActionResult> GetIcebreakerColombiaBANKCERT(Int32 id)
        {
            return await GetDocument($"Icebreakers({id})/Default.ColombiaBANKCERT");
        }
        #endregion

        #region "CONTROLLER: GET: Icebreaker/id/Documents/ImmigrationPermit"
        /// <summary>
        /// Get the document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns> 
        [Route("CompetitiveArena/Icebreakers/{id:int}/Documents/ImmigrationPermit")]
        [HCAuthorizeInternalMenu(eSecurityItem.mnuIcebreakers)]
        public async Task<ActionResult> GetIcebreakerImmigrationPermit(Int32 id)
        {
            return await GetDocument($"Icebreakers({id})/Default.ImmigrationPermit");
        }
        #endregion
        
        #region "CONTROLLER: Recommendations"
        /// <summary>
        /// Display the recommendation dashboard
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuRecommendationDashboard)]
        public ActionResult Recommendations()
        {

            List<SelectListItem> yesNoList = new List<SelectListItem>();
            List<SelectListItem> experienceTypes = new List<SelectListItem>();
            List<SelectListItem> uploadRequiredDocuments = new List<SelectListItem>();

            //-- SET THE WEB ADMIN API URL --
            ViewBag.WEBAPIURL = Helpers.Application.Instance.APIS.DistributorWebAPI + "Recommendations";

            //-- SET THE VIEWBAG FOR THE TUTORIAL VIDEO
            ViewBag.ShowVideoLink = Helpers.Common.GetCurrentEntityToken().Clients.Any(c => c == "HC") && this.CurrentLanguage.ToUpper() == "ES";


            yesNoList.Add(new SelectListItem() { Text = "", Value = "" });
            yesNoList.Add(new SelectListItem() { Text = LocalResources.HCResources.Yes, Value = "Y" });
            yesNoList.Add(new SelectListItem() { Text = LocalResources.HCResources.No, Value = "N" });
       
            experienceTypes.Add(new SelectListItem() { Text = "", Value = "" });
            uploadRequiredDocuments.Add(new SelectListItem() { Text = "", Value = "" });

            //-- SET THE DEFAULT DISTRIBUTOR INFO --
            if (this.EntityAccessToken.IsDistributor)
            {
                ViewBag.DefaultDistributorNo = this.EntityAccessToken.EntityCode;
                ViewBag.DefaultDistributorName = this.EntityAccessToken.EntityName;
            }
            else
            {
                ViewBag.DefaultDistributorNo = "";
                ViewBag.DefaultDistributorName = "";
            }
            ViewBag.MonthSelectionList = Helpers.Common.GetMonthSelectList();
            ViewBag.DaySelectionList = Helpers.Common.GetNumericSelectList(1, 31, 2);
            ViewBag.BirthYearSelectionList = Helpers.Common.GetNumericSelectList(DateTime.Now.AddYears(-100).Year, DateTime.Now.AddYears(-18).Year, reverseOrder: true);
            ViewBag.YesNoList = new SelectList(yesNoList, "Value", "Text");       
            ViewBag.ExperienceTypes = new SelectList(experienceTypes, "Value", "Text");
            ViewBag.UploadRequiredDocuments = new SelectList(uploadRequiredDocuments, "Value", "Text");
            ViewBag.DEFAULTRECOMMENDATIONDETAILTAB = "HOMEADDRESS";
            ViewBag.IsTerritoryDirector = this.EntityAccessToken.IsTerritoryDirector;
            
            return (View());
        }
        #endregion

        #region "CONTROLLER: Salespeople"
        /// <summary>
        /// Display the salespeople list
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuSalespersonSearch)]
        public ActionResult Salespeople()
        {
            return View();
        }
        #endregion

        #region "CONTROLLER: VolumeEntry"
        /// <summary>
        /// Display the volume entry page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuVolumeEntry)]
        public ActionResult VolumeEntry()
        {
            ViewBag.DISTRIBUTORNO = Helpers.CookieHelper.CurrentEntity;
            ViewBag.DefaultClient = Helpers.CookieHelper.DefaultClient;
            List<SelectListItem> volumeEntryDistributors = new List<SelectListItem>();
            List<SelectListItem> viewType = new List<SelectListItem>();
            viewType.Add(new SelectListItem() { Text = LocalResources.HCResources.ViewAllSalespersonForSelectedDistributor, Value = "1" });
            viewType.Add(new SelectListItem() { Text = LocalResources.HCResources.ViewAllSalespersonWithEntriesForSelectedDistributor, Value = "2" });
            viewType.Add(new SelectListItem() { Text = LocalResources.HCResources.ViewAllEntriesforCurrentDistributor, Value = "3" });
            ViewBag.VolumeEntryDistributors = new SelectList(volumeEntryDistributors, "Value", "Text");
            ViewBag.ViewType = new SelectList(viewType, "Value", "Text");
            ViewBag.IsInternalLogin = this.EntityAccessToken.IsInternal;
            
            return View();
        }
        #endregion

        #region "CONTROLLER: SalesVolume"
        /// <summary>
        /// Get the Sales Volume details
        /// </summary>
        /// <returns></returns>
        // GET: CompetitiveArena/SalesVolume
        [HCAuthorizeMenu(eSecurityItem.mnuSalesVolumeSearch)]
        public ActionResult SalesVolume()
        {
            return View();
        }
        #endregion

        #region "CONTROLLER: SalespersonVolume"
        /// <summary>
        /// Get the Salesperson volume
        /// </summary>
        /// <returns></returns>
        // GET: CompetitiveArena/SalespersonVolume
        [HCAuthorizeMenu(eSecurityItem.mnuSalespersonVolume)]
        public ActionResult SalespersonVolume()
        {
            ViewBag.APIENTITYPATH = "Purchases/Default.SalespersonVolume(period='[PERIOD]',range='[TYPE]',from='[FROMDATE]',to='[TODATE]',datetype='[DATETYPE]',carp='[CARP]',sample='[SAMPLE]',bulk='[BULK]',zerobal='[ZEROBAL]')";
            ViewBag.EXPORTACTION = "SalespersonVolume";
            ViewBag.DISTORSP = LocalResources.HCResources.Salesperson;
            return View("Volume");
        }
        #endregion

        #region "CONTROLLER: RecruiterVolume"
        /// <summary>
        /// Get the Recruiter volume
        /// </summary>
        /// <returns></returns>
        // GET: CompetitiveArena/RecruiterVolume
        [HCAuthorizeMenu(eSecurityItem.mnuRecruiterVolume)]
        public ActionResult RecruiterVolume()
        {
            ViewBag.APIENTITYPATH = "Purchases/Default.RecruiterVolume(period='[PERIOD]',range='[TYPE]',from='[FROMDATE]',to='[TODATE]',datetype='[DATETYPE]',carp='[CARP]',sample='[SAMPLE]',bulk='[BULK]',zerobal='[ZEROBAL]')";
            ViewBag.EXPORTACTION = "RecruiterVolume";
            ViewBag.DISTORSP = LocalResources.HCResources.Salesperson;
            return View("Volume");
        }
        #endregion

        #region "CONTROLLER: WeeklyBulletin"

        #region "CONTROLLER: WeeklyBulletin"
        // GET: /Company/Ethics
        [HCAuthorizeMenu(eSecurityItem.mnuWeeklyBulletin)]
        public ActionResult WeeklyBulletins()
        {
            //-- DISPLAY THE MAIN MEMO LISTING --
            return View();
        }
        #endregion

        #region "CONTROLLER: GET: WeeklyBulletin"
        /// <summary>
        /// Get the document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuWeeklyBulletin)]
        // GET: /Company/Memo
        public async Task<ActionResult> WeeklyBulletin(Int32 id)
        {
            return await GetDocument(String.Format("Documents/Default.GetWeeklyBulletin(id='{0}')", id));
        }
        #endregion

        #endregion

        #region "CONTROLLER: Pacemakers"

        #region "CONTROLLER: Pacemakers"
        // GET: /CompetitiveArena/Pacemakers
        [HCAuthorizeMenu(eSecurityItem.mnuPacemakers)]
        public ActionResult Pacemakers()
        {
            //-- DISPLAY THE MAIN MEMO LISTING --
            return View();
        }
        #endregion

        #region "CONTROLLER: GET: Pacemaker"
        /// <summary>
        /// Get the document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuPacemakers)]
        // GET: /CompetitiveArena/Pacemaker
        public async Task<ActionResult> Pacemaker(Int32 id)
        {
            return await GetDocument(String.Format("Documents/Default.GetPacemaker(id='{0}')", id));
        }
        #endregion

        #endregion

        #region "CONTROLLER: Pacesetters"

        #region "CONTROLLER: Pacesetters"
        // GET: /CompetitiveArena/Pacesetters
        [HCAuthorizeMenu(eSecurityItem.mnuPacesetters)]
        public ActionResult Pacesetters()
        {
            //-- DISPLAY THE MAIN MEMO LISTING --
            return View();
        }
        #endregion

        #region "CONTROLLER: GET: Pacesetter"
        /// <summary>
        /// Get the document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuPacesetters)]
        // GET: /Company/Memo
        public async Task<ActionResult> Pacesetter(Int32 id)
        {
            return await GetDocument(String.Format("Documents/Default.GetPacesetter(id='{0}')", id));
        }
        #endregion

        #endregion

        #region "CONTROLLER: MexicoPacemakers"

        #region "CONTROLLER: MexicoPacemakers"
        // GET: /CompetitiveArena/MexicoPacemakers
        [HCAuthorizeMenu(eSecurityItem.mnuMexicoPacemakers)]
        public ActionResult MexicoPacemakers()
        {
            //-- DISPLAY THE MAIN MEMO LISTING --
            return View();
        }
        #endregion

        #region "CONTROLLER: GET: MexicoPacemaker"
        /// <summary>
        /// Get the document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuMexicoPacemakers)]
        // GET: /CompetitiveArena/MexicoPacemaker
        public async Task<ActionResult> MexicoPacemaker(Int32 id)
        {
            return await GetDocument(String.Format("Documents/Default.GetMexicoPacemaker(id='{0}')", id));
        }
        #endregion

        #endregion

        #region "CONTROLLER: Rules"

        #region "CONTROLLER: Rules"
        // GET: /CompetitiveArena/Rules
        [HCAuthorizeMenu(eSecurityItem.mnuRulesAndDefinitions)]
        public ActionResult RulesAndDefinitions()
        {
            //-- DISPLAY THE MAIN MEMO LISTING --
            return View();
        }
        #endregion

        #region "CONTROLLER: GET: Rule"
        /// <summary>
        /// Get the document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuRulesAndDefinitions)]
        // GET: /CompetitiveArena/Rule
        public async Task<ActionResult> Rule(Int32 id)
        {
            return await GetDocument(String.Format("Documents/Default.GetRuleAndDefinition(id='{0}')", id));
        }
        #endregion

        #endregion

        #region "CONTROLLER: AwardsCatalogs"

        #region "CONTROLLER: AwardsCatalogs"
        // GET: /CompetivieArena/AwardsCatalog
        [HCAuthorizeMenu(eSecurityItem.mnuAwardsCatalog)]
        public ActionResult AwardsCatalogs()
        {
            //-- DISPLAY THE MAIN MEMO LISTING --
            return View();
        }
        #endregion

        #region "CONTROLLER: GET: AwardsCatalog"
        /// <summary>
        /// Get the document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuAwardsCatalog)]
        // GET: /Company/Memo
        public async Task<ActionResult> AwardsCatalog(Int32 id)
        {
            return await GetDocument(String.Format("Documents/Default.GetAwardsCatalog(id='{0}')", id));
        }
        #endregion

        #endregion
    }
}
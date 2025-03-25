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
    public class SalesAndMarketingController : BaseController
    {
        #region "CONTROLLER: RPToday"
        /// <summary>
        /// Display the RP Today Page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuRPToday)]
        public ActionResult RPToday()
        {
            return View();
        }
        #endregion

        #region "CONTROLLER: Training"
        /// <summary>
        /// Display the Training page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuTraining)]
        public async Task<ActionResult> Training()
        {
            IEnumerable<DocumentCategoryDTO> categories;

            //-- GET THE LIST OF CATEGORIES --
            using (var oAPI = new Helpers.WebAPI(language: this.CurrentLanguage, existingToken: this.CurrentUser))
            {
                categories = await oAPI.GetTrainingCategoriesAsync();
            }

            return View(categories);
        }
        #endregion

        #region "CONTROLLER: Universidad"
        /// <summary>
        /// Display the Universidad
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuUniversidad)]
        public ActionResult Universidad()
        {
            return View();
        }
        #endregion

        #region "CONTROLLER: SalesMaterials"
        /// <summary>
        /// Display the Sales Materials Page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuSalesMaterials)]
        public ActionResult SalesMaterials()
        {
            return View();
        }
        #endregion

        #region "CONTROLLER: Conventions"
        /// <summary>
        /// Display the Conventions Page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuConventionsAndIncentives)]
        public ActionResult Conventions()
        {
            return View();
        }
        #endregion

        #region "CONTROLLER: GET: ConventionMemo"
        /// <summary>
        /// Get the document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuConventionsAndIncentives)]
        // GET: /SalesAndMarketing/ConventionMemo
        public async Task<ActionResult> ConventionMemo(Int32 id)
        {
            return await GetDocument(String.Format("Documents/Default.GetConventionMemo(id='{0}')", id));
        }
        #endregion

        #region "CONTROLLER: TrainingSeminars"
        /// <summary>
        /// Display the Training Seminars Page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuTrainingSeminars)]
        public ActionResult TrainingSeminars()
        {
            return View();
        }
        #endregion

        #region "CONTROLLER: MagazineLeads"
        /// <summary>
        /// Display the Magazine Leads Page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuMagazineLeads)]
        public async Task<ActionResult> MagazineLeads()
        {
            MagazineLeadCategoriesDTO categories;
            List<SelectListItem> categorySel;

            //-- INITIALIZE THE SELECT LIST --
            categorySel = new List<SelectListItem>();            

            //-- BUILD THE CATEGORY SELECTION LIST --
            using (Helpers.WebAPI oAPI = new Helpers.WebAPI(language: this.CurrentLanguage, existingToken: this.CurrentUser))
            {
                categories = await oAPI.GetMagazineCategoriesAsync();

                if (categories?.Categories == null || categories.Categories.Count <= 0)
                {
                    //-- ADD A DUMMY CATEGORY IF NO CATEGORIES ARE AVAILABLE --
                    categories = new MagazineLeadCategoriesDTO();
                    categories.Categories = new List<MagazineLeadCategoryDTO>();
                    categories.Categories.Add(new MagazineLeadCategoryDTO() { Client = "ZZ", Period = 999999, PeriodName = "", Total = 0 });

                    ViewBag.HASCATEGORIES = false;
                    ViewBag.DEFAULTCATEGORY = "999999-ZZ";
                }
                else
                {
                    //-- ONE OR MORE CATEGORIES WERE FOUND --
                    ViewBag.HASCATEGORIES = true;
                    ViewBag.DEFAULTCATEGORY = categories.Categories.First().Period.ToString() + "-" + categories.Categories.First().Client;
                }

                ViewBag.CATEGORIES = categories;
            }

            return View();
        }
        #endregion

        #region "CONTROLLER: RecipeClubCampaign"
        /// <summary>
        /// Display the Recipe Club Campaign Page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuRecipleClubCampaign)]
        public ActionResult RecipeClubCampaign()
        {
            return View();
        }
        #endregion

        #region "CONTROLLER: DownloadCenter"
        /// <summary>
        /// Display the download center
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuDownloadCenter)]
        public async Task<ActionResult> DownloadCenter(String defaultCategoryName = "", String defaultSubCategoryName = "")
        {
            IEnumerable<DocumentCategoryDTO> categories;
            String defaultCategoryID = "0";
            String defaultSubCategoryID = "";

            bool showDigitialImages = false;
            bool show5PLYDC = false;
            bool showRPDC = false;
            bool showINNOVEDC = false;
            bool setMenuAndTitle = false;

            //-- GET THE CATEGORIES --
            try
            {
                using (var oAPI = new Helpers.WebAPI(language: this.CurrentLanguage, existingToken: this.CurrentUser))
                {
                    categories = await oAPI.GetDownloadCenterCategoriesAsync();
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Trace.TraceError(ex.ToString());
                categories = null;
            } 

            //-- SET THE DEFAULT CATEGORY --
            if (categories == null || categories.Count() <= 0)
                defaultCategoryID = "-1";
            else
            {
                show5PLYDC = categories.Any(o => o.TopLevelCategory.ToUpper() == "DOWNLOADCENTER5PLY");
                showRPDC = categories.Any(o => o.TopLevelCategory.ToUpper() == "DOWNLOADCENTERRP");
                showINNOVEDC = categories.Any(o => o.TopLevelCategory.ToUpper() == "DOWNLOADCENTERINNOVE");
                showDigitialImages = categories.Any(o => o.TopLevelCategory.ToUpper() == "DIGIMAGESDOWNLOADS");
                setMenuAndTitle = !String.IsNullOrWhiteSpace(defaultCategoryName) || !String.IsNullOrWhiteSpace(defaultSubCategoryName);

                if (!String.IsNullOrWhiteSpace(defaultSubCategoryName))
                {
                    //-------------------------------------
                    //-- SELECT THE DEFAULT SUB CATEGORY --
                    //-------------------------------------
                    if (categories.Any(o => o.Category.ToUpper() == defaultSubCategoryName.ToUpper()))
                        defaultSubCategoryID = categories.First(o => o.Category.ToUpper() == defaultSubCategoryName.ToUpper()).CategoryID.ToString();
                    else
                        defaultCategoryID = "-1";
                }
                else if (!String.IsNullOrWhiteSpace(defaultCategoryName))
                {
                    //---------------------------------
                    //-- SELECT THE DEFAULT CATEGORY --
                    //---------------------------------
                    if (categories.Any(o => o.Category.ToUpper() == defaultCategoryName.ToUpper()))
                        defaultCategoryID = categories.First(o => o.Category.ToUpper() == defaultCategoryName.ToUpper()).CategoryID.ToString();
                    else
                        defaultCategoryID = "-1";
                }
                else if (String.IsNullOrWhiteSpace(ViewBag.Title))
                    setMenuAndTitle = true;
            }

            //-- SELECT THE MENU IF NECESSARY --
            if (setMenuAndTitle)
            {
                var mainMenu = this.MenuItems.FirstOrDefault(o => o.ItemName.ToUpper() == "TABSALESANDMARKETING");
                if (mainMenu != null)
                {
                    mainMenu.IsSelected = true;
                    var subMenu = mainMenu.Children.FirstOrDefault(o => o.ItemName.ToUpper() == "MNUDOWNLOADCENTER");
                    if (subMenu != null)
                    {
                        subMenu.IsSelected = true;
                        ViewBag.Title = subMenu.ItemText;
                    }
                    else
                        ViewBag.Title = LocalResources.HCResources.DownloadCenter;
                }
            }

            //-- SET THE VIEWBAG PROPERTIES --
            ViewBag.DEFAULTCATEGORY = defaultCategoryID;
            ViewBag.DEFAULTSUBCATEGORY = defaultSubCategoryID;

            ViewBag.SHOWDIGI = showDigitialImages;
            ViewBag.SHOW5PLY = show5PLYDC;
            ViewBag.SHOWINNOVE = showINNOVEDC;
            ViewBag.SHOWRP = showRPDC;

            return View("DownloadCenter", categories);
        }

        #region "CONTROLLER: DownloadCenter/RP"
        /// <summary>
        /// Default to a specific category or sub-category in the download center
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuDownloadCenter)]
        [Route("SalesAndMarketing/DownloadCenter/RP")]
        public async Task<ActionResult> RPDownloadCenter(string category)
        {
            //-- DISPLAY THE DOWNLOAD CENTER PAGE --
            return await DownloadCenter("DownloadCenterRP");
        }
        #endregion

        #region "CONTROLLER: DownloadCenter/5Ply"
        /// <summary>
        /// Default to a specific category or sub-category in the download center
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuDownloadCenter)]
        [Route("SalesAndMarketing/DownloadCenter/5Ply")]
        public async Task<ActionResult> FivePlyDownloadCenter(string category)
        {
            //-- DISPLAY THE DOWNLOAD CENTER PAGE --
            return await DownloadCenter("DownloadCenter5PLY");
        }
        #endregion

        #region "CONTROLLER: DownloadCenter/Innove"
        /// <summary>
        /// Default to a specific category or sub-category in the download center
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuDownloadCenter)]
        [Route("SalesAndMarketing/DownloadCenter/Innove")]
        public async Task<ActionResult> InnoveDownloadCenter(string category)
        {
            //-- DISPLAY THE DOWNLOAD CENTER PAGE --
            return await DownloadCenter("DownloadCenterInnove");
        }
        #endregion

        #region "CONTROLLER: DownloadCenter/4in14"
        /// <summary>
        /// Default to a specific category or sub-category in the download center
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuDownloadCenter)]
        [Route("SalesAndMarketing/DownloadCenter/4in14")]
        public async Task<ActionResult> FourInFourteenDownloadCenter(string category)
        {
            //-- DISPLAY THE DOWNLOAD CENTER PAGE --
            return await DownloadCenter("", "DIGRP4in14Program");
        }
        #endregion

        #region "CONTROLLER: GET: DownloadCenterDocument"
        /// <summary>
        /// Get a file linked to an individual announcement
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuDownloadCenter)]        
        public async Task<ActionResult> DownloadCenterDocument(Int32 id)
        {
            return await GetDocument(String.Format("Documents/Default.GetDownloadCenterDocument(id={0},language='{1}')", id, this.CurrentLanguage));
        }
        #endregion

        #endregion

        #region "CONTROLLER: CARP"
        /// <summary>
        /// Display the Cartridge Replacement Letter Page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuCARP)]
        public ActionResult CARP()
        {
            //-- BUILD THE CUSTOMER TYPE SELECTION LIST --
            List<SelectListItem> customerTypes = new List<SelectListItem>();
            customerTypes.Add(new SelectListItem() { Value = "A", Text = LocalResources.HCResources.ActiveCARPCustomers });
            customerTypes.Add(new SelectListItem() { Value = "I", Text = LocalResources.HCResources.InactiveCARPCustomers });
            ViewBag.CUSTOMERTYPES = new SelectList(customerTypes, "Value", "Text");

            //-- BUILD THE PERIOD SELECTION LIST --
            ViewBag.PERIODLIST = Helpers.Common.GetPeriodSelectList(this.CurrentCulture, true, 72);

            return View();
        }
        #endregion
    }
}
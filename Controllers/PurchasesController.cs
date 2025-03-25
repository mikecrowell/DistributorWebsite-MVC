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
    public class PurchasesController : BaseController
    {
        #region "CONTROLLER: JVPayments"
        /// <summary>
        /// Get the JV Payments
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuJVPayments)]
        public ActionResult JVPayments()
        {
            return View();
        }
        #endregion

        #region "CONTROLLER: TDMeetingFund"
        /// <summary>
        /// Get the TD Meeting fund details
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuTDMeetingFund)]
        public ActionResult TDMeetingFund()
        {
            return View();
        }
        #endregion
        
        #region "CONTROLLER: Territory"
        /// <summary>
        /// Get the territory purchases
        /// </summary>
        /// <returns></returns>
        // GET: Purchases/Territory
        [HCAuthorizeMenu(eSecurityItem.mnuTerritoryPurchases)]
        public ActionResult Territory()
        {
            ViewBag.APIENTITYPATH = "Purchases/Default.TerritoryPurchases(period='[PERIOD]',range='[TYPE]',from='[FROMDATE]',to='[TODATE]',datetype='[DATETYPE]',carp='[CARP]',sample='[SAMPLE]',bulk='[BULK]',zerobal='[ZEROBAL]')";
            ViewBag.EXPORTACTION = "TerritoryPurchases";
            return View("Purchases");
        }
        #endregion

        #region "CONTROLLER: Company"
        /// <summary>
        /// Get the company purchases
        /// </summary>
        /// <returns></returns>
        // GET: Purchases/Company
        [HCAuthorizeMenu(eSecurityItem.mnuCompanyPurchases)]
        public ActionResult Company()
        {
            ViewBag.APIENTITYPATH = "Purchases/Default.CompanyPurchases(period='[PERIOD]',range='[TYPE]',from='[FROMDATE]',to='[TODATE]',datetype='[DATETYPE]',carp='[CARP]',sample='[SAMPLE]',bulk='[BULK]',zerobal='[ZEROBAL]')";
            ViewBag.EXPORTACTION = "CompanyPurchases";
            return View("Purchases");
        }
        #endregion

        #region "CONTROLLER: JV"
        /// <summary>
        /// Get the JV purchases
        /// </summary>
        /// <returns></returns>
        // GET: Purchases/JV
        [HCAuthorizeMenu(eSecurityItem.mnuJVPurchases)]
        public ActionResult JV()
        {
            ViewBag.APIENTITYPATH = "Purchases/Default.JVPurchases(period='[PERIOD]',range='[TYPE]',from='[FROMDATE]',to='[TODATE]',datetype='[DATETYPE]',carp='[CARP]',sample='[SAMPLE]',bulk='[BULK]',zerobal='[ZEROBAL]')";
            ViewBag.EXPORTACTION = "JVPurchases";
            return View("Purchases");
        }
        #endregion
    }
}
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
    public class SalesLeadAdministrationController : BaseController
    {
        #region "CONTROLLER: PossibleLeads"
        /// <summary>
        /// Display the Possible leads page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuPossibleLeads)]
        public ActionResult PossibleLeads()
        {
            return View();
        }
        #endregion

        #region "CONTROLLER: PurchasedLeads"
        /// <summary>
        /// Display the Purchased leads page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuPurchasedLeads)]
        public ActionResult PurchasedLeads()
        {
            return View();
        }
        #endregion
    }
}
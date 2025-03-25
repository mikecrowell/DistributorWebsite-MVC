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
    public class OverridesController : BaseController
    {
        #region "CONTROLLER: Overrides"
        /// <summary>
        /// Get the Quarterly Overrides
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuMonthlyOverrides)]
        public ActionResult MonthlyOverrides()
        {
            ViewBag.PENDINGSESSIONNOTE = String.Format(LocalResources.HCResources.OverrideProcessingNote, GetPendingSessionCallDate(this.CurrentCulture.ToLower()));
            return View();
        }
        #endregion

        #region "CONTROLLER: EligiblePurchases"
        /// <summary>
        /// Display the Eligible Purchases page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuReserveAdjustment)]
        public ActionResult EligiblePurchases()
        {
            return View();
        }
        #endregion
       
        #region "CONTROLLER: Memos"
        /// <summary>
        /// Display the Reserve Adjustment Memos page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuReserveAdjustmentMemos)]
        public ActionResult Memos()
        {
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
        [HCAuthorizeMenu(eSecurityItem.mnuReserveAdjustmentMemos)]
        public async Task<ActionResult> Memo(Int32 id)
        {
            return await GetDocument(String.Format("Documents/Default.GetReserveAdjustmentMemo(id='{0}')", id));
        }
        #endregion

        #region "HELPER FUNCTIONS"

        #region "FUNCTION: GetPendingSessionCallDate"
        /// <summary>
        /// Get the pending session call date
        /// </summary>
        /// <returns></returns>
        private string GetPendingSessionCallDate(String culture)
        {
            DateTime curDay;

            try
            {
                curDay = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 13);
                while (curDay.DayOfWeek == DayOfWeek.Saturday || curDay.DayOfWeek == DayOfWeek.Sunday)
                {
                    curDay = curDay.AddDays(1);
                }

                return curDay.ToString("dddd, MMMM d, yyyy", new System.Globalization.CultureInfo(culture));
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return "";
            }
        }
        #endregion

        #endregion
    }
}
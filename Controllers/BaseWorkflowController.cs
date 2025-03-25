using DistributorWebsite.MVC.WebUI.Helpers.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace DistributorWebsite.MVC.WebUI.Controllers
{
    /// <summary>
    /// Provide a base controller that can be used by workflow pages that can make use of shared workflow functionality
    /// </summary>
    public class BaseWorkflowController : BaseController
    {
        #region "CONTROLLER: WorkflowBase"
        /// <summary>
        /// Get the customer workflow
        /// </summary>
        /// <returns></returns>
        // GET: Workflow/Customer
        protected ActionResult WorkflowBase(String viewName, eSecurityItem secManageWorkflow, eSecurityItem secMakePayments, Int32? orderno = null, String client = null, bool showStatusMessage = false, bool showPayments = false, bool? showHeader = null)
        {
            String supportedClients = "";

            //-- SET WHETHER OR NOT TO DISPLAY THE PAYMENTS TAB --
            ViewBag.SHOWPAYMENTSTAB = showPayments;

            if (orderno.HasValue && orderno.Value > 0 && !String.IsNullOrWhiteSpace(client))
            {
                //-- OPEN UP THE ORDER THAT WAS SUPPLIED --
                ViewBag.INITIALORDERNO = orderno.Value;
                ViewBag.INITIALORDERCLIENT = client;
                ViewBag.AUTOINIT = false;
            }
            else
            {
                //-- DEFAULT TO THE WORKFLOW LIST --
                ViewBag.INITIALORDERNO = 0;
                ViewBag.INITIALORDERCLIENT = "";
                ViewBag.AUTOINIT = true;
            }

            ViewBag.CANENTERNOTES = Helpers.Common.CanUserAccessSecurityItem(secManageWorkflow);

            //-- BUILD THE LIST OF CLIENTS SUPPORTED BY THE WORKFLOW PAYMENT PAGE --
            foreach (var clientInfo in Helpers.Common.GetSecurityItemClients(secMakePayments, false))
            {
                supportedClients += (supportedClients != "" ? " && " : "") + "entity.Client != '" + clientInfo.Value + "'";
            }

            if (string.IsNullOrWhiteSpace(supportedClients))
                supportedClients = "true";

            ViewBag.APPLYPAYMENTSNGHIDE = supportedClients;

            //-- INITIALIZE THE CREDIT CARD MODEL --
            Helpers.Common.InitializePaymentFromModel(ePaymentPageHost.CustomerWorkflow, this, false, true, true, true, true, secMakePayments, false, showHeader);

            return View(viewName);
        }
        #endregion
    }
}
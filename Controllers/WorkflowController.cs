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
    public class WorkflowController : BaseWorkflowController
    {
        #region "CONTROLLER: Customer"
        /// <summary>
        /// Get the customer workflow
        /// </summary>
        /// <returns></returns>
        // GET: Workflow/Customer
        [HCAuthorizeMenu(eSecurityItem.mnuCustomerWorkflow)]
        public ActionResult Customer(Int32? orderno = null, String client = null, bool showStatusMessage = false, bool showPayments = false)
        {
            ViewBag.ShowCBReportLinks = Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.mnuCreditReports, this.CurrentUser);
            ViewBag.SHOWDEPOSITCOLOR = false;
            ViewBag.ENTITYPATH = "";
            ViewBag.HIDEPAYMENTS = !Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opMakeCustomerWorkflowPayments);
            return WorkflowBase("Customer", eSecurityItem.opManageCustomerWorkflow, eSecurityItem.opMakeCustomerWorkflowPayments, orderno, client, showStatusMessage, showPayments, false);
        }
        #endregion       

        #region "CONTROLLER: Distributor"
        /// <summary>
        /// Get the distributor workflow
        /// </summary>
        /// <returns></returns>
        // GET: Workflow/Distributor
        [HCAuthorizeMenu(eSecurityItem.mnuDistributorWorkflow)]
        public ActionResult Distributor(Int32? orderno = null, String client = null)
        {
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

            ViewBag.CANENTERNOTES = Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opManageDistributorWorkflow);
            ViewBag.SHOWDEPOSITSTATUS = Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewDepositPaid);
            ViewBag.CANMARKDEPOSITPAID = Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opMarkDepositPaid) && this.CurrentUser.IsInternal;
            ViewBag.ENTITYPATH = "";
            ViewBag.SHOWDEPOSITCOLOR = false;

            return View();
        }
        #endregion

        #region "CONTROLLER: Returns"
        /// <summary>
        /// Get the returns workflow
        /// </summary>
        /// <returns></returns>
        // GET: Workflow/Distributor
        [HCAuthorizeMenu(eSecurityItem.mnuReturnsWorkflow)]
        public ActionResult Returns(Int32? returnno = null, String client = null)
        {
            if (returnno.HasValue && returnno.Value > 0 && !String.IsNullOrWhiteSpace(client))
            {
                //-- OPEN UP THE ORDER THAT WAS SUPPLIED --
                ViewBag.INITIALORDERNO = returnno.Value;
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

            ViewBag.CANENTERNOTES = false;

            return View();
        }
        #endregion

        #region "CONTROLLER: CreditReports"
        /// <summary>
        /// Display the credit reports page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuCreditReports)]
        public ActionResult CreditReports()
        {
            return View();
        }
        #endregion

        #region "CONTROLLER: WarehouseSalesOrders"
        /// <summary>
        /// Display the warehouse sales orders page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuWarehouseSalesOrders)]
        public ActionResult WarehouseSalesOrders()
        {
            return View();
        }
        #endregion

        #region "CONTROLLER: OPDepositSlipNeeded"
        /// <summary>
        /// Display the Consumer Deposit Slip Needed page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuOPDepositSlipNeeded)]
        public ActionResult OPDepositSlipNeeded()
        {
            ViewBag.ShowCBReportLinks = Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.mnuCreditReports, this.CurrentUser);
            ViewBag.SHOWDEPOSITCOLOR = true;
            ViewBag.ENTITYPATH = "/Default.DepositSlipRequired";
            ViewBag.HIDEPAYMENTS = true;
            return WorkflowBase("Customer", eSecurityItem.opManageCustomerWorkflow, eSecurityItem.opMakeCustomerWorkflowPayments, null, null, false, false);
        }
        #endregion

        #region "CONTROLLER: DCSDepositSlipNeeded"
        /// <summary>
        /// Display the Distributor Deposit Slip Needed page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuDCSDepositSlipNeeded)]
        public ActionResult DCSDepositSlipNeeded()
        {
            ViewBag.INITIALORDERNO = 0;
            ViewBag.INITIALORDERCLIENT = "";
            ViewBag.AUTOINIT = true;
            ViewBag.CANENTERNOTES = Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opManageDistributorWorkflow);
            ViewBag.SHOWDEPOSITSTATUS = Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opViewDepositPaid);
            ViewBag.CANMARKDEPOSITPAID = Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opMarkDepositPaid) && this.CurrentUser.IsInternal;
            ViewBag.ENTITYPATH = "/Default.DepositSlipRequired";
            ViewBag.SHOWDEPOSITCOLOR = true;

            return View("Distributor");
        }
        #endregion

        #region "CONTROLLER: CreditCardPayments"
        /// <summary>
        /// Get the customer workflow
        /// </summary>
        /// <returns></returns>
        // GET: Workflow/Customer
        [HCAuthorizeMenu(eSecurityItem.mnuCreditCardProcessing)]
        public ActionResult CreditCardPayments()
        {
            //-- INITIALIZE THE CREDIT CARD MODEL --
            Helpers.Common.InitializePaymentFromModel(ePaymentPageHost.CreditCardProcessing, this, true, securityItem: eSecurityItem.mnuCreditCardProcessing, showPaymentEntryHeader: false);

            return View("CreditCardPayments");
        }
        #endregion

        #region "CONTROLLER: DocumentUploads"
        /// <summary>
        /// Display the Document Uploads page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuDocumentUploads)]
        public async Task<ActionResult> DocumentUploads(bool showLicenseError = false)
        {
            Int32 totalOrdersPendingApproval = 0;
            bool canApproveOrders;
            bool canMakePayments;
            string apiurl;

            List<SelectListItem> emptyList = new List<SelectListItem>();
            List<SelectListItem> orderTypeList = new List<SelectListItem>();
            List<SelectListItem> orderTypeBZList = new List<SelectListItem>();
            List<SelectListItem> orderModeList = new List<SelectListItem>();
            List<SelectListItem> deliveredByDistrList = new List<SelectListItem>();
            List<SelectListItem> languageList = new List<SelectListItem>();
            List<SelectListItem> stateList = new List<SelectListItem>();
            List<SelectListItem> stateMXList = new List<SelectListItem>();
            List<SelectListItem> stateBZList = new List<SelectListItem>();
            List<SelectListItem> phoneOptionList = new List<SelectListItem>();
            List<SelectListItem> eSignClients = new List<SelectListItem>();
            List<SelectListItem> witnessList = new List<SelectListItem>();


            phoneOptionList.Add(new SelectListItem() { Text = "", Value = "" });
            phoneOptionList.Add(new SelectListItem() { Text = LocalResources.HCResources.CellPhone, Value = "HomeCell" });
            phoneOptionList.Add(new SelectListItem() { Text = LocalResources.HCResources.WorkPhone, Value = "Work" });
            ViewBag.PhoneOptionList = new SelectList(phoneOptionList, "Value", "Text");

            orderTypeList.Add(new SelectListItem() { Text = "", Value = "" });
            orderTypeList.Add(new SelectListItem() { Text = LocalResources.HCResources.CashOrder, Value = "Cash" });
            orderTypeList.Add(new SelectListItem() { Text = LocalResources.HCResources.FinanceOrderNoCosigner, Value = "Financed Order (No Cosigner)" });
            orderTypeList.Add(new SelectListItem() { Text = LocalResources.HCResources.FinanceOrderWithCosigner, Value = "Financed Order (With Cosigner)" });
            orderTypeList.Add(new SelectListItem() { Text = LocalResources.HCResources.AddOnOrder, Value = "AddOn" });
            ViewBag.OrderTypeList = new SelectList(orderTypeList, "Value", "Text");


            orderTypeBZList.Add(new SelectListItem() { Text = "", Value = "" });
            orderTypeBZList.Add(new SelectListItem() { Text = LocalResources.HCResources.CashOrder, Value = "Cash" });
            orderTypeBZList.Add(new SelectListItem() { Text = LocalResources.HCResources.FinanceOrderNoCosigner, Value = "Financed Order (No Cosigner)" });
            orderTypeBZList.Add(new SelectListItem() { Text = LocalResources.HCResources.FinanceOrderWithCosigner, Value = "Financed Order (With Cosigner)" });
            orderTypeBZList.Add(new SelectListItem() { Text = LocalResources.HCResources.FinancedAddOnOrderNoCosigner, Value = "Financed Add-On (No Cosigner)" });
            orderTypeBZList.Add(new SelectListItem() { Text = LocalResources.HCResources.FinancedAddOnOrderWithCosigner, Value = "Financed Add-On (With Cosigner)" });
            orderTypeBZList.Add(new SelectListItem() { Text = LocalResources.HCResources.CreditCardFinancing, Value = "Credit Card Financing" });
            ViewBag.OrderTypeBZList = new SelectList(orderTypeBZList, "Value", "Text");

            orderModeList.Add(new SelectListItem() { Text = "", Value = "" });
            orderModeList.Add(new SelectListItem() { Text = LocalResources.HCResources.New, Value = "New" });
            orderModeList.Add(new SelectListItem() { Text = LocalResources.HCResources.Resubmit, Value = "Resubmit" });
            ViewBag.OrderModeList = new SelectList(orderModeList, "Value", "Text");

            deliveredByDistrList.Add(new SelectListItem() { Text = "", Value = "" });
            deliveredByDistrList.Add(new SelectListItem() { Text = LocalResources.HCResources.No, Value = "No" });
            deliveredByDistrList.Add(new SelectListItem() { Text = LocalResources.HCResources.Yes, Value = "Yes" });
            deliveredByDistrList.Add(new SelectListItem() { Text = LocalResources.HCResources.Partial, Value = "Partial" });
            ViewBag.DeliveredByDistrList = new SelectList(deliveredByDistrList, "Value", "Text");

            languageList.Add(new SelectListItem() { Text = "", Value = "" });
            languageList.Add(new SelectListItem() { Text = LocalResources.HCResources.English, Value = "EN" });
            languageList.Add(new SelectListItem() { Text = LocalResources.HCResources.Spanish, Value = "ES" });
            ViewBag.LanguageList = new SelectList(languageList, "Value", "Text");

            witnessList.Add(new SelectListItem() { Text = LocalResources.HCResources.Yes, Value = "Yes" });
            witnessList.Add(new SelectListItem() { Text = LocalResources.HCResources.No, Value = "No" });
            ViewBag.WitnessList = new SelectList(witnessList, "Value", "Text");

            ViewBag.SalespersonList = new SelectList(emptyList, "Value", "Text");
                       

            //-- GET THE TOTAL NUMBER OF ORDER PENDING APPROVAL --
            canApproveOrders = Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opApproveMobileOrders, this.CurrentUser);
            if (canApproveOrders)
            {
                try
                {
                    using (var api = new Helpers.WebAPI(language: this.CurrentLanguage, existingToken: this.CurrentUser))
                    {
                        totalOrdersPendingApproval = await api.GetTotaOrdersPendingApproval();
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Trace.TraceError(ex.ToString());
                    totalOrdersPendingApproval = 0;
                }
            }

            ViewBag.IsCreateEsignatureDisabled = true;
            var token = Helpers.Common.GetCurrentEntityToken();
            if (token != null)
            {
                ViewBag.IsCreateEsignatureDisabled = token.IsCreateEsignatureDisabled;
            }
            
            //-- GET WHETHER OR NOT THE CURRENT USER CAN MAKE PAYMENTS --
            canMakePayments = Helpers.Common.CanUserAccessSecurityItem(eSecurityItem.opMakeMobilePayments, this.CurrentUser);
            ViewBag.CanMakePayments = canMakePayments;

            //-- INITIALIZE THE CREDIT CARD MODEL --
            Helpers.Common.InitializePaymentFromModel(ePaymentPageHost.CreditCardProcessing, this, true, true, true, true, true, eSecurityItem.opMakeMobilePayments, false, false, allowEmailReceipt: true, paymentSource: "DocuCite");

            //-- FLAG THE LICENSE ERROR DISPLAY IF APPLICABLE --            
            if (showLicenseError)
                Helpers.CookieHelper.ShowLicenseError = true;

            ViewBag.DistributorList = new SelectList(emptyList, "Value", "Text");
            ViewBag.CanApproveOrders = canApproveOrders;
            ViewBag.TotalPendingOrders = totalOrdersPendingApproval;

            //-- BUILD THE LIST OF VALID CLIENTS --
            var clients = Helpers.Common.GetSecurityItemClientString(eSecurityItem.opMakeMobilePayments);
            String paymentClients = "";
            foreach (string client in clients.Split('|'))
            {
                paymentClients += (paymentClients == "" ? "|| (" : " && ") + "entity.client !== '" + client.Split('^')[0] + "'";
            }
            if (!String.IsNullOrWhiteSpace(paymentClients)) paymentClients += ")";
            ViewBag.NGPaymentClients = paymentClients;

            
            using (var api = new Helpers.WebAPI(existingToken: this.CurrentUser))
            {
                var clientValues = await api.GetEsignClients();
                if (clientValues != null && clientValues.Clients != null && clientValues.Clients.Length > 0)
                {
                    foreach (string client in clients.Split('|'))
                    {
                        string value = client.Split('^')[0];
                        string text = client.Split('^')[1];

                        foreach (ESignClient eClient in clientValues.Clients)
                        {
                            if(eClient.Client == value)
                            {
                               eSignClients.Add(new SelectListItem() { Text = text, Value = value });
                               break;
                            }
                        }
                    }
                }
            }

            if(eSignClients.Count == 0)
            {
                ViewBag.IsCreateEsignatureDisabled = true;
            }
            ViewBag.eSignClientList = new SelectList(eSignClients, "Value", "Text");


            foreach (SelectListItem client in eSignClients)
            {
                if (client.Value == "HC")
                {
                    stateList.Add(new SelectListItem() { Text = "", Value = "" });
                    using (var api = new Helpers.WebAPI(existingToken: this.CurrentUser))
                    {
                        var states = await api.GetStates("US");
                        foreach (var state in states)
                        {
                            stateList.Add(new SelectListItem() { Text = state.StateName, Value = state.StateAbbreviation });
                        }
                    }
                   
                }

                if (client.Value == "MX")
                {
                    stateMXList.Add(new SelectListItem() { Text = "", Value = "" });
                    using (var api = new Helpers.WebAPI(existingToken: this.CurrentUser))
                    {
                        var states = await api.GetStates("MX");
                        foreach (var state in states)
                        {
                            stateMXList.Add(new SelectListItem() { Text = state.StateName, Value = state.StateAbbreviation });
                        }
                    }
                   
                }

                if (client.Value == "BZ")
                {
                    stateBZList.Add(new SelectListItem() { Text = "", Value = "" });
                    using (var api = new Helpers.WebAPI(existingToken: this.CurrentUser))
                    {
                        var states = await api.GetStates("BR");
                        foreach (var state in states)
                        {
                            stateBZList.Add(new SelectListItem() { Text = state.StateName, Value = state.StateAbbreviation });
                        }
                    }
                    
                }
            }

            ViewBag.StateList = new SelectList(stateList, "Value", "Text");
            ViewBag.StateMXList = new SelectList(stateMXList, "Value", "Text");
            ViewBag.StateBZList = new SelectList(stateBZList, "Value", "Text");



            //-- INITIALIZE THE SCANNER API URLS --
            apiurl = (new Uri(Helpers.Application.Instance.APIS.DistributorWebAPI)).GetLeftPart(UriPartial.Authority).ToLower();
            ViewBag.ScannerInterfaceURL = $"{apiurl}/GetWingScanUI";
            ViewBag.ScannerHandlerURL = $"{apiurl}/InitializeWingScan";
            ViewBag.Origin = apiurl;
            ViewBag.RemoteOrigin = Request.Url.GetLeftPart(UriPartial.Authority).ToLower();
            ViewBag.IsSalesperson = this.CurrentUser.IsSalesperson;

            return View("DocumentUploads");
        }
        #endregion

        #region "CONTROLLER: ScannerIframe"
        /// <summary>
        /// Scanner iframe view (for testing)
        /// </summary>
        /// <returns></returns>
        public ActionResult ScannerIframe()
        {
            return View();
        }
        #endregion

        #region "CONTROLLER: GET: GetMobileOrderDocument"
        /// <summary>
        /// Get a mobile order document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        [Route("Workflow/DocumentUploads/Order/{orderNo:int}/Document/{documentId:int}")]
        [HCAuthorizeMenu(eSecurityItem.mnuDocumentUploads)]
        public async Task<ActionResult> GetMobileOrderDocument(Int32 orderNo, Int32 documentId)
        {
            return await GetDocument($"MobileOrders({orderNo})/Default.Document(documentid={documentId})");
        }
        #endregion

        #region "CONTROLLER: GET: GetPendingDocument"
        /// <summary>
        /// Get a mobile order document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        [Route("Workflow/DocumentUploads/PendingDocument/{documentId:alpha}")]
        [HCAuthorizeMenu(eSecurityItem.mnuDocumentUploads)]
        public async Task<ActionResult> GetMobileOrderPendingDocument(String documentId)
        {
            return await GetDocument($"MobileOrders/Default.PendingDocument(documentid='{documentId}')");
        }
        #endregion

        #region "CONTROLLER: GET: SalespersonCommissions"
        /// <summary>
        /// Display the Transmittals page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuSalespersonCommissions)]
        public ActionResult SalespersonCommissions()
        {
            return View();
        }
        #endregion

        [Route("Workflow/Customer/CreditReport/{orderNo:int}/{client:alpha}/{cbtype:alpha}")]
        #region "FUNCTION: DisplayMemo"
        // GET: /Company/Memos
        public async Task<ActionResult> CreditReport(Int32 orderNo, String client = null, String cbtype = null)
        {
            ODataResponse<byte[]> response;
            Helpers.WebAPI.eCreditReportType type;

            //-- GET THE SPECIFIC REPORT ID --
            try
            {
                switch (cbtype.ToUpper())
                {
                    case "CUSTOMER":
                        type = Helpers.WebAPI.eCreditReportType.Customer;
                        break;

                    case "COSIGNOR":
                        type = Helpers.WebAPI.eCreditReportType.Cosignor;
                        break;

                    default:
                        throw new ArgumentException(String.Format("{0} is not a valid credit report type - please specify Customer or Cosignor", cbtype));
                }

                using (Helpers.WebAPI oAPI = new Helpers.WebAPI(language: this.CurrentLanguage, existingToken: this.CurrentUser))
                {
                    response = await oAPI.GetCreditReport(orderNo, client, type);
                }

                if ((response == null) || (response.Value == null) || (response.Value.Length <= 0))
                {
                    //-- THE REPORT COULD NOT BE FOUND OR WASN'T ACCESSIBLE --
                    throw new HttpException(404, "Not Found");
                }
                else
                {
                    //-- STREAM THE MEMO TO THE BROWSER --
                    return this.GetStaticFileResponse(response.Value, "txt", String.Format("CB-{0}-{1}-{2}.txt", cbtype, orderNo, client));
                }
            }
            catch (Exception)
            {
                throw;
            }

            throw new HttpException(404, "Not Found");
        }
        #endregion
    }
}
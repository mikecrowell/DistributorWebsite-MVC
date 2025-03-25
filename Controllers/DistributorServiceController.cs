using DistributorWebsite.MVC.WebUI.Helpers.Models;
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
    public class DistributorServiceController : BaseController
    {
        #region "CONTROLLER: Transmittals"
        /// <summary>
        /// Display the Transmittals page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuTransmittals)]
        public ActionResult Transmittals()
        {
            return View();
        }
        #endregion

        #region "CONTROLLER: TransmittalSummary"
        /// <summary>
        /// Display the Transmittal Summary page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuTransmittalSummary)]
        public ActionResult TransmittalSummary()
        {
            return View();
        }
        #endregion

        #region "CONTROLLER: ShippingSearch"
        /// <summary>
        /// Display the Shipping Search page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuShippingSearch)]
        public ActionResult ShippingSearch()
        {
            ViewBag.SHOWREMITONO = Helpers.Common.GetCurrentEntityToken().Clients.Any(o => o.ToUpper() == "BA");
            return View();
        }
        #endregion

        #region "CONTROLLER: Backorders"
        /// <summary>
        /// Display the Backorders page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuBackorders)]
        public ActionResult Backorders()
        {
            return View();
        }
        #endregion

        #region "CONTROLLER: COTaxFund"
        /// <summary>
        /// Display the COTAXFUNDS report
        /// </summary>
        /// <returns></returns>

        [HCAuthorizeMenu(eSecurityItem.mnuCOTaxFactors)]
        public ActionResult COTaxFactor()
        {
            return View();
        }

        #endregion

        #region "CONTROLLER: JDCOTaxFund"
        /// <summary>
        /// Display the JDCOTAXFUNDS report
        /// </summary>
        /// <returns></returns>

        [HCAuthorizeMenu(eSecurityItem.mnuDistributorCOTaxFactor)]
        public ActionResult JDCOTaxFactor()
        {
            return View();
        }

        #endregion

        #region "CONTROLLER: Pricelist"
        /// <summary>
        /// Display the Price list page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuPricelist)]
        public ActionResult Pricelist()
        {
            Int32 minPriceLevel;

            //-- GET THE MINIMUM PRICE LEVEL --
            if (!this.CurrentUser.DistributorLevel.HasValue || this.CurrentUser.DistributorLevel.Value < 1 || this.CurrentUser.DistributorLevel.Value > 4)
                minPriceLevel = 999;
            else
                minPriceLevel = this.CurrentUser.DistributorLevel.Value;
            ViewBag.MINPRICELEVEL = this.CurrentUser.DistributorLevel;

            //-- ADD THE CLIENTS --
            ViewBag.CLIENTLIST = this.CurrentUser.Clients;

            return View();
        }
        #endregion

        #region "CONTROLLER: TaxLookup"
        /// <summary>
        /// Display the Tax Lookup page
        /// Uses same security as the Sales Tax Report
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuTaxLookup)]
        public async Task<ActionResult> TaxLookup()
        {
            ViewBag.ListUSStates = await Helpers.Common.GetUSStateWithSalesTaxRatesSelectionList(this.CurrentUser);
            ViewBag.CurrentState = this.CurrentUser.State;
            ViewBag.GenericSelectList = Helpers.Common.GetGenericSelectList();
            ViewBag.CurrentClient = this.CurrentUser.DefaultClient;
            ViewBag.CurrentCountry = this.CurrentUser.CountryCode;
            ViewBag.DistributorNo = this.CurrentUser.DistributorNo;

            return View();
        }
        #endregion

        #region "CONTROLLER: PriceQuote"
        /// <summary>
        /// Display the Price Quote page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuPriceQuote)]
        public ActionResult PriceQuote()
        {
            List<SelectListItem> orderTypes = new List<SelectListItem>();
            List<SelectListItem> deliveryMethods = new List<SelectListItem>();

            //-- BUILD THE ORDER TYPE SELECT LIST --
            orderTypes.Add(new SelectListItem() { Value = "", Text = "" });
            orderTypes.Add(new SelectListItem() { Value = "F", Text = LocalResources.HCResources.Financed });
            orderTypes.Add(new SelectListItem() { Value = "C", Text = LocalResources.HCResources.ConsumerCash });
            orderTypes.Add(new SelectListItem() { Value = "D", Text = LocalResources.HCResources.DistributorCash });
            ViewBag.ORDERTYPES = new SelectList(orderTypes, "Value", "Text");

            //-- BUILD THE DELIVERY METHOD SELECT LIST --
            deliveryMethods.Add(new SelectListItem() { Value = "", Text = "" });
            deliveryMethods.Add(new SelectListItem() { Value = "DG", Text = LocalResources.HCResources.DHLGround });
            deliveryMethods.Add(new SelectListItem() { Value = "DL", Text = LocalResources.HCResources.DHLLetter });
            ViewBag.DELIVERYMETHODS = new SelectList(deliveryMethods, "Value", "Text");

            return View();
        }
        #endregion

        #region "CONTROLLER: WebOrders"
        /// <summary>
        /// Display the Web Orders page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuWebOrders)]
        public ActionResult WebOrders()
        {
            ViewBag.ShowShoppingCart = false;
            ViewBag.ShoppingCartClient = "";

            return View();
        }
        #endregion

        #region "CONTROLLER: ShoppingCart"
        /// <summary>
        /// View the shopping cart for a specific client
        /// </summary>
        /// <param name="client"></param>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuWebOrders)]
        public ActionResult ShoppingCart(String client)
        {
            ViewBag.ShowShoppingCart = !String.IsNullOrWhiteSpace(client);
            ViewBag.ShoppingCartClient = client;

            return View("WebOrders");
        }
        #endregion

        #region "CONTROLLER: PreQualificationHotline"
        /// <summary>
        /// Display the Pre-Qualification Hotline page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuPreQualificationHotline)]
        public ActionResult PreQualificationHotline()
        {
            return View();
        }
        #endregion

        #region "CONTROLLER: FacturaSearch"
        /// <summary>
        /// Display the Factura Search page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuFacturaSearch)]
        public ActionResult FacturaSearch()
        {
            return View();
        }
        #endregion

        #region "CONTROLLER: GET: GetInvoice"
        /// <summary>
        /// Get a file linked to an individual announcement
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuFacturaSearch)]
        public async Task<ActionResult> Factura(String id, String format)
        {
            bool isDownloadLink = (format.ToUpper() == "XML");

            return await GetDocument(String.Format("Invoices/Default.GetInvoice(id='{0}',format='{1}')", id, format), isDownloadLink);
        }
        #endregion

        #region "CONTROLLER: AccessPoints"
        /// <summary>
        /// Display the UPS Access Points page
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuAccessPoints)]
        public ActionResult AccessPoints()
        {
            List<SelectListItem> countries = new List<SelectListItem>();

            //-- BUILD THE LIST OF COUNTRIES THE USER CAN SEARCH FOR ACCESS POINTS IN --
            if (this.CurrentUser.Clients.Any(o => o.ToUpper() == "HC"))
                countries.Add(new SelectListItem() { Value = "US", Text = "United States" });

            if (this.CurrentUser.Clients.Any(o => o.ToUpper() == "RC"))
                countries.Add(new SelectListItem() { Value = "CA", Text = "Canada" });

            if (countries.Count <= 0)
            {
                //-- NOT AUTHORIZED --
                TempData["errormessage"] = "User is not set up in US or Canada";

                return new RedirectToRouteResult(new RouteValueDictionary
                                                 {
                                                    { "action", "Forbidden" },
                                                    { "controller", "Error" }
                                                 });
            }

            ViewBag.COUNTRIES = new SelectList(countries, "Value", "Text");

            return View();
        }
        #endregion

        #region "CONTROLLER: TDMeetingFundDetails"
        /// <summary>
        /// Get the TD Meeting fund details
        /// </summary>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuTDMeetingFundDetails)]
        public ActionResult TDMeetingFundDetails()
        {
            return View();
        }
        #endregion

        #region "CONTROLLER: AccountActivityPayments"
        /// <summary>
        /// Get the account activity payments
        /// </summary>
        /// <returns></returns>
        // GET: DistributorService/AccountActivityPayments
        [HCAuthorizeMenu(eSecurityItem.mnuAccountActivityPayments)]
        public ActionResult AccountActivityPayments()
        {
            //-- PAYMENT TYPE --
            List<SelectListItem> statusList = new List<SelectListItem>();
            statusList.Add(new SelectListItem() { Text = "", Value = "" });
            statusList.Add(new SelectListItem() { Text = LocalResources.HCResources.TransmittalPayment, Value = "T" });
            statusList.Add(new SelectListItem() { Text = LocalResources.HCResources.OrderPayment, Value = "O" });
            ViewBag.PAYMENTTYPE = new SelectList(statusList, "Value", "Text");

            //-- PAYMENT ACCOUNT TYPE --
            List<SelectListItem> paymentAccountTypes = new List<SelectListItem>();
            paymentAccountTypes.Add(new SelectListItem() { Text = "", Value = "" });
            paymentAccountTypes.Add(new SelectListItem() { Text = LocalResources.HCResources.PaymentAccountTypeNew, Value = "NEW" });
            paymentAccountTypes.Add(new SelectListItem() { Text = LocalResources.HCResources.PaymentAccountTypeExisting, Value = "ONFILE" });
            ViewBag.PAYMENTACCOUNTTYPE = new SelectList(paymentAccountTypes, "Value", "Text");

            //-- INITIALIZE API URLS --
            ViewBag.APINEWCC = $"{Helpers.Application.Instance.APIS.DistributorWebAPI}InitializeTransmittalPayment";
            ViewBag.APISUBMITPAYMENT = $"{Helpers.Application.Instance.APIS.DistributorWebAPI}SubmitPayment";
            ViewBag.APICANCELSCHEDULEDPAYMENT = $"{Helpers.Application.Instance.APIS.DistributorWebAPI}CancelScheduledPayment";

            //-- SECURITY SETTINGS --
            ViewBag.CanApplyPayments = ((Environment.UserDomainName == "DEV") || (Helpers.Application.Instance.IsRunningLocal) || (!this.EntityAccessToken.IsInternal));
            ViewBag.CanViewScheduledPayments = this.EntityAccessToken.Clients.Contains("HC") || this.EntityAccessToken.Clients.Contains("RC");

            //-- CUSTOMER NAME --
            ViewBag.CustomerName = ViewBag.DisplayName;

            return View();
        }
        #endregion

        #region "CONTROLLER: GET: TDMeetingFundReceipt"
        /// <summary>
        /// Get the document
        /// </summary>
        /// <param name="id"></param>
        /// <param name="language"></param>
        /// <returns></returns>
        [HCAuthorizeMenu(eSecurityItem.mnuTDMeetingFundDetails)]
        // GET: /DistributorService/TDMeetingFundReceipt
        public async Task<ActionResult> TDMeetingFundReceipt(String client, Int32 id, Int32 seq, String docID)
        {
            return await GetDocument($"Distributors/Default.TDMeetingFundReceipt(Client='{client}',ID={id},Seq={seq},DocID='{docID}')");
        }
        #endregion
    }
}
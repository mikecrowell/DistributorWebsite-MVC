using DistributorWebsite.MVC.WebUI.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace DistributorWebsite.MVC.WebUI.Controllers
{
    public class ReportsController : BaseController
    {
        #region "FUNCTION: TerritoryPurchases"
        /// <summary>
        /// Generate the territory purchases report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.mnuTerritoryPurchases)]
        [Route("Reports/TerritoryPurchases")]
        public async Task<ActionResult> TerritoryPurchases()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary("treetype");
                parameters.Add("treetype", "ADMIN");
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.TerritoryPurchases(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "TerritoryPurchases");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }
 
        }
        #endregion

        #region "FUNCTION: CompanyPurchases"
        /// <summary>
        /// Generate the company purchases report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.mnuCompanyPurchases)]
        [Route("Reports/CompanyPurchases")]
        public async Task<ActionResult> CompanyPurchases()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary("treetype");
                parameters.Add("treetype", "COMP");
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.CompanyPurchases(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "CompanyPurchases");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "JDSuccessReports"

        #region "FUNCTION: JDSuccessAnalysisForTerritory"
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.mnuJDSuccessReport)]
        [Route("Reports/JDSuccessAnalysisForTerritory")]
        public async Task<ActionResult> JDSuccessAnalysisForTerritory()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary(); 
                
                parameters.Add("reporttype", "NETWORK");
                parameters.Add("language", CurrentLanguage.ToUpper());
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.JDSuccessAnalysis(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "JDSuccessAnalysis(Territory)");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }


        #endregion

        #region "FUNCTION: JDSuccessAnalysisForSponsor"

        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.mnuJDSuccessReport)]
        [Route("Reports/JDSuccessAnalysisForSponsor")]
        public async Task<ActionResult> JDSuccessAnalysisForSponsor()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
               
                parameters.Add("reporttype", "SPONSOR");
                parameters.Add("language", CurrentLanguage.ToUpper());
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.JDSuccessAnalysis(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "JDSuccessAnalysis(Sponsor)");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }


        #endregion

        #region "FUNCTION: JDSuccessAnalysisForCompany"

        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.mnuJDSuccessReport)]
        [Route("Reports/JDSuccessAnalysisForCompany")]
        public async Task<ActionResult> JDSuccessAnalysisForCompany()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();

                parameters.Add("reporttype", "COMPANY");
                parameters.Add("language", CurrentLanguage.ToUpper());
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.JDSuccessAnalysis(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "JDSuccessAnalysis(Company)");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }


        #endregion


        #endregion

        #region "FUNCTION: JVPurchases"
        /// <summary>
        /// Generate the JV purchases report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.mnuJVPurchases)]
        [Route("Reports/JVPurchases")]
        public async Task<ActionResult> JVPurchases()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary("treetype");
                parameters.Add("treetype", "JV");
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.JVPurchases(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "JVPurchases");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: TerritoryVolume"
        /// <summary>
        /// Generate the territory volume report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.mnuTerritoryVolume)]
        [Route("Reports/TerritoryVolume")]
        public async Task<ActionResult> TerritoryVolume()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary("treetype");
                parameters.Add("treetype", "ADMIN");
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.TerritoryVolume(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "TerritoryVolume");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: CompanyVolume"
        /// <summary>
        /// Generate the company volume report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.mnuCompanyVolume)]
        [Route("Reports/CompanyVolume")]
        public async Task<ActionResult> CompanyVolume()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary("treetype");
                parameters.Add("treetype", "COMP");
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.CompanyVolume(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "CompanyVolume");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: JVVolume"
        /// <summary>
        /// Generate the jv volume report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.mnuJVVolume)]
        [Route("Reports/JVVolume")]
        public async Task<ActionResult> JVVolume()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary("treetype");
                parameters.Add("treetype", "JV");
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.JVVolume(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "JVVolume");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: SalespersonVolume"
        /// <summary>
        /// Generate the salesperson volume report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.mnuSalespersonVolume)]
        [Route("Reports/SalespersonVolume")]
        public async Task<ActionResult> SalespersonVolume()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary("treetype");
                parameters.Add("treetype", "SP");
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.SalespersonVolume(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "SalespersonVolume");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: SPAgreementStatuses"
        /// <summary>
        /// Generate the salesperson volume report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.opViewSalespersonAgreementStatuses)]
        [Route("Reports/SPAgreementStatuses")]
        public async Task<ActionResult> SalespersonAgreementStatuses()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                parameters.Add("distributorno", this.EntityAccessToken.EntityCode);
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.SPAgreementStatuses(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "SPAgreementStatuses");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: CustomerStatement"
        /// <summary>
        /// Generate the customer statement report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpGet]
        [HCAuthorizeMenu(new eSecurityItem[] { eSecurityItem.mnuAccountSearch, eSecurityItem.opViewZeroPayAccounts } )]
        [Route("Reports/CustomerStatement/{statementID:int}")]
        public async Task<ActionResult> CustomerStatement(Int32 statementID)
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = new Dictionary<string, string>();
                parameters.Add("statementid", statementID.ToString());
                format = "PDF";

                //-- FORMAT THE URL --
                url = String.Format("Report.CustomerStatement(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "CustomerStatement");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: DFCustomerStatement"
        /// <summary>
        /// Generate the customer statement report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpGet]
        [HCAuthorizeMenu(new eSecurityItem[] { eSecurityItem.mnuDFAccountSearch, eSecurityItem.opDFViewZeroPayAccounts })]
        [Route("Reports/DFCustomerStatement/{statementID:int}")]
        public async Task<ActionResult> DFCustomerStatement(Int32 statementID)
        {
            return await CustomerStatement(statementID);
        }
        #endregion

        #region "FUNCTION: LWCustomerStatement"
        /// <summary>
        /// Generate the customer statement report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpGet]
        [HCAuthorizeMenu(new eSecurityItem[] { eSecurityItem.mnuLWAccountSearch, eSecurityItem.opLWViewZeroPayAccounts })]
        [Route("Reports/LWCustomerStatement/{statementID:int}")]
        public async Task<ActionResult> LWCustomerStatement(Int32 statementID)
        {
            return await CustomerStatement(statementID);
        }
        #endregion

        #region "FUNCTION: RecruiterVolume"
        /// <summary>
        /// Generate the recruiter volume report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.mnuRecruiterVolume)]
        [Route("Reports/RecruiterVolume")]
        public async Task<ActionResult> RecruiterVolume()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary("treetype");
                parameters.Add("treetype", "RECRUITS");
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.RecruiterVolume(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "RecruiterVolume");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: PriceQuote"
        /// <summary>
        /// Generate the transmittal report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.mnuPriceQuote)]
        [Route("Reports/PriceQuote")]
        public async Task<ActionResult> PriceQuote()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = "pdf";

                //-- FORMAT THE URL --
                url = String.Format("Report.PriceQuote(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "PriceQuote");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: SPCommission"
        /// <summary>
        /// Generate the Salesperson Commission report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.mnuSalespersonCommissions)]
        [Route("Reports/SPCommissions")]
        public async Task<ActionResult> SPCommissions()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.SalespersonCommissions(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "SalespersonCommissions");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: Transmittal"
        /// <summary>
        /// Generate the transmittal report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.mnuTransmittals)]
        [Route("Reports/Transmittal")]
        public async Task<ActionResult> Transmittal()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.Transmittal(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "Transmittal");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: Transmittal"
        /// <summary>
        /// Generate the transmittal report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpGet]
        [HCAuthorizeMenu(eSecurityItem.mnuTransmittals)]
        [Route("Reports/Transmittal/{client}/{id}/{format}")]
        public async Task<ActionResult> TransmittalByID(String client, String id, String format)
        {
            Dictionary<String, String> parameters = new Dictionary<string, string>();
            String url;

            try
            {
                //-- FORMAT THE URL --
                url = String.Format($"Report.TransmittalByID(format='{format}',id={id},client='{client}')");

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "Transmittal");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: TransmittalSummary"
        /// <summary>
        /// Generate the transmittal summary report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.mnuTransmittalSummary)]
        [Route("Reports/TransmittalSummary")]
        public async Task<ActionResult> TransmittalSummary()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.TransmittalSummary(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "TransmittalSummary");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: Backorders"
        /// <summary>
        /// Generate the backorder report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.mnuBackorders)]
        [Route("Reports/Backorders")]
        public async Task<ActionResult> Backorders()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.Backorders(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "Backorders");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: Backorders"
        /// <summary>
        /// Generate the backorder report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.mnuPricelist)]
        [Route("Reports/PriceList")]
        public async Task<ActionResult> PriceList()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;
            String client;
            Int32 level;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- GET THE PARAMETER VALUES --
                client = ParseParameterValue<string>("client", parameters);
                level = ParseParameterValue<Int32>("level", parameters);

                //-- MAKE SURE THE USER CAN ACCESS THE LEVEL HE/SHE IS REQUESTING --
                if (!this.CurrentUser.DistributorLevel.HasValue || (level < this.CurrentUser.DistributorLevel))
                {
                    return ForbiddenResult(String.Format("Distributor tried to generate price list for level {0} but his level is a {1}", level, this.CurrentUser.DistributorLevel));
                }

                //-- MAKE SURE THE CLIENT IS VALID --
                if (!this.CurrentUser.Clients.Any(o => o.ToUpper() == client.ToUpper()))
                {
                    return ForbiddenResult(String.Format("User tried to generate price list for client {0} which he is not set up in", client));
                }

                //-- FORMAT THE URL --
                if (client.ToUpper() == "BZ")
                    url = String.Format("Report.PriceListBZ(format='{0}')", format);
                else
                    url = String.Format("Report.PriceList(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "PriceList");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: CARPCustomers"
        /// <summary>
        /// Generate the CARP customer report
        /// </summary>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.mnuCARP)]
        [Route("Reports/CARPActivity")]
        public async Task<ActionResult> CARPCustomers()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;
            String periodFrom;
            String periodTo;
            String customerType;

            try
            {
                //-- TRANSLATE THE REPORT PARAMETERS --
                periodFrom = Request.Form["fromperiod"].ToString();
                periodTo = Request.Form["toperiod"].ToString();
                customerType = Request.Form["custtype"].ToString();
                format = Request.Form["exportformat"].ToString();

                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = new Dictionary<string, string>();
                parameters.Add("fromdate", periodFrom.Substring(0, 4) + "-" + periodFrom.Substring(4, 2) + "-01");
                parameters.Add("thrudate", DateTime.ParseExact(periodTo + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture).AddMonths(1).AddDays(-1).ToString("yyyy-MM-dd", System.Globalization.CultureInfo.InvariantCulture));
                parameters.Add("showactive", (customerType == "A" ? "true" : "false"));

                //-- FORMAT THE URL --
                url = String.Format("Report.CARPMarketingCustomers(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "CARPActivity");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: CashReceipts"
        /// <summary>
        /// Generate the backorder report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.opViewCashReceiptsReport)]
        [Route("Reports/CashReceipts")]
        public async Task<ActionResult> CashReceipts()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;
            String client;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- GET THE PARAMETER VALUES --
                client = ParseParameterValue<string>("client", parameters);
                                
                //-- MAKE SURE THE CLIENT IS VALID --
                if (!this.CurrentUser.Clients.Any(o => o.ToUpper() == client.ToUpper()))
                {
                    return ForbiddenResult(String.Format("User tried to generate price list for client {0} which he is not set up in", client));
                }

                //-- FORMAT THE URL --
                url = String.Format("Report.CashReceipts(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "CashReceipts");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: DFCashReceipts"
        /// <summary>
        /// Generate the backorder report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.opViewDFCashReceipts)]
        [Route("Reports/DFCashReceipts")]
        public async Task<ActionResult> DFCashReceipts()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;
            String client;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- GET THE PARAMETER VALUES --
                client = ParseParameterValue<string>("client", parameters);

                //-- MAKE SURE THE CLIENT IS VALID --
                if (!this.CurrentUser.Clients.Any(o => o.ToUpper() == client.ToUpper()))
                {
                    return ForbiddenResult(String.Format("User tried to generate price list for client {0} which he is not set up in", client));
                }

                //-- FORMAT THE URL --
                url = String.Format("Report.DFCashReceipts(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "DFCashReceipts");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: LayawayCashReceipts"
        /// <summary>
        /// Generate the backorder report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.opViewLayawayCashReceipts)]
        [Route("Reports/LayawayCashReceipts")]
        public async Task<ActionResult> LayawayCashReceipts()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;
            String client;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- GET THE PARAMETER VALUES --
                client = ParseParameterValue<string>("client", parameters);

                //-- MAKE SURE THE CLIENT IS VALID --
                if (!this.CurrentUser.Clients.Any(o => o.ToUpper() == client.ToUpper()))
                {
                    return ForbiddenResult(String.Format("User tried to generate price list for client {0} which he is not set up in", client));
                }

                //-- FORMAT THE URL --
                url = String.Format("Report.LayawayCashReceipts(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "LayawayCashReceipts");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: DFRepoCashReceipts"
        /// <summary>
        /// Generate the backorder report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.opViewDFCashReceiptsOnRepos)]
        [Route("Reports/DFRepoCashReceipts")]
        public async Task<ActionResult> DFRepoCashReceipts()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;
            String client;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- GET THE PARAMETER VALUES --
                client = ParseParameterValue<string>("client", parameters);

                //-- MAKE SURE THE CLIENT IS VALID --
                if (!this.CurrentUser.Clients.Any(o => o.ToUpper() == client.ToUpper()))
                {
                    return ForbiddenResult(String.Format("User tried to generate price list for client {0} which he is not set up in", client));
                }

                //-- FORMAT THE URL --
                url = String.Format("Report.DFRepoCashReceipts(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "DFCashReceipts");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: MonthlyCashReceipts"
        /// <summary>
        /// Generate the backorder report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.opViewCashReceiptsReport)]
        [Route("Reports/MonthlyCashReceipts")]
        public async Task<ActionResult> MonthlyCashReceipts()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;
            String period;
            DateTime fromDate;
            DateTime toDate;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- CONVERT THE PERIOD PARAMETER INTO DATEFROM AND DATETO PARAMETERS --
                period = parameters["period"];
                parameters.Remove("period");
                fromDate = DateTime.ParseExact(period + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                toDate = fromDate.AddMonths(1).AddDays(-1);
                parameters.Add("fromdate", fromDate.ToString("MM/dd/yyyy"));
                parameters.Add("todate", toDate.ToString("MM/dd/yyyy"));

                //-- FORMAT THE URL --
                url = String.Format("Report.MonthlyCashReceipts(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "CashReceipts");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: DFMonthlyCashReceipts"
        /// <summary>
        /// Generate the distributor financed monthly cash receipts report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.opViewDFCashReceipts)]
        [Route("Reports/DFMonthlyCashReceipts")]
        public async Task<ActionResult> DFMonthlyCashReceipts()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;
            String period;
            DateTime fromDate;
            DateTime toDate;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- CONVERT THE PERIOD PARAMETER INTO DATEFROM AND DATETO PARAMETERS --
                period = parameters["period"];
                parameters.Remove("period");
                fromDate = DateTime.ParseExact(period + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                toDate = fromDate.AddMonths(1).AddDays(-1);
                parameters.Add("fromdate", fromDate.ToString("MM/dd/yyyy"));
                parameters.Add("todate", toDate.ToString("MM/dd/yyyy"));

                //-- FORMAT THE URL --
                url = String.Format("Report.DFMonthlyCashReceipts(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "DFCashReceipts");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: LayawayMonthlyCashReceipts"
        /// <summary>
        /// Generate the distributor financed monthly cash receipts report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.opViewLayawayCashReceipts)]
        [Route("Reports/LayawayMonthlyCashReceipts")]
        public async Task<ActionResult> LayawayMonthlyCashReceipts()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;
            String period;
            DateTime fromDate;
            DateTime toDate;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- CONVERT THE PERIOD PARAMETER INTO DATEFROM AND DATETO PARAMETERS --
                period = parameters["period"];
                parameters.Remove("period");
                fromDate = DateTime.ParseExact(period + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                toDate = fromDate.AddMonths(1).AddDays(-1);
                parameters.Add("fromdate", fromDate.ToString("MM/dd/yyyy"));
                parameters.Add("todate", toDate.ToString("MM/dd/yyyy"));

                //-- FORMAT THE URL --
                url = String.Format("Report.LayawayMonthlyCashReceipts(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "LayawayCashReceipts");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: DFRepoMonthlyCashReceipts"
        /// <summary>
        /// Generate the distributor financed monthly cash receipts report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.opViewDFCashReceiptsOnRepos)]
        [Route("Reports/DRepoFMonthlyCashReceipts")]
        public async Task<ActionResult> DFRepoMonthlyCashReceipts()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;
            String period;
            DateTime fromDate;
            DateTime toDate;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- CONVERT THE PERIOD PARAMETER INTO DATEFROM AND DATETO PARAMETERS --
                period = parameters["period"];
                parameters.Remove("period");
                fromDate = DateTime.ParseExact(period + "01", "yyyyMMdd", System.Globalization.CultureInfo.InvariantCulture);
                toDate = fromDate.AddMonths(1).AddDays(-1);
                parameters.Add("fromdate", fromDate.ToString("MM/dd/yyyy"));
                parameters.Add("todate", toDate.ToString("MM/dd/yyyy"));

                //-- FORMAT THE URL --
                url = String.Format("Report.DFRepoMonthlyCashReceipts(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "DFRepoCashReceipts");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: DFIVAOnPayments"
        /// <summary>
        /// Generate the backorder report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.opViewDFIVAPaymentsReport)]
        [Route("Reports/DFIVAOnPayments")]
        public async Task<ActionResult> DFIVAOnPayments()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.DFIVAOnPayments(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "DFIVAOnPayments");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: ServiceCharges"
        /// <summary>
        /// Generate the service charge report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.opViewServiceChargeReport)]
        [Route("Reports/ServiceCharges")]
        public async Task<ActionResult> ServiceCharges()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- SWITCH THE WEEK PARAMETER TO THE DATEFROM AND DATETO PARAMETERS --
                var week = parameters["week"];
                parameters.Add("fromdate", week.Substring(0, 10));
                parameters.Add("todate", week.Substring(10, 10));
                parameters.Remove("week");

                //-- FORMAT THE URL --
                url = String.Format("Report.ServiceCharges(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "ServiceCharges");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: AccountSummary"
        /// <summary>
        /// Generate the backorder report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.opViewAccountSummaryReport)]
        [Route("Reports/AccountSummary")]
        public async Task<ActionResult> AccountSummary()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.AccountSummary(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "AccountSummary");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: TDDirectVolume"
        /// <summary>
        /// Generate the backorder report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.opTerritoryDirectorVolumeSummary)]
        [Route("Reports/TDDirectVolume")]
        public async Task<ActionResult> TDDirectVolume()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.TDDirectVolumeReport(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "TDDirectVolumeReport");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: TDVolumeWorld"
        /// <summary>
        /// Generate the backorder report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.opTDVolumeWorldReport)]
        [Route("Reports/TDVolumeWorld")]
        public async Task<ActionResult> TDVolumeWorld()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.TDVolumeWorldReport(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "TDVolumeWorldReport");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: TDVolumeMXSummary"
        /// <summary>
        /// Generate the backorder report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.opTDVolumeMXSummary)]
        [Route("Reports/TDDirectVolumeMX")]
        public async Task<ActionResult> TDVolumeMXSummary()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.TDVolumeMXSummaryReport(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "TDVolumeMXSummaryReport");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: MasterNetworkSplit"
        /// <summary>
        /// Generate the backorder report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.opMasterNetworkSplit)]
        [Route("Reports/MasterNetworkSplit")]
        public async Task<ActionResult> MasterNetworkSplit()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.MasterNetworkSplitReport(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "MasterNetworkSplitReport");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: TerritoryDirectorPurchaseSummaryAmerican"
        /// <summary>
        /// Generate the backorder report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.opTerritoryDirectorPurchaseSummaryAmerican)]
        [Route("Reports/TerritoryDirectorPurchaseSummaryAmerican")]
        public async Task<ActionResult> TerritoryDirectorPurchaseSummaryAmerican()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.TerritoryDirectorPurchaseSummaryAmericanReport(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "TerritoryDirectorPurchaseSummaryAmericanReport");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: COTaxFactor"
        /// <summary>
        /// Generate the backorder report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.mnuCOTaxFactors)]
        [Route("Reports/COTaxFactors")]
        public async Task<ActionResult> COTaxFactor() 
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.COTaxFactorsReport(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "COTaxFactors");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion
        #region "FUNCTION: JDCOTaxFactor"
        /// <summary>
        /// Generate the backorder report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.mnuDistributorCOTaxFactor)]
        [Route("Reports/JDCOTaxFactor")]
        public async Task<ActionResult> JDCOTaxFactor()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.JDCOTaxFactorsReport(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "JDCOTaxFactor");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: DFAccountSummary"
        /// <summary>
        /// Generate the backorder report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.opViewDFAccountSummaryReport)]
        [Route("Reports/DFAccountSummary")]
        public async Task<ActionResult> DFccountSummary()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.DFAccountSummary(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "DFAccountSummary");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: CARPLetters"
        /// <summary>
        /// Generate the cartridge letter report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.opViewCartridgeLetterReport)]
        [Route("Reports/CARPLetters")]
        public async Task<ActionResult> CARPLetters()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.CARPLetters(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "CARPLetters");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: CustBirthdays"
        /// <summary>
        /// Generate the customer birthday report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.opViewCustomerBirthdayReport)]
        [Route("Reports/CustBirthdays")]
        public async Task<ActionResult> CustBirthdays()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.CustBirthdays(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "CustomerBirthdays");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: ReserveSummary"
        /// <summary>
        /// Generate the reserve summary report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.opViewReserveSummaryReport)]
        [Route("Reports/ReserveSummary")]
        public async Task<ActionResult> ReserveSummary()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.ReserveSummary(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "ReserveSummary");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion        

        #region "FUNCTION: HighRiskReserveSummary"
        /// <summary>
        /// Generate the high risk reserve summary report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.opViewReserveSummaryReport)]
        [Route("Reports/HighRiskReserveSummary")]
        public async Task<ActionResult> HighRiskReserveSummary()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.HighRiskReserveSummary(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "HighRiskReserveSummary");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: TerritoryAging"
        /// <summary>
        /// Generate the territory aging report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.opViewTerritoryAgingReport)]
        [Route("Reports/TerritoryAging")]
        public async Task<ActionResult> TerritoryAging()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.TerritoryAging(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "TerritoryAging");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: TDMeetingFund"
        /// <summary>
        /// Generate the territory director meeting fund report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.opViewTDMeetingFundReport)]
        [Route("Reports/TDMeetingFund")]
        public async Task<ActionResult> TDMeetingFund()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.TDMeetingFund(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "TDMeetingFund");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: ExchangeRates"
        /// <summary>
        /// Generate the monthly exchange rate
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.opViewMonthlyExchangeRates)]
        [Route("Reports/ExchangeRates")]
        public async Task<ActionResult> ExchangeRates()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.ExchangeRates(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "ExchangeRates");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: RPCustCards"
        /// <summary>
        /// Generate the sales tax rates report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.opViewCustomerCardReport)]
        [Route("Reports/RPCustCards")]
        public async Task<ActionResult> RPCustCards()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;
            String fromPeriod;
            String toPeriod;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- PERIODS --
                var period = parameters["period"];
                parameters.Remove("period");
                toPeriod = DateTime.Now.ToString("yyyyMM");

                switch (period)
                {
                    case "THISMONTH":
                        fromPeriod = DateTime.Now.ToString("yyyyMM");
                        break;

                    case "LASTMONTH":
                        fromPeriod = DateTime.Now.AddMonths(-1).ToString("yyyyMM");
                        toPeriod = fromPeriod;
                        break;

                    case "LAST3":
                        fromPeriod = DateTime.Now.AddMonths(-3).ToString("yyyyMM");
                        break;

                    case "LAST6":
                        fromPeriod = DateTime.Now.AddMonths(-6).ToString("yyyyMM");
                        break;

                    case "LASTYEAR":
                        fromPeriod = DateTime.Now.AddMonths(-12).ToString("yyyyMM");
                        break;

                    case "ANY":
                        fromPeriod = "";
                        break;

                    default:
                        fromPeriod = "200000";
                        break;
                }

                parameters.Add("fromdate", fromPeriod);
                parameters.Add("todate", toPeriod);

                //-- FORMAT THE URL --
                url = String.Format("Report.RPCustCards(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "RPCustCards");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: WarehouseSalesOrders"
        /// <summary>
        /// Generate the sales tax rates report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpGet]
        [HCAuthorizeMenu(eSecurityItem.mnuWarehouseSalesOrders)]
        [Route("Reports/WarehouseSalesOrders")]
        public async Task<ActionResult> WarehouseSalesOrders()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = new Dictionary<string, string>();
                parameters.Add("distr", "99999");
                format = "pdf";

                //-- FORMAT THE URL --
                url = String.Format("Report.WarehouseSalesOrders(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "WarehouseSalesOrders");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: JVPayments"
        /// <summary>
        /// Generate the JV Payments detail report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.mnuJVPayments)]
        [Route("Reports/JVPayments")]
        public async Task<ActionResult> JVPayments()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.JVPayments(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "JVPayments");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: Overrides"
        /// <summary>
        /// Generate the Override detail report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.mnuMonthlyOverrides)]
        [Route("Reports/Overrides")]
        public async Task<ActionResult> Overrides()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.MonthlyOverrides(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "MonthlyOverrides");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: PreliminaryOverrides"
        /// <summary>
        /// Generate the Reserve Adjustment detail report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.mnuMonthlyOverrides)]
        [Route("Reports/PreliminaryOverrides")]
        public async Task<ActionResult> PreliminaryOverrides()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.ReserveAdjustments(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "PreliminaryOverrides");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: ReserveAdjustments"
        /// <summary>
        /// Generate the Reserve Adjustment detail report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.mnuReserveAdjustment)]
        [Route("Reports/ReserveAdjustments")]
        public async Task<ActionResult> ReserveAdjustments()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.ReserveAdjustments(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "ReserveAdjustments");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: ReserveAdjustmentPurchases"
        /// <summary>
        /// Generate the Reserve Adjustment Purchase report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.mnuReserveAdjustment)]
        [Route("Reports/ReserveAdjustmentPurchases")]
        public async Task<ActionResult> ReserveAdjustmentPurchases()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.ReserveAdjustmentPurchases(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "ReserveAdjustmentPurchases");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: CustomerSummary"
        /// <summary>
        /// Generate the Reserve Adjustment detail report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        //[HCAuthorizeMenu(eSecurityItem.mnuAccountSearch, eSecurityItem.mnuDFAccountSearch, eSecurityItem.mnuLWAccountSearch)]
        [AllowAnonymous]
        [Route("Reports/CustomerSummary")]
        public async Task<ActionResult> CustomerSummary()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.CustomerSummary(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "CustomerSummary");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: CustomerList"
        /// <summary>
        /// Generate the detailed customer report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        //[HCAuthorizeMenu(eSecurityItem.mnuAccountSearch, eSecurityItem.mnuDFAccountSearch, eSecurityItem.mnuLWAccountSearch)]
        [AllowAnonymous]
        [Route("Reports/CustomerList")]
        public async Task<ActionResult> CustomerList()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.CustomerList(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "CustomerList");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: CustomerDetails"
        /// <summary>
        /// Generate the detailed customer report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        [Route("Reports/CustomerDetails")]
        public async Task<ActionResult> CustomerDetails()
        {
            Dictionary<String, String> inputParms;
            Dictionary<String, String> parameters;
            String url;
            String format;
            String etag;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                inputParms = ConvertCurrentFormToDictionary();
                etag = (inputParms.ContainsKey("custdetexportetag") ? inputParms["custdetexportetag"] : "");
                format = (inputParms.ContainsKey("custdetexportformat") ? inputParms["custdetexportformat"] : "pdf");
                
                //-- BUILD THE REPORT PARAMETERS --
                parameters = new Dictionary<string, string>();
                parameters.Add("customerno", (inputParms.ContainsKey("custdetexportcustomerno") ? inputParms["custdetexportcustomerno"] : ""));
                parameters.Add("customername", (inputParms.ContainsKey("custdetexportcustomername") ? inputParms["custdetexportcustomername"] : ""));
                parameters.Add("client", (inputParms.ContainsKey("custdetexportclient") ? inputParms["custdetexportclient"] : ""));
                parameters.Add("distributorno", this.EntityAccessToken.DistributorNo);

                //-- FORMAT THE URL --
                url = String.Format("Report.CustomerDetails(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "CustomerDetails", etag);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: MasterDirectVolume"
        /// <summary>
        /// Generate the backorder report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.opMasterTerritoryVolumeSummary)]
        [Route("Reports/MasterDirectVolume")]
        public async Task<ActionResult> MasterDirectVolume()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.MasterDirectVolumeReport(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "MasterDirectVolumeReport");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: CreditReport"
        /// <summary>
        /// Generate the customer credit report
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        [HttpPost]
        [HCAuthorizeMenu(eSecurityItem.mnuCreditReports)]
        [Route("Reports/CreditReport")]
        public async Task<ActionResult> CreditReport()
        {
            Dictionary<String, String> parameters;
            String url;
            String format;

            try
            {
                //-- CONVERT THE FORM INTO A DICTIONARY OBJECT --
                parameters = ConvertCurrentFormToDictionary();
                format = Request.Form["exportformat"];

                //-- FORMAT THE URL --
                url = String.Format("Report.CustomerCreditReport(format='{0}')", format);

                //-- VALIDATE AND GENERATE THE REPORT --
                return await GenerateReportResponse(format, ConvertToFormDataString(parameters), url, "CreditReport");
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }

        }
        #endregion

        #region "FUNCTION: GenerateReportResponse"
        /// <summary>
        /// Generate the report response
        /// </summary>
        /// <param name="format"></param>
        /// <returns></returns>
        private async Task<ActionResult> GenerateReportResponse(String format, String content, String url, String reportName, String etag = null)
        {
            //-- VALIDATE THE URL --
            if (String.IsNullOrWhiteSpace(url))
                return BadRequestResult(String.Format("The url parameter was missing or invalid"));

            //-- VALIDATE THE FORMAT PARAMETER --
            if (!ValidateFormatParameter(format))
                return BadRequestResult(String.Format("The format {0} is not supported", format));

            //-- GENERATE THE REPORT --
            return await this.GenerateReport(url, content, reportName, etag);
        }
        #endregion

        #region "PROCEDURE: ValidateFormatParameter"
        /// <summary>
        /// Validate the format parameter
        /// </summary>
        /// <param name="format"></param>
        private bool ValidateFormatParameter(String format)
        {
            switch (format.ToUpper())
            {
                case "IMAGE":
                case "EXCEL":
                case "WORD":
                case "CSV":
                case "PDF":
                case "XML":

                    //-- THE REPORT FORMAT IS VALID --
                    return (true);

                default:
                    return (false);
            }
        }
        #endregion

        #region "PROCEDURE: HandleUnknownAction"
        /// <summary>
        /// Display an appripriate error when an action isn't found
        /// </summary>
        /// <param name="actionName"></param>
        protected override void HandleUnknownAction(string actionName)
        {
            throw new HttpException(404, String.Format("{0} not found in reports controller", actionName)) { HelpLink = "NOHEADER" };

            //base.HandleUnknownAction(actionName);
        }
        #endregion

        #region "PROCEDURE: OnException"
        /// <summary>
        /// Add the NOHEADER option to the exception so it is displayed without the headers in the error page
        /// </summary>
        /// <param name="filterContext"></param>
        protected override void OnException(ExceptionContext filterContext)
        {
            filterContext.Exception.HelpLink = "NOHEADER";

            base.OnException(filterContext);
        }
        #endregion

        #region "FUNCTION: GetFileName"
        /// <summary>
        /// Generate a random file name for the specified file
        /// </summary>
        /// <param name="extension"></param>
        /// <param name="entityCode"></param>
        /// <param name="reportType"></param>
        /// <returns></returns>
        private String GetFileName(String extension, String entityCode, String reportName)
        {
            return String.Format("{0}-{1}-{2}.{3}",
                                 reportName,
                                 entityCode,
                                 DateTime.Now.ToString("yyyyMMddhhmmsstt"),
                                 extension);
        }
        #endregion

        #region "FUNCTION: GenerateReport"
        /// <summary>
        /// Generate the specified SSRS report and stream it back to the browser
        /// </summary>
        /// <param name="url"></param>
        /// <param name="fileName"></param>
        /// <returns></returns>
        private async Task<ActionResult> GenerateReport(String url, String content, String reportName, String etag = null)
        {
            ReportDTO reportResult = null;

            try
            {
                //-- GENERATE AND DOWNLOAD THE REPORT IN THE REQUESTED FORMAT --
                using (Helpers.WebAPI oAPI = new Helpers.WebAPI(language: this.CurrentLanguage, existingToken: this.CurrentUser))
                {
                    reportResult = await oAPI.GenerateSSRSReport(url, content, etag);
                }

                if ((reportResult == null) || (reportResult.Report == null) || (reportResult.Report.Length <= 0))
                {
                    //-- THE REPORT IS EMPTY OR FAILED TO GENERATE --
                    return NotFoundResult("The report could not be found or failed to generate");
                }
                else
                {
                    //-- STREAM THE MEMO TO THE BROWSER --
                    return this.GetStaticFileResponse(reportResult.Report, reportResult.Extension, GetFileName(reportResult.Extension, this.CurrentUser.UserName, reportName), reportResult.ContentType);
                }
            }
            catch (HttpException exHttp)
            {
                //-- PASS THE HTTP EXCEPTION BACK TO THE CALLER --
                throw new HttpException(exHttp.ErrorCode, exHttp.Message, exHttp) { HelpLink = "NOHEADER" };
            }
            catch (Exception ex)
            {
                //-- BUILD A NEW HTTP EXCEPTION FROM THE OTHER EXCEPTION --
                Logger.LogError(ex.ToString());
                return InternalServerErrorResult(ex.ToString());
            }
        }
        #endregion

        #region "HELPER FUNCTIONS"

        #region "FUNCTION: ConvertFormToDictionary"
        /// <summary>
        /// Convert the current form post into a json object
        /// </summary>
        /// <returns></returns>
        private Dictionary<String, String> ConvertCurrentFormToDictionary(params string[] keysToExclude)
        {
            Dictionary<String, String> retVal = new Dictionary<String, String>();

            if (System.Web.HttpContext.Current == null)
                return (retVal);

            if (System.Web.HttpContext.Current.Request == null)
                return (retVal);

            if (System.Web.HttpContext.Current.Request.Form == null || System.Web.HttpContext.Current.Request.Form.Count <= 0)
                return (retVal);

            foreach (var keyName in System.Web.HttpContext.Current.Request.Form.AllKeys.Where(o => o.ToUpper() != "EXPORTFORMAT"))
            {
                if (keysToExclude == null || !keysToExclude.Any(o => o.ToUpper() == keyName.ToUpper()))
                {
                    retVal.Add(keyName, System.Web.HttpContext.Current.Request.Form[keyName].ToString());
                }
            }

            return (retVal);
        }
        #endregion

        #region "FUNCTION: ConvertToFormDataString"
        /// <summary>
        /// Convert the dictionary into a URL encoded querystring
        /// </summary>
        /// <param name="dict"></param>
        /// <returns></returns>
        private String ConvertToFormDataString(Dictionary<String, String> dict)
        {
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            foreach (var item in dict)
            {
                if (!String.IsNullOrWhiteSpace(item.Key))
                {
                    if (sb.Length > 0) sb.Append('&');
                    sb.Append(HttpUtility.UrlEncode(item.Key));
                    sb.Append('=');
                    sb.Append(HttpUtility.UrlEncode(item.Value));
                }
            }

            return (sb.ToString());
        }
        #endregion

        #region "FUNCTION: ParseParameterValue"
        /// <summary>
        /// Parse the specified parameter value
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="key"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        private T ParseParameterValue<T>(String key, Dictionary<String, String> parameters) 
        {
            var actualKey = parameters.Keys.Where(o => o.ToUpper() == key.ToUpper()).FirstOrDefault();
            if (actualKey == null)
                throw new ArgumentNullException(key);

            return (T)Convert.ChangeType(parameters[actualKey], typeof(T));
        }
        #endregion

        #endregion

        #region "ERROR RESULTS"

        #region "FUNCTION: NotFoundResult"
        /// <summary>
        /// Redirect the browser to the not found
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private RedirectToRouteResult NotFoundResult(String message)
        {
            TempData["errormessage"] = message;

            return new RedirectToRouteResult(new RouteValueDictionary
                                                 {
                                                    { "action", "NotFound" },
                                                    { "controller", "Error" },
                                                    { "id", "NOHEADER" },
                                                    { "showHeader", false },
                                                    { "showLanguageLinks", false },
                                                    { "showReturnToLoginButton", false }
                                                 });
        }
        #endregion

        #region "FUNCTION: BadRequest"
        /// <summary>
        /// Redirect the browser to the bad request error message page
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private RedirectToRouteResult BadRequestResult(String message)
        {
            TempData["errormessage"] = message;

            return new RedirectToRouteResult(new RouteValueDictionary
                                                 {
                                                    { "action", "BadRequest" },
                                                    { "controller", "Error" },
                                                    { "id", "NOHEADER" },
                                                    { "showHeader", false }
                                                 });
        }
        #endregion

        #region "FUNCTION: InternalServerErrorResult"
        /// <summary>
        /// Redirect the browser to the internal server error message page
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private RedirectToRouteResult InternalServerErrorResult(String message)
        {
            TempData["errormessage"] = message;

            return new RedirectToRouteResult(new RouteValueDictionary
                                                 {
                                                    { "action", "Index" },
                                                    { "controller", "Error" },
                                                    { "id", "NOHEADER" },
                                                    { "showHeader", false }
                                                 });
        }
        #endregion

        #region "FUNCTION: ForbiddenResult"
        /// <summary>
        /// Redirect the browser to the forbidden
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        private RedirectToRouteResult ForbiddenResult(String message)
        {
            TempData["errormessage"] = message;

            return new RedirectToRouteResult(new RouteValueDictionary
                                                 {
                                                    { "action", "Forbidden" },
                                                    { "controller", "Error" },
                                                    { "id", "NOHEADER" },
                                                    { "showHeader", false },
                                                    { "showLanguageLinks", false },
                                                    { "showReturnToLoginButton", false }
                                                 });
        }
        #endregion

        #endregion
    }
}
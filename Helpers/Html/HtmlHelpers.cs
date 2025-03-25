using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Mvc.Html;

namespace DistributorWebsite.MVC.WebUI.Helpers.Html
{
    public static class HtmlHelpers
    {
        #region "FUNCTION: GetAntiForgeryTokenHTML"
        /// <summary>
        /// Get the html for an anti forgery token that can be passed to the server
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static System.Web.Mvc.MvcHtmlString AntiForgeryTokenForAjaxPost(this HtmlHelper helper)
        {
            var antiForgeryInputTag = helper.AntiForgeryToken().ToString();

            // Above gets the following: <input name="__RequestVerificationToken" type="hidden" value="PnQE7R0MIBBAzC7SqtVvwrJpGbRvPgzWHo5dSyoSaZoabRjf9pCyzjujYBU_qKDJmwIOiPRDwBV1TNVdXFVgzAvN9_l2yt9-nf4Owif0qIDz7WRAmydVPIm6_pmJAI--wvvFQO7g0VvoFArFtAR2v6Ch1wmXCZ89v0-lNOGZLZc1" />
            var removedStart = antiForgeryInputTag.Replace(@"<input name=""__RequestVerificationToken"" type=""hidden"" value=""", "");
            var tokenValue = removedStart.Replace(@""" />", "");
            if (antiForgeryInputTag == removedStart || removedStart == tokenValue)
                throw new InvalidOperationException("Oops! The Html.AntiForgeryToken() method seems to return something I did not expect.");
            return new MvcHtmlString(tokenValue);
        }
        #endregion

        #region "FUNCTION: HCValidationSummary"
        /// <summary>
        /// Build the Boostrap formatted validation summary 
        /// </summary>
        /// <param name="helper"></param>
        /// <param name="excludePropertyErrors"></param>
        /// <returns></returns>
        public static MvcHtmlString HCValidationSummary(this HtmlHelper helper, Boolean excludePropertyErrors = true, String errorHeaderText = null)
        {
            MvcHtmlString validationSummary;
            String validationSummaryText;

            validationSummary = helper.ValidationSummary(excludePropertyErrors);
            if (validationSummary != null)
            {
                //-- ADD THE BOOTSTRAP CLASSES TO THE MAIN DIV --
                validationSummaryText = helper.ValidationSummary(excludePropertyErrors).ToString();
                validationSummaryText = validationSummaryText.Replace("class=\"validation-summary-valid\"", "class=\"validation-summary-valid panel panel-danger\"");
                validationSummaryText = validationSummaryText.Replace("class=\"validation-summary-errors\"", "class=\"validation-summary-errors panel panel-danger\"");

                //-- ADD THE HEADER --
                validationSummaryText = validationSummaryText.Replace("<ul>",
                                                                                        String.Format("<div class=\"panel-heading\">" + Environment.NewLine +
                                                                                                          "  <i class=\"fa fa-exclamation-circle fa-lg\"></i> <strong>{0}</strong> : {1}" + Environment.NewLine +
                                                                                                          "</div>" + Environment.NewLine +
                                                                                                          "<div class=\"panel-body\">" + Environment.NewLine +
                                                                                                          "<ul>" + Environment.NewLine,
                                                                                                          LocalResources.HCResources.Error.ToUpper(),
                                                                                                          errorHeaderText ?? LocalResources.HCResources.InvalidRequest));
                validationSummaryText = validationSummaryText.Replace("</ul>",
                                                                                        "</ul>" + Environment.NewLine +
                                                                                        "</div>");

                //-- HIDE THE FORM IF THERE AREN'T ANY PROPERTY ERRORS --
                if (!validationSummaryText.Contains("<li>") && excludePropertyErrors)
                    validationSummaryText = validationSummaryText.Replace("validation-summary-errors", "validation-summary-valid");
            }
            else
            {
                validationSummaryText = "";
            }

            return new MvcHtmlString(validationSummaryText);
        }
        #endregion

        #region "FUNCTION: HCValidationMessageFor"
        /// <summary>
        /// Create a validation message for the specified property using a DIV instead of a SPAN
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="htmlHelper"></param>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MvcHtmlString HCValidationMessageFor<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, System.Linq.Expressions.Expression<Func<TModel, TProperty>> expression)
        {
            String retVal = htmlHelper.ValidationMessageFor(expression).ToString();

            return MvcHtmlString.Create(htmlHelper.ValidationMessageFor(expression).ToString().Replace("span", "div").Replace("class=\"field-validation-error\"","class=\"field-validation-error col-sm-12\""));
        }
        #endregion

        #region "FUNCTION: CreatePaymentStatusResourceText"
        /// <summary>
        /// Create the payment status text javascript
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static MvcHtmlString CreatePaymentStatusResourceText(this HtmlHelper htmlHelper)
        {
            System.Text.StringBuilder retVal = new System.Text.StringBuilder();
            String[] statuses = { "SUBMITTED", "EMAILED", "INITIALIZED", "CANCELLED", "ERROR", "REFUSED", "PENDING", "SCHEDULED", "UNKNOWN", "AUTHORIZED", "CAPTURED", "CHARGEDBACK", "SETTLED", "REFUNDED", "REFUNDREQUESTED", "PROCESSED" };
            String statusText;

            foreach (string status in statuses)
            {
                try
                {
                    statusText = LocalResources.HCResources.ResourceManager.GetString(String.Format("PaymentStatus_{0}", status));
                    if (String.IsNullOrWhiteSpace(statusText))
                        statusText = LocalResources.HCResources.PaymentStatus_UNKNOWN;
                }
                catch (Exception)
                {
                    statusText = LocalResources.HCResources.PaymentStatus_UNKNOWN;
                }

                retVal.AppendLine(String.Format("HCE.Resources.{0} = '{1}';", status, statusText));
            }

            retVal.AppendLine(String.Format("HCE.Resources.LPDUPHeader = '{0}'", LocalResources.HCResources.ListPaymentExistingCustomerHeader));
            retVal.AppendLine(String.Format("HCE.Resources.LPDUPMessage = '{0}'", LocalResources.HCResources.ListPaymentExistingCustomerMessage));
            retVal.AppendLine(String.Format("HCE.Resources.LPCreated = '{0}'", LocalResources.HCResources.ListPaymentCreatedSuccessfully));
            retVal.AppendLine(String.Format("HCE.Resources.LPCreatedMSG = '{0}'", LocalResources.HCResources.ListPaymentCreatedSuccessfullyDetail));

            return new MvcHtmlString(retVal.ToString());
        }
        #endregion

        #region "FUNCTION: AntiForgeryTokenJS"
        /// <summary>
        /// Generate the angular anti forgery token that can be used in angular form submissions
        /// </summary>
        /// <param name="helper"></param>
        /// <returns></returns>
        public static System.Web.Mvc.MvcHtmlString AntiForgeryTokenJS(this HtmlHelper helper)
        {
            return new MvcHtmlString(String.Format("<input id=\"antiForgeryToken\" data-ng-model=\"antiForgeryToken\" type=\"hidden\" data-ng-init=\"antiForgeryToken='{0}'\" />",
                                     Helpers.Common.GetAntiForgeryToken()));
        }
        #endregion
    }
}
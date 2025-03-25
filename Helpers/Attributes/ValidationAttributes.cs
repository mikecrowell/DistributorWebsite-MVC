using HyCite.Validation.Web.Attributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DistributorWebsite.MVC.WebUI
{
    public class HCVALRequiredAttribute : HCWebRequiredAttribute
    {
        /// <summary>
        /// Initialize the required attribute
        /// </summary>
        /// <param name="fieldName">Property name in the Messages.resx resource file to pull the field value from</param>
        public HCVALRequiredAttribute(string fieldName) : base(fieldName.ToString(), typeof(LocalResources.HCResources))
        {
        }

    }

    public class HCVALStringLengthAttribute : HCWebStringLengthAttribute
    {
        /// <summary>
        /// Initialize the string length attribute
        /// </summary>
        /// <param name="minLength"></param>
        /// <param name="maxLength"></param>
        /// <param name="fieldName"></param>
        /// <param name="validateIfEmpty"></param>
        public HCVALStringLengthAttribute(Int32 minLength, Int32 maxLength, string fieldName, bool validateIfEmpty = false) : base(minLength, maxLength, fieldName, typeof(LocalResources.HCResources), validateIfEmpty)
        {

        }
    }

    /// <summary>
    /// Email address validation attribute
    /// </summary>
    public class HCVALEmailAddress : HCWebValidateFormatAttribute
    {
        public HCVALEmailAddress(bool validateIfEmpty = false) : base(eFormatType.EmailAddress, LocalResources.ResourceKey.Email, typeof(LocalResources.HCResources), validateIfEmpty)
        {
        }
    }

    /// <summary>
    /// Salesperson code validation
    /// </summary>
    public class HCVALSalespersonCode : HCWebValidateFormatAttribute
    {
        public HCVALSalespersonCode(bool validateIfEmpty = false) : base(eFormatType.SalespersonCode, LocalResources.ResourceKey.Salesperson, typeof(LocalResources.HCResources), validateIfEmpty)
        {

        }
    }

    /// <summary>
    /// Distributor number code validation
    /// </summary>
    public class HCVALDistributorNumber : HCWebValidateFormatAttribute
    {
        public HCVALDistributorNumber(bool validateIfEmpty = false) : base(eFormatType.DistributorNumber, LocalResources.ResourceKey.DistributorNumber, typeof(LocalResources.HCResources), validateIfEmpty)
        {

        }
    }

    /// <summary>
    /// Customer number code validation
    /// </summary>
    public class HCVALCustomerNumber : HCWebValidateFormatAttribute
    {
        public HCVALCustomerNumber(bool validateIfEmpty = false) : base(eFormatType.CustomerNumber, LocalResources.ResourceKey.CustomerNo, typeof(LocalResources.HCResources), validateIfEmpty)
        {

        }
    }
}
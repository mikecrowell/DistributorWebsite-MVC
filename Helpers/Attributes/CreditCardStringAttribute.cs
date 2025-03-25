using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace DistributorWebsite.MVC.WebUI.Helpers.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public class CreditCardStringAttribute : ValidationAttribute, IClientValidatable
    {
        private const string _defaultErrorMessage = "Invalid Note/Comment: For security purposes, you are not allowed to enter notes or comments that contain credit card information";
        public CreditCardStringAttribute()
           : base(_defaultErrorMessage)
        {
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
          
            string valString = (value as string ?? String.Empty).Trim();
            bool isValid = true;
            if(!string.IsNullOrEmpty(valString))
            {
                var replaceValue = valString.Replace("-", string.Empty).Replace(" ", string.Empty);
                isValid = Regex.IsMatch(replaceValue, Constants.REGEX_NOTE_WITH_CREDITCARD);
            }

            if (isValid)
                return ValidationResult.Success;
            else
                return new ValidationResult(_defaultErrorMessage);
        }

        public IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRequiredRule reqRule = new ModelClientValidationRequiredRule(LocalResources.HCResources.ResourceManager.GetString(this.ErrorMessageResourceName))
            {
                ValidationType = "creditcardstring"
            };
          
            if (Helpers.Application.Instance.PCIEnabled)
            {
               
                reqRule.ValidationParameters.Add("regex", Constants.REGEX_NOTE_WITH_CREDITCARD);
            }
            else
            {
                reqRule.ValidationParameters.Add("regex", "");
            }

            yield return reqRule;
        }
    }
}
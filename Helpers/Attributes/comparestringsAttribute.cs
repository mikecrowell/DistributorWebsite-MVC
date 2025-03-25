using System;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace DistributorWebsite.MVC.WebUI.Helpers.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public  class CompareStringsAttribute : ValidationAttribute, IClientValidatable
    {
        private const string _defaultErrorMessage = "Email Addresses do not match - please re-enter the email address you entered in the Email field";
        public bool IgnoreCase { get; set; }
        public string OtherPropertyName { get; set; }
        public CompareStringsAttribute(string otherPropertyName)
            : base(_defaultErrorMessage)
        {
            OtherPropertyName = otherPropertyName;
        }
        
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            string otherPropVal = validationContext.ObjectInstance.GetType().GetProperty(OtherPropertyName).GetValue(validationContext.ObjectInstance, null) as string;

            //Convert nulls to empty strings and trim spaces off the result
            string valString = (value as string ?? String.Empty).Trim();
            string otherPropValString = (otherPropVal ?? String.Empty).Trim();

            bool isMatch = String.Compare(valString, otherPropValString, IgnoreCase) == 0;

            if (isMatch)
                return ValidationResult.Success;
            else
                return new ValidationResult(_defaultErrorMessage);
        }

        public System.Collections.Generic.IEnumerable<ModelClientValidationRule> GetClientValidationRules(ModelMetadata metadata, ControllerContext context)
        {
            ModelClientValidationRequiredRule reqRule = new ModelClientValidationRequiredRule(LocalResources.HCResources.ResourceManager.GetString(this.ErrorMessageResourceName))
            {
                ValidationType = "comparestrings"
            };
            reqRule.ValidationParameters.Add("otherprop", this.OtherPropertyName);
            reqRule.ValidationParameters.Add("ignorecase", this.IgnoreCase.ToString().ToLower());
            yield return reqRule;
        }


    }


}
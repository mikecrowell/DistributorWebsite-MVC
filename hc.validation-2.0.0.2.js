/* *********************************************************************************************
 * SCRIPT:		hc.validation
 * VERSION:		1.0.0
 * 
 * Copyright 2015, Hy Cite Enterprises, LLC
 *
 * DATE		VERSION			AUTHOR					DESCRIPTION
 * -------- -------------- -------------------- ------------------------------------------------
 * 09/15/15 1.0.0				JASON BRENSON			CREATED
 * ******************************************************************************************* */
 
//*********************************************************************************************
// FUNCTION:	hcrequired
// PURPOSE:		MAKE SURE THE REQUIRE FIELD HAS BEEN FILLED IN 
//*********************************************************************************************
$.validator.addMethod("hcrequired", function (value, element, params) {

    if (value == undefined || value == null)
        return false;
 
    if (value.toString().length == 0)
        return false;
 
    return true;
});

$.validator.unobtrusive.adapters.add("hcrequired", [], function (options) {
	options.rules["hcrequired"] = options.params;
   options.messages["hcrequired"] = options.message;
});

//*********************************************************************************************
// FUNCTION:	hcpattern
// PURPOSE:		VALIDATE THE PATTERN (REGULAR EXPRESSION)
//*********************************************************************************************
$.validator.addMethod("hcpattern", function(value, element, params) {
	if ((params.ifempty != "true") && (value == null || typeof(value) == "undefined" || value == ""))
		return true;

	var regEx = new RegExp(params.regex);
	return(regEx.test(value));
});

$.validator.unobtrusive.adapters.add("hcpattern", ["regex", "ifempty"], function(options){
	options.rules["hcpattern"] = options.params;
	options.messages["hcpattern"] = options.message;
});

//*********************************************************************************************
// FUNCTION:	hcroutingno
// PURPOSE:		VALIDATE THE PATTERN (REGULAR EXPRESSION)
//*********************************************************************************************
$.validator.addMethod("hcroutingno", function (value, element, params) {
    if ((params.ifempty != "true") && (value == null || typeof (value) == "undefined" || value == ""))
        return true;

    var regEx = new RegExp(params.regex);
    if (regEx.test(value) == false) {
        return false;
    }

    var checkDigit = (parseInt(value.substring(0, 1)) * 3) +
                     (parseInt(value.substring(1, 2)) * 7) +
                     (parseInt(value.substring(2, 3)) * 1) +
                     (parseInt(value.substring(3, 4)) * 3) +
                     (parseInt(value.substring(4, 5)) * 7) +
                     (parseInt(value.substring(5, 6)) * 1) +
                     (parseInt(value.substring(6, 7)) * 3) +
                     (parseInt(value.substring(7, 8)) * 7) +
                     (parseInt(value.substring(8, 9)) * 1);

    if (checkDigit == 0) {
        return false;
    }

    if (checkDigit % 10 != 0) {
        return false;
    }

    return (true);
});

$.validator.unobtrusive.adapters.add("hcroutingno", ["regex", "ifempty"], function (options) {
    options.rules["hcroutingno"] = options.params;
    options.messages["hcroutingno"] = options.message;
});

//**********************************************************************************************
// FUNCTION:	hcstrlen
// PURPOSE:		MAKE SURE THE STRING IS AN ACCEPTABLE LENGTH
//**********************************************************************************************
$.validator.addMethod("hcstrlen", function(value, element, params) {
	var regEx;
	var min = -1;
	var max = -1;
	var minStr = "0";
	var maxStr = "";

	if ((params.ifempty != "true") && (value == null || typeof(value) == "undefined" || value == ""))
		return true;

	//-- PARSE THE MIN AND MAX VALUES --
	if (!isNaN(params.min)) min = parseInt(params.min);
	if (!isNaN(params.max)) max = parseInt(params.max);
	if ((min <= 0) && (max <= 0))
		return true;

	if (min >= 0) minStr = min;
	if (max >= 0) maxStr = max;

	regEx = new RegExp("^.{" + minStr + "," + maxStr + "}$");
	return(regEx.test(value));
});

$.validator.unobtrusive.adapters.add("hcstrlen", ["min", "max", "ifempty"], function(options){
	options.rules["hcstrlen"] = options.params;
	options.messages["hcstrlen"] = options.message;
});

/* The adapter signature:
adapterName is the name of the adapter, and matches the name of the rule in the HTML element.
 
params is an array of parameter names that you're expecting in the HTML attributes, and is optional. If it is not provided,
then it is presumed that the validator has no parameters.
 
fn is a function which is called to adapt the HTML attribute values into jQuery Validate rules and messages.
 
The function will receive a single parameter which is an options object with the following values in it:
element
The HTML element that the validator is attached to
 
form
The HTML form element
 
message
The message string extract from the HTML attribute
 
params
The array of name/value pairs of the parameters extracted from the HTML attributes
 
rules
The jQuery rules array for this HTML element. The adapter is expected to add item(s) to this rules array for the specific jQuery Validate validators
that it wants to attach. The name is the name of the jQuery Validate rule, and the value is the parameter values for the jQuery Validate rule.
 
messages
The jQuery messages array for this HTML element. The adapter is expected to add item(s) to this messages array for the specific jQuery Validate validators that it wants to attach, if it wants a custom error message for this rule. The name is the name of the jQuery Validate rule, and the value is the custom message to be displayed when the rule is violated.
*/
//$.validator.unobtrusive.adapters.add("hcrequiredifexternal", ["isexternal"], function (options) {
//    options.rules["hcrequiredifexternal"] = options.params;
//    options.messages["hcrequiredifexternal"] = options.message;
//});


/* ************************************************************************** */
/*-- HELPERS TO APPLY BOOTSTRAP FORMATTING TO JQUERY UNOBTRUSIVE VALIDATION --*/
/* ************************************************************************** */
$(function () 
{
	//-- FORMAT FORM-GROUPS WITH BOOTSTRAP ERROR FORMATTING FOR SERVER VALIDATIONS --
   $('.input-validation-error').parents('.form-group').addClass('has-error');
   $('.field-validation-error').addClass('text-danger');
});

(function ($) {
        var defaultOptions = {
            errorClass: 'has-error',
            validClass: '',
            highlight: function (element, errorClass, validClass) {
					var formGroup = $(element).closest(".form-group");

					formGroup.addClass(errorClass);
               formGroup.removeClass(validClass);
            },
            unhighlight: function (element, errorClass, validClass) {
                $(element).closest(".form-group")
                .removeClass(errorClass)
                .addClass(validClass);
        }
    };

    $.validator.setDefaults(defaultOptions);

    $.validator.unobtrusive.options = {
        errorClass: defaultOptions.errorClass,
        validClass: defaultOptions.validClass,
    };
})(jQuery);

//**************************************************************************************
// FUNCTION:	formReset
// PURPOSE:		RESET THE FORM AND ALL THE FIELDS IN IT
//**************************************************************************************
(function ($) {
  
    //reset a form given a jQuery selected form or a child
    //by default validation is also reset
    $.fn.formReset = function () {

		var form = this.closest("form");

		if (typeof(form) != "undefined")
		{
			form[0].reset();

			try
			{
				var scope =  $(form[0]).scope();

				if ($(scope).length > 0)
				{
					//-- CLEAR THE ANGULAR FORM INFORMATION AND RESET THE STATUSES --
					if (typeof(scope.entity) != "undefined")
						scope.entity = null;
					scope[scope.DetailForm].$setPristine();
					scope[scope.DetailForm].$setUntouched();
					scope[scope.DetailForm].$setValidity();
				}
			}
			catch(error)
			{
			}

			$(form).find("div.form-group .form-control").val("").removeClass("input-validation-error");

			//reset jQuery Validate's internals
			form.validate().resetForm();

			//reset unobtrusive validation summary, if it exists
			form.find("[data-valmsg-summary=true]")
					.removeClass("validation-summary-errors")
					.addClass("validation-summary-valid")
					.find("ul").empty();
 
			//reset unobtrusive field level, if it exists
			form.find("[data-valmsg-replace]")
				  .removeClass("field-validation-error")
				  .addClass("field-validation-valid")
				  .empty();

			//server side validation summary cleanup
			form.find("div.validation-summary-errors")
			   .removeClass("validation-summary-errors")
				.addClass("validation-summary-valid")
				.find("ul").empty();

			//server side field level validation cleanup
			form.find("div.form-group.has-error")
				.removeClass("has-error")
				.find("div.field-validation-error")
				.removeClass()
				.addClass("field-validation-valid");
			}
			
        return form;
    }

})(jQuery);

//**************************************************************************************
// FUNCTION:	formReset
// PURPOSE:		RESET THE FORM AND ALL THE FIELDS IN IT
//**************************************************************************************
(function ($) {

    //reset a form given a jQuery selected form or a child
    //by default validation is also reset
    $.fn.formResetValidation = function () {

        var form = this.closest("form");

        if (typeof (form) != "undefined") {

            $(form).find("div.form-group .form-control").removeClass("input-validation-error");

            //reset jQuery Validate's internals
            form.validate().resetForm();

            //reset unobtrusive validation summary, if it exists
            form.find("[data-valmsg-summary=true]")
                .removeClass("validation-summary-errors")
                .addClass("validation-summary-valid")
                .find("ul").empty();

            //reset unobtrusive field level, if it exists
            form.find("[data-valmsg-replace]")
                .removeClass("field-validation-error")
                .addClass("field-validation-valid")
                .empty();

            //server side validation summary cleanup
            form.find("div.validation-summary-errors")
                .removeClass("validation-summary-errors")
                .addClass("validation-summary-valid")
                .find("ul").empty();

            //server side field level validation cleanup
            form.find("div.form-group.has-error")
                .removeClass("has-error")
                .find("div.field-validation-error")
                .removeClass()
                .addClass("field-validation-valid");
        }

        return form;
    }

})(jQuery);
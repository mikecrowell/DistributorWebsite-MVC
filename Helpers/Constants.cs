using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributorWebsite.MVC.WebUI.Helpers
{
    public static class Constants
    {
        /// <summary>
        /// Generic ID for internal user logins using windows auth
        /// </summary>
        public static Int32 INTERNALUSERID = 99999999;

        /// <summary>
        /// Select list constants for the Department filter box on the Employee Directory page
        /// </summary>
        public static readonly string[] SelectList_HyCiteDepartments = { "SEL_HCDEPT_1", "SEL_HCDEPT_2", "SEL_HCDEPT_3", "SEL_HCDEPT_4", "SEL_HCDEPT_5", "SEL_HCDEPT_6", "SEL_HCDEPT_7", "SEL_HCDEPT_8", "SEL_HCDEPT_9", "SEL_HCDEPT_10" };

        /// <summary>
        /// Regular expression for validating form values to make sure they are safe
        /// </summary>
        public const string REGEX_VALIDATEFORMVALUE = @"^(?!(.|\n)*<[a-z!\/?])(?!(.|\n)*&#)(.|\n)*$";

        /// <summary>
        /// Regulare express for validating alpha form values with spaces allowed
        /// </summary>
        public const string REGEX_VALIDATEALPHAWITHSPACES = @"^[ a-zA-Z]+$";

        public const string REGEX_SALESPERSONCODE = @"^[A-Za-z]{4}\d{4}$";

        public const string REGEX_DISTRIBUTORNUMBER = @"^\d{5}$";

        public const String REGEX_SSN = @"^\d{3}\-?\d{2}\-?\d{4}$";

        public const String REGEX_TIN = @"^\d{2}\-?\d{7}$";

        public const string REGEX_POSTALCODE = @"^\d{5}$";

        public const String REGEX_BZPOSTALCODE = @"^\d{5}\-\d{3}$";

        public const string REGEX_PHONE = @"^\d{3}\-?\d{3}\-?\d{4}$";

        public const string REGEX_MXPHONE = @"^\(?\d{2}\)?\s?\d{4}-?\d{4}$";

        public const string REGEX_BZPHONE = @"^\d{2}\s?\d{5}\-?\d{4}$";
        public const string REGEX_NOTE_WITH_CREDITCARD = @"(?:4[0-9]{12}(?:[0-9]{3})?|[25][1-7][0-9]{14}|6(?:011|5[0-9][0-9])[0-9]{12}|3[47][0-9]{13}|3(?:0[0-5]|[68][0-9])[0-9]{11}|(?:2131|1800|35\d{3})\d{11})";
    }
}
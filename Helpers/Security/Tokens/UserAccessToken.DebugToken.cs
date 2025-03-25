using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributorWebsite.MVC.WebUI.Helpers.Security.Tokens
{
	/// <summary>
    /// Debuggin user access token
    /// </summary>
	public class DebugToken : UserAccessTokenBase
	{
        /// <summary>
        /// Get/set the list of querystring parameters
        /// </summary>
        private System.Collections.Specialized.NameValueCollection Parameters { get; set; }

        /// <summary>
        /// Intialize the debug user token class
        /// </summary>
        /// <param name="context"></param>
        public DebugToken(HttpRequestBase context)
        {
            System.Diagnostics.Trace.TraceInformation("******************************************");
            System.Diagnostics.Trace.TraceInformation("UserToken.Debug");
            System.Diagnostics.Trace.TraceInformation("******************************************");

            //-- SAVE THE QUERYSTRING VALUES --
            this.Parameters = context?.QueryString;

            //-- INITIALIZE THE TOKEN FROM THE QUERYSTRING VALUES --
            InitializeFromQuerystring();
        }

        /// <summary>
        /// Create a generic user access token from the querystring values
        /// </summary>
        /// <param name="context"></param>
        private void InitializeFromQuerystring()
        {
            this.BrandBits = GetQuerystringValueOrDefaultInt("BrandBits", (Int32)eBrand.RP);
            this.ClientBits = GetQuerystringValueOrDefaultInt("ClientBits", (Int32)(eClient.HC | eClient.RC | eClient.MX));
            this.Country = GetQuerystringValueOrDefaultString("Country", "US");
            this.EntityCode = GetQuerystringValueOrDefaultString("EntityCode", "10303");
            this.IsInternalLogin = true;
            this.LeagueBits = GetQuerystringValueOrDefaultInt("LeagueBits", (Int32)eLeague.American);
            this.SecurityString = GetQuerystringValueOrDefaultString("SecurityString", new string('F', 64));
            this.State = GetQuerystringValueOrDefaultString("State", "WI");
            //this.TypeBits = GetQuerystringValueOrDefaultInt("TypeBits", (Int32)(eEntityType.Distributor | eEntityType. | eEntityType.Level1));
            this.UserID = GetQuerystringValueOrDefaultInt("UserID", 99999999);
            this.UserName = GetQuerystringValueOrDefaultString("UserName", $"DEBUG [{Environment.UserDomainName}]");
            this.ValidateIdentity = false;
        }

        #region "FUNCTION: (Int32) GetQuerystringValueOrDefaultInt"
        /// <summary>
        /// Search for an Integer querystring value
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private Int32 GetQuerystringValueOrDefaultInt(String fieldName, Int32 defaultValue = 0)
        {
            Int32 value;
            String sValue;

            //-- MAKE SURE WE HAVE ONE OR MORE PARAMETERS TO SEARCH --
            if (this.Parameters == null || this.Parameters.Count <= 0)
                return (defaultValue);

            //-- SEARCH FOR THE FIELD --
            var key = this.Parameters.AllKeys.FirstOrDefault(o => o == $"debug.{fieldName.ToLower()}");
            if (String.IsNullOrWhiteSpace(key))
                return (defaultValue);

            //-- PARSE THE INTEGER --
            sValue = this.Parameters[key].ToString();
            if (!Int32.TryParse(sValue, out value))
                throw new ArgumentException($"{sValue} is an invalid integer value in the querystring parameter debug.{fieldName}");

            //-- RETURN THE PARSED INTEGER VALUE --
            return (value);
        }
        #endregion

        #region "FUNCTION: (String) GetQuerystringValueOrDefaultString"
        /// <summary>
        /// Search for an Integer querystring value
        /// </summary>
        /// <param name="fieldName"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        private String GetQuerystringValueOrDefaultString(String fieldName, String defaultValue = "")
        {
            //-- MAKE SURE WE HAVE ONE OR MORE PARAMETERS TO SEARCH --
            if (this.Parameters == null || this.Parameters.Count <= 0)
                return (defaultValue);

            //-- SEARCH FOR THE FIELD --
            var key = this.Parameters.AllKeys.FirstOrDefault(o => o == $"debug.{fieldName.ToLower()}");
            if (String.IsNullOrWhiteSpace(key))
                return (defaultValue);

            //-- PARSE THE INTEGER --
            return this.Parameters[key];
        }
        #endregion

    }
}
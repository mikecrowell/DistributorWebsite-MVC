using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Serilog;

namespace DistributorWebsite.MVC.WebUI.Helpers.Exceptions
{
    #region "ENUM: eCustomErrorType"
    /// <summary>
    /// Define the custom error types
    /// </summary>
    public enum eCustomErrorType
    {
        Unknown = 0,
        CanNotConnectToIdentityServer = 900,
        AccessIsForbidden = 901
    }
    #endregion

    public static class ExceptionHelpers
    {
        #region "FUNCTION: ParseException"
        /// <summary>
        /// Parse the specified unhandled exception
        /// </summary>
        /// <param name="exception"></param>
        /// <returns></returns>
        public static HCExceptionInfo ParseException(Exception exception)
        {
            HttpException exHttp;
            HCExceptionInfo exceptionInfo = new HCExceptionInfo();
            HCForbiddenException exForbidden;

            try
            {
                //-- CHECK TO SEE IF THIS IS AN HTTP EXCEPTION --
                exHttp = exception as HttpException;

                //-- GET THE EXCEPTION CODE --
                if (exHttp != null)
                    exceptionInfo.ErrorCode = exHttp.ErrorCode.ToString();
                else
                {
                    //-- CHECK FOR HC FORBIDDEN EXCEPTION --
                    exForbidden = exception as HCForbiddenException;
                    if (exForbidden != null)
                    {
                        exceptionInfo.ErrorCode = "403";
                        exceptionInfo.ErrorType = eCustomErrorType.AccessIsForbidden;
                        exceptionInfo.ErrorTitle = LocalResources.HCResources.Forbidden;
                        exceptionInfo.ErrorDescription = LocalResources.HCResources.ForbiddenDesc;
                    }
                    else
                        exceptionInfo.ErrorCode = "500";
                }

                //-- CHECK FOR IDENTITY SERVER CONNECTION ERROR --
                if ((exception.Message.StartsWith("IDX10803")) || (exception.Message.Contains("/.well-known/openid-configuration")))
                {
                    exceptionInfo.ErrorType = eCustomErrorType.CanNotConnectToIdentityServer;
                    exceptionInfo.ErrorTitle = DistributorWebsite.MVC.LocalResources.HCResources.IdentityServerUnavailable;
                    exceptionInfo.ErrorDescription = DistributorWebsite.MVC.LocalResources.HCResources.IdentityServerUnavailableDescription;
                }

                //-- ADD THE ERROR DETAILS --
                exceptionInfo.ShowErrorDetails = Helpers.Application.Instance.ShowErrorDetails;
                if (exceptionInfo.ShowErrorDetails)
                {
                    exceptionInfo.ErrorDetails = exception.Message;
                }

            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
            }

            return (exceptionInfo);
        }
        #endregion
    }

    #region "CLASS: HCExceptionInfo"
    /// <summary>
    /// Define a transport class for exception information
    /// </summary>
    public class HCExceptionInfo
    {
        public String PageTitle { get; set; }
        public Boolean ShowErrorDetails { get; set; }
        public eCustomErrorType ErrorType { get; set; }
        public String ErrorCode { get; set; }
        public String ErrorTitle { get; set; }
        public String ErrorDescription { get; set; }
        public String ErrorDetails { get; set; }

        /// <summary>
        /// Get/set whether or not to display the return to login button on the error page
        /// </summary>
        public Boolean ShowReturnToLoginButton { get; set; }

        /// <summary>
        /// Get/set whether or not to display the language links on the error page
        /// </summary>
        public Boolean ShowLanguageLinks { get; set; }

        /// <summary>
        /// Get/set whether or not to display the error layout page
        /// </summary>
        public Boolean ShowErrorHeader { get; set; }

        public String RedirectURL
        {
            get
            {
                switch (this.ErrorType)
                {
                    case eCustomErrorType.AccessIsForbidden:

                        return String.Format("~/Error/Forbidden?id={0}&title={1}&desc={2}{3}",
                                             ((Int32)this.ErrorType).ToString(),
                                             HttpUtility.UrlEncode(this.ErrorTitle),
                                             HttpUtility.UrlEncode(this.ErrorDescription),
                                             ((this.ShowErrorDetails ? "&message=" + HttpUtility.UrlEncode(ErrorDetails) : "")));

                    default:

                        return String.Format("~/Error/Index?id={0}&title={1}&desc={2}{3}",
                                             ((Int32)this.ErrorType).ToString(),
                                             HttpUtility.UrlEncode(this.ErrorTitle),
                                             HttpUtility.UrlEncode(this.ErrorDescription),
                                             ((this.ShowErrorDetails ? "&message=" + HttpUtility.UrlEncode(ErrorDetails) : "")));

                }
            }
        }

        public HCExceptionInfo()
        {
            this.ErrorType = eCustomErrorType.Unknown;
            this.ErrorCode = "";
            this.ErrorTitle = "";
            this.ErrorDescription = "";
            this.ErrorDetails = "";
            this.PageTitle = "";
            this.ShowReturnToLoginButton = false;
            this.ShowLanguageLinks = false;
            this.ShowErrorHeader = false;
        }

    }
    #endregion
}
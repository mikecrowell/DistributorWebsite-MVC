using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DistributorWebsite.MVC
{
    public class MVCError
    {
        [JsonProperty("MVCError", DefaultValueHandling = DefaultValueHandling.Populate)]
        public ErrorInfo Error { get; set; }

        public MVCError()
        {

        }

        public MVCError(ErrorInfo error)
        {
            this.Error = error;
        }
    }

    public class ErrorInfo
    {
        [JsonProperty("status", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("500")]
        public String StatusCode { get; set; }

        [JsonProperty("statusText", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("")]
        public String StatusDesc { get; set; }

        [JsonProperty("data", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("")]
        public ErrorInfoData StatusMessages { get; set; }

        [JsonProperty("showdetails", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("")]
        public Boolean ShowDetails { get; set; }

        [JsonProperty("showlogout", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("")]
        public Boolean ShowLogoutButton { get; set; }

        [JsonProperty("showtryagain", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue(true)]
        public Boolean ShowPleaseTryAgainMessage { get; set; }

        [JsonProperty("config")]
        public ErrorInfoConfig Config { get; set; }
    }

    public class ErrorInfoInnerError
    {
        [JsonProperty("Message", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("")]
        public string Message { get; set; }

        [JsonProperty("StackTrace", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("")]
        public string StackTrace { get; set; }
    }

    public class ErrorInfoConfig
    {
        [JsonProperty("method", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("")]
        public string Method { get; set; }

        [JsonProperty("url", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("")]
        public string URL { get; set; }
    }

    public class ErrorInfoData
    {
        [JsonProperty("Message", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("")]
        public String Message { get; set; }

        [JsonProperty("MessageDetail", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("")]
        public String MessageDetail { get; set; }

        [JsonProperty("error")]
        public ODataErrorInfo Error { get; set; }

        [JsonProperty("InnerError")]
        public ErrorInfoInnerError InnerError { get; set; }

        public ErrorInfoData()
        {
            this.Message = "";
            this.MessageDetail = "";
            this.Error = new ODataErrorInfo();
            this.InnerError = new ErrorInfoInnerError();
        }
    }


    public class ODataErrorInfo
    {
        public string code { get; set; }
        public string message { get; set; }
        public string target { get; set; }
        public ODataInnererror innererror { get; set; }

        public ODataErrorInfo()
        {
            this.code = "";
            this.message = "";
            this.innererror = new ODataInnererror();
            this.target = "";
        }
    }

    public class ODataInnererror
    {
        public string message { get; set; }
        public string type { get; set; }
        public string stacktrace { get; set; }

        public ODataInnererror()
        {
            this.message = "";
            this.type = "";
            this.stacktrace = "";
        }

        public override string ToString()
        {
            return ((!String.IsNullOrWhiteSpace(message) ? message + Environment.NewLine : "") +
                    (!String.IsNullOrWhiteSpace(type) ? type + Environment.NewLine : "") +
                    (!String.IsNullOrWhiteSpace(stacktrace) ? Environment.NewLine + Environment.NewLine + "Stack Trace:" + Environment.NewLine + stacktrace : ""));
        }
    }

}
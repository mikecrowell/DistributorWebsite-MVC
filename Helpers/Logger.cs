using Microsoft.ApplicationInsights;
using Serilog;
using Serilog.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;

namespace DistributorWebsite.MVC.WebUI
{
    public static class Logger
    {
        #region "PROCEDURE: LogCurrentRequest"
        /// <summary>
        /// Log the current request
        /// </summary>
        public static void LogCurrentRequest()
        {
            String sourceAddress;
            String requestedURL;
            String currentUsername;

            if ((Int32)Helpers.Application.Instance.LogLevel.MinimumLevel <= (Int32)LogEventLevel.Information)
            {
                try
                {
                    //-- LOG THE UNAUTHORIZED REQUEST --
                    sourceAddress = Helpers.Common.GetUserAddress();

                    //-- GET THE REQUESTED URL --
                    try
                    {
                        requestedURL = System.Web.HttpContext.Current.Request.Url.ToString();
                    }
                    catch (Exception)
                    {
                        requestedURL = "UNABLE TO DETERMINE";
                    }

                    currentUsername = Helpers.Common.GetCurrentUserName(null);

                    Logger.LogInformation(String.Format("PAGE ACCESS: USER={0} URL={1} SOURCE IP={2}",
                                    currentUsername,
                                    requestedURL,
                                    sourceAddress));
                }
                catch (Exception)
                {
                }
            }
        }
        #endregion

        #region "PROCEDURE: LogControllerAndAction"
        /// <summary>
        /// Log the current controller and action
        /// </summary>
        /// <param name="source"></param>
        public static void LogControllerAndAction([CallerMemberName] string source = null)
        {
            string controllerName = "UNKNOWN";
            string action = "UNKNOWN";

            try
            {
                if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Request != null && System.Web.HttpContext.Current.Request.RequestContext != null && System.Web.HttpContext.Current.Request.RequestContext.RouteData != null)
                {
                    if (System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values.ContainsKey("controller"))
                        controllerName = System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["controller"].ToString();

                    if (System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values.ContainsKey("action"))
                        action = System.Web.HttpContext.Current.Request.RequestContext.RouteData.Values["action"].ToString();
                }
            }
            catch (Exception ex)
            {
                LogError(ex.ToString());
            }

            LogInformation(String.Format("{0}: CONTROLLER:{1} ACTION:{2}",
                           source ?? "UNKNOWN",
                           controllerName,
                           action));
        }
        #endregion

        #region "PROCEDURE: LogMessage"
        /// <summary>
        /// Log a message to the seri log sync
        /// </summary>
        /// <param name="message"></param>
        public static void LogMessage(String message, Serilog.Events.LogEventLevel level)
        {
            Log.Write(level, message);
        }
        #endregion

        #region "PROCEDURE: LogInformation"
        /// <summary>
        /// Log and informational message
        /// </summary>
        /// <param name="message"></param>
        public static void LogInformation(String message, Serilog.Events.LogEventLevel? minimumLevel = null)
        {
            if (!minimumLevel.HasValue || (minimumLevel.HasValue && (Int32)minimumLevel.Value <= (Int32)LogEventLevel.Information))
                LogMessage(message, Serilog.Events.LogEventLevel.Information);
        }
        #endregion

        #region "PROCEDURE: LogError"
        /// <summary>
        /// Log an error message
        /// </summary>
        /// <param name="message"></param>
        public static void LogError(String message, LogEventLevel? minimumLevel = null)
        {
            if (!minimumLevel.HasValue || (minimumLevel.HasValue && (Int32)minimumLevel.Value <= (Int32)LogEventLevel.Error))
            {
                LogMessage(message, Serilog.Events.LogEventLevel.Error);
                LogErrorToAppInsights(new Exception(message));
            }
        }
        #endregion

        #region "PROCEDURE: LogError"
        /// <summary>
        /// Log an error message
        /// </summary>
        /// <param name="message"></param>
        public static void LogError(String message, Exception ex, LogEventLevel? minimumLevel = null)
        {
            if (!minimumLevel.HasValue || (minimumLevel.HasValue && (Int32)minimumLevel.Value <= (Int32)LogEventLevel.Error))
            {
                Log.Write(Serilog.Events.LogEventLevel.Error, ex, message);
                LogErrorToAppInsights(ex);
            }
        }
        #endregion

        #region "PROCEDURE: LogWarning"
        /// <summary>
        /// Log an error message
        /// </summary>
        /// <param name="message"></param>
        public static void LogWarning(String message, LogEventLevel? minimumLevel = null)
        {
            if (!minimumLevel.HasValue || (minimumLevel.HasValue && (Int32)minimumLevel.Value <= (Int32)LogEventLevel.Warning))
                LogMessage(message, Serilog.Events.LogEventLevel.Warning);
        }
        #endregion

        #region "PROCEDURE: LogDebug"
        /// <summary>
        /// Log an error message
        /// </summary>
        /// <param name="message"></param>
        public static void LogDebug(String message, LogEventLevel? minimumLevel = null)
        {
            if (!minimumLevel.HasValue || (minimumLevel.HasValue && (Int32)minimumLevel.Value <= (Int32)LogEventLevel.Debug))
                LogMessage(message, Serilog.Events.LogEventLevel.Debug);
        }
        #endregion

        #region "PROCEDURE: LogFatal"
        /// <summary>
        /// Log an error message
        /// </summary>
        /// <param name="message"></param>
        public static void LogFatal(String message, LogEventLevel? minimumLevel = null)
        {
            if (!minimumLevel.HasValue || (minimumLevel.HasValue && (Int32)minimumLevel.Value <= (Int32)LogEventLevel.Fatal))
                LogMessage(message, Serilog.Events.LogEventLevel.Fatal);
        }
        #endregion

        #region "PROCEDURE: LogVerbose"
        /// <summary>
        /// Log an error message
        /// </summary>
        /// <param name="message"></param>
        public static void LogVerbose(String message, LogEventLevel? minimumLevel = null)
        {
            if (!minimumLevel.HasValue || (minimumLevel.HasValue && (Int32)minimumLevel.Value <= (Int32)LogEventLevel.Verbose))
                LogMessage(message, Serilog.Events.LogEventLevel.Verbose);
        }
        #endregion

        #region "PROCEDURE: LogErrorToAppInsights"
        /// <summary>
        /// Log an error to application insights
        /// </summary>
        /// <param name="error"></param>
        public static void LogErrorToAppInsights(Exception ex, IDictionary<string, string> properties = null, IDictionary<string, double> metrics = null)
        {
            TelemetryClient ai;

            try
            {
                //-- INITIALIZE THE AI CLIENT --
                ai = new TelemetryClient(new Microsoft.ApplicationInsights.Extensibility.TelemetryConfiguration(Helpers.Application.Instance.AppInsightsKey));

                ai.TrackException(ex, properties, metrics);
            }
            catch (Exception exAI)
            {
                System.Diagnostics.Trace.TraceError("Failed to log to app insights: " + exAI.ToString());
            }
            finally
            {
                ai = null;
            }
        }
        #endregion
    }
}
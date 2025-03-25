using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributorWebsite.MVC.WebUI.Helpers.Structures
{
    #region "ENUM: eTokenRefreshResponse"
    /// <summary>
    /// Refresh the access token
    /// </summary>
    public enum eTokenRefreshResponse
    {
        TokenIsValid,
        TokenWasRefreshed,
        RefreshTokenIsExpired,
        FailedToRefreshAccessToken,
        ClientUpdatedFromSession,
        CustomMessage
    }
    #endregion

    #region "CLASS: TokenRefreshResponse"
    /// <summary>
    /// Refresh the response token
    /// </summary>
    public class TokenRefreshResponse
    {
        [JsonConverter(typeof(StringEnumConverter))]
        public eTokenRefreshResponse Status { get; set; }

        /// <summary>
        /// Get/set the error type returned from the identity server
        /// </summary>
        public String Error { get; set; }

        /// <summary>
        /// Get/set the status message
        /// </summary>
        public String ErrorMessage { get; set; }

        /// <summary>
        /// Get/set the token expiration date
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Get/set the new access token
        /// </summary>
        public String NewAccessToken { get; set; }
    }
    #endregion
}
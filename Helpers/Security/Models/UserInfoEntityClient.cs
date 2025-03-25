using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributorWebsite.MVC.WebUI.Helpers.Security
{
    public class UserInfoEntityClient
    {
        /// <summary>
        /// Get/set the client
        /// </summary>
        [JsonProperty("cl")]
        public string Client { get; set; }

        /// <summary>
        /// Get/set the client namne
        /// </summary>
        [JsonProperty("clnm")]
        public string ClientName { get; set; }

        /// <summary>
        /// Get/set the active status
        /// </summary>
        [JsonProperty("active")]
        public bool IsActive { get; set; }
    }
}
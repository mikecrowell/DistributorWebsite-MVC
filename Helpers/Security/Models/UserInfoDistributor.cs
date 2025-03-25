using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributorWebsite.MVC.WebUI.Helpers.Security
{
    /// <summary>
    /// Defines an individual distributor linked to the current user
    /// </summary>
    public class UserInfoDistributor
    {
        /// <summary>
        /// Get/set the current distributor number
        /// </summary>
        [JsonProperty("dn")]
        public string DistributorNo { get; set; }

        /// <summary>
        /// Get/set the distributor's company name
        /// </summary>
        [JsonProperty("nm")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Get/set whether or not the current distributor is active
        /// </summary>
        [JsonProperty("act")]
        public bool Active { get; set; }

        /// <summary>
        /// Get/set whether or not this is the user's primary distributor
        /// </summary>
        [JsonProperty("pr")]
        public bool IsPrimary { get; set; }
    }
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributorWebsite.MVC.WebUI.Helpers.Models
{
    public class RecentUser
    {
        /// <summary>
        /// Get/set the user's entity code
        /// </summary>
        [JsonProperty("ec")]
        public String EntityCode { get; set; }

        /// <summary>
        /// Get/set the entity's name
        /// </summary>
        [JsonProperty("en")]
        public String EntityName { get; set; }

        /// <summary>
        /// Get/set the user's user type
        /// </summary>
        [JsonIgnore]
        public eUserType UserType
        {
            get
            {
                if (EntityCode.Length == 5)
                    return eUserType.Distributor;
                else
                    return eUserType.Salesperson;
            }
        }

        /// <summary>
        /// Get/set the user's brand
        /// </summary>
        [JsonProperty("ub")]
        public string Brand { get; set; }

        /// <summary>
        /// Get/set the username to impersonate
        /// </summary>
        [JsonProperty("uimp")]
        public string UsernameToImpersonate { get; set; }

        /// <summary>
        /// Get/set the time the user was last logged into
        /// </summary>
        [JsonProperty("dt")]
        public DateTime LastLoginDate { get; set; }
    }
}
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace DistributorWebsite.MVC.WebUI.Helpers.Security
{
    public class UserInfoEntity
    {
        /// <summary>
        /// Get/set the distributor number
        /// </summary>
        [JsonProperty("dno")]
        public string DistributorNo { get; set; }

        /// <summary>
        /// Get/set the distributor's name
        /// </summary>
        [JsonProperty("dnm")]
        public string DistributorName { get; set; }

        /// <summary>
        /// Get/set the distributor's company name
        /// </summary>
        [JsonProperty("dcnm")]
        public string CompanyName { get; set; }

        /// <summary>
        /// Get/set the default client
        /// </summary>
        [JsonProperty("dfc")]
        public string DefaultClient { get; set; }

        /// <summary>
        /// Get/set the salesperson code
        /// </summary>
        [JsonProperty("spcd")]
        public string SalespersonCode { get; set; }

        /// <summary>
        /// Get/set the salesperson name
        /// </summary>
        [JsonProperty("spnm")]
        public string SalespersonName { get; set; }

        /// <summary>
        /// Get/set the brand
        /// </summary>
        [JsonProperty("brand")]
        public string Brand { get; set; }

        /// <summary>
        /// Get/set the league
        /// </summary>
        [JsonProperty("lg")]
        public string League { get; set; }

        /// <summary>
        /// Get/set whether or not the current entity is a distributor
        /// </summary>
        [JsonProperty("isdist")]
        public bool IsDistributor { get; set; }

        /// <summary>
        /// Get/set the current entity's level
        /// </summary>
        [JsonProperty("lvl")]
        public Int32 Level { get; set; }

        /// <summary>
        /// Get/set whether or not the current user is a Joint Venture
        /// </summary>
        [JsonProperty("isjv")]
        public bool IsJV { get; set; }

        /// <summary>
        /// Get/set whether or not the current user is a Territory Director
        /// </summary>
        [JsonProperty("istd")]
        public bool IsTD { get; set; }

        /// <summary>
        /// Get/set whether or not the current user is a newcomer
        /// </summary>
        [JsonProperty("isnc")]
        public bool IsNewcomer { get; set; }

        /// <summary>
        /// Get/set whether or not the current user is a territory master
        /// </summary>
        [JsonProperty("istmast")]
        public bool IsMaster { get; set; }

        /// <summary>
        /// Get/set whether or not the current entity is active
        /// </summary>
        [JsonProperty("active")]
        public bool IsActive { get; set; }

        /// <summary>
        /// Get/set the current user's sponsor
        /// </summary>
        [JsonProperty("sponsor")]
        public string Sponsor { get; set; }

        /// <summary>
        /// Get/set whether or not the current entity is a fair show distributor
        /// </summary>
        [JsonProperty("ifsd")]
        public bool IsFairShowDistributor { get; set; }

        /// <summary>
        /// Get/set whether or not the current entity is a bridal distributor
        /// </summary>
        [JsonProperty("ibd")]
        public bool IsBridalDistributor { get; set; }

        /// <summary>
        /// Get/set the current user's home distributor
        /// </summary>
        [JsonProperty("hd")]
        public string HomeDistributor { get; set; }

        /// <summary>
        /// Get/set the current entity's territory director
        /// </summary>
        [JsonProperty("td")]
        public string TerritoryDirector { get; set; }

        /// <summary>
        /// Get/set the current entity's company name
        /// </summary>
        [JsonProperty("tdcn")]
        public string TerritoryDirectorCompanyName { get; set; }

        /// <summary>
        /// Get/set the current entity's territory director's name
        /// </summary>
        [JsonProperty("tdnm")]
        public string TerritoryDirectorName { get; set; }

        /// <summary>
        /// Get/set the current entity's country code
        /// </summary>
        [JsonProperty("ctry")]
        public string CountryCode { get; set; }

        /// <summary>
        /// Get/set the current entity's state
        /// </summary>
        [JsonProperty("state")]
        public string State { get; set; }

        /// <summary>
        /// Get/set the current entity's security string
        /// </summary>
        [JsonProperty("ss", DefaultValueHandling = DefaultValueHandling.Populate)]
        [DefaultValue("0000000000000000000000000000000000000000000000000000000000000000")]
        public string SecurityString { get; set; }

        /// <summary>
        /// Get/set the current entity's market segment
        /// </summary>
        [JsonProperty("ms")]
        public string MarketSegment { get; set; }

        /// <summary>
        /// Get/set the list of clients
        /// </summary>
        [JsonProperty("clients")]
        public UserInfoEntityClient[] Clients { get; set; }

        /// <summary>
        /// Get the current client bit based on the supported clients for the current entity
        /// </summary>
        public eClient ClientBits
        {
            get
            {
                eClient clients = eClient.Unknown;
                eClient currentClient;

                foreach (var client in Clients)
                {
                    if (Enum.TryParse<eClient>(client.Client.ToUpper(), out currentClient))
                        clients = clients | currentClient;
                }

                return (clients);
            }
        }

        /// <summary>
        /// Get the current type bit flags for the current user
        /// </summary>
        public eType TypeBits
        {
            get
            {
                return (this.IsDistributor ? eType.Distributor : 0) |
                       (this.MarketSegment == "US" ? eType.Anglo : 0) |
                       (this.MarketSegment == "ES" ? eType.Hispanic : 0) |
                       (this.IsJV ? eType.JV : 0) |
                       (this.Level == 1 ? eType.Level1 : 0) |
                       (this.Level == 2 ? eType.Level2 : 0) |
                       (this.Level == 3 ? eType.Level3 : 0) |
                       (this.Level == 4 ? eType.Level4 : 0) |
                       (this.IsMaster ? eType.Master : 0) |
                       (this.IsNewcomer ? eType.Newcomer : 0) |
                       (!this.IsDistributor ? eType.Salesperson : 0) |
                       (this.IsTD ? eType.TD : 0) |
                       (this.IsFairShowDistributor ? eType.FairShowDistributor : 0) |
                       (this.IsBridalDistributor ? eType.BridalDistributor : 0);
            }
        }
    }
}
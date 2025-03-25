using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributorWebsite.MVC.WebUI.Helpers.Models
{
    /// <summary>
    /// Define information about an individual client
    /// </summary>
    public class ClientInfo
    {
        /// <summary>
        /// Get/set the client code
        /// </summary>
        public string Client { get; set; }

        /// <summary>
        /// Get/set the client bit
        /// </summary>
        public Int32 ClientBit { get; set; }

        /// <summary>
        /// Get/set the client name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Get/set the country code
        /// </summary>
        public string CountryCode { get; set; }

        /// <summary>
        /// Get/set the currency
        /// </summary>
        public string Currency { get; set; }

        /// <summary>
        /// Generic constructor
        /// </summary>
        public ClientInfo()
        {

        }

        /// <summary>
        /// Initialize the class with values
        /// </summary>
        /// <param name="client"></param>
        /// <param name="name"></param>
        /// <param name="country"></param>
        /// <param name="currency"></param>
        public ClientInfo(string client, Int32 clientbit, string name, string country, string currency)
        {
            this.Client = client;
            this.ClientBit = clientbit;
            this.Name = name;
            this.CountryCode = country;
            this.Currency = currency;
        }
    }
}
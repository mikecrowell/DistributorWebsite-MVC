using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributorWebsite.MVC.WebUI.Helpers.Models
{
    public class ClientPaymentSettingsDTO
    {
        /// <summary>
        /// Get/set whether or not credit card payments are allowed in the client
        /// </summary>
        public bool AllowCreditCardPayments { get; set; }

        /// <summary>
        /// Get/set whether or not credit card email links are allowed in the client
        /// </summary>
	    public bool AllowCreditCardEmailLinks { get; set; }

        /// <summary>
        /// Get/set whether or not credit card email receipts are allowed in the client
        /// </summary>
		public bool AllowCreditCardEmailReceipts { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributorWebsite.MVC.WebUI.Helpers.Models
{
    public class PaymentStatusDTO
    {
        /// <summary>
        /// Get/set the payment ID
        /// </summary>
        public int PaymentID { get; set; }

        /// <summary>
        /// Get/set the current status
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Get/set the last 4 of the card number
        /// </summary>
        public object CardNumber { get; set; }

        /// <summary>
        /// Get/set the card type that was used
        /// </summary>
        public object CardType { get; set; }

        /// <summary>
        /// Get/set the calculated submission status
        /// </summary>
        public string SubmissionStatus { get; set; }

        /// <summary>
        /// Get/set the reason code 
        /// </summary>
        public string ReasonCode { get; set; }

        /// <summary>
        /// Get/set the reason
        /// </summary>
        public string Reason { get; set; }
    }

}
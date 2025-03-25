using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributorWebsite.MVC.WebUI
{
    /// <summary>
    /// Generic forbidden message
    /// </summary>
    public class HCForbiddenException : System.Exception
    {
        /// <summary>
        /// Initialize the generic message
        /// </summary>
        /// <param name="message"></param>
        public HCForbiddenException(String message):base(message)
        {

        }
    }
}
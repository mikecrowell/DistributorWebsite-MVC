using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributorWebsite.MVC.WebUI.Helpers.Models
{
    public class ODataResponse<T>
    {
        /// <summary>
        /// Get/set the odata response value
        /// </summary>
        public T Value { get; set; }
    }
}
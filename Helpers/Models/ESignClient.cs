using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributorWebsite.MVC.WebUI.Helpers.Models
{
    public class ESignClient
    {
        public string Client { get; set; }
    }

    public class eSignClientDTO
    {
        public ESignClient[] Clients { get; set; }
    }
}
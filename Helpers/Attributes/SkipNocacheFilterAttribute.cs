using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributorWebsite.MVC.WebUI
{
    /// <summary>
    /// Custom authorization attribute to authorize a controller against the custom WEBDistributorSecurityItems table
    /// </summary>
    public class SkipNocacheFilterAttribute : Attribute
    {
    }
}
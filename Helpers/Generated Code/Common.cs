using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DistributorWebsite.MVC.WebUI.Helpers.Models;

namespace DistributorWebsite.MVC.WebUI.Helpers
{
    public static partial class Common
    { 
		#region "FUNCTION: GetClientInfo"
		/// <summary>
		/// Get information about the specified client
		/// </summary>
		public static ClientInfo GetClientInfo(string client)
		{			
			switch (client)
			{
				case "BA":
					return new ClientInfo("BA",1,"***DEV****Hy Cite BA S.R.L.","AR","ARS");

				case "BR":
					return new ClientInfo("BR",2,"***DEV****Consumer Financing USA - HCC","US","USD");

				case "BZ":
					return new ClientInfo("BZ",4,"***DEV****Royal Prestige Brazil Consolidated","BR","BRL");

				case "CO":
					return new ClientInfo("CO",8,"***DEV****Hy Cite Enterprises Colombia, SAS","CO","COP");

				case "DR":
					return new ClientInfo("DR",16,"***DEV****Royal Prestige DR S.R.L.","DO","DOP");

				case "HC":
					return new ClientInfo("HC",32,"***DEV****Hy Cite Enterprises, LLC","US","USD");

				case "MX":
					return new ClientInfo("MX",64,"***DEV****Hy Cite Mexico Consolidated","MX","MXP");

				case "PE":
					return new ClientInfo("PE",128,"***DEV****Hy Cite Peru, SRL","PE","PEN");

				case "PH":
					return new ClientInfo("PH",256,"***DEV****Hy Cite Philippines Corporation","PH","PHP");

				case "RC":
					return new ClientInfo("RC",512,"***DEV****Royal Charm, LTD","CA","CAD");

				case "EC":
					return new ClientInfo("EC",1024,"***DEV****Hy Cite Enterprises-Ecuador Cia. Ltda","EC","USD");

				default:
					return(null);
			}
		}	
		#endregion
	}
}


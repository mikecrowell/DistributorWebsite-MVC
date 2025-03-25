using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributorWebsite.MVC.WebUI
{
	#region "ENUM: eClientBits"
	/// <summary>
	/// Define the various client bit types
	/// </summary>
	[Flags]
	public enum eClientBits
	{	
	   Unknown = 0
	   	,BA = 1 
				  	,BR = 2 
				  	,BZ = 4 
				  	,CO = 8 
				  	,DR = 16 
				  	,HC = 32 
				  	,MX = 64 
				  	,PE = 128 
				  	,PH = 256 
				  	,RC = 512 
				  	,EC = 1024 
				  	}
	#endregion
}
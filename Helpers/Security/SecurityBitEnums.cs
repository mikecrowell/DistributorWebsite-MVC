using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System.Runtime.Serialization;

namespace DistributorWebsite.MVC.WebUI
{
	#region "ENUM: eBrand"
	/// <summary>
	/// Define the various product lines that distributors and salespeople can sell
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	[Flags]
	public enum eBrand
	{
			/// <summary>
		/// Unknown client
		/// </summary>
		Unknown = 0

		/// <summary>
		/// HyCite
		/// </summary>
		,HC = 1

		/// <summary>
		/// Kitchen Charm
		/// </summary>
		,KC = 2

		/// <summary>
		/// Nautilus
		/// </summary>
		,NA = 4

		/// <summary>
		/// NutraEase
		/// </summary>
		,NE = 8

		/// <summary>
		/// Royal Prestige
		/// </summary>
		,RP = 16

	}
	#endregion

	#region "ENUM: eClient"
	/// <summary>
	/// Define the various clients that distributors and salespeople can sell in
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	[Flags]
	public enum eClient
	{
			/// <summary>
		/// Unknown client
		/// </summary>
		Unknown = 0

		/// <summary>
		/// ***DEV****Hy Cite BA S.R.L.
		/// </summary>
		,BA = 1

		/// <summary>
		/// ***DEV****Consumer Financing USA - HCC
		/// </summary>
		,BR = 2

		/// <summary>
		/// ***DEV****Royal Prestige Brazil Consolidated
		/// </summary>
		,BZ = 4

		/// <summary>
		/// ***DEV****Hy Cite Enterprises Colombia, SAS
		/// </summary>
		,CO = 8

		/// <summary>
		/// ***DEV****Royal Prestige DR S.R.L.
		/// </summary>
		,DR = 16

		/// <summary>
		/// ***DEV****Hy Cite Enterprises, LLC
		/// </summary>
		,HC = 32

		/// <summary>
		/// ***DEV****Hy Cite Mexico Consolidated
		/// </summary>
		,MX = 64

		/// <summary>
		/// ***DEV****Hy Cite Peru, SRL
		/// </summary>
		,PE = 128

		/// <summary>
		/// ***DEV****Hy Cite Philippines Corporation
		/// </summary>
		,PH = 256

		/// <summary>
		/// ***DEV****Royal Charm, LTD
		/// </summary>
		,RC = 512

		/// <summary>
		/// ***DEV***Hy Cite Enterprises-Ecuador Cia. Ltda
		/// </summary>
		,EC = 1024

	}
	#endregion

	#region "ENUM: eType"
	/// <summary>
	/// Define the various categories that distributors and salespeople can be clasified in
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	[Flags]
	public enum eType
	{
			/// <summary>
		/// Unknown client
		/// </summary>
		Unknown = 0

		/// <summary>
		/// Entity is a distributor
		/// </summary>
		,Distributor = 1

		/// <summary>
		/// Entity is a Joint Venture
		/// </summary>
		,JV = 2

		/// <summary>
		/// Entity is a Territory Director
		/// </summary>
		,TD = 4

		/// <summary>
		/// Entity is a Territory Master
		/// </summary>
		,Master = 8

		/// <summary>
		/// Entity is a level 1 distributor
		/// </summary>
		,Level1 = 16

		/// <summary>
		/// Entity is a level 2 distributor
		/// </summary>
		,Level2 = 32

		/// <summary>
		/// Entity is a level 3 distributor
		/// </summary>
		,Level3 = 64

		/// <summary>
		/// Entity is a level 4 distributor
		/// </summary>
		,Level4 = 128

		/// <summary>
		/// Entity is a newcomer
		/// </summary>
		,Newcomer = 256

		/// <summary>
		/// Entity is a salesperson (not a distributor)
		/// </summary>
		,Salesperson = 512

		/// <summary>
		/// Entity is a fair show distributor
		/// </summary>
		,FairShowDistributor = 1024

		/// <summary>
		/// Entity is a bridal distributor
		/// </summary>
		,BridalDistributor = 2048

		/// <summary>
		/// Entity is in the anglo market segment
		/// </summary>
		,Anglo = 4096

		/// <summary>
		/// Entity is in the hispanic market segment
		/// </summary>
		,Hispanic = 8192

		/// <summary>
		/// Entity has a FCR level of ROYAL
		/// </summary>
		,ROYAL = 16384

		/// <summary>
		/// Entity has a FCR level of BLUE
		/// </summary>
		,BLUE = 32768

	}
	#endregion

	#region "ENUM: eLeague"
	/// <summary>
	/// Define the various leagues that distributors and salespeople can sell in
	/// </summary>
	[JsonConverter(typeof(StringEnumConverter))]
	[Flags]
	public enum eLeague
	{
			/// <summary>
		/// Unknown client
		/// </summary>
		Unknown = 0

		/// <summary>
		/// American League
		/// </summary>
		,American = 1

		/// <summary>
		/// World League
		/// </summary>
		,World = 2

		/// <summary>
		/// International League
		/// </summary>
		,International = 4

	}
	#endregion
}

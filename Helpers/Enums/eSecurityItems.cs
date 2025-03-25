using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DistributorWebsite.MVC.WebUI
{
	#region "ENUM: eSecurityItem"
	/// <summary>
	/// Define the various security item index positions
	/// </summary>
	public enum eSecurityItem
	{			
	   
		
		/// <summary>
		/// Distributor Website
		/// </summary>
		DistributorWebsite = 0,
		
		/// <summary>
		/// Company Information Tab
		/// </summary>
		tabCompanyInformation = 1,
		
		/// <summary>
		/// Announcements
		/// </summary>
		mnuAnnouncements = 4,
		
		/// <summary>
		/// Documents and Forms
		/// </summary>
		mnuDocumentsAndForms = 5,
		
		/// <summary>
		/// View Sanction Policy Document(s)
		/// </summary>
		opViewPenaltyPolicy = 6,
		
		/// <summary>
		/// View the Sales Practices Manual
		/// </summary>
		opViewSalesPracticesManual = 7,
		
		/// <summary>
		/// View the California Acknowledgement Document
		/// </summary>
		opViewCaliforniaAcknowledgement = 8,
		
		/// <summary>
		/// View the Distributor Agreement
		/// </summary>
		opViewDistributorAgreement = 9,
		
		/// <summary>
		/// Contact Us
		/// </summary>
		mnuContactUs = 10,
		
		/// <summary>
		/// Memos
		/// </summary>
		mnuMemos = 11,
		
		/// <summary>
		/// Distributor Directory
		/// </summary>
		mnuDistributorDirectory = 12,
		
		/// <summary>
		/// Employee Directory
		/// </summary>
		mnuEmployeeDirectory = 13,
		
		/// <summary>
		/// View the Distributor Manual
		/// </summary>
		opViewDistributorManual = 14,
		
		/// <summary>
		/// Workflow Tab
		/// </summary>
		tabWorkflow = 15,
		
		/// <summary>
		/// Customer Workflow
		/// </summary>
		mnuCustomerWorkflow = 16,
		
		/// <summary>
		/// Manage Customer Workflow Notes
		/// </summary>
		opManageCustomerWorkflow = 17,
		
		/// <summary>
		/// Distributor Workflow
		/// </summary>
		mnuDistributorWorkflow = 18,
		
		/// <summary>
		/// Manage Distributor Workflow Notes
		/// </summary>
		opManageDistributorWorkflow = 19,
		
		/// <summary>
		/// Returns Workflow
		/// </summary>
		mnuReturnsWorkflow = 20,
		
		/// <summary>
		/// Make Customer Workflow Credit Card Payments
		/// </summary>
		opMakeCustomerWorkflowPayments = 21,
		
		/// <summary>
		/// Credit Card Processing
		/// </summary>
		mnuCreditCardProcessing = 22,
		
		/// <summary>
		/// Purchases Tab
		/// </summary>
		tabPurchases = 23,
		
		/// <summary>
		/// Territory Purchases
		/// </summary>
		mnuTerritoryPurchases = 24,
		
		/// <summary>
		/// Company Purchases
		/// </summary>
		mnuCompanyPurchases = 25,
		
		/// <summary>
		/// JD Purchases
		/// </summary>
		mnuJVPurchases = 26,
		
		/// <summary>
		/// Competitive Arena Tab
		/// </summary>
		tabCompetetiveArena = 27,
		
		/// <summary>
		/// Territory Volume
		/// </summary>
		mnuTerritoryVolume = 28,
		
		/// <summary>
		/// Company Volume
		/// </summary>
		mnuCompanyVolume = 29,
		
		/// <summary>
		/// JD Volume
		/// </summary>
		mnuJVVolume = 30,
		
		/// <summary>
		/// Entrepreneur Volume
		/// </summary>
		mnuSalespersonVolume = 31,
		
		/// <summary>
		/// Accounts Receivable Tab
		/// </summary>
		tabAccountsReceivable = 32,
		
		/// <summary>
		/// Balance Summary
		/// </summary>
		mnuBalanceSummary = 33,
		
		/// <summary>
		/// Collector Messages
		/// </summary>
		mnuCollectorMessages = 34,
		
		/// <summary>
		/// Account Search
		/// </summary>
		mnuAccountSearch = 35,
		
		/// <summary>
		/// Addon Account Search
		/// </summary>
		mnuAddonAccountSearch = 36,
		
		/// <summary>
		/// List Payments
		/// </summary>
		mnuListPayments = 37,
		
		/// <summary>
		/// Reports
		/// </summary>
		mnuReports = 38,
		
		/// <summary>
		/// Credit Reports
		/// </summary>
		mnuCreditReports = 39,
		
		/// <summary>
		/// Warehouse Sales Order Report
		/// </summary>
		mnuWarehouseSalesOrders = 40,
		
		/// <summary>
		/// Consumer - Deposit Slip Needed
		/// </summary>
		mnuOPDepositSlipNeeded = 41,
		
		/// <summary>
		/// Distributor - Deposit Slip Needed
		/// </summary>
		mnuDCSDepositSlipNeeded = 42,
		
		/// <summary>
		/// Landing Page
		/// </summary>
		pageLandingPage = 43,
		
		/// <summary>
		/// Ethics Training
		/// </summary>
		mnuEthicsTraining = 44,
		
		/// <summary>
		/// View  Loan for Merchandise Details
		/// </summary>
		opViewSampleBalanceDetails = 45,
		
		/// <summary>
		/// Transfer Excess Reserve to Account Activity Statement
		/// </summary>
		opTransferExcessReserve = 46,
		
		/// <summary>
		/// View the Sales Tax Rates Report
		/// </summary>
		opViewSalesTaxRatesReport = 47,
		
		/// <summary>
		/// View the Account Summary Report
		/// </summary>
		opViewAccountSummaryReport = 48,
		
		/// <summary>
		/// View the Cash Receipts Report
		/// </summary>
		opViewCashReceiptsReport = 49,
		
		/// <summary>
		/// View the Service Charge Report
		/// </summary>
		opViewServiceChargeReport = 50,
		
		/// <summary>
		/// View the Cartridge Replacement Letter Report
		/// </summary>
		opViewCartridgeLetterReport = 51,
		
		/// <summary>
		/// View the Reserve Summary Report
		/// </summary>
		opViewReserveSummaryReport = 52,
		
		/// <summary>
		/// View the Territory Aging Report
		/// </summary>
		opViewTerritoryAgingReport = 53,
		
		/// <summary>
		/// View the Inactive Carp Addon Report
		/// </summary>
		opViewInactiveCarpAddons = 54,
		
		/// <summary>
		/// View the RP Customer Card Report
		/// </summary>
		opViewCustomerCardReport = 55,
		
		/// <summary>
		/// Weekly Bulletins
		/// </summary>
		mnuWeeklyBulletin = 56,
		
		/// <summary>
		/// Pacemakers
		/// </summary>
		mnuPacemakers = 57,
		
		/// <summary>
		/// Pacesetters
		/// </summary>
		mnuPacesetters = 58,
		
		/// <summary>
		/// Entrepreneur Application
		/// </summary>
		mnuIcebreakers = 59,
		
		/// <summary>
		/// Mexico Pacemakers
		/// </summary>
		mnuMexicoPacemakers = 60,
		
		/// <summary>
		/// JD Application
		/// </summary>
		mnuJVApplication = 61,
		
		/// <summary>
		/// JD/Distributor Recommendation Dashboard
		/// </summary>
		mnuRecommendationDashboard = 62,
		
		/// <summary>
		/// View the Entrepreneur Promoter Report
		/// </summary>
		mnuSalespeople = 63,
		
		/// <summary>
		/// Volume Entry
		/// </summary>
		mnuVolumeEntry = 64,
		
		/// <summary>
		/// Rules and Definitions
		/// </summary>
		mnuRulesAndDefinitions = 65,
		
		/// <summary>
		/// Awards Catalog
		/// </summary>
		mnuAwardsCatalog = 66,
		
		/// <summary>
		/// Sales and Marketing Tab
		/// </summary>
		tabSalesAndMarketing = 67,
		
		/// <summary>
		/// RP Today
		/// </summary>
		mnuRPToday = 68,
		
		/// <summary>
		/// Training
		/// </summary>
		mnuTraining = 69,
		
		/// <summary>
		/// Universidad Online
		/// </summary>
		mnuUniversidad = 70,
		
		/// <summary>
		/// Download Center (WIDEN)
		/// </summary>
		mnuSalesMaterials = 71,
		
		/// <summary>
		/// Conventions and Incentives
		/// </summary>
		mnuConventionsAndIncentives = 72,
		
		/// <summary>
		/// Training Seminars
		/// </summary>
		mnuTrainingSeminars = 73,
		
		/// <summary>
		/// Cardridge Replacement Letters
		/// </summary>
		mnuCARP = 74,
		
		/// <summary>
		/// Recipe Club Campaign
		/// </summary>
		mnuRecipleClubCampaign = 75,
		
		/// <summary>
		/// Magazine Leads
		/// </summary>
		mnuMagazineLeads = 76,
		
		/// <summary>
		/// Territory Director Meeting Fund
		/// </summary>
		mnuTDMeetingFund = 77,
		
		/// <summary>
		/// JD Payments
		/// </summary>
		mnuJVPayments = 78,
		
		/// <summary>
		/// Monthly Overrides
		/// </summary>
		mnuMonthlyOverrides = 79,
		
		/// <summary>
		/// Monthly Overrides
		/// </summary>
		tabOverrides = 80,
		
		/// <summary>
		/// Eligible Purchases
		/// </summary>
		mnuReserveAdjustment = 81,
		
		/// <summary>
		/// Reserve Adjustment Memos
		/// </summary>
		mnuReserveAdjustmentMemos = 83,
		
		/// <summary>
		/// Distributor Service
		/// </summary>
		tabDistributorService = 84,
		
		/// <summary>
		/// Account Activity Statements
		/// </summary>
		mnuTransmittals = 85,
		
		/// <summary>
		/// Account Activity Statement Summary
		/// </summary>
		mnuTransmittalSummary = 86,
		
		/// <summary>
		/// Shipping Search
		/// </summary>
		mnuShippingSearch = 87,
		
		/// <summary>
		/// Backorders
		/// </summary>
		mnuBackorders = 88,
		
		/// <summary>
		/// Pricelist
		/// </summary>
		mnuPricelist = 89,
		
		/// <summary>
		/// Price Quote
		/// </summary>
		mnuPriceQuote = 90,
		
		/// <summary>
		/// Web Orders
		/// </summary>
		mnuWebOrders = 91,
		
		/// <summary>
		/// View Web Order History
		/// </summary>
		opViewWebOrderHistory = 92,
		
		/// <summary>
		/// Pre-Qualification Hotline
		/// </summary>
		mnuPreQualificationHotline = 93,
		
		/// <summary>
		/// Factura Search
		/// </summary>
		mnuFacturaSearch = 94,
		
		/// <summary>
		/// Order Approvals & Uploads
		/// </summary>
		mnuDocumentUploads = 95,
		
		/// <summary>
		/// Access Point Availability
		/// </summary>
		mnuAccessPoints = 96,
		
		/// <summary>
		/// Distributor Financing
		/// </summary>
		tabDistributorFinancing = 97,
		
		/// <summary>
		/// Balance Summary
		/// </summary>
		mnuDFBalanceSummary = 98,
		
		/// <summary>
		/// Account Search
		/// </summary>
		mnuDFAccountSearch = 99,
		
		/// <summary>
		/// Reports
		/// </summary>
		mnuDFReports = 100,
		
		/// <summary>
		/// Eligible Accounts
		/// </summary>
		mnuDFEligibleAccounts = 101,
		
		/// <summary>
		/// Payment Posting
		/// </summary>
		mnuDFPaymentPosting = 102,
		
		/// <summary>
		/// Layaway
		/// </summary>
		tabLayaway = 103,
		
		/// <summary>
		/// Approved Layaway Orders
		/// </summary>
		mnuLWWorkflow = 104,
		
		/// <summary>
		/// Account Search
		/// </summary>
		mnuLWAccountSearch = 105,
		
		/// <summary>
		/// Eligible Accounts
		/// </summary>
		mnuLWEligibleAccounts = 106,
		
		/// <summary>
		/// Balance Summary
		/// </summary>
		mnuLWBalanceSummary = 107,
		
		/// <summary>
		/// Reports
		/// </summary>
		mnuLWReports = 108,
		
		/// <summary>
		/// Sales Lead Administration
		/// </summary>
		tabSalesLeadAdministration = 109,
		
		/// <summary>
		/// Possible Sales Leads
		/// </summary>
		mnuPossibleLeads = 110,
		
		/// <summary>
		/// Purchases Sales Leads
		/// </summary>
		mnuPurchasedLeads = 111,
		
		/// <summary>
		/// Bridal Telemarketing
		/// </summary>
		tabBridalTelemarketing = 112,
		
		/// <summary>
		/// Reports
		/// </summary>
		mnuBridalReports = 113,
		
		/// <summary>
		/// Show Results
		/// </summary>
		mnuShowResults = 114,
		
		/// <summary>
		/// Show Management
		/// </summary>
		mnuShowManagement = 115,
		
		/// <summary>
		/// Show Venues
		/// </summary>
		tabShowVenues = 116,
		
		/// <summary>
		/// Overview
		/// </summary>
		mnuShowVenuesOverview = 117,
		
		/// <summary>
		/// Book A Show
		/// </summary>
		mnuBookAShow = 118,
		
		/// <summary>
		/// Caution List
		/// </summary>
		mnuCautionList = 119,
		
		/// <summary>
		/// 52 Week Show Calendar
		/// </summary>
		mnu52WeekCalendar = 120,
		
		/// <summary>
		/// Search for Zero Pay Accounts
		/// </summary>
		opViewZeroPayAccounts = 121,
		
		/// <summary>
		/// Search for Paid in Full Accounts
		/// </summary>
		opViewPaidInFullAccounts = 122,
		
		/// <summary>
		/// Search for All Accounts
		/// </summary>
		opViewAllAccounts = 123,
		
		/// <summary>
		/// Search for Accounts Carrying a Balance
		/// </summary>
		opViewAccountsWithBalances = 124,
		
		/// <summary>
		/// Search for Repurchase Letter Accounts
		/// </summary>
		opViewRepurchaseLetterAccounts = 125,
		
		/// <summary>
		/// Search for Repurchased Accounts
		/// </summary>
		opViewRepurchasedAccounts = 126,
		
		/// <summary>
		/// Search for Inactive Distributor Addons
		/// </summary>
		opViewInactiveDistributorAddons = 127,
		
		/// <summary>
		/// Search for Accounts Delinquent 0 to 30 Days
		/// </summary>
		opViewDelinquent0to30 = 128,
		
		/// <summary>
		/// Search for Accounts Delinquent 31 to 60 Days
		/// </summary>
		opViewDelinquent31to60 = 129,
		
		/// <summary>
		/// Search for Accounts Delinquent 61 to 90 Days
		/// </summary>
		opViewDelinquent61to90 = 130,
		
		/// <summary>
		/// Search for Accounts Delinquent Over 90 Days
		/// </summary>
		opViewDelinquentOver90 = 131,
		
		/// <summary>
		/// Search for All Accounts
		/// </summary>
		opDFViewAllAccounts = 132,
		
		/// <summary>
		/// Search for Zero Pay Accounts
		/// </summary>
		opDFViewZeroPayAccounts = 133,
		
		/// <summary>
		/// Search for Paid in Full Accounts
		/// </summary>
		opDFViewPaidInFullAccounts = 134,
		
		/// <summary>
		/// Search for Accounts With Balances
		/// </summary>
		opDFViewAccountsWithBalances = 135,
		
		/// <summary>
		/// Search for Accounts to be Purged
		/// </summary>
		opDFViewAccountsToBePurged = 136,
		
		/// <summary>
		/// Search for Consecutive Payment Accounts
		/// </summary>
		opDFViewConsecutivePaymentAccounts = 137,
		
		/// <summary>
		/// Search for Accounts that have been Purged
		/// </summary>
		opDFViewPurgedAccounts = 138,
		
		/// <summary>
		/// Search for Accounts that are Delinquent 0 to 30 Days
		/// </summary>
		opDFViewDelinquent0to30 = 139,
		
		/// <summary>
		/// Search for Accounts that are Delinquent 31 to 60 Days
		/// </summary>
		opDFViewDelinquent31to60 = 140,
		
		/// <summary>
		/// Search for Accounts that are Delinquent 61 to 90 Days
		/// </summary>
		opDFViewDelinquent61to90 = 141,
		
		/// <summary>
		/// Search for Accounts that are Delinquent Over 90 Days
		/// </summary>
		opDFViewDelinquentOver90 = 142,
		
		/// <summary>
		/// Search for All Accounts
		/// </summary>
		opLWViewAllAccounts = 143,
		
		/// <summary>
		/// Search for Zero Pay Accounts
		/// </summary>
		opLWViewZeroPayAccounts = 144,
		
		/// <summary>
		/// Search for Accounts on Layaway until Retail Price is Met
		/// </summary>
		opLWUntilRetailPriceIsMet = 145,
		
		/// <summary>
		/// Search for Accounts on Layaway until Merchandise Price is Met
		/// </summary>
		opLWUntilMerchandisePriceIsMet = 146,
		
		/// <summary>
		/// Search for Accounts that are 0 to 30 Days Delinquent
		/// </summary>
		opLWViewDelinquent0to30 = 147,
		
		/// <summary>
		/// Search for Accounts that are 31 to 60 Days Delinquent
		/// </summary>
		opLWViewDelinquent31to60 = 148,
		
		/// <summary>
		/// Search for Accounts that are 61 to 90 Days Delinquent
		/// </summary>
		opLWViewDelinquent61to90 = 149,
		
		/// <summary>
		/// Search for Accounts that are over 90 Days Delinquent
		/// </summary>
		opLWViewDelinquentOver90 = 150,
		
		/// <summary>
		/// Manage Layaway Workflow Notes
		/// </summary>
		opManageLayawayWorkflow = 151,
		
		/// <summary>
		/// Make Layaway Workflow Credit Card Payments
		/// </summary>
		opMakeLayawayWorkflowPayments = 152,
		
		/// <summary>
		/// View the Customer Birthday Report
		/// </summary>
		opViewCustomerBirthdayReport = 153,
		
		/// <summary>
		/// Mark Deposit as Pending Approval
		/// </summary>
		opMarkDepositPaid = 154,
		
		/// <summary>
		/// View the Deposit Paid Status
		/// </summary>
		opViewDepositPaid = 155,
		
		/// <summary>
		/// View the Colorado Acknowledgement
		/// </summary>
		opViewColoradoAcknowledgement = 156,
		
		/// <summary>
		/// View Distributor Performance Charts
		/// </summary>
		secDistributorPerformanceCharts = 157,
		
		/// <summary>
		/// View the Distributor Entrepreneur Application Performance Charts
		/// </summary>
		opViewDistributorIcebreakerCharts = 158,
		
		/// <summary>
		/// View the Distributor Sales Volume Performance Charts
		/// </summary>
		opViewDistributorVolumeCharts = 159,
		
		/// <summary>
		/// My Profile
		/// </summary>
		tabProfile = 160,
		
		/// <summary>
		/// Entrepreneur Access
		/// </summary>
		mnuManageUsers = 161,
		
		/// <summary>
		/// View My Profile
		/// </summary>
		mnuViewProfile = 162,
		
		/// <summary>
		/// View Distributor Profile
		/// </summary>
		mnuViewDistributorProfile = 163,
		
		/// <summary>
		/// View Entrepreneurs Profile
		/// </summary>
		mnuSalespersonProfile = 164,
		
		/// <summary>
		/// View the Distributor Details
		/// </summary>
		opViewDistributorDetails = 165,
		
		/// <summary>
		/// View the TD Meeting Fund Report
		/// </summary>
		opViewTDMeetingFundReport = 166,
		
		/// <summary>
		/// View the Distributor Financed Account Summary Report
		/// </summary>
		opViewDFAccountSummaryReport = 167,
		
		/// <summary>
		/// View the Distributor Financed Cash Receipts Report
		/// </summary>
		opViewDFCashReceipts = 168,
		
		/// <summary>
		/// View the Distributor Financed IVA Payments Report
		/// </summary>
		opViewDFIVAPaymentsReport = 169,
		
		/// <summary>
		/// View Cash Receipts on Repurchased Accounts
		/// </summary>
		opViewDFCashReceiptsOnRepos = 170,
		
		/// <summary>
		/// View Layaway Cash Receipt Reports
		/// </summary>
		opViewLayawayCashReceipts = 171,
		
		/// <summary>
		/// Territory Director Meeting Fund Payments
		/// </summary>
		mnuTDMeetingFundDetails = 172,
		
		/// <summary>
		/// Update the Entrepreneurs Profile Information
		/// </summary>
		opUpdateSalespersonProfile = 173,
		
		/// <summary>
		/// Edit Entrepreneur Application Until Processed
		/// </summary>
		opEditIcebreaker = 174,
		
		/// <summary>
		/// View the Universidad Online link
		/// </summary>
		opViewUniversityLink = 175,
		
		/// <summary>
		/// View the SAS Link
		/// </summary>
		opViewSASLink = 176,
		
		/// <summary>
		/// View the SAS Mexico Link
		/// </summary>
		opViewSASMXLink = 177,
		
		/// <summary>
		/// View Vimeo Training Link
		/// </summary>
		opVimeoTrainingLink = 178,
		
		/// <summary>
		/// View the 4 Appointments in 14 Days Link
		/// </summary>
		opView4in14Link = 179,
		
		/// <summary>
		/// Download Center
		/// </summary>
		mnuDownloadCenter = 180,
		
		/// <summary>
		/// View the RP Download Center
		/// </summary>
		opViewRPDownloadCenter = 181,
		
		/// <summary>
		/// View the 5-Ply Download Center
		/// </summary>
		opView5PlyDownloadCenter = 182,
		
		/// <summary>
		/// View the Innove Download Center
		/// </summary>
		opViewInnoveDownloadCenter = 183,
		
		/// <summary>
		/// Digital Image Downloads
		/// </summary>
		opViewDigitalImageDownloads = 184,
		
		/// <summary>
		/// View the Training Material Downloads
		/// </summary>
		opViewTrainingMaterials = 185,
		
		/// <summary>
		/// View the Sales Materials Downloads
		/// </summary>
		opViewSalesMaterials = 186,
		
		/// <summary>
		/// View Private Messages
		/// </summary>
		mnuPrivateMessages = 187,
		
		/// <summary>
		/// Sales Volume Search
		/// </summary>
		mnuSalesVolumeSearch = 188,
		
		/// <summary>
		/// Recruiter Volume
		/// </summary>
		mnuRecruiterVolume = 189,
		
		/// <summary>
		/// Royal Star Conventions
		/// </summary>
		opRoyalStarConventions = 190,
		
		/// <summary>
		/// The Grand Convention
		/// </summary>
		opViewGrandConvention = 191,
		
		/// <summary>
		/// VIP
		/// </summary>
		opViewVIP = 192,
		
		/// <summary>
		/// Approve Mobile Orders
		/// </summary>
		opApproveMobileOrders = 193,
		
		/// <summary>
		/// Telemarketer Access
		/// </summary>
		mnuTelemarketers = 194,
		
		/// <summary>
		/// Admin Users
		/// </summary>
		mnuChildUsers = 195,
		
		/// <summary>
		/// InCite
		/// </summary>
		tabInCite = 196,
		
		/// <summary>
		/// InCite Login
		/// </summary>
		mnuInciteLogin = 197,
		
		/// <summary>
		/// Entrepreneur Commissions
		/// </summary>
		mnuSalespersonCommissions = 198,
		
		/// <summary>
		/// Entrepreneur Search
		/// </summary>
		mnuSalespersonSearch = 199,
		
		/// <summary>
		/// View the Monthly Exchange Rates report
		/// </summary>
		opViewMonthlyExchangeRates = 200,
		
		/// <summary>
		/// Apply Payments
		/// </summary>
		opMakeMobilePayments = 201,
		
		/// <summary>
		/// View the Entrepreneur Agreement Status report
		/// </summary>
		opViewSalespersonAgreementStatuses = 202,
		
		/// <summary>
		/// Premier Network Leader Volume Summary
		/// </summary>
		opTerritoryDirectorVolumeSummary = 203,
		
		/// <summary>
		/// Master Territory Volume Summary
		/// </summary>
		opMasterTerritoryVolumeSummary = 204,
		
		/// <summary>
		/// Premier Network Leader Purchase Summary
		/// </summary>
		opTerritoryDirectorPurchaseSummary = 205,
		
		/// <summary>
		/// Returned e-Originals
		/// </summary>
		mnuReturnsEOriginals = 206,
		
		/// <summary>
		/// Master Network Volume Worlds Report
		/// </summary>
		opTDVolumeWorldReport = 207,
		
		/// <summary>
		/// Master Network Volume Summary MX 
		/// </summary>
		opTDVolumeMXSummary = 208,
		
		/// <summary>
		/// Master Network Report (split)
		/// </summary>
		opMasterNetworkSplit = 209,
		
		/// <summary>
		/// Master Network Purchase Summary American
		/// </summary>
		opTerritoryDirectorPurchaseSummaryAmerican = 210,
		
		/// <summary>
		/// View the Sales Form Link
		/// </summary>
		opviewsalesformlink = 211,
		
		/// <summary>
		/// Funds Received from the Distributors
		/// </summary>
		mnuCOTaxFactors = 213,
		
		/// <summary>
		/// Apply Customer Payments
		/// </summary>
		mnuCustomerPayments = 214,
		
		/// <summary>
		/// Tax Lookup
		/// </summary>
		mnuTaxLookup = 215,
		
		/// <summary>
		/// Junior Distributor Performance
		/// </summary>
		tabJuniorDistributor = 216,
		
		/// <summary>
		/// Junior Distributor Reports
		/// </summary>
		mnuJDSuccessReport = 217,
		
		/// <summary>
		/// Junior Distributor Success Analysis Report (Territory)
		/// </summary>
		opViewJDSuccessAnalysisForTerritory = 218,
		
		/// <summary>
		/// Funds Paid to TD/Sponsor
		/// </summary>
		mnuDistributorCOTaxFactor = 219,
		
		/// <summary>
		/// Junior Distributor Success Analysis Report (Sponsor)
		/// </summary>
		opViewJDSuccessAnalysisForSponsor = 220,
		
		/// <summary>
		/// Junior Distributor Success Analysis Report (Company)
		/// </summary>
		opViewJDSuccessAnalysisForCompany = 221,
		
		/// <summary>
		/// Payment Accounts
		/// </summary>
		mnuPaymentAccounts = 222,
		
		/// <summary>
		/// Account Activity Payments
		/// </summary>
		mnuAccountActivityPayments = 223	
	}
	#endregion
}


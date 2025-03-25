//-- ROOT HCE JAVASCRIPT OBJECTS --
if (typeof HCE == 'undefined') {
    var HCE = {};
}

if (typeof HCE.Resources == 'undefined') {
    HCE.Resources = {};
}

HCE.Resources.Error = 'Error';
HCE.Resources.OrderBy = 'Order By';
HCE.Resources.ThenBy = 'Then By';
HCE.Resources.NoRecordsFound = 'No Records Found';
HCE.Resources.NoRecordsFoundText = 'No matching [ENTITY] could be found.<br/><br/>Please try again or check back later.';
HCE.Resources.Page = 'Page';
HCE.Resources.Of = 'of';
HCE.Resources.RecordsPerPage = 'Records Per Page';
HCE.Resources.TotalRecords = 'Total Records';
HCE.Resources.From = 'From';
HCE.Resources.To = 'To';
HCE.Resources.Value = 'Value';
HCE.Resources.UnexpectedError = 'Unexpected Error';
HCE.Resources.UnexpectedErrorDesc = 'An unexpected error ocurred while processing your request.';
HCE.Resources.RequestProcessedSuccessfully = 'Your request was processed successfully!';
HCE.Resources.IsRequired = 'is required';

HCE.Resources.Alerts = {};
HCE.Resources.Alerts.CDS = 'You have [TOTAL] Consumer order(s) sitting in the Wait For Deposit Queue for more than 10 days, please review those orders';
HCE.Resources.Alerts.DDS = 'You have [TOTAL] Distributor Cash order(s) sitting in the Wait For Deposit Queue for more than 10 days, please review those orders';
HCE.Resources.Alerts.CWF = 'You have [TOTAL] contract(s) in the customer workflow with unread comments!';
HCE.Resources.Alerts.DWF = 'You have [TOTAL] cash order(s) in the distributor workflow with unread comments!';
HCE.Resources.Alerts.LWF = 'You have [TOTAL] contract(s) in the Layaway workflow with unread comments!';
HCE.Resources.Alerts.COL = 'You have [TOTAL] unread collector messages!';
HCE.Resources.Alerts.DFE = 'You have [TOTAL] distributor financed account(s) eligible for regular financing!';
HCE.Resources.Alerts.LWE = 'You have [TOTAL] Layaway account(s) eligible for regular financing!';
HCE.Resources.Alerts.PRV = 'You have [TOTAL] unread private messages!';
HCE.Resources.Alerts.ORD = 'You have [TOTAL] items in your shopping cart in client [CLIENT]!';
HCE.Resources.Alerts.APP = 'You have [TOTAL] order uploads pending your approval!';
HCE.Resources.Alerts.SPP = 'Your salesperson profile is incomplete! Please complete the missing profile information as soon as possible to prevent delays in processing your orders or payments.';
HCE.Resources.Alerts.ZTH = 'You have [TOTAL] customers that are 0 to 30 days delinquent!';
HCE.Resources.Alerts.RLT = 'You have [TOTAL] repurchase letter customers!';
-----------------------------------------------------------
HOW TO: ADD LINK TO DISTRIBUTOR INFORMATION POPUP TO A PAGE
-----------------------------------------------------------
1. Add the following link to the page:

@Html.Partial("~/Views/Shared/_DistributorInfo.cshtml", null, new ViewDataDictionary { { "SOURCEGRIDID", "HCDYNAMICGRID_Distributors" } })

2. Add the following script to the page:

$("table.dynamic-search-table").on("HC:ROWCLICKED", function (event, rowInfo) {
    var grid = $("#HCDYNAMICGRID_Distributors");
    var scope = angular.element(grid).scope();
    HCE.ShowDistributorInfo(grid, rowInfo.Keys.DistributorNo);
});

//-- ROOT HCE JAVASCRIPT OBJECTS --
if (typeof HCE === 'undefined') {
    var HCE = {};
}

if (typeof HCE.DynamicDisplay === 'undefined') {
    HCE.DynamicDisplay = {};
}

//-----------------------------------------------------------------------------------------------------
//-- FUNCTION:	InitializeDynamicSearch
//-- PURPOSE:	INITIALIZE THE DYNAMIC SEARCH EVENT HANDLERS
//-----------------------------------------------------------------------------------------------------
HCE.DynamicDisplay.InitializeDynamicSearch = function () {
    //-- CREATE EVENT HANDLERS FOR THE GRID FILTER FIELDS --
    HCE.DynamicDisplay.InitializeFilterEvents();

    //-- INITIALIZE THE SORTING -- 
    HCE.DynamicDisplay.InitializeSort();

    $("div[data-type=HCDYNAMICGRID]")
	.on("click", "div.HC-pager li", function () {

	    //-- RELOAD THE GRID WHEN THE PAGE NUMBER HAS BEEN CHANGED --
	    HCE.DynamicDisplay.RefreshGridData(this, $(this).data("page"));

	})
	.on("change", "div.HC-pager select.records-per-page", function () {

	    //-- CHANGE THE RECORDS PER PAGE AND REFRESH THE GRID --
	    var containers = HCE.DynamicDisplay.GetSearchContainers(this);
	    var recsperpage = HCE.DynamicDisplay.GetRecsPerPage($(this).val());
	    containers.SearchGrid.data("pagesize", recsperpage);

	    HCE.DynamicDisplay.RefreshGridData(this, 1);

	})
	.on("click", "button[data-type=HCCLEARFILTERS]", function () {

	    //-- CLEAR THE FORMAT AND REFRESH THE GRID --
	    HCE.DynamicDisplay.ClearForm($(this));

	});

    $("form.advanced-search-form").on("submit", function (event) {
        $(this).closest("div.dynamic-search-modal").modal("hide");
        HCE.DynamicDisplay.ApplyAdvancedSort(this);
        HCE.DynamicDisplay.RefreshGridData(this);
    });

    $("[data-type=HCPRINTDYNAMIC]").on("click", function (event) {
        window.print();
    });

    $("[data-type=HCREFRESHGRID]").on("click", function (event) {
        var grid = $(this).closest("div[data-type=HCDYNAMICGRID]");
        HCE.DynamicDisplay.RefreshGridData(grid, grid.data('currentpagenumber'));
    });

    $('.advanced-search-form')
	.on('change', '[data-type=hcadvanced]:not([data-field-type=date])', function (event) {

	    //-- KEEP THE ADVANCED SEARCH SELECTION IN SYNC WITH THE FREE TEXT SEARCH BOX --
	    HCE.DynamicDisplay.SyncAdvancedSearchField($(this));

	})
	.on('change', '[data-type=hcadvanced][data-field-type=date]', function (event) {

	    //-- TOGGLE THE ADVANCED SEARCH BOX BASED ON THE SELECTION --
	    HCE.DynamicDisplay.ToggleAdvancedSearchBox($(this));

	})
	.on('click', 'button[data-type=hcadvtoggle]', function (event) {

	    //-- TOGGLE THE ADVANCED SEARCH BOX WHEN A TOGGLE BUTTON IS CLICKED --
	    HCE.DynamicDisplay.ToggleAdvancedSearchBox($(this));

	})
	.on('change', 'select.advanced-search-operator', function (event) {

	    //-- FORMAT THE ADVANCED SEARCH FIELDS BASED ON THE SELECTED OPERATOR --
	    HCE.DynamicDisplay.FormatAdvancedSearchFields($(this));
	    HCE.DynamicDisplay.SyncAdvancedSearchField($(this));

	})

    //-- CUSTOM FORM SUBMISSION HANDLER --
    $('form[data-type=HCCUSTOMFORM]').submit(function (event) {
        event.preventDefault();

        if ($(this).valid()) {
            var formObject;
            var angularScope = angular.element($(this)).scope();
            var url;
            var callbackFunction;
            var callbackSource;
            var authType;
            var validateAntiforgery = false;
            var etag;
            var formmethod;

            authType = $(this).data("auth-type");
            etag = $(this).data("etag");
            formmethod = $(this).attr("method");

            //-- BUILD THE FORM OBJECT TO BE SUBMITTED --
            formObject = HCE.DynamicDisplay.ParseFormValues($(this));

            url = $(this).attr("action");
            callbackFunction = $(this).data("callback");
            callbackSource = $(this).data("callback-source");

            angularScope.PostAPIDataWithRefresh(url, false, formObject, callbackSource, callbackFunction, authType, formmethod, etag);
        }
    });

    //-- CURRENCY SELECTION HANDLER --
    $("[data-type=CURRENCYSEL]").on("click", function (event) {
        var containers = HCE.DynamicDisplay.GetSearchContainers($(this));
        var containerID = containers.Container.attr("id");

        containers.Container.data("displaycurrency", $(this).data("currency"));

        //-- UPDATE THE SCOPE CURRENCY VALUE --
        try {

            //-- GET THE ANGULAR SCOPE TO BE UPDATED --
            var angularScope = angular.element('#' + containerID).scope();
            angularScope.DisplayCurrency = $(this).data("currency");
            angularScope.$apply();

        } catch (e) {
            HCE.DynamicDisplay.RaiseErrorEvent(containers.Container, e);
            HCE.Error.HandleErrorMessage(HCE.Error.GetErrorObject(500, '', e));
        }

        //-- UPDATE THE BUTTON HTML --
        var button = $(this).closest('div.btn-group').find('button:first');
        var hasCaret = /<span class=\"caret\">/.test;

        button.html($(this).data("currency-desc") + " &nbsp;<span class=\"fa " + $(this).data("currency-faclass") + " fa-lg\"></span>" + (hasCaret ? ' &nbsp;<span class=\"caret\"></span>' : ""));

        //-- UPDATE THE PAGE CURRENCY --
        HCE.DynamicDisplay.UpdatePageCurrency(button);
    });
}

//-----------------------------------------------
//-- GET THE CUSTOM FORM NAME VALUE COLLECTION --
//-----------------------------------------------
HCE.DynamicDisplay.ParseFormValues = function (form) {
    var formObject = {};

    //-- BUILD THE FORM OBJECT TO BE SUBMITTED --
    form.find("[data-type=HCFORMVALUE]").each(function () {
        var val = $(this).val();
        var type = $(this).attr('type');
        var includeField = true;
        var suppressIfBlank = ((typeof $(this).data('suppressifblank') !== 'undefined') && ($(this).data('suppressifblank') === true));

        switch (type) {
            case 'checkbox':
                if ($(this).is(":checked"))
                    val = 'true';
                else
                    val = 'false';
                break;
                  
            case 'number':
                if ($(this).data("model-type") == 'string')
                {
                    if (val === '') {
                        if (suppressIfBlank)
                            includeField = false;
                    }
                }
                else if (val === '') {
                    val = 0.00;
                    if (suppressIfBlank)
                        includeField = false;
                }
                else
                    val = parseFloat(val);
                break;

            default:
                if (val === '') {
                    if (suppressIfBlank)
                        includeField = false;
                }
        }

        if (includeField)
            formObject[$(this).data("model")] = val;
    });

    return (formObject);
};

//---------------------------------------------------------
//-- SET A DEFAULT FILTER TO BE USED FOR THE SEARCH GRID --
//---------------------------------------------------------
HCE.DynamicDisplay.SetDefaultFilter = function (fieldName, fieldValue, source) {
    var containers = HCE.DynamicDisplay.GetSearchContainers(source);
    var advSearchField;

    if ((typeof containers !== 'undefined') && (containers != null) && (containers.Container.length > 0)) {
        //-- SET THE DEFAULT SEARCH VALUE --
        advSearchField = containers.Container.find('input[data-type=hcadvanced][data-field=' + fieldName + ']').first();

        if ((typeof advSearchField !== 'undefined') && (advSearchField !== null) && (advSearchField.length > 0)) {
            advSearchField.val(fieldValue);

            if (advSearchField.data('field-type') !== 'date')
                HCE.DynamicDisplay.SyncAdvancedSearchField(advSearchField);
            else
                HCE.DynamicDisplay.ToggleAdvancedSearchBox(advSearchField);
        }
    }
    else {
        $('.advanced-search-form').each(function () {
            //-- SET THE DEFAULT SEARCH VALUE --
            advSearchField = $(this).find('input[data-type=hcadvanced][data-field=' + fieldName + ']').first();
            if ((typeof advSearchField !== 'undefined') && (advSearchField !== null) && (advSearchField.length > 0)) {
                advSearchField.val(fieldValue);

                if (advSearchField.data('field-type') !== 'date')
                    HCE.DynamicDisplay.SyncAdvancedSearchField(advSearchField);
                else
                    HCE.DynamicDisplay.ToggleAdvancedSearchBox(advSearchField);
            }
        });
    }
};

//------------------------------------------------------------------------------------
// FUNCTION:	ClearForm
// PURPOSE:		CLEAR THE SEARCH AND SORT FIELDS ON THE FORM AND RE-GENERATE
//------------------------------------------------------------------------------------
HCE.DynamicDisplay.ClearForm = function (source) {
    var containers = HCE.DynamicDisplay.GetSearchContainers(source);

    //-- CHECK FOR THE AUTO INIT FLAG --
    var autoInit = containers.Container.data("auto-init").toLowerCase() == 'false' ? false : true;

    //-- CLEAR THE REGULAR SEARCH FIELDS --
    containers.SearchGrid.find("[data-type=hcfilter]").val("");

    //-- CLEAR THE ADVANCED SEARCH FIELDS --
    containers.SearchModal.find('[data-type=hcadvanced]').val("");

    //-- RESET THE OPERATORS --
    containers.SearchModal.find('select.advanced-search-operator').each(function () {
        $(this)[0].selectedIndex = 0;
        $(this).trigger("change");
    });

    //-- DEFAULT ALL THE SEARCH ORDERS TO ASCENDING --
    containers.SearchModal.find("button[data-type=hcadvsort] span:first").removeClass("fa-sort-alpha-asc fa-sort-alpha-desc").addClass("fa-sort-alpha-asc");

    //-- CLEAR THE ADVANCED SORTING SETUP AND APPLY THE DEFAULT SORT --
    HCE.DynamicDisplay.ClearSort(containers.SearchModal.find("div[data-type=hcadvsortrow][data-sortindex=1]"));
    containers.SearchGrid.data("sortorder", HCE.DynamicDisplay.GetDefaultSortOrder(containers.SearchGrid));
    HCE.DynamicDisplay.ApplySort(containers.SearchGrid, true);

    //-- REFRESH THE GRID --
    if (autoInit === true)
        HCE.DynamicDisplay.RefreshGridData($(source), 1);
    else {
        //-- GET THE ANGULAR SCOPE TO BE UPDATED --
        var angularScope = angular.element(containers.Container).scope();
        angularScope.entities = {};
        HCE.DynamicDisplay.BuildDynamicPager(containers.Container, '{"PageSize":0,"Page":0,"TotalRecords":0,"TotalPages":0,"PrevLink":"","CurLink":"","NextLink":""}', true);
        angularScope.showdetails = false;
        angularScope.$apply();
    }
}

//------------------------------------------------------------------------------------
// FUNCTION:	ApplyAdvancedSort
// PURPOSE:		BUILD THE ADVANCED SORT STRING
//------------------------------------------------------------------------------------
HCE.DynamicDisplay.ApplyAdvancedSort = function (searchForm) {
    var sortString = "";

    $(searchForm).find("div[data-type=hcadvsortrow]").each(function () {
        var selectBox = $(this).find("select").first();
        if ((selectBox.length > 0) && (selectBox.val() != "") && (selectBox.val() != "undefined")) {
            var sortButton = $(this).find("button[data-type=hcadvsort]");
            var sortDir = "asc";

            if (sortButton.length > 0) {
                var sortButtonSpan = sortButton.find("span").first();
                if (sortButtonSpan.length > 0)
                    sortDir = (sortButtonSpan.hasClass("fa-sort-alpha-asc") ? "asc" : "desc");
            }

            if (selectBox.val() != "") {
                sortString = sortString + (sortString != "" ? "," : "") + selectBox.val() + (sortDir == "desc" ? " desc" : "");
            }
        }
    });

    //-- UPDATE THE SORT INFO ON THE MAIN TABLE --
    var containers = HCE.DynamicDisplay.GetSearchContainers(searchForm);
    if (containers.SearchGrid.length > 0)
        containers.SearchGrid.data("sortorder", sortString);
}

//------------------------------------------------------------------------
// FUNCTION:	InitializeFilterEvents
// PURPOSE:		INITIALIZE THE FILTER EVENTS
//------------------------------------------------------------------------
HCE.DynamicDisplay.InitializeFilterEvents = function () {
    $('[data-type=hcfilter]')
		.on('keypress', function () {
		    if (event.keyCode == 13) {
		        //-- FORCE THE CHANGE EVENT TO BE FIRED WHEN THE ENTER KEY IS PRESSED --
		        $(this).blur();
		        return false;
		    }
		})
		.on('change', function () {
		    HCE.DynamicDisplay.CopyFilterToAdvanced($(this));
		    HCE.DynamicDisplay.RefreshGridData($(this), 1);
		    return false;
		});
}

//-------------------------------------------------------------
// FUNCTION:	hc_InitializeAngular
// PURPOSE:		INITIALIZE THE ANGULAR MODULE AND CONTROLLERS
//-------------------------------------------------------------
HCE.DynamicDisplay.InitializeAngular = function () {
    HCE.DynamicDisplay.EmbedDetailDivsInContainer();

    if ($('div [ng-app=HCE]').length > 0) {
        var angularAPP = angular.module("HCE", ['HCE.CustomFilters'])
				            .config(['$httpProvider', function ($httpProvider) {
				                delete $httpProvider.defaults.headers.common["X-Requested-With"];
				                $httpProvider.defaults.withCredentials = true;
				                $httpProvider.defaults.headers.common["Accept-Language"] = HCE.Language;
				            }]);

        if (typeof HCE.Whitelist != 'undefined' && HCE.Whitelist != null && HCE.Whitelist != undefined) {
            angularAPP.config(['$sceDelegateProvider', function ($sceDelegateProvider) {
                $sceDelegateProvider.resourceUrlWhitelist(HCE.Whitelist);
            }]);
        }

        $('div [data-type=HCDYNAMICGRID]').each(function () {
            var controllerName = $(this).attr('ng-controller');
            var formName = $(this).find("div[data-type=HCDYNAMICDET] form:first").attr("name");
            var entityName = $(this).data('entity-name');
            var entityPath = $(this).data('entity-path');
            var rooturl = $(this).data('root-url');
            var container = $(this);
            var searchTable = $(this).find('table.dynamic-search-table, div.dynamic-search-table');
            var displayCurrency = $(this).data("displaycurrency");
            var authType = $(this).data("auth-type");
            var autoInit = ($(this).data("auto-init").toLowerCase() == 'false' ? false : true);

            if (authType === null || authType === 'undefined' || typeof authType === 'undefined')
                authType = 'N';

            angularAPP.controller(controllerName, function ($scope, $filter, $http, $sce) {
                $scope.ControllerName = controllerName;
                $scope.DetailForm = formName;
                $scope.DisplayCurrency = displayCurrency;
                $scope.AuthType = authType;
                $scope.filterRef = $filter;

                // function to submit the form after all validation has occurred            
                $scope.submitForm = function () {
                    $scope.submitted = true;
                };

                //----------------------------------------------------------------------------------------
                // FUNCTION:    UpdateGrid
                // PURPOSE:     UPDATE THE entities SCOPE VARIABLE WHICH WILL AUTOMATICALLY UPDATE ANY LINKED
                //              DYNAMIC GRID ON THE CURRENT FORM
                //----------------------------------------------------------------------------------------
                $scope.UpdateGrid = function (url, retry, entityname) {
                    if (typeof entityname === 'undefined' || entityname === null || entityname === '')
                        entityname = 'entities';

                    $scope.GetAPIDataWithRefresh(url, retry, entityname);
                };

                //-----------------------------------------------------------------------------------------
                // FUNCTION:    GetDetails
                // PURPOSE:     UPDATE THE entity SCOPE VARIABLE WHICH WILL AUTOMATICALLY UPDATE ANY LINKED
                //              DETAIL FORM
                //-----------------------------------------------------------------------------------------
                $scope.GetDetails = function (url, retry) {
                    $scope.GetAPIDataWithRefresh(url, retry, 'entity');
                }

                //----------------------------------------------------------------------------------------
                // FUNCTION:    GetAPIDataWithRefresh
                // PURPOSE:     ATTEMPT TO DOWNLOAD THE JSON FROM THE SPECIFIED WEB API AND BIND IT TO THE
                //              SPECIFIED SCOPE VARIABLE NAME
                //----------------------------------------------------------------------------------------
                $scope.GetAPIDataWithRefresh = function (url, retry, scopeVariableToUpdate, eventIndicator, suppressErrorMessages) {
                    $('#MODALSPINNER').spin('modal');
                    $('body').addClass('modal-processing');

                    var options = {};

                    if ($scope.AuthType === 'W')
                        options.withCredential = true;
                    else if ($scope.AuthType === 'B') {
                        options.headers = { 'Authorization': 'Bearer ' + HCE.Token };
                    }

                    $http.get(url, options).
	    			then(function (response) {

	    			    //-- HIDE THE SPINNER AND UPDATE THE OBJECT COLLECTION --
	    			    $('#MODALSPINNER').spin();
	    			    $('body').removeClass('modal-processing');

	    			    switch (scopeVariableToUpdate) {
	    			        case "entities":

	    			            //-- UPDATE THE SCOPE VARIABLE --
	    			            $scope.entities = response.data.value;

	    			            //-- BUIlD THE DYNAMIC PAGER FOR ARRAY OBJECTS --
	    			            var parent = $("div[ng-controller=" + $scope.ControllerName + "]");
	    			            HCE.DynamicDisplay.BuildDynamicPager(parent, response.headers("x-pagination"));
	    			            $scope.showdetails = false;

	    			            container.trigger("HC:ENTITYUPDATED", "entities");
	    			            break;

	    			        case "entity":

	    			            //-- UPDATE THE SCOPE VARIABLE --
	    			            $scope.entity = response.data;

	    			            //-- SHOW THE DETAILS --
	    			            $scope.showdetails = true;

	    			            container.trigger("HC:ENTITYUPDATED", "entity");
	    			            break;

	    			        default:

	    			            if (($(response.data.value).length > 0) && ($.isArray(response.data.value))) {
	    			                if (scopeVariableToUpdate.substr(0, 7) == "entity.")
	    			                    $scope.entity[scopeVariableToUpdate.substr(7)] = response.data.value;
	    			                else
	    			                    $scope[scopeVariableToUpdate] = response.data.value;
	    			            }
	    			            else {
	    			                if (scopeVariableToUpdate.substr(0, 7) == "entity.")
	    			                    $scope.entity[scopeVariableToUpdate.substr(7)] = response.data;
	    			                else
	    			                    $scope[scopeVariableToUpdate] = response.data;
	    			            }

	    			            if ((typeof eventIndicator === 'undefined') || (eventIndicator == undefined) || (eventIndicator === null) || (eventIndicator === ''))
	    			                eventIndicator = scopeVariableToUpdate;

	    			            container.trigger("HC:ENTITYUPDATED", eventIndicator);
	    			    }

	    			}, function (response) {

	    			    //-- HANDLE SERVICE NOT AVAILABLE EXCEPTIONS AND ADD DETAILS --
	    			    if ((response != null) && (typeof response != 'undefined') && (response.status != null) && (typeof response.status != 'undefined') && (response.status === -1)) {
	    			        response.status = 503;
	    			        if ((typeof response.data === 'undefined') || (response.data === null))
	    			            response.data = {};
	    			        response.data.MessageDetail = 'Failed to connect to ' + url;
	    			    }

	    			    if (retry) {

	    			        //-- ATTEMPTED TO GET A NEW ACCESS TOKEN AND THIS STILL DIDN'T WORK - THIS IS AN ACTUAL ERROR --
	    			        HCE.DynamicDisplay.RaiseErrorEvent($scope.DetailForm, response);
	    			        HCE.Error.HandleErrorMessage(response);
	    			    }
	    			    else {
	    			        var statusCode = 400;

	    			        if ((response != null) && (typeof (response) != 'undefined') && (response.status != null) && (typeof (response.status) != 'undefined'))
                                statusCode = response.status;
                            

	    			        switch (statusCode) {

	    			            case 401:

	    			                //-- TRY TO REFRESH THE ACCESS TOKEN --
	    			                $.ajax({
	    			                    url: HCE.TokenRefreshURL,
	    			                    data: { "__RequestVerificationToken": HCE.AntiForgery },
	    			                    type: 'post',
	    			                    traditional: true,
	    			                    success: function (data) {
	    			                        var retVal = $.parseJSON(data);
	    			                        status = retVal.Status;

	    			                        switch (status) {
	    			                            case "TokenWasRefreshed":
	    			                                HCE.Token = retVal.NewAccessToken;
	    			                                $scope.GetAPIDataWithRefresh(url, true, scopeVariableToUpdate);
	    			                                break;

	    			                            default:
	    			                                HCE.DynamicDisplay.RaiseErrorEvent($scope.DetailForm, response);
	    			                                HCE.Error.HandleErrorMessage(response);
	    			                        }
	    			                    },
	    			                    error: function (data) {
	    			                        HCE.DynamicDisplay.RaiseErrorEvent($scope.DetailForm, response);
	    			                        HCE.Error.HandleErrorMessage(response);
	    			                    }
	    			                });
	    			                break;

                                case 503:

                                    //-- RETRY THE REQUEST (POSSIBLE SPDY ERROR) --
                                    console.log("503 ERROR - RETRYING REQUEST...");

                                    setTimeout(function () {
                                        $scope.GetAPIDataWithRefresh(url, true, scopeVariableToUpdate);
                                    }, 500);                                                                        
                                    break;

                                default:
                                    if (suppressErrorMessages == null || suppressErrorMessages == false) {
                                        //-- CLOSE THE SPINNER AND SHOW THE ERROR DIALOG --
                                        HCE.DynamicDisplay.RaiseErrorEvent($scope.DetailForm, response);
                                        HCE.Error.HandleErrorMessage(response);
                                    }
                                    else {
                                        //-- HIDE THE SPINNER  --
                                        $('#MODALSPINNER').spin();
                                        $('body').removeClass('modal-processing');
                                    }
	    			        }
	    			    }
	    			});
                }

                //----------------------------------------------------------------------------------------
                // FUNCTION:    PostAPIDataWithRefresh
                // PURPOSE:     ATTEMPT TO POST THE JSON FROM THE SPECIFIED WEB API AND BIND IT TO THE
                //              SPECIFIED SCOPE VARIABLE NAME
                //----------------------------------------------------------------------------------------
                $scope.PostAPIDataWithRefresh = function (url, retry, entity, source, callbackFunction, authType, method, etag, contentType) {
                    $('#MODALSPINNER').spin('modal');
                    $('body').addClass('modal-processing');
                    var isFormData = Object.prototype.toString.call(entity) === "[object FormData]";

                    if ((typeof (method) === 'undefined') || (method === undefined) || (method === null))
                        method = 'POST';

                    var sendAntiForgery = false;
                    var options = {};
                    options.method = method;
                    options.url = url;
                    options.data = entity;

                    if ((typeof $scope.antiForgeryToken != 'undefined') && ($scope.antiForgeryToken != ''))
                        sendAntiForgery = true;

                    if (authType === null || authType === undefined || typeof authType === 'undefined')
                        authType = $scope.AuthType;

                    if (authType === 'W') {
                        options.withCredential = true;
                        if (sendAntiForgery == true)
                            options.headers = { 'RequestVerificationToken': $scope.antiForgeryToken, 'X-Requested-With': 'XMLHttpRequest' };
                    }
                    else if (authType === 'B') {
                        if (sendAntiForgery == true)
                            options.headers = { 'Authorization': 'Bearer ' + HCE.Token };
                        else
                            options.headers = { 'Authorization': 'Bearer ' + HCE.Token, 'RequestVerificationToken': $scope.antiForgeryToken, 'X-Requested-With': 'XMLHttpRequest' };
                    }

                    //-- ADD ANTI FOREGERY TOKEN --
                    if (sendAntiForgery == true) {
                        if (typeof options.headers === 'undefined') {
                            options.headers = {};
                        }
                        options.headers.RequestVerificationToken = $scope.antiForgeryToken;
                        options.headers['X-Requested-With'] = 'XMLHttpRequest';
                    }

                    //-- ADD THE ETAG VERIFICATION HEADER --
                    if ((etag != null) && (etag != undefined) && (typeof etag != 'undefined')) {
                        if (typeof options.headers === 'undefined') {
                            options.headers = {};
                        }
                        options.headers['If-Match'] = etag;
                    }

                    //-- ADD THE MULTI-PART FORM DATA TAGS --
                    if (isFormData === true) {
                        options.transformRequest = angular.identity;
                        options.headers["Content-Type"] = undefined;
                    }
                    else if ((typeof contentType !== 'undefined') && (contentType != undefined) && (contentType != null)) {
                        options.headers["Content-Type"] = contentType;

                        if (contentType === 'application/x-www-form-urlencoded')
                        {
                            options.transformRequest = function (obj)
                            {
                                var str = [];
                                for (var p in obj)
                                    str.push(encodeURIComponent(p) + "=" + encodeURIComponent(obj[p]));
                                return str.join("&");
                            }
                        }
                    }

                    $http(options).
	    			then(function (response) {

	    			    //-- HIDE THE SPINNER AND UPDATE THE OBJECT COLLECTION --
	    			    $('#MODALSPINNER').spin();
	    			    $('body').removeClass('modal-processing');

	    			    if (typeof callbackFunction !== 'undefined') {
	    			        if (typeof callbackFunction === 'string')
	    			            HCE.DynamicDisplay.ExecuteFunctionByName(callbackFunction, window, source, response);
	    			        else
	    			            callbackFunction(source, response);
	    			    }

	    			}, function (response) {

	    			    //-- HANDLE SERVICE NOT AVAILABLE EXCEPTIONS AND ADD DETAILS --
	    			    if ((response != null) && (typeof response != 'undefined') && (response.status != null) && (typeof response.status != 'undefined') && (response.status === -1)) {
	    			        response.status = 503;
	    			        if ((typeof response.data === 'undefined') || (response.data === null))
	    			            response.data = {};
	    			        response.data.MessageDetail = 'Failed to connect to ' + url;
	    			    }

	    			    if (retry) {

	    			        //-- ATTEMPTED TO GET A NEW ACCESS TOKEN AND THIS STILL DIDN'T WORK - THIS IS AN ACTUAL ERROR --
	    			        HCE.DynamicDisplay.RaiseErrorEvent($scope.DetailForm, response);
	    			        HCE.Error.HandleErrorMessage(response);
	    			    }
	    			    else {
	    			        var statusCode = 400;

	    			        if ((response != null) && (typeof (response) != 'undefined') && (response.status != null) && (typeof (response.status) != 'undefined'))
	    			            statusCode = response.status;

	    			        switch (statusCode) {

	    			            case 401:

	    			                //-- TRY TO REFRESH THE ACCESS TOKEN --
	    			                $.ajax({
	    			                    url: HCE.TokenRefreshURL,
	    			                    data: { "__RequestVerificationToken": HCE.AntiForgery },
	    			                    type: 'post',
	    			                    traditional: true,
	    			                    success: function (data) {
	    			                        var retVal = $.parseJSON(data);
	    			                        status = retVal.Status;

	    			                        switch (status) {
	    			                            case "TokenWasRefreshed":
	    			                                HCE.Token = retVal.NewAccessToken;
	    			                                $scope.PostAPIDataWithRefresh(url, true, entity, null, null, authType, method, etag);
	    			                                break;

	    			                            default:
	    			                                HCE.DynamicDisplay.RaiseErrorEvent($scope.DetailForm, response);
	    			                                HCE.Error.HandleErrorMessage(response);
	    			                        }
	    			                    },
	    			                    error: function (data) {
	    			                        HCE.DynamicDisplay.RaiseErrorEvent($scope.DetailForm, response);
	    			                        HCE.Error.HandleErrorMessage(response);
	    			                    }
	    			                });
	    			                break;

	    			            default:

	    			                //-- CLOSE THE SPINNER AND SHOW THE ERROR DIALOG --
	    			                HCE.DynamicDisplay.RaiseErrorEvent($scope.DetailForm, response);
	    			                HCE.Error.HandleErrorMessage(response);
	    			        }
	    			    }
	    			});
                }

                $(searchTable).data("sortorder", HCE.DynamicDisplay.GetDefaultSortOrder(searchTable));

                if (autoInit === true)
                    $scope.UpdateGrid(HCE.DynamicDisplay.ProcessAdvancedSearch(searchTable, 1), false);
            });
        });
    }
}

//-----------------------------------------------------------------------------------------------
// FUNCTION:		AppendPageSize
// PURPOSE:			APPEND THE PAGE SIZE PARAMETER TO THE SPECIFIED QUERY STRING
//-----------------------------------------------------------------------------------------------
HCE.DynamicDisplay.AppendPageSize = function (queryString) {
    var recsperage = HCE.DynamicDisplay.GetRecsPerPage();
    return (queryString == "" ? "?" : "&") + "$pagesize=" + recsperage;
}

//-----------------------------------------------------------------------------------------------
// FUNCTION:		InitializeSort
// PURPOSE:			INITIALIZE THE SORTING INFORMATION
//-----------------------------------------------------------------------------------------------
HCE.DynamicDisplay.InitializeSort = function () {
    //-- INITIALIZE THE ADVANCED SORT BOXES --
    HCE.DynamicDisplay.InitializeSortBoxes();

    //-- ADD THE ADVANCED SORT EVENT HANDLERS --
    $("div[data-type=hcadvsortbox]")
	.on("change", "select", function (event) {
	    HCE.DynamicDisplay.CascadeSortOptions(this);
	})
	.on("click", "button[data-type=hcadvclose]", function (event) {

	    //-- HANDLE THE CLEAR BUTTON CLICKS --
	    HCE.DynamicDisplay.ClearSort(this);

	})
	.on("click", "button[data-type=hcadvsort]", function (event) {

	    //-- TOGGLE THE ASCENDING/DESCENDING ADVANCED SORT BUTTON --
	    HCE.DynamicDisplay.ToggleDynamicSortButton(this);

	});

    //-- ADD THE SORT COLUMN HEADER EVENT HANDLERS --
    $("table.dynamic-search-table, div.dynamic-search-table").each(function () {
        var defaultSort = "";
        var parentTable = $(this);

        $(this).data("sortorder", HCE.DynamicDisplay.GetDefaultSortOrder(parentTable));

        parentTable.find("th.sortable").each(function () {
            $(this).html("<div>" + $(this).html() + "<div></div></div>");
        });

        HCE.DynamicDisplay.ApplySort(parentTable, true);
    })
	.on("click", "th.sortable", function (event) {

	    //-- APPLY THE HEADER SORT --
	    HCE.DynamicDisplay.HandleHeaderSort(this);

	})
    .on("click", ".actionbutton", function (event) {

        //-- DON'T RAISE THE TD CLICK EVENT --
        event.stopImmediatePropagation();
        if (!$(this).hasClass("disabled"))
            HCE.DynamicDisplay.RaiseActionClickEvent($(this));
        })
    .on("click", "a", function (event) {

        //-- ALLOW LINKS INSIDE THE GRID TO BE CLICKED --
        event.stopImmediatePropagation();
    })
	.on("click", "td", function (event) {

	    //-- RAISE THE ROW CLICK EVENT --
	    HCE.DynamicDisplay.RaiseRowClickedEvent(this);
	});
}

//----------------------------------------------------------------------------------------------
// FUNCTION:	RaiseActionClickEvent
// PURPOSE:		RAISE THE SPECIFIED ACTION CLICK EVENT
//----------------------------------------------------------------------------------------------
HCE.DynamicDisplay.RaiseActionClickEvent = function (item) {
    var retVal = {};
    var containers = HCE.DynamicDisplay.GetSearchContainers(item);

    retVal.Action = item.data("action");
    retVal.ActionID = item.data("actionid");
    retVal.ActionName = item.data("actionname");

    $.each(item.data(), function (i, v) {
        if (i !== 'action' && i !== 'actionid' && i !== 'actionname')
            retVal[i] = v;
    });

    containers.SearchGrid.trigger("HC:ACTIONCLICKED", retVal);
}

//----------------------------------------------------------------------------------------------
// FUNCTION:	RaiseRowClickEvent
// PURPOSE:		RAISE THE SPECIFIED ROW CLICK EVENT
//----------------------------------------------------------------------------------------------
HCE.DynamicDisplay.RaiseRowClickedEvent = function (td) {
    var retVal = {};
    var containers = HCE.DynamicDisplay.GetSearchContainers(td);
    var tr = $(td).closest("tr");
    var i = 0;
    var keyInfo;

    if ((containers.Container.length > 0) && (tr.length > 0)) {
        retVal.ContainerID = containers.Container.attr("id");
        var keys = $(tr).data("keys");
        if ((typeof keys != "undefined") && (keys != null)) {
            retVal.Keys = keys;
        }
    }

    containers.SearchGrid.trigger("HC:ROWCLICKED", retVal);
}

//----------------------------------------------------------------------------------------------
// FUNCTION:    HCE.DynamicDisplay.RaiseErrorEvent
// PURPOSE:     RAISE AN ERROR EVENT TO THE CALLING GRID
//----------------------------------------------------------------------------------------------
HCE.DynamicDisplay.RaiseErrorEvent = function (source,errorInfo) {
    var containers = HCE.DynamicDisplay.GetSearchContainers(source);
    if (containers !== null)
        containers.SearchGrid.trigger("HC:ERROR", errorInfo);
}

//----------------------------------------------------------------------------------------------
// FUNCTION:	HandleHeaderSort
// PURPOSE:		SORT THE RESULTS WHEN A SORTABLE HEADER COLUMN IS CLICKED
//----------------------------------------------------------------------------------------------
HCE.DynamicDisplay.HandleHeaderSort = function (item) {
    var containers = HCE.DynamicDisplay.GetSearchContainers(item);
    var sortdir = "asc";
    var sortorder = "";
    var fieldname = "";
    var advSortRow;
    var advSortSelect;
    var advSortDir;

    if (containers.SearchGrid.data().hasOwnProperty("sortorder"))
        sortorder = containers.SearchGrid.data("sortorder").toUpperCase();

    if ($(item).data().hasOwnProperty("field"))
        fieldname = $(item).data("field");

    if ((sortorder == (fieldname.toUpperCase() + " ASC")) || (sortorder == fieldname.toUpperCase())) {
        sortdir = "desc";
        sortorder = fieldname + " desc";
    }
    else
        sortorder = fieldname;

    //-- UPDATE THE ADVANCED SORT BOX SO IT MATCHES THE CURRENT SORT --
    if (containers.SortBox.length > 0) {
        advSortRow = containers.SortBox.find("div[data-type=hcadvsortrow][data-sortindex=1]");
        if (advSortRow.length > 0) {
            HCE.DynamicDisplay.ClearSort(advSortRow);
            advSortSelect = advSortRow.find("select").first();
            advSortDir = advSortRow.find("button[data-type=hcadvsort]");

            if (advSortSelect.length > 0) {
                advSortSelect.val(fieldname);
                advSortSelect.trigger("change");
            }

            if (advSortDir.length > 0)
                HCE.DynamicDisplay.ToggleDynamicSortButton(advSortDir, sortdir);
        }
    }

    containers.SearchGrid.data("sortorder", sortorder);

    HCE.DynamicDisplay.RefreshGridData($(item));
}

//------------------------------------------------------------------------------------------------
// FUNCTION:	ToggleDynamicSortButton
// PURPOSE:		TOGGLE THE SPECIFIED DYNAMIC SORT BUTTON
//------------------------------------------------------------------------------------------------
HCE.DynamicDisplay.ToggleDynamicSortButton = function (button, sortdir) {
    var span = $(button).find("span").first();
    if (typeof sortdir === 'undefined')
        sortdir = '';

    if ((sortdir == 'desc') || ((sortdir != 'asc') && (span.hasClass("fa-sort-alpha-asc"))))
        span.removeClass("fa-sort-alpha-asc").addClass("fa-sort-alpha-desc");
    else
        span.removeClass("fa-sort-alpha-desc").addClass("fa-sort-alpha-asc");
}

//------------------------------------------------------------------------------------------------
// FUNCTION:	ClearSort
// PURPOSE:		CLEAR THE ADVANCED SORT OPTIONS
//------------------------------------------------------------------------------------------------
HCE.DynamicDisplay.ClearSort = function (item) {
    //-- HANDLE THE CLEAR BUTTON CLICKS --
    var parentBox = $(item).closest("div[data-type=hcadvsortrow]");
    var selectBox = parentBox.find("select").first();
    var clearButton = parentBox.find("button[data-type=hcadvsort] span");

    selectBox.val("");
    clearButton.removeClass("fa-sort-alpha-asc fa-sort-alpha-desc").addClass("fa-sort-alpha-asc");

    HCE.DynamicDisplay.CascadeSortOptions(selectBox);
}

//------------------------------------------------------------------------------------------------
// FUNCTION:	CascadeSortOptions
// PURPOSE:		CASCADE THE SORT OPTIONS
//------------------------------------------------------------------------------------------------
HCE.DynamicDisplay.CascadeSortOptions = function (sortItem) {
    var parentGroup = $(sortItem).closest('div[data-type=hcadvsortrow]');
    var parentBox = $(parentGroup).closest('div[data-type=hcadvsortbox]');
    var currentIndex = parentGroup.data("sortindex");
    var nextIndex = currentIndex + 1;
    var nextCustomGroup;

    //-- REMOVE ANY CUSTOM SORT ROWS THAT WERE GENERATED AFTER THE CURRENT ROW --
    nextCustomGroup = parentBox.find("div[data-type=hcadvsortrow][data-sortindex=" + nextIndex++ + "]");
    while ($(nextCustomGroup).length > 0) {
        nextCustomGroup.empty();
        nextCustomGroup = parentBox.find("div[data-type=hcadvsortrow][data-sortindex=" + nextIndex++ + "]");
    }

    //-- ADD THE NEXT GROUP --
    if ($(sortItem).val() != "") {
        HCE.DynamicDisplay.AddSortRow(sortItem, currentIndex + 1);
    }
}

//-------------------------------------------------------------------------------------------------
// FUNCTION:	InitializeSortBoxes
// PURPOSE:		INITIALIZE THE DYNAMIC SORT BOXES
//-------------------------------------------------------------------------------------------------
HCE.DynamicDisplay.InitializeSortBoxes = function () {
    $("div.advanced-search-rows").each(function () {
        var sortItems = [];
        var sortItemIdx = 0;

        //-- INITIALIZE THE DYNAMIC SORT BOXES --
        $(this).find("[data-type=hcadvanced][data-subtype=field]").each(function () {
            curRow = $(this);
            if (curRow.data("sortable") == true) {
                var fieldName = $(this).data("field");
                var fieldLabel = $(this).closest("div.row[data-type=advsearch-row]").find("label.control-label");
                if (fieldLabel.length > 0)
                    fieldLabel = fieldLabel.html();
                else
                    fieldLabel = "UNKNOWN";

                sortItems[sortItemIdx++] = { Value: fieldName, Text: fieldLabel };
            }
        });

        if (sortItems.length > 0) {
            //-- ADD THE NEW DYNAMIC SORT ROW --
            var lastRow = $(this).find("div.row[data-type=advsearch-row]").last();
            if (lastRow.length > 0)
                lastRow.after('<div class="row" data-type="advsearch-row">' +
								    '<div class="col-sm-3"><label class="control-label colon">' + HCE.Resources.OrderBy + '</label></div>' +
									   '<div class="col-sm-9" data-type="hcadvsortbox">' +
				                     HCE.DynamicDisplay.GetSortItemHTML(sortItems, 1) +
										'</div>' +
									 '</div>' +
								   '</div>');
        }

    });
}

//-----------------------------------------------------------------------------------------
// FUNCTION:	AddSortRow
// PURPOSE:		ADD A NEW SORT ROW TO THE ADVANCED SORT SCREEN
//-----------------------------------------------------------------------------------------
HCE.DynamicDisplay.AddSortRow = function (field, index) {
    var parent = $(field).closest("div[data-type=hcadvsortbox]");
    var html = '';

    if (parent.length > 0) {
        //-- FIND THE SELECTED ITEMS --
        var selectedValues = [];
        var availableValues = [];
        var i = 0;

        parent.find("select").each(function () {
            if ($(this).val() != "")
                selectedValues[i++] = $(this).val();
        });

        //-- ADD THE LIST OF REMAINING ITEMS --
        i = 0;
        parent.find("select").first().find("option").each(function () {
            if ($(this).val() != "") {
                if ($.inArray($(this).val(), selectedValues) == -1)
                    availableValues[i++] = { Value: $(this).val(), Text: $(this).text() };
            }
        });

        if (availableValues.length > 0) {
            //-- ADD THE NEW DYNAMIC SORT ROW --
            var lastRow = $(parent).find("div[data-type=hcadvsortrow]").last();
            if ($(lastRow).length > 0)
                $(lastRow).after(HCE.DynamicDisplay.GetSortItemHTML(availableValues, index));
        }
    }
}

//------------------------------------------------------------------------------------
// FUNCTION:	GetSortItemHTML
// PURPOSE:		BUILD THE DYNAMIC SORT ITEM HTML
//------------------------------------------------------------------------------------
HCE.DynamicDisplay.GetSortItemHTML = function (itemList, sequence) {
    var html = '';

    if (itemList.length > 0) {
        html = '<div data-type="hcadvsortrow" data-sortindex="' + sequence + '">';

        if (sequence > 1) {
            html += '<div class="thenby">' + HCE.Resources.ThenBy + ':</div>';
        }

        html += '<div class="input-group">' +
					    '<select class="form-control input-sm">' +
						   '<option value=""></option>';

        for (var i = 0; i < itemList.length; i++) {
            html += '<option value="' + itemList[i].Value + '">' + itemList[i].Text + '</option>';
        }

        html += '</select>' +
				    '<span class="input-group-btn">' +
					   '<button type="button" class="btn btn-default btn-sm" data-type="hcadvsort"><span class="fa fa-sort-alpha-asc fa-lg"></span></button>' +
					   '<button type="button" class="btn btn-default btn-sm" data-type="hcadvclose"><span class="fa fa-close fa-lg"></span></button>' +
					  '</span>' +
					'</div>' +
				 '</div>';
    }

    return (html);
}

//-----------------------------------------------------------------------------------------------
// FUNCTION:	ApplySort
// PURPOSE:		APPLY THE SORT TO THE SPECIFIED TABLE
//-----------------------------------------------------------------------------------------------
HCE.DynamicDisplay.ApplySort = function (table, initAdvanced) {
    var retVal = "";
    var sortOrder;
    var sortItems;
    var sortField;
    var sortDir;
    var containers;
    var sortIndex = 1;

    containers = HCE.DynamicDisplay.GetSearchContainers(table);

    //-- REMOVE ANY PREVIOUS ASCENDING OR DESCENDING SORT ICONS --
    $(table).find("th.sortable").removeClass("asc desc");

    if ($(table).data().hasOwnProperty("sortorder")) {
        sortOrder = $(table).data("sortorder");

        if (sortOrder != "") {
            var sortItems = sortOrder.split(",");
            for (var i = 0; i < sortItems.length; i++) {
                //-- PARSE THE SORT FIELD AND DIRECTION --
                if (/^.* (asc|desc)$/.test(sortItems[i])) {
                    sortField = sortItems[i].replace(/ (asc|desc)$/g, "").trim();
                    sortDir = / desc$/g.test(sortItems[i]) ? "desc" : "asc";
                }
                else {
                    sortField = sortItems[i].trim();
                    sortDir = "asc";
                }

                //-- UPDATE THE SORT HEADERS --
                $(table).find("th.sortable[data-field=" + sortField + "]").addClass(sortDir);

                //-- INITIALIZE THE ADVANCED SEARCH SORT BUTTONS --
                if ((typeof initAdvanced != 'undefined') && (initAdvanced == true)) {
                    if ((containers.SearchGrid.length > 0) && (containers.SortBox.length > 0)) {
                        var sortItem = containers.SortBox.find("div[data-type=hcadvsortrow][data-sortindex=" + sortIndex + "]");
                        if (sortItem.length > 0) {
                            var sortSel = sortItem.find("select").first();
                            var sortButton = sortItem.find("button[data-type=hcadvsort]");
                            sortSel.val(sortField);
                            if ((sortSel.val() !== null) && (sortSel.val() !== '')) {
                                sortIndex++;
                                sortSel.trigger("change");
                                HCE.DynamicDisplay.ToggleDynamicSortButton(sortButton, sortDir);
                            }
                        }
                    }
                }

                if (sortField != "")
                    retVal = retVal + (retVal != "" ? "," : "") + sortField.trim() + (sortDir == "desc" ? " desc" : "");
            }
        }
    }

    return (retVal);
}

//---------------------------------------------------------------------------------------------
// FUNCTION:	GetDefaultSortOrder
// PURPOSE:		GET THE DEFAULT SORT ORDER FOR THE SPECIFIED DYNAMIC TABLE
//---------------------------------------------------------------------------------------------
HCE.DynamicDisplay.GetDefaultSortOrder = function (table) {
    var defaultSort = "";

    if ($(table).data().hasOwnProperty("defaultsort"))
        defaultSort = $(table).data("defaultsort");
    else {
        //-- GET THE FIELD NAME OF THE FIRST FIELD IN THE GRID --
        var firstField = $(table).find("tr.column-header-row th").first();
        if (firstField.length > 0)
            defaultSort = firstField.data("field");
    }

    return defaultSort;
}

//-----------------------------------------------------------------------
// FUNCTION:	RefreshGridData
// PURPOSE:		KEEP ADVANCED SEARCH FIELDS IN SYNC WITH THE FILTER FIELDS
//             AND RELOAD THE GRID WHEN ANYTHING CHANGES
//-----------------------------------------------------------------------
HCE.DynamicDisplay.RefreshGridData = function (sourceField, pageNumber, entityname) {
    var containers = HCE.DynamicDisplay.GetSearchContainers(sourceField);
    var containerID = containers.Container.attr("id");

    try {

        //-- GET THE ANGULAR SCOPE TO BE UPDATED --
        var angularScope = angular.element('#' + containerID).scope();
        angularScope.UpdateGrid(HCE.DynamicDisplay.ProcessAdvancedSearch($(sourceField), pageNumber, entityname), false);

        angularScope.$apply();

    } catch (e) {
        HCE.DynamicDisplay.RaiseErrorEvent(containers.Container, e);
        HCE.Error.HandleErrorMessage(HCE.Error.GetErrorObject(500, '', e));
    }
}

//-----------------------------------------------------------------------
// FUNCTION:	RefreshDetailData
// PURPOSE:		KEEP ADVANCED SEARCH FIELDS IN SYNC WITH THE FILTER FIELDS
//             AND RELOAD THE GRID WHEN ANYTHING CHANGES
//-----------------------------------------------------------------------
HCE.DynamicDisplay.RefreshDetailData = function (sourceField, rowInfo) {
    var containers = HCE.DynamicDisplay.GetSearchContainers(sourceField);
    var containerID = containers.Container.attr("id");

    try {

        //-- GET THE ANGULAR SCOPE TO BE UPDATED --
        var angularScope = angular.element('#' + containerID).scope();
        angularScope.GetDetails(HCE.DynamicDisplay.ParseDetailURL($(sourceField), rowInfo), false);
        angularScope.$apply();

    } catch (e) {
        HCE.DynamicDisplay.RaiseErrorEvent(containers.Container, e);
        HCE.Error.HandleErrorMessage(HCE.Error.GetErrorObject(500, '', e));
    }
}

//----------------------------------------------------------------------------------------------------
// FUNCTION:    ParseDetailURL
// PURPOSE:     PARSE THE DETAILED ITEM URL
//----------------------------------------------------------------------------------------------------
HCE.DynamicDisplay.ParseDetailURL = function (field, rowInfo) {
    var containers = HCE.DynamicDisplay.GetSearchContainers(field);
    var odataurl = containers.Container.data("root-url") + containers.Container.data("det-url");
    var odataparms = containers.Container.data("det-parms");

    if ((odataparms.length > 0) && (odataparms != null) && (odataparms != undefined))
        odataurl = odataurl + "?" + odataparms;

    if ((typeof (rowInfo) != "undefined") && (rowInfo != undefined) && (rowInfo.Keys != undefined)) {
        var parms = odataurl.match(/[^{}]+(?=\})/g);
        var itemVal;

        for (var i = 0; i < parms.length; i++) {
            itemVal = null;

            for (var property in rowInfo.Keys) {
                if (rowInfo.Keys.hasOwnProperty(property)) {
                    if (property.toLowerCase() == parms[i].toLowerCase()) {
                        itemVal = rowInfo.Keys[property];
                        break;
                    }
                }
            }

            if (itemVal != null) {
                odataurl = odataurl.replace('{' + parms[i] + '}', itemVal);
            }
            else {
                throw parms[i] + " was not included in the rowInfo object in the row click event - make sure the IncludeInRowClick option is enabled for this field on the DTO object";
            }
        }
    }

    return odataurl;
}

//-----------------------------------------------------------------------------
// FUNCTION:    UpdateGridWithUpdatedEntity
// PURPOSE:     UPDATE A SEARCH GRID WITH AN UPDATED ENTITY
//-----------------------------------------------------------------------------
HCE.DynamicDisplay.UpdateGridWithUpdatedEntity = function (scope, keys, entity) {
    var bFound = false;

    try {
        if ((scope.entities != undefined) && (typeof scope.entities != 'undefined') && (scope.entities != null)) {
            for (var i in scope.entities) {
                bFound = true;

                for (var j in keys) {
                    if (scope.entities[i][keys[j].Key] != keys[j].Value) {
                        bFound = false;
                        break;
                    }
                }

                if (bFound) {
                    scope.entities[i] = entity;
                    break;
                }
            }
        }
    } catch (e) {
    }
}

//-----------------------------------------------------------------------------------------------------
// FUNCTION:	ProcessAdvancedSearch
// PURPOSE:		BUILD THE ODATA QUERY STRING AND SYNC THE FILTER BOXES WITH THE ADVANCED SEARCH BOXES
//-----------------------------------------------------------------------------------------------------
HCE.DynamicDisplay.ProcessAdvancedSearch = function (field, pageNumber) {
    var orderby;
    var recsperpage;
    var odataurl;
    var queryString = "";
    var query;
    var filteritem;

    var i = 0;

    var odatafilters = [];
    var odatafilterindex = 0;

    var hcfilters = [];
    var hcfilterindex = 0;

    var containers = HCE.DynamicDisplay.GetSearchContainers(field);
    odataurl = containers.Container.data("root-url") + containers.Container.data("entity-name");

    if ((typeof containers.Container.data("entity-path") !== 'undefined') && (containers.Container.data("entity-path") !== ''))
        odataurl += containers.Container.data("entity-path");

    //-- ADD THE FILTERS --
    containers.SearchModal.find("[data-type=hcadvanced][data-subtype=field]").each(function () {
        if ($(this).val() != '') {
            //-- COPY THE VALUE TO THE CORRESPONDING FILTER BOX --
            filteritem = containers.SearchGrid.find("[data-type=hcfilter][data-field=" + $(this).data("field") + "]");
            if (filteritem.length > 0)
                filteritem.val($(this).val());

            //-- ADD THE FILTER ITEM --
            query = HCE.DynamicDisplay.GetODATAWhereClauseForField($(this));

            if (query.Filter != "") {
                switch (query.FilterType) {
                    case "$filter":
                        odatafilters[odatafilterindex++] = query.Filter;
                        break;

                    case "hcfilter":
                        hcfilters[hcfilterindex++] = query.Filter;
                        break;
                }
            }
        }
    });

    if (odatafilters.length > 0) {
        for (i = 0; i < odatafilters.length; i++) {
            queryString = queryString + (queryString == "" ? "$filter=" : " and ") + odatafilters[i];
        }
    }

    if (hcfilters.length > 0) {
        for (i = 0; i < hcfilters.length; i++) {
            queryString = queryString + (queryString == "" ? "" : "&") + hcfilters[i];
        }
    }

    //-- ENABLE THE CLEAR FILTER BUTTON --
    containers.Container.find("button[data-type=HCCLEARFILTERS]").first().prop("disabled", false);

    //-- ADD THE ORDER BY CLAUSE --
    orderby = HCE.DynamicDisplay.ApplySort(containers.SearchGrid);
    if ((orderby !== null) && (orderby !== "")) {
        queryString = queryString + (queryString == "" ? "" : "&") + "$orderby=" + orderby;
    }

    //-- ADD THE PAGING INFORMATION --
    if ((typeof pageNumber == 'undefined') || (pageNumber == null))
        pageNumber = 1;

    containers.Container.data("currentpagenumber", pageNumber);

    if (containers.Container.attr("data-fixedpagesize")) {
        recsperpage = containers.Container.data("fixedpagesize");
    }
    else {
        recsperpage = containers.SearchGrid.data("pagesize");
        if ((typeof recsperpage == 'undefined') || (recsperpage == null))
            recsperpage = HCE.DynamicDisplay.GetRecsPerPage();
    }

    queryString = queryString + (queryString == "" ? "" : "&") + "$pagesize=" + recsperpage;

    if (pageNumber > 1)
        queryString = queryString + (queryString == "" ? "" : "&") + "$page=" + pageNumber;

    if (containers.Container.data("query-parms") != '') {
        queryString = queryString + (queryString == "" ? "" : "&") + containers.Container.data("query-parms");
    }

    var retURL = odataurl + (queryString != "" ? (odataurl.indexOf("?") > 0 ? "&" : "?") + queryString : "")
    //console.log(retURL);
    return retURL;
}

//---------------------------------------------------------------------------------------
// FUNCTION:	GetRecsPerPage
// PURPOSE:		GET THE NUMBER OF RECORDS PER PAGE
//---------------------------------------------------------------------------------------
HCE.DynamicDisplay.GetRecsPerPage = function (pagesize) {
    var recsPerPage;
    var setCookie = false;

    if ((typeof pagesize != 'undefined') && (pagesize != null)) {
        recsPerPage = pagesize;
        setCookie = true;
    }
    else
        recsPerPage = $.cookie('RECSPERPAGE');

    if ((typeof recsPerPage == 'undefined') || (recsPerPage == null) || (recsPerPage == undefined) || (isNaN(recsPerPage))) {
        recsPerPage = 10;
        setCookie = true;
    }
    else if (!(/^(5|10|15|20|25|50)$/.test(recsPerPage))) {
        recsPerPage = 10;
        setCookie = true;
    }

    if (setCookie) {
        $.cookie('RECSPERPAGE', recsPerPage, { expires: 1000, path: '/' });
    }

    return (recsPerPage);
}

//-----------------------------------------------------------------------------------------------------
//-- FUNCTION:	SyncAdvancedSearchField
//-- PURPOSE:	SYNCHRONIZE THE VARIOUS SEARCH FIELDS 
//-----------------------------------------------------------------------------------------------------
HCE.DynamicDisplay.SyncAdvancedSearchField = function (field) {
    var searchFields = HCE.DynamicDisplay.GetAdvancedSearchFields(field);
    var fieldType = searchFields.Field.data("field-type");

    if (fieldType != "date") {
        var fieldValues;

        switch ($(field).data("subtype")) {
            case "field":
                fieldValues = HCE.DynamicDisplay.ParseSearchString($(field).val(), fieldType);
                searchFields.Field1.val(fieldValues.SearchField1);
                searchFields.Field2.val(fieldValues.SearchField2);
                searchFields.Operator.val(fieldValues.Operator);
                break;

            default:
                fieldValues = HCE.DynamicDisplay.GetSearchStringFromAdvanced(searchFields.Operator.val(), searchFields.Field1.val(), searchFields.Field2.val());
                searchFields.Field.val(fieldValues);
                break;
        }
    }
}

//----------------------------------------------------------------------------------------
// FUNCTION:	CopyFilterToAdvanced
// PURPOSE:		COPY THE FILTER VALUE TO ANY MATCHING ADVANCED FILTER FIELDS
//----------------------------------------------------------------------------------------
HCE.DynamicDisplay.CopyFilterToAdvanced = function (filterField) {
    var containers = HCE.DynamicDisplay.GetSearchContainers($(filterField));

    //-- FIND THE MATCHING ADVANCED SEARCH FIELD --
    var advSearchField = containers.SearchModal.find("[data-type=hcadvanced][data-field=" + $(filterField).data("field") + "]");
    var searchBox = advSearchField.closest("div.row[data-type=advsearch-row]").find("div.advanced-search-box");

    if (advSearchField.length > 0) {
        //-- COPY THE VALUE TO THE ADVANCED FILTER FIELD --
        advSearchField.val($(filterField).val());
    }

    if (searchBox.length > 0) {
        //-- SYNC THE FILTER AND THE ADVANCED SEARCH BOX --
        HCE.DynamicDisplay.SyncAdvancedSearchField($(advSearchField));
        HCE.DynamicDisplay.FormatAdvancedSearchFields($(advSearchField));

        if ($(filterField).data("filter-type") == "date")
            HCE.DynamicDisplay.ToggleAdvancedSearchBox(advSearchField);
    }
}

//------------------------------------------------------------------------------------
// FUNCTION:	FormatAdvancedSearchFields
// PURPOSE:		FORMAT THE ADVANCED SEARCH FIELD SECTION BASED ON THE SELECTED OPERATOR
//------------------------------------------------------------------------------------
HCE.DynamicDisplay.FormatAdvancedSearchFields = function (field) {
    var searchFields = HCE.DynamicDisplay.GetAdvancedSearchFields(field);

    switch (searchFields.Operator.val()) {
        case "between":
            searchFields.Field1.attr("placeholder", HCE.Resources.From);
            searchFields.Field2.attr("placeholder", HCE.Resources.To);
            searchFields.Field1.closest('div').removeClass("col-sm-5 col-sm-12").addClass("col-sm-5");
            searchFields.And.show();
            searchFields.Field2.closest('div').show();
            break;

        default:
            searchFields.Field1.attr("placeholder", HCE.Resources.Value);
            searchFields.Field2.attr("placeholder", "");
            searchFields.Field1.closest('div').removeClass("col-sm-5 col-sm-12").addClass("col-sm-12");
            searchFields.And.hide();
            searchFields.Field2.closest('div').hide();
            break;
    }
}

//--------------------------------------------------------------------------------------
// FUNCTION:	GetAdvancedSearchFields
// PURPOSE:		GET THE ADVANCED SEARCH FIELDS
//--------------------------------------------------------------------------------------
HCE.DynamicDisplay.GetAdvancedSearchFields = function (field) {
    var advSearchRow = $(field).closest('div.row[data-type=advsearch-row]');
    var advSearchField = advSearchRow.find('[data-type=hcadvanced][data-subtype=field]');
    var advSearchFieldVal = advSearchField.val();
    var advSearchButton = advSearchRow.find('button[data-type=hcadvtoggle]');
    var advSearchBox = advSearchRow.find('div.advanced-search-box');
    var field1 = advSearchBox.find('[data-type=hcadvanced][data-subtype=from]');
    var fieldand = advSearchBox.find('[data-type=andlabel]');
    var field2 = advSearchBox.find('[data-type=hcadvanced][data-subtype=to]');
    var operator = advSearchBox.find('select.advanced-search-operator');

    return { Row: advSearchRow, Field: advSearchField, FieldVal: advSearchFieldVal, Button: advSearchButton, Box: advSearchBox, Field1: field1, And: fieldand, Field2: field2, Operator: operator }
}

//------------------------------------------------------------------------------------
// FUNCTION:	ToggleAdvancedSearchBox
// PURPOSE:		SHOW/HIDE THE ADVANCED SEARCH BOX 
//------------------------------------------------------------------------------------
HCE.DynamicDisplay.ToggleAdvancedSearchBox = function (field) {
    var forceShow = false;
    var forceHide = false;

    var searchFields = HCE.DynamicDisplay.GetAdvancedSearchFields(field);

    if (searchFields.Field.data("field-type") == "date") {
        forceShow = (searchFields.Field.val() == "custom");
        forceHide = !forceShow;
    }

    if ((forceShow) || ((!forceHide) && (!searchFields.Box.is(":visible")))) {
        if (searchFields.Button.length > 0) {
            searchFields.Button.html('<span class="fa fa-chevron-circle-up fa-lg"></span>');
            searchFields.Field.prop('disabled', 'disabled');
        }
        searchFields.Box.slideDown({ duration: 400 }, function () { });
    }
    else if ((forceHide) || ((!forceShow) && (searchFields.Box.is(":visible")))) {
        if (searchFields.Button.length > 0) {
            searchFields.Button.html('<span class="fa fa-chevron-circle-down fa-lg"></span>');
            searchFields.Field.prop('disabled', false);
        }
        searchFields.Box.slideUp({ duration: 100 }, function () { });
    }
}

//---------------------------------------------------------------------
//-- FUNCTION:	GetSearchStringFromAdvanced
//-- PURPOSE:	BUIILD THE CUSTOM SEARCH STRING FROM THE ADVANCED SEARCH
//--           OPTIONS
//---------------------------------------------------------------------
HCE.DynamicDisplay.GetSearchStringFromAdvanced = function (operator, fieldValue1, fieldValue2) {
    if ((fieldValue1 != undefined) && (fieldValue1 != null) && (fieldValue1 != ''))			//*JCB 11/04/2015 Fixed formatting when field is empty
    {
        switch (operator) {
            case "startswith":
                return (fieldValue1 + '*');

            case "endswith":
                return ('*' + fieldValue1);

            case "contains":
                return (fieldValue1);

            case "eq":
                return ('=' + fieldValue1);

            case "between":
                return (fieldValue1 + '~' + fieldValue2);

            case "ne":
                return ('!=' + fieldValue1);

            case "not startswith":
                return ('!' + fieldValue1 + '*');

            case "not endswith":
                return ('!*' + fieldValue1);

            case "not contains":
                return ('!' + fieldValue1);

            case "ge":
                return ('>=' + fieldValue1);

            case "gt":
                return ('>' + fieldValue1);

            case "le":
                return ('<=' + fieldValue1);

            case "lt":
                return ('<' + fieldValue1);

            default:
                return (fieldValue1);
                break;
        }
    }
    else
        return '';
}

//---------------------------------------------------------------------
//-- FUNCTION:	ParseSearchString                                  
//-- PURPOSE:	PARSE A CUSTOM SEARCH STRING INTO THE OPERATOR, FIELD1
//--           AND FIELD 2 VALUES                                    
//---------------------------------------------------------------------
HCE.DynamicDisplay.ParseSearchString = function (fieldValue, fieldType) {
    var fieldOperator = "";
    var advSearch1 = "";
    var advSearch2 = "";

    if (fieldValue != "") {
        if (/^!\*.*$/.test(fieldValue)) {
            fieldOperator = 'not endswith';
            advSearch1 = fieldValue.substr(2);
        }
        else if (/^\*.*$/.test(fieldValue)) {
            fieldOperator = 'endswith';
            advSearch1 = fieldValue.substr(1);
        }
        else if (/^!.*\*$/.test(fieldValue)) {
            fieldOperator = 'not startswith';
            advSearch1 = fieldValue.substr(1).slice(0, -1);
        }
        else if (/^.*\*$/.test(fieldValue)) {
            fieldOperator = 'startswith';
            advSearch1 = fieldValue.slice(0, -1);
        }
        else if (/^=.*$/.test(fieldValue)) {
            fieldOperator = 'eq';
            advSearch1 = fieldValue.substr(1);
        }
        else if (/^!=.*$/.test(fieldValue)) {
            fieldOperator = 'ne';
            advSearch1 = fieldValue.substr(2);
        }
        else if (/^!.*$/.test(fieldValue)) {
            fieldOperator = 'not contains';
            advSearch1 = fieldValue.substr(1);
        }
        else if (/^.*~.*$/.test(fieldValue)) {
            fieldOperator = 'between';
            advSearch1 = fieldValue.split('~')[0];
            advSearch2 = fieldValue.split('~')[1];
        }
        else if (/^>=.*$/.test(fieldValue)) {
            fieldOperator = 'ge';
            advSearch1 = fieldValue.substr(2);
        }
        else if (/^<=.*$/.test(fieldValue)) {
            fieldOperator = 'le';
            advSearch1 = fieldValue.substr(2);
        }
        else if (/^<.*$/.test(fieldValue)) {
            fieldOperator = 'lt';
            advSearch1 = fieldValue.substr(1);
        }
        else if (/^>.*$/.test(fieldValue)) {
            fieldOperator = 'gt';
            advSearch1 = fieldValue.substr(1);
        }
        else {
            switch (fieldType) {
                case "int":
                case "decimal":
                    fieldOperator = 'eq';
                    break;

                default:
                    fieldOperator = 'contains';
                    break;
            }

            advSearch1 = fieldValue;
        }
    }

    return { Operator: fieldOperator, SearchField: fieldValue, SearchField1: advSearch1, SearchField2: advSearch2 };
}

//-------------------------------------------------------------------------------------------------------
// FUNCTION:	GetODATAWhereClause
// PURPOSE:		BUILD THE APPROPRIATE FILTER CLAUSE FOR THE ODATA FIELD
//-------------------------------------------------------------------------------------------------------
HCE.DynamicDisplay.GetODATAWhereClauseForField = function (field) {
    var searchFields = HCE.DynamicDisplay.GetAdvancedSearchFields(field);

    var whereClause = "";
    var filterType = "$filter";
    var fieldType = searchFields.Field.data("field-type");
    var fieldName = searchFields.Field.data("field");
    var isList = searchFields.Field.data("islist");
    var fieldVal = searchFields.FieldVal;
    var operator = searchFields.Operator.val();
    var field1 = searchFields.Field1.val();
    var field2 = searchFields.Field2.val();
    var fieldValue = searchFields.Field.val();

    var dtPeriod;
    var dtNumberOfPeriods;
    var dtValue1;
    var dtValue2;

    if ((typeof (isList) == "undefined") || (isList == undefined) || (isList == null))
        isList = false;

    if (isList == true) {
        if (fieldVal != '') {
            if (/^OP:.+$/.test(fieldVal))
                whereClause = fieldVal.substr(3);
            else {
                switch (fieldType) {
                    case "string":
                        whereClause = fieldName + " eq '" + fieldVal + "'";
                        break;

                    default:
                        whereClause = fieldName + " eq " + fieldVal;
                        break;
                }
            }
        }
    }
    else {

        switch (fieldType) {
            case "string":

                switch (operator) {
                    case "endswith":
                    case "not endswith":
                    case "contains":
                    case "not contains":
                    case "startswith":
                    case "not startswith":
                        whereClause = operator + "(" + fieldName + ",'" + HCE.DynamicDisplay.ReplaceSpecialCharacters(field1) + "')";
                        break;

                    case "between":
                        whereClause = fieldName + " ge '" + HCE.DynamicDisplay.ReplaceSpecialCharacters(field1) + "' and " + fieldName + " le '" + HCE.DynamicDisplay.ReplaceSpecialCharacters(field2) + "'";
                        break;

                    default:
                        whereClause = fieldName + " " + operator + " '" + HCE.DynamicDisplay.ReplaceSpecialCharacters(field1) + "'";
                        break;

                }
                break;

            case "bool":
                if (fieldVal != '')
                    whereClause = fieldName + " eq " + fieldVal;
                break;

            case "int":
                switch (operator) {
                    case "between":
                        whereClause = fieldName + " ge " + field1 + " and " + fieldName + " le " + field2;
                        break;

                    default:
                        whereClause = fieldName + " " + operator + " " + field1;
                        break;
                }
                break;

            case "decimal":
                switch (operator) {
                    case "between":
                        whereClause = fieldName + " ge " + HCE.DynamicDisplay.ParseAmount(field1).toString() + " and " + fieldName + " le " + HCE.DynamicDisplay.ParseAmount(field2).toString();
                        break;

                    default:
                        whereClause = fieldName + " " + operator + " " + HCE.DynamicDisplay.ParseAmount(fieldVal).toString();
                        break;
                }
                break;

            case "date":
                filterType = "hcfilter";

                if (fieldValue != "") {
                    switch (fieldValue) {
                        case "custom":

                            //-- PARSE THE FIRST DATE FIELD --
                            dtValue1 = moment(field1, 'YYYY-MM-DD', true);
                            if (dtValue1 == null)
                                break;
                            else
                                dtValue1 = dtValue1.format("YYYY-MM-DD", true);

                            switch (operator) {
                                case "between":

                                    //-- PARSE THE SECOND DATE FIELD --
                                    dtValue2 = moment(field2, 'YYYY-MM-DD', true);
                                    if (dtValue2 == null)
                                        break;
                                    else
                                        dtValue2 = dtValue2.format("YYYY-MM-DD", true);

                                    whereClause = "hcfilter=date(" + fieldName + ",between," + dtValue1 + "," + dtValue2 + ")";
                                    break;

                                default:
                                    whereClause = "hcfilter=date(" + fieldName + "," + operator + "," + dtValue1 + ")";
                                    break;
                            }
                            break;

                        default:

                            if (/^(day|week|month|year)!-?[0-9]{1,5}$/.test(fieldValue)) {
                                dtPeriod = fieldValue.split("!")[0];
                                dtNumberOfPeriods = fieldValue.split("!")[1];
                                whereClause = "hcfilter=daterange(" + fieldName + ",between," + dtPeriod + "," + dtNumberOfPeriods + ")";
                            }
                            else if (/^(mtd|pmtd|ytd|pytd)$/.test(fieldValue)) {
                                whereClause = "hcfilter=daterange(" + fieldName + ",between," + fieldValue + ")";
                            }
                    }
                }
                break;
        }
    }

    return { FilterType: filterType, Filter: whereClause };
}

//---------------------------------------------------------------------------------------
// FUNCTION:        HCE.DynamicDisplay.ReplaceSpecialCharacters
// PURPOSE:         FIX ANY SPECIAL CHARACTERS BEFORE SUBMITTING THE ODATA QUERY
//---------------------------------------------------------------------------------------
HCE.DynamicDisplay.ReplaceSpecialCharacters = function (attribute) {
    attribute = attribute.replace(/'/g, "''");
    attribute = attribute.replace(/%/g, "%25");
    attribute = attribute.replace(/\+/g, "%2B");
    attribute = attribute.replace(/\//g, "%2F");
    attribute = attribute.replace(/\?/g, "%3F");
    attribute = attribute.replace(/#/g, "%23");
    attribute = attribute.replace(/&/g, "%26");
    return attribute;
}

//----------------------------------------------------------------------------------------
// FUNCTION:        HCE.DynamicDisplay.ParseAmount
// PURPOSE:         PARSE AN AMOUNT VALUE
//----------------------------------------------------------------------------------------
HCE.DynamicDisplay.ParseAmount = function (value) {
    var isNegative = value !== null && value !== '' && ((value.indexOf('-') >= 0) || ((value.indexOf('(') >= 0 && value.indexOf(')') > value.indexOf('('))));
    try {
        var retVal = parseFloat(value.replace(/[^\d.]/g, ''));
        if (isNegative)
            retVal = retVal * -1;
    }
    catch (err) {
        retVal = -999999999;
    }
    return (retVal);
}

//----------------------------------------------------------------------------------------
// FUNCTION:	GetSearchContainers
// PURPOSE:		GET THE DYNAMIC SEARCH CONTAINERS THAT CONTAIN THE SPECIFIED FIELD 
//----------------------------------------------------------------------------------------
HCE.DynamicDisplay.GetSearchContainers = function (item) {
    var container;

    if ($(item).data("type") == "HCDYNAMICGRID")
        container = $(item);
    else
        container = $(item).closest('[data-type=HCDYNAMICGRID]');

    var dynamicTable = $(container).find('table.dynamic-search-table, div.dynamic-search-table').first();    //*JCB 11/04/2015
    var advSearchModal = $(container).find('div.dynamic-search-modal').first();
    var advSortBox = $(advSearchModal).find('div[data-type=hcadvsortbox]');
    return { Container: container, SearchGrid: dynamicTable, SearchModal: advSearchModal, SortBox: advSortBox }
}

//------------------------------------------------------------------------------------------
// FUNCTION:	BuildDynamicPager
// PURPOSE:		BUILD THE DYNAMIC PAGER
//------------------------------------------------------------------------------------------
HCE.DynamicDisplay.BuildDynamicPager = function (sourceGrid, pagingInfo, hideNoRecords) {
    var containers = HCE.DynamicDisplay.GetSearchContainers(sourceGrid);
    var pagingJSON;
    var html = '';
    var showNoRecords = (typeof hideNoRecords === 'undefined' || hideNoRecords != true);

    if (containers.SearchGrid.length <= 0)
        return;
    else if (containers.Container.attr("data-fixedpagesize"))
        return;

    var pager = containers.Container.find("div.HC-pager");
    if (pager.length == 0) {
        //-- BUILD THE DYNAMIC PAGER AND ADD IT BELOW THE TABLE --
        html = '<div class="hidden-print HC-pager">' +
		         '<div>' + HCE.Resources.NoRecordsFound.toUpperCase() + '</div>' +
				   '<nav>' +
					  '<ul class="pagination">' +
					  '</ul>' +
				   '</nav>' +
				   '<div>' +
					  '<select class="records-per-page">' +
					    '<option>5</option>' +
						 '<option>10</option>' +
						 '<option>15</option>' +
						 '<option>20</option>' +
						 '<option>25</option>' +
						 '<option>50</option>' +
					  '</select>' +
					  'RECORDS PER PAGE' +
				   '</div>' +
				 '</div>';

        containers.SearchGrid.after(html);
        pager = containers.Container.find("div.HC-pager");
    }


    try {
        if (pagingInfo != null)
            pagingJSON = $.parseJSON(pagingInfo);
        else
            pagingJSON = $.parseJSON('{"PageSize":0,"Page":0,"TotalRecords":0,"TotalPages":0,"PrevLink":"","CurLink":"","NextLink":""}');
    } catch (e) {
        pagingJSON = $.parseJSON('{"PageSize":0,"Page":0,"TotalRecords":0,"TotalPages":0,"PrevLink":"","CurLink":"","NextLink":""}');
    }

    var pagerRecordCount = pager.find("div").first();
    var pagerList = pager.find("nav ul").first();
    var pagerRecsPerPage = pager.find("div").last();
    var pagerRecsPerPageSel = pagerRecsPerPage.find("select").first();

    if (pagingJSON.TotalRecords <= 0) {
        if (showNoRecords) {
            if (typeof containers.Container.data("norecords-entity") !== 'undefined' && containers.Container.data("norecords-entity") !== undefined && containers.Container.data("norecords-entity") !== '')
            {
                pagerRecordCount.html("<div class='row'>" +
                                      "  <div class='col-xs-12'>" +
                                      "    <div class='norecordsfound panel panel-info'>" +
                                      "      <div class='panel-body'>" +
                                      "        <div class='row'>" +
                                      "          <div class='col-xs-12 col-lg-offset-1 col-lg-10'>" +
                                      "             <div><span class='fa fa-search fa-5x'></span></div>" +
                                      "             <div>" +
                                      "               <h2>" + HCE.Resources.NoRecordsFound.toUpperCase() + "</h2>" +
                                      "               <p>" + HCE.Resources.NoRecordsFoundText.replace("[ENTITY]", containers.Container.data("norecords-entity")) + "</p>" +
                                      "               <br />" +
                                      "             </div>" +
                                      "          </div>" +
                                      "        </div>" +
                                      "     </div>" +
                                      "   </div>" +
                                      "</div>");
            }
            else if (typeof HCE.Resources.NoRecordsFoundHTML !== 'undefined' && HCE.Resources.NoRecordsFoundHTML !== null && HCE.Resources.NoRecordsFoundHTML !== '')
                pagerRecordCount.html(HCE.Resources.NoRecordsFoundHTML);
            else
                pagerRecordCount.html('<div class="no-records-found"><span class="fa fa-info-circle fa-lg"></span> &nbsp;' + HCE.Resources.NoRecordsFound.toUpperCase() + '</h2></div>');
            pagerRecordCount.show();
        }
        else
            pagerRecordCount.hide();
        pagerList.hide();
        pagerRecsPerPage.hide();
        $("[data-type=HCPRINTDYNAMIC]").prop('disabled', true);
    }
    else {
        $("[data-type=HCPRINTDYNAMIC]").prop('disabled', false);

        pagerRecordCount.html(HCE.Resources.Page.toUpperCase() + " " + pagingJSON.Page + " " + HCE.Resources.Of.toUpperCase() + " " + pagingJSON.TotalPages + " &nbsp;<span>(" + pagingJSON.TotalRecords + " " + HCE.Resources.TotalRecords.toUpperCase() + ')</span>');

        if ((pagingJSON.TotalRecords > 0) && (pagingJSON.TotalPages <= 1)) {
            pagerList.hide();
        }
        else {
            //-- BUILD THE PAGER --
            var startPage = 1;
            var endPage;
            html = '';

            if (pagingJSON.Page >= 4) {
                html += '<li data-page="1"><a href="#">««</a><li>';
                startPage = pagingJSON.Page - 2;
            }

            endPage = startPage + 4;
            if (endPage > pagingJSON.TotalPages)
                endPage = pagingJSON.TotalPages;

            if (pagingJSON.Page > 1) {
                html += '<li data-page="' + String(pagingJSON.Page - 1) + '"><a href="#">«</a></li>';
            }

            for (var curPage = startPage; curPage <= endPage; curPage++) {
                html += '<li data-page="' + String(curPage) + '"' + (curPage == pagingJSON.Page ? ' class="active"' : '') + '><a href="#">' + String(curPage) + '</a></li>';
            }

            if (pagingJSON.Page < pagingJSON.TotalPages) {
                html += '<li data-page="' + String(pagingJSON.Page + 1) + '"><a href="#">»</a></li>';
            }

            if (endPage < pagingJSON.TotalPages) {
                html += '<li data-page="' + String(pagingJSON.TotalPages) + '"><a href="#">»»</a></li>';
            }

            pagerList.empty().append(html).show();
        }

        pagerRecsPerPageSel.val(pagingJSON.PageSize);
        pagerRecsPerPage.show();
    }

}

//--------------------------------------------------------------------------------------------------------
// FUNCTION:	EmbedDetailDevsInContainer
// PURPOSE:		EMBED THE DETAIL DIVS IN THE CONTAINER SO ANGULAR CAN PROCESS THEM CORRECTLY
//--------------------------------------------------------------------------------------------------------
HCE.DynamicDisplay.EmbedDetailDivsInContainer = function () {
    $("div[data-type=HCDYNAMICDET],div[data-type=HCDYNAMICSUB],div[data-type=HCDYNAMICOPTIONS]").each(function () {
        var contentDiv = $(this).detach();
        var target = $(this).data("container");
        var options = $("#" + target).first("[data-type=HCDYNAMICOPTIONBAR]");

        if ($(this).data("type") === "HCDYNAMICOPTIONS")
            $("#" + target).prepend(contentDiv);
        else if (options.length > 0)
            options.append(contentDiv);
        else
            $("#" + target).prepend(contentDiv);
    });
}

//----------------------------------------------------------------------------------------------------------------
// FUNCTION:    UpdatePageCurrency
// PURPOSE:     UPDATE THE CURRENCY ITEMS ON THE PAGE WHEN THE CURRENCY SELECTION IS CHANGED
//----------------------------------------------------------------------------------------------------------------
HCE.DynamicDisplay.UpdatePageCurrency = function (source) {
    var containers = HCE.DynamicDisplay.GetSearchContainers($(source));
    var currency;
    var hasCurrencyFields = false;

    if ((typeof containers !== 'undefined') && (containers !== undefined) && (containers !== null)) {
        currency = containers.Container.data("displaycurrency");
        if ((typeof currency === 'undefined') || (currency === undefined) || (currency === null) || (currency !== 'USD' && currency !== 'NAT'))
            currency = 'USD';

        containers.Container.find("span[data-type=LOCALCURRENCYGROUP]").each(function () {
            hasCurrencyFields = true;

            if (currency === 'USD') {
                $(this).find("span[data-currency=NAT]").hide();
                $(this).find("span[data-currency=USD]").show();
            }
            else {
                $(this).find("span[data-currency=NAT]").show();
                $(this).find("span[data-currency=USD]").hide();
            }
        });

        if (!hasCurrencyFields)
            containers.Container.find("[data-type=CURRENCYSEL]").hide();
        else
            containers.Container.find("[data-type=CURRENCYSEL]").show();
    }
}

//---------------------------------------------------------------------------------------------------
// FUNCTION:    executeFunctionByName
// PURPOSE:     EXECUTE A CALLBACK FUNCTION BY NAME
//---------------------------------------------------------------------------------------------------
HCE.DynamicDisplay.ExecuteFunctionByName = function (functionName, context /*, args */) {
    var args = Array.prototype.slice.call(arguments, 2);
    var namespaces = functionName.split(".");
    var func = namespaces.pop();
    for (var i = 0; i < namespaces.length; i++) {
        context = context[namespaces[i]];
    }
    return context[func].apply(context, args);
}

//-----------------------------------------------------------------------------------------------------
// FUNCTION:        HCE.DynamicDisplay.InitializePeriodSelectList
// PURPOSE:         INITIALIZE THE PERIOD SELECTION LIST
//-----------------------------------------------------------------------------------------------------
HCE.DynamicDisplay.InitializePeriodSelectList = function (fieldName, language, includeCurrentPeriod, numberOfPeriods) {
    var advancedSelect = $("select[data-type=hcadvanced][data-field=" + fieldName + "]");
    var filter = $("select[data-type=hcfilter][data-field=" + fieldName + "]");
    var currentMonth;
    var currentYear;
    var totalMonths = 0;
    var period;
    var periodText;

    switch (language.toUpperCase()) {
        case "ES":
            monthNames = 'enero_febrero_marzo_abril_mayo_junio_julio_agosto_septiembre_octubre_noviembre_diciembre'.split('_');
            break;

        case "PT":
            monthNames = 'janeiro_fevereiro_março_abril_maio_junho_julho_agosto_setembro_outubro_novembro_dezembro'.split('_');
            break;

        default:
            monthNames = 'January_February_March_April_May_June_July_August_September_October_November_December'.split('_');
            break;
    }

    advancedSelect.find('option').remove().end().append("<option value=''></option>").val("");
    filter.find('option').remove().end().append("<option value=''></option>").val("");

    currentMonth = new Date().getMonth() + 1;
    currentYear = new Date().getFullYear();
    if (!includeCurrentPeriod) {
        if (currentMonth == 1) {
            currentMonth = 12;
            currentYear = currentYear - 1
        }
        else
            currentMonth = currentMonth - 1;
    }

    while (totalMonths < numberOfPeriods) {
        period = currentYear.toString() + (currentMonth <= 9 ? "0" : "") + currentMonth.toString();
        periodText = monthNames[currentMonth - 1] + " " + currentYear.toString();

        advancedSelect.append("<option value='" + period + "'>" + periodText + "</option>");
        filter.append("<option value='" + period + "'>" + periodText + "</option>");

        if (currentMonth == 1) {
            currentMonth = 12;
            currentYear -= 1;
        }
        else
            currentMonth -= 1;

        totalMonths += 1;
    }
}

//-- INITIALIZE THE ANGULAR MODULE AND CONTROLLER --
HCE.DynamicDisplay.InitializeAngular();
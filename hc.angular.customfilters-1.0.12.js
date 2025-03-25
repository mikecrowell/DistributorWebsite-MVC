angular.module('HCE.CustomFilters', [])

//-----------------------------------------------------------------------------------------------------------------------
//-- DIRECTIVE:     convert-to_number
//-- PURPOSE:       USED TO BIND NUMERIC VALUES TO SELECT OPTIONS
//-----------------------------------------------------------------------------------------------------------------------
.directive('convertToNumber', function () {
    return {
        require: 'ngModel',
        link: function (scope, element, attrs, ngModel) {
            ngModel.$parsers.push(function (val) {
                //saves integer to model null as null
                return val == null ? null : parseInt(val, 10);
            });
            ngModel.$formatters.push(function (val) {
                //return string for formatter and null as null
                return val == null ? null : '' + val;
            });
        }
    };
})

//------------------------------------------------------------------------------------------------------------------------
//-- FILTER:    to_trusted
//-- PURPOSE:   display trusted html
//------------------------------------------------------------------------------------------------------------------------
.filter('to_trusted', ['$sce', function ($sce) {
    return function (text) {
        return $sce.trustAsHtml(text);
    };
}])

//------------------------------------------------------------------------------------------------------------------------
//-- FILTER:    hcaddress
//-- PURPOSE:   display address link
//------------------------------------------------------------------------------------------------------------------------
.filter('hcaddress', ['$sce', function ($sce) {
    return function (address) {
        return $sce.trustAsHtml(address);
    };
}])

//------------------------------------------------------------------------------------------------------------------------
//-- FILTER:  hcboolean
//-- PURPOSE: filter a boolean value to a yes/no result. Returns an empty string if the boolean value is null.
//-- PARMS:          value   The boolean value you want to convert.
//------------------------------------------------------------------------------------------------------------------------
.filter('hcboolean', ['$sce', function ($sce) {
    return function (input, trueval, falseval) {
        if ((typeof input === 'undefined') || (input === null) || (input === ''))
            return $sce.trustAsHtml("");

        if ((typeof trueval === 'undefined') || (trueval === undefined) || (trueval === null))
            trueval = 'Yes';
        else if (trueval === 'CHECKSQUARE')
            trueval = "<span class='fa fa-check-square fa-lg'></span>";
        else if (trueval.indexOf('fa ' === 0))
            trueval = "<span class='" + trueval + "'></span>";

        if ((typeof falseval === 'undefined') || (falseval === undefined) || (falseval === null))
            falseval = 'No';
        else if (falseval === 'SQUARE')
            falseval = "<span class='fa fa-square-o fa-lg'></span>";
        else if (falseval.indexOf('fa ' === 0))
            falseval = "<span class='" + falseval + "'></span>"

        switch (input) {
            case true:
                return $sce.trustAsHtml(trueval);
            case false:
                return $sce.trustAsHtml(falseval);
            default:
                return $sce.trustAsHtml("");
        }
    }
}])

//------------------------------------------------------------------------------------------------------------------------
//-- FILTER:    hcphone
//-- PURPOSE:   display address link
//------------------------------------------------------------------------------------------------------------------------
.filter('hcphone', ['$sce', function ($sce) {
    return function (phone, type, faclass, link) {
        if ((phone === null) || (phone === ''))
            return '';
        else {
            if ((typeof type === 'undefined') || (type === null) || (!/^(home|work|cell)$/.test(type)))
                type = 'home';

            if ((typeof link === 'undefined' || (link === null)))
                link = true;

            if ((typeof faclass === 'undefined') || (faclass === null) || (faclass === '')) {
                switch (type) {
                    case 'cell':
                        faclass = "fa-mobile";
                        break;

                    default:
                        faclass = "fa-phone-square";
                        break;
                }
            }

            if (link === true)
                return $sce.trustAsHtml("<a href='tel:+" + phone + "'><span class=\'fa fa-fw " + faclass + "'></span>&nbsp;" + phone + "</a>");
            else
                return $sce.trustAsHtml('<span class=\"fa fa-fw ' + faclass + '\"></span>&nbsp;' + phone);
        }
    };
}])

//------------------------------------------------------------------------------------------------------------------------
//-- FILTER:    hcemail
//-- PURPOSE:   display email link
//------------------------------------------------------------------------------------------------------------------------
.filter('hcemail', ['$sce', function ($sce) {
    return function (email) {
        if ((email === null) || (email === ''))
            return '';
        else
            return $sce.trustAsHtml("<a href='mailto:" + email + "'><span class=\'fa fa-fw fa-envelope'></span>&nbsp;" + email + "</a>");
    };
}])

//------------------------------------------------------------------------------------------------------------------------
//-- FILTER:    hcorderstatus
//-- PURPOSE:   generate file type specific icon
//------------------------------------------------------------------------------------------------------------------------
.filter('hcorderstatus', ['$sce', function ($sce) {
    return function (text) {
        var className = "label-default";

        if ((typeof text !== 'undefined') && (text !== undefined) && (text != null)) {
            switch (text.toLowerCase()) {
                case "approved":
                    className = "label-success";
                    break;

                case "denied":
                    className = "label-danger";
                    break;

                case "cancelled":
                    className = "label-warning";
                    break;
            }
        }

        return $sce.trustAsHtml("<span class=\"label " + className + "\">" + text + "</span>");
    }
}])

//------------------------------------------------------------------------------------------------------------------------
//-- FILTER:    hcblinkingindicator
//-- PURPOSE:   generate file type specific icon
//------------------------------------------------------------------------------------------------------------------------
.filter('hcblinkingindicator', ['$sce', function ($sce) {
    return function (show, type, color, faicon) {
        //-- SET THE COLOR TO DISPLAY THE ICON IN --
        if ((typeof (color) == "undefined") || (color == undefined) || (color == null) || (color == "")) {
            switch (type) {
                case "newmessage":
                    color = "#009933";
                    break;

                default:
                    color = "#000000";
                    break;
            }
        }

        //-- SET THE ICON --
        if ((typeof (faicon) == "undefined") || (faicon == undefined) || (faicon == null) || (faicon == "")) {
            switch (type) {
                case "newmessage":
                    faicon = "fa-envelope";
                    break;

                default:
                    faicon = "fa-square-share";
                    break;
            }
        }

        if (show) {
            return $sce.trustAsHtml("<i class=\"fa fa-fw " + faicon + " blink\" style=\"color: " + color + ";\"></i>");
        }
        else {
            return $sce.trustAsHtml("<i class=\"fa fa-fw\"></i>");
        }

    }
}])

//------------------------------------------------------------------------------------------------------------------------
//-- FILTER:    hcfiletypeicon
//-- PURPOSE:   generate file type specific icon
//------------------------------------------------------------------------------------------------------------------------
.filter('hcfiletypeicon', ['$sce', function ($sce) {
    return function (text) {
        var curColor = "#ff006e";
        var curClass = "fa-file-o";

        if ((typeof text == 'undefined') || (text === undefined) || (text === null))
            text = "txt";

        switch (text.toLowerCase()) {
            case "txt":
                curColor = "#8b0101";
                curClass = "fa-file-text-o";
                break;

            case "zip":
                curColor = "#caab04";
                curClass = "fa-file-zip-o";
                break;

            case "doc":
            case "docx":
                curColor = "#0026ff";
                curClass = "fa-file-word-o";
                break;

            case "wmv":
            case "avi":
            case "mpg":
            case "mp4":
                curColor = "#ff6a00";
                curClass = "fa-file-movie-o";
                break;

            case "ppt":
            case "pptx":
                curColor = "#4800ff";
                curClass = "fa-file-powerpoint-o";
                break;

            case "pdf":
                curColor = "#ff0000";
                curClass = "fa-file-pdf-o";
                break;

            case "xls":
            case "xlsx":
            case "xlsm":
            case "csv":
                curColor = "#009900";
                curClass = "fa-file-excel-o";
                break;

            case "mp3":
            case "wma":
                curColor = "#808080";
                curClass = "fa-file-audio-o";
                break;

            case "jpg":
            case "tif":
            case "tiff":
            case "jpeg":
            case "gif":
            case "psd":
            case "png":
                curColor = "#0094ff";
                curClass = "fa-file-image-o";
                break;
        }

        return $sce.trustAsHtml("<span class=\"fa " + curClass + " fa-lg\" style=\"color: " + curColor + ";\"></span>");
    }
}])

//------------------------------------------------------------------------------------------------------------------------
//-- FILTER:    hccreditcard
//-- PURPOSE:   display address link
//------------------------------------------------------------------------------------------------------------------------
.filter('hccreditcard', ['$sce', function ($sce) {
    return function (cc, includetext, textpos) {
        var faclass = "fa-credit-card";

        if ((cc === null) || (cc === '') || (typeof cc === 'undefined') || (cc === undefined))
            return '';
        else {
            switch (cc.toUpperCase()) {
                case 'VISA':
                    faclass = "fa-cc-visa";
                    break;

                case 'MASTERCARD':
                case 'ECMC':
                    faclass = "fa-cc-mastercard";
                    break;

                case 'AMEX':
                    faclass = "fa-cc-amex";
                    break;

                case 'DINERS':
                    faclass = "fa-cc-diners-club";
                    break;

                case 'JCB':
                    faclass = "fa-cc-jcb";
                    break;

                case 'DISCOVER':
                    faclass = "fa-cc-discover";
                    break;

                case 'PAYPAL':
                    faclass = "fa-cc-paypal";
                    break;

                case 'STRIPE':
                    faclass = "fa-cc-stripe";
                    break;

                default:
                    faclass = "fa-credit-card";
                    break;
            }

            if ((typeof includetext === 'undefined') || (includetext === null))
                includetext = true;

            if ((typeof textpos === 'undefined') || (textpos === null)) {
                textpos = 'AFTER';
            }

            if (includetext === true) {
                if (textpos === 'AFTER')
                    return $sce.trustAsHtml('<span class=\"fa ' + faclass + '\"></span>&nbsp;' + cc);
                else
                    return $sce.trustAsHtml(cc + '&nbsp;<span class=\"fa ' + faclass + '\"></span>');
            }
            else
                return $sce.trustAsHtml('<span class=\"fa ' + faclass + '\"></span>');
        }
    };
}])

//------------------------------------------------------------------------------------------------------------------------
//-- FILTER:	hccurrency
//-- PURPOSE:	filter a currency field
//-- PARMS:		number      - number to format
//--			currency    - currency to format the number in
//------------------------------------------------------------------------------------------------------------------------
// sample use {{ value | hccurrency:"USD" }}
.filter('hccurrency', ['$sce', function ($sce) {
    return function (value, type, currency, decimals, showCurrencyCode, colorize, posColor, negColor) {
        var localizedAmount;
        var amtColor = "";
        var currencyInfo = { Amount: 0, AmountUSD: 0, Currency: '', AmountDISP: '', AmountUSDDISP: '', IsNegative: false, DisplayUSD: false, DisplayNAT: false, DisplayBoth: false };

        //-- PARSE THE XML LOCALIZED NUMBER STRUCTURE - DEFAULT TO 0 IF INVALID STRUCTURE IS PASSED IN --
        try {
            localizedAmount = $.parseJSON(value);
        } catch (e) {
            localizedAmount = { RATE: 0, USD: 0, CUR: 'NAT', AMT: 0, RATE: 0 };
        }

        //-- VALIDATE THE TYPE PARAMETER --
        if (!/^(rate|usd|amount|both)$/.test(type))
            return "invalid type";
        //if ((type === "both") && (localizedAmount.CUR === "USD"))
        //    type = "usd";

        //-- SKIP ALL OTHER PROCESSING IF THE USER JUST WANTS THE EXCHANGE RATE --
        if (type === "rate")
            return parseFloat(localizedAmount.RATE).toFixed(decimals);

        //-- VALIDATE THE CURRENCY PARAMETER --
        if (!/^(USD|NAT)$/.test(currency))
            currency = 'NAT';           //-- DEFAULT TO NATIVE CURRENCY --

        //-- VALIDATE THE DECIMALS PARAMETER --
        if ((typeof decimals === 'undefined') || (decimals == 'DEFAULT'))
            decimals = 2;

        //-- HANDLE MXP - MXN CONVERSION --
        if (localizedAmount.CUR == 'MXP') localizedAmount.CUR = 'MXN';

        //-- PARSE THE showCurrencyCode PARAMETER --
        if (typeof showCurrencyCode === 'undefined')
            showCurrencyCode = false;

        //-- SET THE STARTING OBJECT VALUES --
        currencyInfo.Amount = localizedAmount.AMT;
        currencyInfo.AmountUSD = localizedAmount.USD;

        //-- HANDLE THE NEGATIVE NUMBER FORMATTING --
        if (currencyInfo.Amount < 0) {
            currencyInfo.IsNegative = true;
            currencyInfo.Amount = currencyInfo.Amount * -1;
            currencyInfo.AmountUSD = currencyInfo.AmountUSD * -1;
        }

        //-- PARSE THE colorize PARAMETER --
        if (typeof colorize === 'undefined')
            colorize = false;
        else if (colorize) {
            if (currencyInfo.IsNegative)
                amtColor = "color:" + (typeof negColor != 'undefined' && negColor != '' ? '#' + negColor : '#990000') + ";";
            else if (currencyInfo.Amount > 0)
                amtColor = "color:" + (typeof posColor != 'undefined' && posColor != '' ? '#' + posColor : '#009900') + ";";
        }

        //-- DETERMINE WHICH AMOUNTS TO DISPLAY --
        if (type === "both") {
            currencyInfo.DisplayUSD = true;
            currencyInfo.DisplayNAT = true;
            currencyInfo.DisplayBoth = true;
        }
        else if (type === "usd") {
            currency = "USD";
            currencyInfo.DisplayUSD = true;
        }
        else if (type === "amount")
            currencyInfo.DisplayNAT = true;

        //-- CREATE THE OPENING SPAN TAG --
        currencyInfo.AmountDISP = "<span data-currency=\"NAT\" style=\"" + (currency === "USD" ? "display:none;" : "") + amtColor + "\">";
        currencyInfo.AmountUSDDISP = "<span data-currency=\"USD\" style=\"" + (currency !== "USD" ? "display:none;" : "") + amtColor + "\">";

        //-- CREATE THE USD FORMATTED NUMBER --
        currencyInfo.AmountUSDDISP = currencyInfo.AmountUSDDISP + (currencyInfo.IsNegative ? "(" : "") + "$" + accounting.formatNumber(currencyInfo.AmountUSD, decimals, ",", ".") + (currencyInfo.IsNegative ? ")" : "") + (showCurrencyCode ? "<span class='currencycode'>&nbsp;USD</span>" : "");

        switch (localizedAmount.CUR) {
            case "USD":
                currencyInfo.AmountDISP = currencyInfo.AmountDISP + (currencyInfo.IsNegative ? "(" : "") + "$" + accounting.formatNumber(currencyInfo.Amount, decimals, ",", ".") + (currencyInfo.IsNegative ? ")" : "") + (showCurrencyCode ? "<span class='currencycode'>&nbsp;USD</span>" : "");
                break;

            case "CAD":
                currencyInfo.AmountDISP = currencyInfo.AmountDISP + (currencyInfo.IsNegative ? "-" : "") + "$" + accounting.formatNumber(currencyInfo.Amount, decimals, ",", ".") + (showCurrencyCode ? "<span class='currencycode'>&nbsp;" + localizedAmount.CUR + "</span>" : "");
                break;

            case "BRL":
                currencyInfo.AmountDISP = currencyInfo.AmountDISP + (currencyInfo.IsNegative ? "-" : "") + "R$&nbsp;" + accounting.formatNumber(currencyInfo.Amount, decimals, ".", ",");
                break;

            case "MXN":
                currencyInfo.AmountDISP = currencyInfo.AmountDISP + (currencyInfo.IsNegative ? "-" : "") + "$" + accounting.formatNumber(currencyInfo.Amount, decimals, ",", ".") + (showCurrencyCode ? "<span class='currencycode'>&nbsp;" + localizedAmount.CUR + "</span>" : "");
                break;

            case "CLP":
                currencyInfo.AmountDISP = currencyInfo.AmountDISP + (currencyInfo.IsNegative ? "-" : "") + "$&nbsp;" + accounting.formatNumber(currencyInfo.Amount, decimals, ".", ",") + (showCurrencyCode ? "<span class='currencycode'>&nbsp;" + localizedAmount.CUR + "</span>" : "");
                break;

            case "ARS":
                currencyInfo.AmountDISP = currencyInfo.AmountDISP + "$" + (currencyInfo.IsNegative ? "-" : "&nbsp;") + accounting.formatNumber(currencyInfo.Amount, decimals, ".", ",") + (showCurrencyCode ? "<span class='currencycode'>&nbsp;" + localizedAmount.CUR + "</span>" : "");
                break;

            case "DOP":
                currencyInfo.AmountDISP = currencyInfo.AmountDISP + (currencyInfo.IsNegative ? "(" : "") + "RD$" + accounting.formatNumber(currencyInfo.Amount, decimals, ",", ".") + (currencyInfo.IsNegative ? ")" : "");
                break;

            case "PEN":
                currencyInfo.AmountDISP = currencyInfo.AmountDISP + "S/.&nbsp;" + (currencyInfo.IsNegative ? "-" : "") + accounting.formatNumber(currencyInfo.Amount, decimals, ",", ".");
                break;

            case "COP":
                currencyInfo.AmountDISP = currencyInfo.AmountDISP + (currencyInfo.IsNegative ? "(" : "") + "$&nbsp;" + accounting.formatNumber(currencyInfo.Amount, decimals, ".", ",") + (currencyInfo.IsNegative ? ")" : "") + (showCurrencyCode ? "<span class='currencycode'>&nbsp;" + localizedAmount.CUR + "</span>" : "");
                break;

            case "PHP":
                currencyInfo.AmountDISP = currencyInfo.AmountDISP + (currencyInfo.IsNegative ? "(" : "") + "Php" + accounting.formatNumber(currencyInfo.Amount, decimals, ",", ".") + (currencyInfo.IsNegative ? ")" : "");
                break;

            default:
                currencyInfo.AmountDISP = currencyInfo.AmountDISP + "UNHANDLED CURRENCY: " + localizedAmount.CUR;
                break;
        }

        //-- CLOSE THE NUMERIC SPAN TAG --
        currencyInfo.AmountDISP = currencyInfo.AmountDISP + "</span>";
        currencyInfo.AmountUSDDISP = currencyInfo.AmountUSDDISP + "</span>";

        if (currencyInfo.DisplayBoth === true) {
            return $sce.trustAsHtml("<span data-type='LOCALCURRENCYGROUP'>" +
                                      currencyInfo.AmountDISP +
                                      currencyInfo.AmountUSDDISP +
                                    "</span>");
        }
        else {
            return $sce.trustAsHtml(currencyInfo.AmountDISP);
        }
    }
}])

//------------------------------------------------------------------------------------------------------------------------
//-- FILTER:	hcdate
//-- PURPOSE:	filter a date field using the current datetime format (from the HCE.AngularFormats.Date format string)
//-- PARMS:		includeTZ	- true to use the current user timezone when formatting the date
//--				showTZ      - true to show the current user timezone when formatting the date
//------------------------------------------------------------------------------------------------------------------------
.filter('hcdate', ['$filter',function ($filter) {
    var angularDateFilter = $filter('date');
    return function (input, includeTZ, showTZ) {
        var formatString = HCE.AngularFormats.Date;

        if (showTZ) {
            formatString = formatString + " '" + HCE.TimeZone + "'";
        }

        if (includeTZ)
            return angularDateFilter(input, formatString, HCE.TimeZone);
        else
            return angularDateFilter(input, formatString);
    }
}])

//------------------------------------------------------------------------------------------------------------------------
//-- FILTER:	hctime
//-- PURPOSE:	filter a date field using the current datetime format (from the HCE.AngularFormats.Date format string)
//-- PARMS:		includeTZ	- true to use the current user timezone when formatting the date
//--				showTZ      - true to show the current user timezone when formatting the date
//------------------------------------------------------------------------------------------------------------------------
.filter('hctime', ['$filter', function ($filter) {
    var angularDateFilter = $filter('date');
    return function (input, includeTZ, showTZ) {
        var formatString = HCE.AngularFormats.Time;

        if (showTZ) {
            formatString = formatString + " '" + HCE.TimeZone + "'";
        }

        if (includeTZ)
            return angularDateFilter(input, formatString, HCE.TimeZone);
        else
            return angularDateFilter(input, formatString);
    }
}])

//------------------------------------------------------------------------------------------------------------------------
//-- FILTER:	hcdatetime
//-- PURPOSE:	filter a date/time field using the current datetime format (from the HCE.AngularFormats.Date format string)
//-- PARMS:		includeTZ	- true to use the current user timezone when formatting the date
//--				showTZ      - true to show the current user timezone when formatting the date
//------------------------------------------------------------------------------------------------------------------------
.filter('hcdatetime', ['$filter',function ($filter) {
    var angularDateFilter = $filter('date');
    return function (input, includeTZ, showTZ) {
        var formatString = HCE.AngularFormats.DateTime;

        if (showTZ) {
            formatString = formatString + " '" + HCE.TimeZone + "'";
        }

        if (includeTZ)
            return angularDateFilter(input, formatString, HCE.TimeZone);
        else
            return angularDateFilter(input, formatString);
    }
    }])

    //------------------------------------------------------------------------------------------------------------------------
    //-- FILTER:	hcperiod
    //-- PURPOSE:	filter a period
    //-- PARMS:		includeTZ	- true to use the current user timezone when formatting the date
    //--				showTZ      - true to show the current user timezone when formatting the date
    //------------------------------------------------------------------------------------------------------------------------
    .filter('hcperiod', ['$filter', function ($filter) {
        var angularDateFilter = $filter('date');
        return function (input, language) {
            var formatString = "MMMM yyyy";
            var strPeriod = new String(input);
            var month = parseInt(strPeriod.substring(4, 6));
            var year = strPeriod.substring(0, 4);
            var monthNames;

            switch (language) {
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

            return (monthNames[month - 1] + ' ' + year);
        }
    }]);
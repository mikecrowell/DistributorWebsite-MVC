var lastMQ;
var menuVisible;
var menuLeft;
var isExpanded;
var canMenuCollapse;

$(function () {

	lastMQ = getMQ();
	FormatMenu(lastMQ);

	$(document).keyup(function(e) {
		  if ((e.keyCode == 27) && (isExpanded) && (canMenuCollapse)) 
		  { 
				// escape key maps to keycode `27`
				FormatMenu(lastMQ);
		  }
	});

	$(window).resize(function(event){
				
		//-- DETERMINE WHICH MEDIA QUERY IS ACTIVE --
		var mq = getMQ();

		if (mq != lastMQ)
		{
			lastMQ = mq;
			FormatMenu(lastMQ);
		}
	});

	$(".HC-side-nav").on("click", function(event){
		var canCollapse = $(this).data('cancollapse') == true;
		ToggleNav(lastMQ, canCollapse);
	});

	//-- INITILIZE ANY TAB CONTROLS --
	InitializeTabs();

	//----------------------------------------------------------------------------
	// FUNCTION:	ShowHideNav
	// PURPOSE:		SHOW THE NAV IF IT IS COLLAPSED OR HIDE IT IF EXPANDED
	//----------------------------------------------------------------------------
	function ToggleNav(mq, canCollapse)
	{
		var navDiv = $('.HC-container-left');

		if (!isExpanded)
		{
			switch (mq)
			{
				case 'xs':
					navDiv.animate({opacity: 0.0}, 0, function(){}).animate({left: '0px', opacity: 1.0}, 300, function(){});		
					navDiv.addClass('in');
					isExpanded = true;
					break;

				case 'sm':
					navDiv.animate({left: '0px', opacity: 1.0}, 300, function(){});
					navDiv.addClass('in');
					$('#HIDESIDENAV').show(300, function(){});
					isExpanded = true;
					break;
			}
		}
		else if (canCollapse)
		{
			FormatMenu(mq);
		}
	}

	//----------------------------------------------------------------------------
	// FUNCTION:	FormatMenu
	// PURPOSE:		FORMAT THE SIDE BAR NAVIGATION MENU FOR THE CURRENT SCREEN SIZE 
	//----------------------------------------------------------------------------
	function FormatMenu(mq)
	{
		var navDiv = $('.HC-container-left');
		$('#HIDESIDENAV').hide();

		var selList = navDiv.find(".collapse.in.default");

		switch (mq)
		{
			case 'lg':
			case 'md':
				//-- JUST COLLAPSE ANY EXPANDED PANELS --
				navDiv.find('.collapse.in:not(.default)').collapse('hide');
				navDiv.removeClass('in');
				menuLeft = 0;
				isExpanded = true;
				canMenuCollapse = false;
				break;

			case 'sm':
				navDiv.animate({left: '-205px', opacity: 1.0}, 300, function(){});
				navDiv.find('.collapse.in').collapse('hide');
				navDiv.removeClass('in');
				menuLeft = -205;
				isExpanded = false;
				canMenuCollapse = true;
				break;

			case 'xs':
				navDiv.animate({left: '-250px', opacity: 1.0}, 300, function(){});
				navDiv.find('.collapse.in').collapse('hide');
				navDiv.removeClass('in');
				menuLeft = -250;
				isExpanded = false;
				canMenuCollapse = true;
				break;
		}
	}

	//---------------------------------------------------
	// FUNCTION:	getMQ
	// PURPOSE:		GET THE CURRENT ACTIVE MEDIA QUERY
	//---------------------------------------------------
	function getMQ()
	{
		var mq;

		if ($('#HCLG').is(':visible'))
			mq = 'lg';
		else if ($('#HCMD').is(':visible'))
			mq = 'md';
		else if ($('#HCSM').is(':visible'))
			mq = 'sm';
		else if ($('#HCXS').is(':visible'))
			mq = 'xs';

		return(mq);
	}

	//-----------------------------------------------------
	// FUNCTION:	InitializeTabs
	// PURPOSE:		Initialize any tab controls
	//-----------------------------------------------------
	function InitializeTabs()
	{
		$('ul.nav-tabs.collapsible').each(function(){
			var tabList = $(this);
			var tabID = tabList.attr("id");
			var selID = 'tabSEL' + tabID;

			tabList.data("tab-sel",selID);
			tabList.removeClass("HC-hidden-xs").addClass("HC-hidden-xs");
			$(this).find("a:first").removeClass("HC-visible-xs").addClass("HC-visible-xs");		//-- KEEP THIS FIRST TAB VISIBLE IN EXTRA SMALL MODE --

			var selHTML = '<select id="' + selID + '" data-tab-ul="' + tabID + '" class="form-control input-lg tab-sel-list HC-visible-xs hidden-print" style="max-width: 100%; margin-bottom: 8px;">';

			tabList.find("li[role=presentation] a").each(function(){
				selHTML = selHTML + '<option href="' + $(this).attr("href") + '" role="tabsel">' + $(this).html() + '</option>';
			});

			selHTML = selHTML + '</select>';

			tabList.before(selHTML);
		});

		$("select.tab-sel-list").on("change",function(){

			//-- KEEP THE MAIN TAB IN SYNC --
		    var tabList = $("#" + $(this).data("tab-ul"));
		    var tabPage = tabList.find("a[href='" + $(this).find(':selected').attr('href') + "']")
		    tabPage.tab('show');

		});

		$('a[data-toggle="tab"]').on('shown.bs.tab', function (e) {
			var selList = $('#' + $(e.target).closest('ul.nav-tabs').data("tab-sel"));
			selList.val($(e.target).html());
		})
	}

});		
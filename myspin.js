/*

You can now create a spinner using any of the variants below:

$("#el").spin(); // Produces default Spinner using the text color of #el.
$("#el").spin("small"); // Produces a 'small' Spinner using the text color of #el.
$("#el").spin("large", "white"); // Produces a 'large' Spinner in white (or any valid CSS color).
$("#el").spin({ ... }); // Produces a Spinner using your custom settings.

$("#el").spin(false); // Kills the spinner.

*/
(function($) {
	$.fn.spin = function(opts, color) {
		var presets = {
			"tableheader": { lines: 8, length: 2, width: 2, radius: 3, color: '#FFF' },
			"small": { lines: 8, length: 4, width: 3, radius: 5 },
			"large": { lines: 10, length: 8, width: 4, radius: 8 },
			"modal": { lines: 11, length: 23, width: 8, radius: 40, corners: 1, rotate: 9, color: '#FFF', speed: 1, trail: 50, shadow: true, hwaccel: false}
		};
		if (Spinner) {
			return this.each(function() {
				var $this = $(this),
					data = $this.data();
				
				if (data.spinner) {
					data.spinner.stop();
					delete data.spinner;
					if ($(this).data('type') == 'modalspinner')
						$("#spin_modal_overlay").remove();
				}

				if (opts !== false) {
					if (typeof opts === "string") {
						if (opts in presets) {
							opts = presets[opts];
						} else {
							opts = {};
						}
						if (color) {
							opts.color = color;
						}

						var spinElem = this;
						if (opts == presets["modal"])
						{
							$('body').append('<div id="spin_modal_overlay" style="background-color: rgba(0, 0, 0, 0.6); width:100%; height:100%; position:fixed; top:0px; left:0px; z-index:999999;"/>');
							spinElem = $("#spin_modal_overlay")[0];
						}

						data.spinner = new Spinner($.extend({color: $this.css('color')}, opts)).spin(spinElem);
					}
				}
			});
		} else {
			throw "Spinner class not available.";
		}
	};
})(jQuery);
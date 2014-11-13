/* global NAP: false */

"use strict";

var Log = (function ($) {

	return {
		grid: null,
		buttonImage: null,
		form: null,

		InitDateTimePicker: function () {
			$("#From").datepicker({
				defaultDate: "+1w",
				changeMonth: true,
				numberOfMonths: 1,
				dateFormat: "M dd,yy",
				showOn: "button",
				buttonImage: Log.buttonImage,
				buttonText: "From",
				buttonImageOnly: true
			});

			$("#To").datepicker({
				defaultDate: "+1w",
				changeMonth: true,
				numberOfMonths: 1,
				dateFormat: "M dd,yy",
				showOn: "button",
				buttonImage: Log.buttonImage,
				buttonText: "To",
				buttonImageOnly: true
			});
		}
	};

} (jQuery));

$(document).ready(function () {
	Log.grid = $('.grid');
	Log.buttonImage = $('#imagePath').val();
	Log.form = $('form:first');
	Log.InitDateTimePicker();

	$('.loginsert').click(function () {
		var logId = $(this).attr('logid');

		$.ajax({
			url: NAP.resolveClientUrl("ConnectionString/LogDetail/" + logId),
			type: 'POST',
			contentType: 'application/json',
			dataType: 'html',
			cache: false,
			beforeSend: function () {
				NAP.loadingBox.show();
			},
			complete: function () {
				NAP.loadingBox.close();
			},
			success: function (result) {
				if (result.length > 0) {
					NAP.popup("Log detail", result);
				}
			},
			error: function (xhr, status, err) {
				NAP.handleAjaxError(xhr, status, err);
			}
		});
	});

	// shows loading on change page
	Log.form.submit(function () {
		NAP.loadingBox.show();
	});

	// changes items/page
	Log.grid.on('change', '.items-per-page', function () {
		var $this = $(this),
			pageSize = $this.val(),
			username = $('#Username').val(),
			from = $('#From').val(),
			to = $('#To').val(),
			name = $('#Name').val(),
			filter = $('#FilterType').val();

		var url = NAP.resolveClientUrl('ConnectionString/Log' +
			'?name=' + name +
			'&username=' + username +
			'&from=' + from +
			'&to=' + to +
			'&FilterType=' + filter +
			'&pageSize=' + pageSize);

		NAP.loadingBox.show();
		window.location.replace(url);
	});
});

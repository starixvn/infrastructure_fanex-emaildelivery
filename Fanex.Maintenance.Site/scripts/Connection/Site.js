/* global NAP: false */

"use strict";

var Site = (function ($) {

	return {
		grid: null,

		UpdateStatusSite: function (siteId, isActive) {
			$.ajax({
				url: NAP.resolveClientUrl("ConnectionString/UpdateStatusSite"),
				data: { siteId: siteId, isActive: isActive },
				type: 'POST',
				cache: false,
				beforeSend: function () {
					NAP.loadingBox.show();
				},
				complete: function () {
					NAP.loadingBox.close();
				},
				success: function (result) {
					if (result === 'true') {
						NAP.headlineMsgBox.showInfo('Your changes were saved!', { closeBtn: false });
						NAP.redirect('ConnectionString/Site');
					}
				},
				error: function (xhr, status, err) {
					NAP.handleAjaxError(xhr, status, err);
				}
			});
		},

		GetConnection: function (siteId) {
			$.ajax({
				url: NAP.resolveClientUrl("ConnectionString/GetConnectionBySite"),
				data: { siteId: siteId },
				type: 'GET',
				dataType: 'HTML',
				cache: false,
				beforeSend: function () {
					NAP.loadingBox.show();
				},
				complete: function () {
					NAP.loadingBox.close();
				},
				success: function (result) {
					NAP.popup("Details", result);
				},
				error: function (xhr, status, err) {
					NAP.handleAjaxError(xhr, status, err);
				}
			});
		}
	};

} (jQuery));

$(document).ready(function () {
	Site.grid = $('.grid');

	Site.grid.on('click', 'input[type=checkbox][sid]', function () {
		var $this = $(this);
		var siteId = $this.attr('sid');
		var siteName = $this.attr('sname');
		var active = $this.is(':checked'),
			action = active ? 'activate' : 'deactivate',
			msg = "Are you sure you want to " + action + " '" + siteName + "' ?";

		$this.attr('checked', !active).change();

		NAP.headlineMsgBox.confirm(msg, function () {
			$this.attr('checked', active).change();
			Site.UpdateStatusSite(siteId, active);
		});
	});

	$('#btnSave').click(function () {
		NAP.headlineMsgBox.confirm("This change will effect all application! Are you sure?", function () {
			document.forms[0].submit();
		});
	});

	$('.connection').click(function () {
		var $this = $(this);
		var siteId = $this.attr('sid');
		Site.GetConnection(siteId);
	});
});

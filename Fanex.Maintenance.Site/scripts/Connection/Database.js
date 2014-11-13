/* global NAP: false */

"use strict";

var Database = (function ($) {

	return {
		grid: null,

		UpdateStatusDatabase: function (databaseId, isActive) {
			$.ajax({
				url: NAP.resolveClientUrl("ConnectionString/UpdateStatusDatabaseServer"),
				data: { databaseServerId: databaseId, isActive: isActive },
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
						NAP.redirect('ConnectionString/DatabaseServer');
					}
				},
				error: function (xhr, status, err) {
					NAP.handleAjaxError(xhr, status, err);
				}
			});
		}
	};

} (jQuery));

$(document).ready(function () {
	Database.grid = $('.grid');

	Database.grid.on('click', 'input[type=checkbox][did]', function () {
		var $this = $(this);
		var databaseId = $this.attr('did');
		var name = $this.attr('dname');
		var active = $this.is(':checked'),
			action = active ? 'activate' : 'deactivate',
			msg = "Are you sure you want to " + action + " '" + name + "' ?";

		$this.attr('checked', !active).change();

		NAP.headlineMsgBox.confirm(msg, function () {
			$this.attr('checked', active).change();
			Database.UpdateStatusDatabase(databaseId, active);
		});
	});

	$('#btnSave').click(function () {
		NAP.headlineMsgBox.confirm("This change will effect all application! Are you sure?", function () {
			document.forms[0].submit();
		});
	});
});

/* global NAP: false */

"use strict";

var AliasDatabase = (function ($) {

	return {
		grid: null,

		UpdateStatusAliasDatabase: function (id, isActive) {
			$.ajax({
				url: NAP.resolveClientUrl("ConnectionString/UpdateStatusDatabase"),
				data: { databaseId: id, isActive: isActive },
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
						NAP.redirect('ConnectionString/Database');
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
	AliasDatabase.grid = $('.grid');

	AliasDatabase.grid.on('click', 'input[type=checkbox][did]', function () {
		var $this = $(this);
		var id = $this.attr('did');
		var name = $this.attr('dname');
		var active = $this.is(':checked'),
			action = active ? 'activate' : 'deactivate',
			msg = "Are you sure you want to " + action + " '" + name + "' ?";

		$this.attr('checked', !active).change();

		NAP.headlineMsgBox.confirm(msg, function () {
			$this.attr('checked', active).change();

			AliasDatabase.UpdateStatusAliasDatabase(id, active);
		});
	});

	$('#btnSave').click(function () {
		NAP.headlineMsgBox.confirm("This change will effect all application! Are you sure?", function () {
			document.forms[0].submit();
		});
	});
});

/* global NAP: false */

"use strict";

var ChangeConnection = (function ($) {

	return {
		grid: null,
		changeConnectionGrid: null,

		CheckOrUncheckAll: function (isCheck) {
			ChangeConnection.grid.find('input[type=checkbox]').each(function () {
				var $this = $(this);
				if (isCheck) {
					$this.attr('checked', 'checked');
				}
				else {
					$this.removeAttr('checked');
				}
			});
		},

		ChangeConnectionFromDbServer: function (alaisConnectionId) {
			$.ajax({
				url: NAP.resolveClientUrl("ConnectionString/ChangeConnectionFromDbServerToDbServer"),
				data: { aliasConnection: alaisConnectionId },
				type: 'POST',
				cache: false,
				beforeSend: function () {
					NAP.loadingBox.show();
				},
				complete: function () {
					NAP.loadingBox.close();
				},
				success: function (result) {
					if (result) {
						NAP.headlineMsgBox.showInfo('Your changes were saved!', { closeBtn: false });
						setTimeout(NAP.redirect('ConnectionString/Alias'), 2000);
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
	ChangeConnection.grid = $('.grid');
	ChangeConnection.changeConnectionGrid = $('#change-connection');

	$('#all').click(function () {
		var isCheck = $(this).attr('checked');
		if (isCheck === "checked") {
			ChangeConnection.CheckOrUncheckAll(true);
		}
		else {
			ChangeConnection.CheckOrUncheckAll(false);
		}
	});

	ChangeConnection.changeConnectionGrid.find('.form input[type=submit]').click(function () {
		var listAliasChecked = ChangeConnection.grid.find('input[type=checkbox][aid]:checked');

		if (listAliasChecked.length > 0) {

			var strAliasConnectionId = "";
			listAliasChecked.each(function () {
				var aliasName = $(this).attr('aname');
				var aliasId = $(this).attr('aid');

				var connectionChecked = ChangeConnection.grid.find('input[type=radio][name="' + aliasName + '"]:checked');
				var connectionId = connectionChecked.attr('cid');

				strAliasConnectionId += aliasId + ',' + connectionId + ';';
			});

			if (strAliasConnectionId.length > 0) {
				ChangeConnection.ChangeConnectionFromDbServer(strAliasConnectionId);
			}

		}
		else {
			NAP.headlineMsgBox.showInfo('Select at least 1 alias!', { closeBtn: false });
		}

	});

});

"use strict";
/* jshint camelcase: false,
unused: vars */
var sharedScriptManager;
var SharedScriptManager = function () {
    var self = this;
    self.cachedData = {
        pagingOptions: [50, 100, 200, 500],
        defaultItemsPerPage: 50,
        alertSuccessTime: 3000
    };

    self.callAjax = function (currentHandler,
        url,
        requestParams,
        successFunction,
        errorFunction,
        completeFunction) {
        $.ajax({
            type: "POST",
            url: url,
            contentType: "application/json; charset=utf-8",
            dataType: "json",
            data: requestParams ? JSON.stringify(requestParams) : null,
            beforeSend: function () {
            },
            success: function (rs) {
                if (successFunction && typeof (successFunction) === 'function') {
                    successFunction.call(currentHandler, rs);
                }
            },
            error: function (jqXHR, event, ajaxSettings) {
                if (errorFunction && typeof (errorFunction) === 'function') {
                    errorFunction.call(currentHandler);
                }
            },
            complete: function () {
                if (completeFunction && typeof (completeFunction) === 'function') {
                    completeFunction();
                }
            }
        });

    };

    self.blockUI = function () {
        NAP.loadingBox.show();
    };
    self.unblockUI = function () {
        NAP.loadingBox.close();
    };
    self.renderActionResult = function (result, successFunction) {
        var options = {
            closeBtn: !result.Code,
            sticky: result.Code
        };

        sharedScriptManager.unblockUI();
        if (result.Code === 0) {
            NAP.headlineMsgBox.showInfo(result.Message, options);
            if (successFunction) {
                successFunction();
            }

            return;
        }

        NAP.headlineMsgBox.showErr(result.Message, options);
    };
};

sharedScriptManager = new SharedScriptManager();
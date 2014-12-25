$(document).ready(function () {
    editRecipientController.initPage();
});

var EditRecipientController = function () {
    var self = this;
    self.initPage = function () {
        self.bindEvents();
    };
    self.bindEvents = function () {
        $("#button-cancel").click(function () {
            NAP.headlineMsgBox.confirm(viewResources.Confirm_CancelEditRecipient, function () {
                NAP.redirect('MailStation/MailRecipient/Index', 0);
            });
        });

        $("#button-save").click(function () {
            self.saveRecipient();
        });
    };

    self.autoSuggestMailOption = function (element) {
        return {
            serviceUrl: window.rootUrl + 'MailStation/MailManage/SuggestMail',
            minChars: 2,
            isCacheSuggestion: false,
            zIndex: 9999,
            width: 185,
            deferRequestBy: 0,
            params: {
                mailName: element.val()
            },

            closeSuggestion: null
        };
    };

    self.saveRecipient = function () {
        NAP.headlineMsgBox.confirm(viewResources.Confirm_EditRecipient, function () {
            sharedScriptManager.blockUI();
            self.hideError();
            var recipientInfo = self.getRecipientInfo();
            var errorList = self.validateRecipientInfo(recipientInfo);
            if (errorList.length === 0) {
                self.hideError();
                sharedScriptManager.callAjax(self,
                window.rootUrl + "MailStation/MailRecipient/UpdateRecipient",
                    recipientInfo,
                    self.renderActionResult,
                    null,
                    null);
            } else {
                self.showError(errorList);
                sharedScriptManager.unblockUI();
            }
        });
    };

    self.getRecipientInfo = function () {
        var recipient = {
            RecipientId: viewResources.RecipientId,
            Username: $("#RecipientName").val(),
            Email1: $("#Email1").val(),
            Email2: $("#Email2").val(),
            Email3: $("#Email3").val(),
            Note: $("#Note").val(),
            Status: $("#IsActive").is(':checked')
        };

        return recipient;
    };
    self.validateRecipientInfo = function (recipientInfo) {
        var errorList = [];
        if (recipientInfo.Email1 !== '' && !sharedScriptManager.isValidEmail(recipientInfo.Email1)) {
            errorList.push({
                Id: "#Email1",
                Message: "Email 1 is invalid!"
            });
        }

        if (recipientInfo.Email2 !== '' && !sharedScriptManager.isValidEmail(recipientInfo.Email2)) {
            errorList.push({
                Id: "#Email2",
                Message: "Email 2 is invalid!"
            });
        }
        if (recipientInfo.Email3 !== '' && !sharedScriptManager.isValidEmail(recipientInfo.Email3)) {
            errorList.push({
                Id: "#Email3",
                Message: "Email 3 is invalid!"
            });
        }

        return errorList;
    };

    self.hideError = function () {
        $("#validation-summary-row").addClass("display-none");
        $(".validation-summary-errors-container").removeClass("validation-summary-errors-container");
    };

    self.showError = function (errorList) {
        var errorLength = errorList.length;
        var errorMessage = '';
        for (var i = 0; i < errorLength; i++) {
            var error = errorList[i];
            errorMessage += '<li>' + error.Message + '</li>';
            $(error.Id).addClass("validation-summary-errors-container");
        }

        var validationRow = $("#validation-summary-row");
        validationRow.find(".validation-summary")
            .addClass("validation-summary-errors")
            .empty()
            .append('<ul>' + errorMessage + '</ul>');
        validationRow.removeClass("display-none");
    };
    self.renderActionResult = function (result) {
        var result = {
            Code: 0,
            Message: 'aaa'
        };
        sharedScriptManager.renderActionResult(result, null);
    };
};

var editRecipientController = new EditRecipientController();

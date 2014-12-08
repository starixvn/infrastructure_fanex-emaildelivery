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

        $("#username").blur(function () {
            self.checkRecipientInfo();
        });
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
                window.rootUrl + "MailStation/MailRecipient/EditRecipient",
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
            Username: $("#username").val(),
            Email1: $("#email1").val(),
            Email2: $("#email2").val(),
            Email3: $("#email3").val(),
            Note: $("#note").val(),
            Status: $("#status").attr("checked") ? 1 : 0
        };

        return recipient;
    };
    self.validateRecipientInfo = function (recipientInfo) {
        var errorList = [];
        if (recipientInfo.Email1 !== '' && !self.isValidEmail(recipientInfo.Email1)) {
            errorList.push({
                Id: "#email1",
                Message: "Email 1 is invalid!"
            });
        }

        if (recipientInfo.Email2 !== '' && !self.isValidEmail(recipientInfo.Email2)) {
            errorList.push({
                Id: "#email2",
                Message: "Email 2 is invalid!"
            });
        }
        if (recipientInfo.Email3 !== '' && !self.isValidEmail(recipientInfo.Email3)) {
            errorList.push({
                Id: "#email3",
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
    self.isValidEmail = function (email) {
        var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    };
    self.checkRecipientInfo = function () {
        var checkParams = {
            Username: $("#username").val()
        };

        if (checkParams.Username !== '') {
            $("#btn-loading").removeClass("display-none");
            self.hideError();
            sharedScriptManager.callAjax(self,
            window.rootUrl + "MailStation/MailRecipient/CheckRecipient",
            checkParams,
            self.renderCheckRecipientInfoResult,
            null,
            null);
        }
    };
    self.renderCheckRecipientInfoResult = function (result) {
        $("#btn-loading").addClass("display-none");
        self.copyRecipientInfo(result);
    };
    self.copyRecipientInfo = function (result) {
        result = {
            Id: 1,
            Username: 'sa',
            IsNapUser: true,
            Email1: "starifix@nexcel.vn",
            Email2: '',
            EMail3: '',
            NoRelevantMail: 3,
            Active: true
        };

        $("#email1").val(result.Email1);
        $("#email2").val(result.Email2);
        $("#email3").val(result.Email3);
        $("#note").val(result.Note);
        $("#status").attr("checked", result.Active ? "checked" : "");
    };
};

var editRecipientController = new EditRecipientController();

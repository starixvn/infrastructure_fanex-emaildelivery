$(document).ready(function () {
    createRecipientController.initPage();
});

var CreateRecipientController = function () {
    var self = this;

    var DOMElements = {
        UserName: null,
        Email1: null,
        Email2: null,
        Email3: null,
        Note: null,
        Status: null,
        BtnSave: null,
        BtnCancel: null,
        BtnLoading: null
    };

    self.initPage = function () {
        self.cachedDOMElements();
        self.bindEvents();
    };

    self.cachedDOMElements = function () {
        DOMElements.UserName = $('#UserName');
        DOMElements.Email1 = $('#Email1');
        DOMElements.Email2 = $('#Email2');
        DOMElements.Email3 = $('#Email3');
        DOMElements.Note = $('#Note');
        DOMElements.Status = $('#Status');
        DOMElements.BtnSave = $('#button-save');
        DOMElements.BtnCancel = $('#button-cancel');
        DOMElements.BtnLoading = $('#btn-loading');
    }

    self.bindEvents = function () {
        DOMElements.BtnCancel.click(function () {
            NAP.headlineMsgBox.confirm(viewResources.Confirm_CancelInsertRecipient, function () {
                NAP.redirect('MailStation/MailRecipient/Index', 0);
            });
        });

        DOMElements.BtnSave.click(function () {
            self.saveRecipient();
        });
        DOMElements.UserName.blur(function () {
            self.checkRecipientInfo();
        });
    };

    self.saveRecipient = function () {
        if (self.validateRecipientInfo()) {
            var recipientInfo = self.getParams();
            sharedScriptManager.blockUI();
            sharedScriptManager.callAjax(self,
            window.rootUrl + "MailStation/MailRecipient/CreateRecipient",
            recipientInfo,
            function (result) {
                if (result.IsSuccessful) {
                    sharedScriptManager.createSuccessSummary(viewResources.CreateRecipientSuccessMsg);
                    NAP.showSuccessMsg(viewResources.CreateRecipientSuccessMsg, "MailStation/MailRecipient/Index", 6000);
                }
                else {
                    sharedScriptManager.createErrorSummary([result.Message]);
                }
            },
            null,
            sharedScriptManager.unblockUI);
        }
    };

    self.getParams = function () {
        var recipient = {
            Username: DOMElements.UserName.val(),
            Email1: DOMElements.Email1.val(),
            Email2: DOMElements.Email2.val(),
            Email3: DOMElements.Email3.val(),
            Note: DOMElements.Note.val(),
            Status: DOMElements.Status.attr("checked") ? 1 : 0
        };

        return recipient;
    };

    self.validateRecipientInfo = function () {
        $('.input-validation-error').removeClass('input-validation-error');
        if (!sharedScriptManager.isValidEmail(DOMElements.Email1.val())) {
            DOMElements.Email1.addClass('input-validation-error');
            sharedScriptManager.createErrorSummary(['Email 1 is not a valid email.']);
            return false;
        }

        if (!sharedScriptManager.isValidEmail(DOMElements.Email2.val())) {
            DOMElements.Email2.addClass('input-validation-error');
            sharedScriptManager.createErrorSummary(['Email 2 is not a valid email.']);
            return false;
        }

        if (!sharedScriptManager.isValidEmail(DOMElements.Email3.val())) {
            DOMElements.Email3.addClass('input-validation-error');
            sharedScriptManager.createErrorSummary(['Email 3 is not a valid email.']);
            return false;
        }
    };

    self.checkRecipientInfo = function () {
        var checkParams = {
            Username: DOMElements.UserName.val()
        };

        if (checkParams.Username !== '') {
            DOMElements.BtnLoading.removeClass("display-none");
            sharedScriptManager.callAjax(self,
            window.rootUrl + "MailStation/MailRecipient/CheckRecipient",
            checkParams,
            self.renderCheckRecipientInfoResult,
            null,
            null);
        }
    };

    self.renderCheckRecipientInfoResult = function (result) {
        DOMElements.BtnLoading.addClass("display-none");
        self.copyRecipientInfo(result);
    };

    self.copyRecipientInfo = function (result) {
        if (!result) {
            return;
        }

        DOMElements.Email1.val(result.Email);
    };
};

var createRecipientController = new CreateRecipientController();

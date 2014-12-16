$(document).ready(function () {
    insertMailController.initPage();
});

var InsertMailController = function () {
    var self = this;

    var Enums = {
        TriggerType: {
            Function: '0',
            Schedule: '1'
        }
    };

    var magicSuggestOptions = {
        allowFreeEntries: false,
        data: window.rootUrl + "MailStation/MailManage/SuggestMail"
    };

    self.initPage = function () {
        self.init();
        self.bindEvents();
    };

    var DOMElements = {
        MailGroupID: null,
        MailName: null,
        SiteID: null,
        TriggerType: null,
        CronExpression: null,
        MailFrom: null,
        MailTo: null,
        MailCc: null,
        MailBcc: null,
        Description: null,
        IsActive: null,
        BtnCancel: null,
        BtnSave: null
    };

    var initualValueForMagicSuggestion = function ($ms) {
        var valueArr = $ms.val();
        if (valueArr != undefined && valueArr.length > 0) {
            valueArr = $.map(valueArr.split(','), function (value) { return parseInt(value) });
        }
        else {
            valueArr = [];
        }

        $ms.attr('value', '');
        $ms = $ms.magicSuggest(magicSuggestOptions);
        $($ms).on('load', function () {
            if (this._dataSet === undefined) {
                this._dataSet = true;
                $ms.setValue(valueArr);
                $ms.setDataUrlParams({});
            }
        });

        return $ms;
    }

    self.init = function () {
        self.cacheDOMElement();
        $('select.combobox-autocomplete').each(function () {
            $(this).combobox();
        });

        DOMElements.MailTo = initualValueForMagicSuggestion(DOMElements.MailTo);
        DOMElements.MailCc = initualValueForMagicSuggestion(DOMElements.MailCc);
        DOMElements.MailBcc = initualValueForMagicSuggestion(DOMElements.MailBcc);
    };

    self.cacheDOMElement = function () {
        DOMElements.MailGroupID = $('#MailGroupID');
        DOMElements.MailName = $('#MailName');
        DOMElements.SiteID = $('#SiteID');
        DOMElements.TriggerType = $('#TriggerType');
        DOMElements.CronExpression = $('#nexCron');
        DOMElements.MailFrom = $('#MailFrom');
        DOMElements.MailTo = $('#MailTo');
        DOMElements.MailCc = $('#MailCc');
        DOMElements.MailBcc = $('#MailBcc');
        DOMElements.Description = $('#Description');
        DOMElements.IsActive = $('#IsActive');
        DOMElements.BtnSave = $('#button-save');
        DOMElements.BtnCancel = $('#button-cancel');
    };

    self.bindEvents = function () {
        $("#btn-add-group").click(function () {
            var groupName = $(".ui-autocomplete-input").val();
            var errorList = [];
            if (groupName === undefined || groupName === '') {
                errorList.push({
                    Message: "The group name cannot be empty, please try again!"
                });
            }

            if (errorList.length === 0) {
                sharedScriptManager.hideValidateSummary();
                self.addGroup(groupName);
            } else {
                sharedScriptManager.showError(errorList);
            }
        });

        DOMElements.TriggerType.change(self.triggerChange);

        $("#button-cancel").click(function () {
            NAP.confirm("Confirm", viewResources.CancelCreateMail, function () {
                NAP.redirect('MailStation/MailManage/Index', 0);
            });
        });

        $("#button-save").click(function () {
            sharedScriptManager.callAjax(
                self,
                window.rootUrl + "MailStation/MailManage/CreateMail",
                self.getParams(),
                function (result) {
                    if (result.IsSuccessful) {
                        sharedScriptManager.createSuccessSummary(viewResources.CreateMailSuccessMsg);
                        NAP.showSuccessMsg(viewResources.AddMailSuccessMsg, "MailStation/MailManage/Index", 6000);
                        self.refreshMailGroup();
                    }
                    else {
                        sharedScriptManager.createErrorSummary([result.Message]);
                    }
                },
                null,
                sharedScriptManager.unblockUI
            );

        });
    };

    self.getParams = function () {
        var triggerType = DOMElements.TriggerType.val();
        if (triggerType === Enums.TriggerType.Function) {
            return {
                MailName: DOMElements.MailName.val(),
                MailGroupID: DOMElements.MailGroupID.val(),
                SiteID: DOMElements.SiteID.val(),
                TriggerType: DOMElements.TriggerType.val(),
                MailFrom: DOMElements.MailFrom.val(),
                MailTo: DOMElements.MailTo.getValue().join(','),
                MailCc: DOMElements.MailCc.getValue().join(','),
                MailBcc: DOMElements.MailBcc.getValue().join(','),
                IsActive: DOMElements.IsActive.is(':checked'),
                Description: DOMElements.Description.val()
            };
        }

        var cronResult = DOMElements.CronExpression.nexCron('getCronExpression');
        return {
            MailName: DOMElements.MailName.val(),
            MailGroupID: DOMElements.MailGroupID.val(),
            SiteID: DOMElements.SiteID.val(),
            TriggerType: DOMElements.TriggerType.val(),
            CronExpression: cronResult.CronExpression,
            MailFrom: DOMElements.MailFrom.val(),
            MailTo: DOMElements.MailTo.getValue().join(','),
            MailCc: DOMElements.MailCc.getValue().join(','),
            MailBcc: DOMElements.MailBcc.getValue().join(','),
            IsActive: DOMElements.IsActive.val(),
            StartOn: cronResult.StartOn,
            EndOn: cronResult.EndOn,
            Attemp: cronResult.Attemp,
            Interval: cronResult.Interval,
            Description: DOMElements.Description.val()
        };
    };

    self.addGroup = function (groupName) {
        sharedScriptManager.callAjax(
            self,
            window.rootUrl + "MailStation/MailManage/AddMailGroup",
            { MailGroupName: groupName },
            function (result) {
                if (result.IsSuccessful) {
                    sharedScriptManager.createSuccessSummary(viewResources.AddMailGroupSuccessMsg);
                    self.refreshMailGroup();
                }
                else {
                    sharedScriptManager.createErrorSummary([result.Message]);
                }
            },
            null,
            sharedScriptManager.unblockUI
        );
    };

    self.refreshMailGroup = function () {
        sharedScriptManager.callAjax(self,
            window.rootUrl + "MailStation/MailManage/MailGroups",
            null,
            function (result) {
                var mailGroupHtml = '';
                for (var i = 0; i < result.length; i++) {
                    mailGroupHtml += '<option value="' + result[i].Value + '">' + result[i].Text + '</option>';
                }

                DOMElements.MailGroupID.empty().append(mailGroupHtml);
            },
            null,
            sharedScriptManager.unblockUI);
    };

    self.triggerChange = function () {
        var triggerId = DOMElements.TriggerType.val();
        if (triggerId === Enums.TriggerType.Schedule) {
            $(".schedule-row").removeClass("display-none");
            DOMElements.CronExpression.nexCron();
        } else {
            $(".schedule-row").addClass("display-none");
        }
    };
};

var insertMailController = new InsertMailController(); 
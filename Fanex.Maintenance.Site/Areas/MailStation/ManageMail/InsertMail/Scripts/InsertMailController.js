$(document).ready(function () {
    insertMailController.initPage();
});

var InsertMailController = function () {
    var self = this;
    self.Enums = {
        TriggerType: {
            Schedule: 1
        },
        ScheduleType: {
            Daily: 1,
            Weekly: 2,
            Monthly: 3
        }
    };
    self.cachedData = {
        groups: [{ Id: 0, Name: 'All Types' },
            { Id: 1, Name: 'By Schedule' },
            { Id: 2, Name: 'By Function'}],
        mailList: [],
        lastScheduleElement: null
    };
    self.initPage = function () {
        self.loadOptions();
        self.bindEvents();
    };
    self.loadOptions = function () {
        self.renderDropdownGroup();
        self.renderDropdownSite();
        self.renderDropdownTrigger();
        $('#nexcron').nexCron();
    };

    self.bindEvents = function () {
        $("#btn-add-group").click(function () {
            var groupName = $(".ui-autocomplete-input").val();
            var errorList = self.validateGroupName(groupName);
            if (errorList.length === 0) {
                self.hideError();
                self.addGroup(groupName);
            } else {
                self.showError(errorList);
            }
        });

        $("#to-mail").keyup(function () {
            self.validateToEmail();
        });

        $("#button-cancel").click(function () {
            NAP.headlineMsgBox.confirm(viewResources.Confirm_CancelInsertMail, function () {
                NAP.redirect('MailStation/MailManage/Index', 0);
            });
        });

        $("#button-save").click(function () {
            self.saveEmail();
        });
    };

    self.renderDropdownGroup = function () {
        var groups = self.cachedData.groups;
        var options = '';
        var groupLength = groups.length;
        for (var i = 0; i < groupLength; i++) {
            var group = groups[i];
            options += '<option value="' + group.Id + '">' + group.Name + '</option>'
        }

        $("#group").empty().append(options);
        $('select.combobox-autocomplete').each(function () {
            $(this).combobox();
        });
    };

    self.validateGroupName = function (groupName) {
        var errorList = [];
        if (groupName === "") {
            errorList.push({
                Id: "span#group",
                Message: "The group name cannot be empty, please try again!"
            });

            return errorList;
        }

        var groups = self.cachedData.groups;
        var groupLength = groups.length;
        var isExist = false;
        for (var i = 0; i < groupLength; i++) {
            if (groupName === groups[i].Name) {
                isExist = true;
                break;
            }
        }

        if (isExist) {
            errorList.push({
                Id: "span#group",
                Message: "The group exists, please try again!"
            });
            return errorList;
        }

        return errorList;
    };

    self.addGroup = function (groupName) {
        var groups = self.cachedData.groups;
        var groupLength = groups.length;
        var newGroup = {
            Id: groupLength,
            Name: groupName
        };
        self.cachedData.groups.push(newGroup);
        $("#group").append('<option value="' + newGroup.Id + '">' + newGroup.Name + '</option>');
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

    self.renderDropdownSite = function () {
        var sites = [{ Id: 1, Name: 'Site 1' },
            { Id: 2, Name: 'Site 2'}];
        var options = '';
        var siteLength = sites.length;
        for (var i = 0; i < siteLength; i++) {
            var site = sites[i];
            options += '<option value="' + site.Id + '">' + site.Name + '</option>'
        }

        $("#site").empty().append(options);
    };

    self.renderDropdownTrigger = function () {
        var triggers = [{ Id: 1, Name: 'By Schedule' },
            { Id: 2, Name: 'By Function'}];
        var options = '';
        var triggerLength = triggers.length;
        for (var i = 0; i < triggerLength; i++) {
            var trigger = triggers[i];
            options += '<option value="' + trigger.Id + '">' + trigger.Name + '</option>'
        }

        var dropdownTrigger = $("#trigger");
        dropdownTrigger.empty().append(options);
        dropdownTrigger.change(function () {
            self.triggerChange();
        });
        self.triggerChange();
    };

    self.triggerChange = function () {
        var triggerId = parseInt($("#trigger").val());
        if (triggerId === insertMailController.Enums.TriggerType.Schedule) {
            $(".schedule-row").removeClass("display-none");
        } else {
            $(".schedule-row").addClass("display-none");
        }
    };

    self.validateToEmail = function () {
        var toMailElement = $("#to-mail");
        var toMail = toMailElement.val();
        var emails = toMail.split(";");
        var isValid = false;
        var emailLength = emails.length;
        for (var i = 0; i < emailLength; i++) {
            var email = $.trim(emails[i]);
            if (email === '') {
                continue;
            }

            isValid = self.isValidateEmail(email);
            if (!isValid) {
                break;
            }
        }

        if (isValid) {
            toMailElement.removeClass("validation-summary-errors-container");
        } else {
            toMailElement.addClass("validation-summary-errors-container");
        }
    };

    self.isValidateEmail = function (email) {
        var re = /^(([^<>()[\]\\.,;:\s@\"]+(\.[^<>()[\]\\.,;:\s@\"]+)*)|(\".+\"))@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\])|(([a-zA-Z\-0-9]+\.)+[a-zA-Z]{2,}))$/;
        return re.test(email);
    };

    self.saveEmail = function () {
        sharedScriptManager.blockUI();
        self.hideError();
        var emailInfo = self.getEmailInfo();
        var errorList = self.validateEmailInfo(emailInfo);
        if (errorList.length === 0) {
            self.hideError();
            sharedScriptManager.callAjax(self,
                window.rootUrl + "MailStation/MailManage/InsertMail",
                emailInfo,
                self.renderActionResult,
                null,
                null);
        } else {
            self.showError(errorList);
            sharedScriptManager.unblockUI();
        }
    };

    self.validateEmailInfo = function (emailInfo) {
        var isValid = true;
        isValid = self.validateGroupName(emailInfo.GroupName);
        return isValid;
    };

    self.getEmailInfo = function () {
        var email = {
            GroupName: $(".ui-autocomplete-input").val(),
            MailName: $("#mail-name").val(),
            SiteId: $("#site").val(),
            TriggerType: parseInt($("#trigger").val()),
            FromMail: $("#from-mail").val(),
            ToMail: $("#to-mail").val(),
            CCMail: $("#cc-mail").val(),
            BCCMail: $("#bcc-mail").val(),
            Description: $("#description").val(),
            Status: $("#status").attr("checked") ? 1 : 0
        };

        if (email.TriggerType === insertMailController.Enums.TriggerType.Schedule) {
            email.ScheduleType = parseInt($("#repeat-type").val());
            switch (email.ScheduleType) {
                case insertMailController.Enums.ScheduleType.Daily:
                    $.extend(email, self.getDailySchedule());
                    break;
                case insertMailController.Enums.ScheduleType.Weekly:
                    $.extend(email, self.getWeeklySchedule());
                    break;
                case insertMailController.Enums.ScheduleType.Monthly:
                    $.extend(email, self.getMonthlySchedule());
                    break;
            }
        }

        return email;
    };

    self.renderActionResult = function (result) {
        var result = {
            Code: 0,
            Message: 'aaa'
        };
        sharedScriptManager.renderActionResult(result, null);
    };
};

var insertMailController = new InsertMailController(); 
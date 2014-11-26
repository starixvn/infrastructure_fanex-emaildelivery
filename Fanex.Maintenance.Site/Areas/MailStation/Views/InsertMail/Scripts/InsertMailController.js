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
        self.initDailySchedule();
        self.initWeeklySchedule();
        self.initMonthlySchedule();

        self.renderDropdownRepeatDay();
        self.renderDropdownAttempt();
        self.bindEvents();
    };
    self.loadOptions = function () {
        self.renderDropdownGroup();
        self.renderDropdownSite();
        self.renderDropdownTigger();
        self.renderDropdownRepeatType();
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
                NAP.redirect('MailStation/Mail/Index', 0);
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
    self.renderDropdownTigger = function () {
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
    self.renderDropdownRepeatType = function () {
        var scheduleTypes = [{ Id: 1, Name: 'Daily' },
            { Id: 2, Name: 'Weekly' },
            { Id: 3, Name: 'Monthly'}];
        var options = '';
        var scheduleTypeLength = scheduleTypes.length;
        for (var i = 0; i < scheduleTypeLength; i++) {
            var scheduleType = scheduleTypes[i];
            options += '<option value="' + scheduleType.Id + '">' + scheduleType.Name + '</option>'
        }

        var dropdownRepeatType = $("#repeat-type");
        dropdownRepeatType.empty().append(options);
        dropdownRepeatType.change(function () {
            self.repeatTypeChange();
        });
        self.repeatTypeChange();
    };
    self.triggerChange = function () {
        var triggerId = parseInt($("#trigger").val());
        if (triggerId === insertMailController.Enums.TriggerType.Schedule) {
            $(".schedule-row").removeClass("display-none");
        } else {
            $(".schedule-row").addClass("display-none");
        }
    };
    self.initDailySchedule = function () {
        var scheduleContainer = $(".daily-schedule");

        var startPicker = scheduleContainer.find("#start-day-daily");
        startPicker.datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 1,
            dateFormat: "M dd,yy",
            showOn: "button",
            buttonImage: $('#imagePath').val(),
            buttonText: "Start day",
            buttonImageOnly: true
        }).datepicker('init', '#start-day-daily');

        var endPicker = scheduleContainer.find("#end-day-daily");
        endPicker.datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 1,
            dateFormat: "M dd,yy",
            showOn: "button",
            buttonImage: $('#imagePath').val(),
            buttonText: "End day",
            buttonImageOnly: true
        }).datepicker('init', '#end-day-daily');

    };
    self.initWeeklySchedule = function () {
        var scheduleContainer = $(".weekly-schedule");

        var startPicker = scheduleContainer.find("#start-day-weekly");
        startPicker.datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 1,
            dateFormat: "M dd,yy",
            showOn: "button",
            buttonImage: $('#imagePath').val(),
            buttonText: "Start day",
            buttonImageOnly: true
        }).datepicker('init', '#start-day-weekly');

        var endPicker = scheduleContainer.find("#end-day-weekly");
        endPicker.datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 1,
            dateFormat: "M dd,yy",
            showOn: "button",
            buttonImage: $('#imagePath').val(),
            buttonText: "End day",
            buttonImageOnly: true
        }).datepicker('init', '.#end-day-weekly');

    };
    self.initMonthlySchedule = function () {
        var scheduleContainer = $(".monthly-schedule");

        var startPicker = scheduleContainer.find("#start-day-monthly");
        startPicker.datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 1,
            dateFormat: "M dd,yy",
            showOn: "button",
            buttonImage: $('#imagePath').val(),
            buttonText: "Start day",
            buttonImageOnly: true
        }).datepicker('init', '#start-day-monthly');

        var endPicker = scheduleContainer.find("#end-day-monthly");
        endPicker.datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 1,
            dateFormat: "M dd,yy",
            showOn: "button",
            buttonImage: $('#imagePath').val(),
            buttonText: "End day",
            buttonImageOnly: true
        }).datepicker('init', '.#end-day-monthly');

    };
    self.repeatTypeChange = function () {
        var dropdownRepeatType = $("#repeat-type");
        var repeatType = parseInt(dropdownRepeatType.val());
        self.showScheduleOptions(repeatType);
    };
    self.showScheduleOptions = function (scheduleType) {
        var scheduleGroup;
        switch (scheduleType) {
            case insertMailController.Enums.ScheduleType.Daily:
                scheduleGroup = $(".daily-schedule");
                break;
            case insertMailController.Enums.ScheduleType.Weekly:
                scheduleGroup = $(".weekly-schedule");
                break;
            case insertMailController.Enums.ScheduleType.Monthly:
                scheduleGroup = $(".monthly-schedule");
                break;
        }

        if (self.cachedData.lastScheduleElement) {
            self.cachedData.lastScheduleElement.addClass("display-none");
        }

        scheduleGroup.removeClass("display-none");
        self.cachedData.lastScheduleElement = scheduleGroup;
    };
    self.renderDropdownRepeatDay = function () {
        var options = '';
        var repeatLength = 30;
        for (var i = 1; i <= repeatLength; i++) {
            options += '<option value="' + i + '">' + i + '</option>'
        }

        $(".repeat-day").empty().append(options);
    };
    self.renderDropdownRepeatTime = function () {
        var options = '';
        var repeatLength = 24;
        for (var i = 0; i < repeatLength; i++) {
            var time = i + ":00";
            options += '<option value="' + time + '">' + time + '</option>'
        }

        $("#repeat-time").empty().append(options);
    };
    self.renderDropdownAttempt = function () {
        var options = '';
        var attemptLength = 10;
        for (var i = 0; i <= attemptLength; i++) {
            options += '<option value="' + i + '">' + i + '</option>'
        }

        $(".attempt-count").empty().append(options);
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
        //        NAP.headlineMsgBox.confirm(viewResources.Confirm_InsertMail, function () {
        sharedScriptManager.blockUI();
        self.hideError();
        var emailInfo = self.getEmailInfo();
        var errorList = self.validateEmailInfo(emailInfo);
        if (errorList.length === 0) {
            self.hideError();
            sharedScriptManager.callAjax(self,
                window.rootUrl + "MailStation/Mail/InsertMail",
                emailInfo,
                self.renderActionResult,
                null,
                null);
        } else {
            self.showError(errorList);
            sharedScriptManager.unblockUI();
        }
        //        });
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
    self.getDailySchedule = function () {
        var scheduleGroup = $(".daily-schedule");
        return {
            RepeatDay: scheduleGroup.find(".repeat-day").val(),
            RepeatTime: scheduleGroup.find("#repeat-time").val(),
            StartDay: scheduleGroup.find("#start-day-daily").val(),
            IsEndLimit: scheduleGroup.find('input:radio[name=is-end-limit-daily]:checked').val() === "2",
            EndDay: scheduleGroup.find("#end-day-daily").val(),
            AttempCount: scheduleGroup.find("#attempt-count-daily").val(),
            AttempInterval: scheduleGroup.find("#interval-time").val()
        };
    };
    self.getWeeklySchedule = function () {
        var scheduleGroup = $(".weekly-schedule");
        var repeatOnDays = [];
        var checkedDay = scheduleGroup.find('input:checkbox[name=repeat-day-checkbox]:checked');
        var checkedDayLength = checkedDay.length;
        for (var i = 0; i < checkedDayLength; i++) {
            repeatOnDays.push($(checkedDay[i]).val());
        }

        return {
            RepeatOnDays: repeatOnDays.join(","),
            RepeatTime: scheduleGroup.find("#repeat-time").val(),
            StartDay: scheduleGroup.find("#start-day-weekly").val(),
            IsEndLimit: scheduleGroup.find('input:radio[name=is-end-limit-weekly]:checked').val() === "2",
            EndDay: scheduleGroup.find("#end-day-weekly").val(),
            AttempCount: scheduleGroup.find("#attempt-count-weekly").val(),
            AttempInterval: scheduleGroup.find("#interval-time").val()
        };
    };
    self.getMonthlySchedule = function () {
        var scheduleGroup = $(".monthly-schedule");
        return {
            RepeatMonth: scheduleGroup.find(".repeat-day").val(),
            RepeatTime: scheduleGroup.find("#repeat-time").val(),
            RepeatOnDays: scheduleGroup.find("#repeat-on-day").val(),
            StartDay: scheduleGroup.find("#start-day-monthly").val(),
            IsEndLimit: scheduleGroup.find('input:radio[name=is-end-limit-monthly]:checked').val() === "2",
            EndDay: scheduleGroup.find("#end-day-monthly").val(),
            AttempCount: scheduleGroup.find("#attempt-count-monthly").val(),
            AttempInterval: scheduleGroup.find("#interval-time").val()
        };
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
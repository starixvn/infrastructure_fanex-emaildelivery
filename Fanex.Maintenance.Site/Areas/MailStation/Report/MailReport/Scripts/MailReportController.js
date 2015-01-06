$(document).ready(function () {
    mailReportController.initPage();
});

var MailReportController = function () {
    var self = this;
    self.Enums = {
        TaskStatus: {
            All: -1,
            Waiting: 0,
            Running: 4,
            Sent: 3,
            DataNotReady: 1,
            Failed: 2
        }
    };

    self.cachedData = {
        searchParams: null
    };

    self.initPage = function () {
        self.loadFilter();
        self.bindEvents();
        self.getMailReport();
    };

    self.loadFilter = function () {
        self.renderDropdownRefresh();
        self.initDatePicker();
        countdownController.initPage();
    };
    self.renderDropdownRefresh = function () {
        var countDowns = [{ Id: 0, Value: viewResources.Not_Refresh },
            { Id: 1, Value: 30 },
            { Id: 2, Value: 60 },
            { Id: 3, Value: 90 },
            { Id: 4, Value: 120 },
            { Id: 5, Value: 150 },
            { Id: 6, Value: 180}];
        var options = '';
        var countDownsLength = countDowns.length;
        for (var i = 0; i < countDownsLength; i++) {
            var countDown = countDowns[i];
            var displayName = countDown.Value === viewResources.Not_Refresh ?
                countDown.Value :
                countDown.Value + viewResources.Symbol_Second;
            options += '<div class="countDownOption' + (i === 0 ? ' countDownOptionCheck' : '') +
                '" value="' + countDown.Value + '">' +
                    displayName +
                '</div>'
        }

        $(".countDownOptionContainer").empty().append(options);
    };

    self.initDatePicker = function () {
        $("#FromDate").datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 1,
            dateFormat: "M dd,yy",
            showOn: "button",
            buttonImage: $('#imagePath').val(),
            buttonText: "From",
            buttonImageOnly: true
        }).datepicker('init', '#FromDate');

        $("#ToDate").datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 1,
            dateFormat: "M dd,yy",
            showOn: "button",
            buttonImage: $('#imagePath').val(),
            buttonText: "To",
            buttonImageOnly: true
        }).datepicker('init', '#ToDate');
    };

    self.bindEvents = function () {
        countdownController.setRefreshEvent(self.refreshMailReport);

        $("#button-search").click(function () {
            self.getMailReport();
        });
    };
    self.refreshMailReport = function () {
        sharedScriptManager.blockUI();
        sharedScriptManager.callAjax(self,
            window.rootUrl + "MailStation/MailReport/MailReport",
            self.cachedData.searchParams,
            self.renderMailReport,
            null,
            sharedScriptManager.unblockUI);
    };
    self.getMailReport = function () {
        sharedScriptManager.blockUI();
        var searchParams = {
            MailName: $("#MailName").val(),
            FromDate: $("#FromDate").val(),
            ToDate: $("#ToDate").val(),
            Status: $("#Status").val()
        };
        self.cachedData.searchParams = searchParams;
        sharedScriptManager.callAjax(self,
            window.rootUrl + "MailStation/MailReport/MailReport",
            searchParams,
            self.renderMailReport,
            null,
            sharedScriptManager.unblockUI);
    };

    self.renderMailReport = function (result) {
        var mailReportLength = result.length;
        var bodyData = [];
        for (var i = 0; i < mailReportLength; i++) {
            var mail = result[i];
            var rowColumns = [];
            var mailIndex = ' mail-index="' + mail.MailTaskId + '"';
            rowColumns.push({ Name: "ColId", Value: mail.MailTaskId });
            rowColumns.push({
                Name: "ColMailName",
                Value: '<a class="link mail-name" href="javascript:;"' + mailIndex + '>' +
                           mail.MailName +
                       '</a>'
            });

            rowColumns.push({ Name: "ColSite", Value: mail.SiteName });
            rowColumns.push({ Name: "ColScheduledTime", Value: mail.ScheduledTime });
            rowColumns.push({ Name: "ColLastSent", Value: mail.LastSent });
            rowColumns.push({ Name: "ColStatus", Value: self.getStatusDisplay(mail, i) });
            if (viewResources.hasEditRight) {
                rowColumns.push({ Name: "ColAction", Value: self.getActionColumn(mail, i) });
            }

            rowColumns.push({ Name: "ColLog", Value: self.getLogColumn(mail, i) });

            bodyData.push({
                Columns: rowColumns
            });
        };

        var headerColumns = [];
        headerColumns.push({ HeaderText: viewResources.Id, Width: 40, Name: "ColId", HeaderAlign: "center", CellAlign: "center" });
        headerColumns.push({ HeaderText: viewResources.Mail_Name, Width: 300, Name: "ColMailName", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: viewResources.Site, Width: 120, Name: "ColSite", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: viewResources.Scheduled_Time, Width: 140, Name: "ColScheduledTime", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "MMM DD, YYYY hh:mm:ss A" });
        headerColumns.push({ HeaderText: viewResources.Last_Sent, Width: 120, Name: "ColLastSent", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "MMM DD, YYYY hh:mm:ss A" });
        headerColumns.push({ HeaderText: viewResources.Status, Width: 120, Name: "ColStatus", HeaderAlign: "center", CellAlign: "center" });
        if (viewResources.hasEditRight) {
            headerColumns.push({ HeaderText: viewResources.Action, Width: 120, Name: "ColAction", HeaderAlign: "center", CellAlign: "center" });
        }

        headerColumns.push({ HeaderText: viewResources.Log, Width: 120, Name: "ColLog", HeaderAlign: "center", CellAlign: "center" });
        var gridOptions = {
            columns: headerColumns,
            showTitle: false,
            bodyRows: bodyData,
            numberOfFrozenColumn: 0,
            fullWidth: true,
            fullHeight: false,
            gridHeight: 10000,
            showDistinctRow: false,
            showHighlightRow: true,
            showTree: true,
            treeParentPaddingLeft: 5,
            headerColumnHeight: 29,
            bodyRowHeight: 30,
            showPager: true,
            pagerOption: {
                itemsPerPage: sharedScriptManager.cachedData.defaultItemsPerPage,
                currentPage: 0,
                itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions,
                totalItem: 800
            }
        };

        $("#mail-report").removeClass("display-none").fanexGrid(gridOptions);
        self.bindGridEvent();
    };

    self.getStatusDisplay = function (mail, index) {
        var mailIndex = ' mail-index="' + mail.MailTaskId + '"';
        switch (mail.Status) {
            case self.Enums.TaskStatus.Waiting:
                return '<div class="column-status waiting-status"' + mailIndex + '>' + viewResources.Waiting.toUpperCase() + '</div>';
            case self.Enums.TaskStatus.Running:
                return '<div class="column-status running-status">' +
                            '<span class="status">' + viewResources.Running.toUpperCase() + '</span>' +
                            (mail.Attempt > 0 ? ' <span>(' + mail.Attempt + ')</span>' : '') +
                        '</div>';
            case self.Enums.TaskStatus.DataNotReady:
                return '<div class="column-status data-not-ready-status"' + mailIndex + '>' + viewResources.Data_not_ready.toUpperCase() + '</div>';
            case self.Enums.TaskStatus.Sent:
                return '<div class="column-status sent-status"' + mailIndex + '>' + viewResources.Sent.toUpperCase() + '</div>';
            case self.Enums.TaskStatus.Failed:
                return '<div class="column-status failed-status"' + mailIndex + '>' + viewResources.Failed.toUpperCase() + '</div>';
        }
    };

    self.getActionColumn = function (mail, index) {
        var mailIndex = ' mail-index="' + mail.MailTaskId + '"';
        switch (mail.Status) {
            case self.Enums.TaskStatus.DataNotReady:
            case self.Enums.TaskStatus.Sent:
            case self.Enums.TaskStatus.Failed:
                return '<input type="submit" value="Retry" class="button-retry"' + mailIndex + '>';
        }
    };

    self.getLogColumn = function (mail, index) {
        var mailIndex = ' mail-index="' + mail.MailTaskId + '"';
        switch (mail.Status) {
            case self.Enums.TaskStatus.Running:
            case self.Enums.TaskStatus.DataNotReady:
            case self.Enums.TaskStatus.Sent:
            case self.Enums.TaskStatus.Failed:
                return '<div class="icon-view-log"' + mailIndex + '></div>';
        }
    };

    self.bindGridEvent = function () {
        var gridContainer = $("#mail-report");
        gridContainer.find(".mail-name").click(function (event) {
            var $this = $(this);
            var mailIndex = parseInt($this.attr("mail-index"));
            self.viewMailDetail(mailIndex);
        });

        gridContainer.find(".button-retry").click(function (event) {
            var $this = $(this);
            var mailIndex = parseInt($this.attr("mail-index"));
            self.retrySendMail(mailIndex, function () {
                setTimeout(function () {
                    NAP.headlineMsgBox.close();
                    self.refreshMailReport();
                }, sharedScriptManager.cachedData.alertSuccessTime);
            });
        });

        gridContainer.find(".icon-view-log").click(function (event) {
            var $this = $(this);
            var mailIndex = parseInt($this.attr("mail-index"));
            self.viewMailLog(mailIndex);
        });
    };

    self.retrySendMail = function (mailIndex, afterRetryEvent) {
        NAP.headlineMsgBox.confirm(viewResources.Confirm_RetrySendMail, function () {
            sharedScriptManager.blockUI();
            var retryParams = {
                TaskId: mailIndex
            };
            sharedScriptManager.callAjax(self,
            window.rootUrl + "MailStation/MailReport/RetrySendMail",
            retryParams,
            function (result) {
                result = {
                    Code: 0,
                    Message: 'Job has been retried.'
                };
                sharedScriptManager.renderActionResult(result, afterRetryEvent);
            },
            null,
            sharedScriptManager.unblockUI);
        });
    };

    self.renderActionResult = function (result) {
        sharedScriptManager.renderActionResult(
            result,
            function () {
                setTimeout(function () {
                    self.refreshMailReport();
                }, sharedScriptManager.cachedData.alertSuccessTime);
            }
        );
    };

    self.updateRowStatus = function (mailStatus, index) {
        var gridContainer = $("#mail-report");
        switch (mailStatus) {
            case "RUNNING":
                gridContainer.find(".column-status[mail-index=" + index + "]")
                    .removeClass("data-not-ready-status")
                    .removeClass("sent-status")
                    .removeClass("failed-status")
                    .addClass("running-status")
                    .empty()
                    .html('<span class="status">' + mailStatus + '</span>');
                gridContainer.find(".icon-view-log[mail-index=" + index + "]").remove();
                break;
        }
    };

    self.viewMailDetail = function (mailIndex) {
        popUpManager.openPopup(
            window.rootUrl + 'MailStation/MailReport/MailDetail?' +
            $.param({
                mailId: mailIndex
            }),
            viewResources.Details,
            $(window).width() - 200,
            $(window).height() - 200,
            true,
            false,
            parent.window,
            3000,
            null,
            null,
            ['center', 80]);
    };

    self.viewMailLog = function (mailIndex) {
        popUpManager.openPopup(
            window.rootUrl + 'MailStation/MailReport/MailLog?' +
            $.param({
                mailId: mailIndex
            }),
            viewResources.Log_details,
            $(window).width() - 200,
            $(window).height() - 200,
            true,
            false,
            parent.window,
            3000,
            null,
            null,
            ['center', 80]);
    };
};

var mailReportController = new MailReportController();
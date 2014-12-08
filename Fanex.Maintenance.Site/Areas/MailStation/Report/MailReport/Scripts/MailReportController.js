$(document).ready(function () {
    mailReportController.initPage();
});

var MailReportController = function () {
    var self = this;
    self.cachedData = {
        searchParams: null,
        mailReport: null
    };
    self.initPage = function () {
        self.loadFilter();
        self.getMailReport();
    };
    self.loadFilter = function () {
        self.renderDropdownStatus();
        self.initDatePicker();
        self.bindEvents();
    };
    self.renderDropdownStatus = function () {
        var statuses = [{ Id: 0, Name: 'All' },
            { Id: 1, Name: 'Active' },
            { Id: 2, Name: 'Inactive'}];
        var options = '';
        var statusLength = statuses.length;
        for (var i = 0; i < statusLength; i++) {
            var status = statuses[i];
            options += '<option value="' + status.Id + '">' + status.Name + '</option>'
        }

        $("#status").empty().append(options);
    };
    self.initDatePicker = function () {
        $("#from-date").datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 1,
            dateFormat: "M dd,yy",
            showOn: "button",
            buttonImage: $('#imagePath').val(),
            buttonText: "From",
            buttonImageOnly: true
        }).datepicker('init', '#from-date');

        $("#to-date").datepicker({
            defaultDate: "+1w",
            changeMonth: true,
            changeYear: true,
            numberOfMonths: 1,
            dateFormat: "M dd,yy",
            showOn: "button",
            buttonImage: $('#imagePath').val(),
            buttonText: "To",
            buttonImageOnly: true
        }).datepicker('init', '#to-date');
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
            MailName: $("#mail-name").val(),
            FromDate: $("#from-date").val(),
            ToDate: $("#to-date").val(),
            Status: $("#status").val()
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
        result = self.dummyData();
        self.cachedData.mailReport = result;
        var mailReportLength = result.length;
        var bodyData = [];
        for (var i = 0; i < mailReportLength; i++) {
            var mail = result[i];
            var rowColumns = [];
            var mailIndex = ' mail-index="' + i + '"';
            rowColumns.push({ Name: "ColId", Value: mail.Id });
            rowColumns.push({
                Name: "ColMailName",
                Value: '<a class="link mail-name" href="javascript:;"' + mailIndex + '>' +
                           mail.MailName +
                       '</a>'
            });

            rowColumns.push({ Name: "ColSite", Value: mail.Site });
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
        headerColumns.push({ HeaderText: viewResources.Scheduled_Time, Width: 140, Name: "ColScheduledTime", HeaderAlign: "center", CellAlign: "center" });
        headerColumns.push({ HeaderText: viewResources.Last_Sent, Width: 120, Name: "ColLastSent", HeaderAlign: "center", CellAlign: "center" });
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
    self.dummyData = function () {
        var result = [];
        result.push({
            Id: 1,
            MailName: 'Name 1',
            Site: 'System Configuration',
            ScheduledTime: "Sep 25, 2014 02:54 AM",
            LastSent: 'Sep 24,2014 06:08 AM',
            Status: "WAITING",
            AttemptCount: 3
        });
        result.push({
            Id: 2,
            MailName: 'Name 2',
            Site: 'System Configuration',
            ScheduledTime: "Sep 25, 2014 02:54 AM",
            LastSent: 'Sep 24,2014 06:08 AM',
            Status: "RUNNING",
            AttemptCount: 3
        });
        result.push({
            Id: 2,
            MailName: 'Name 2',
            Site: 'System Configuration',
            ScheduledTime: "Sep 25, 2014 02:54 AM",
            LastSent: 'Sep 24,2014 06:08 AM',
            Status: "DATA NOT READY"
        });
        result.push({
            Id: 2,
            MailName: 'Name 2',
            Site: 'System Configuration',
            ScheduledTime: "Sep 25, 2014 02:54 AM",
            LastSent: 'Sep 24,2014 06:08 AM',
            Status: "SENT"
        });
        result.push({
            Id: 2,
            MailName: 'Name 2',
            Site: 'System Configuration',
            ScheduledTime: "Sep 25, 2014 02:54 AM",
            LastSent: 'Sep 24,2014 06:08 AM',
            Status: "FAILED"
        });
        result.push({
            Id: 2,
            MailName: 'Name 2',
            Site: 'System Configuration',
            ScheduledTime: "Sep 25, 2014 02:54 AM",
            LastSent: 'Sep 24,2014 06:08 AM',
            Status: "RUNNING",
            AttemptCount: 1
        });

        for (var i = 0; i < 5; i++) {
            result.push(result[0]);
        }

        return result;
    };
    self.getStatusDisplay = function (mail, index) {
        var mailIndex = ' mail-index="' + index + '"';
        switch (mail.Status) {
            case "WAITING":
                return '<div class="column-status waiting-status"' + mailIndex + '>' + mail.Status + '</div>';
            case "RUNNING":
                return '<div class="column-status running-status">' +
                            '<span class="status">' + mail.Status + '</span>' +
                            (mail.AttemptCount > 1 ? ' <span>(' + mail.AttemptCount + ')</span>' : '') +
                        '</div>';
            case "DATA NOT READY":
                return '<div class="column-status data-not-ready-status"' + mailIndex + '>' + mail.Status + '</div>';
            case "SENT":
                return '<div class="column-status sent-status"' + mailIndex + '>' + mail.Status + '</div>';
            case "FAILED":
                return '<div class="column-status failed-status"' + mailIndex + '>' + mail.Status + '</div>';
        }
    };
    self.getActionColumn = function (mail, index) {
        var mailIndex = ' mail-index="' + index + '"';
        switch (mail.Status) {
            case "DATA NOT READY":
            case "SENT":
            case "FAILED":
                return '<input type="submit" value="Retry" class="button-retry"' + mailIndex + '>';
        }
    };
    self.getLogColumn = function (mail, index) {
        var mailIndex = ' mail-index="' + index + '"';
        switch (mail.Status) {
            case "RUNNING":
            case "DATA NOT READY":
            case "SENT":
            case "FAILED":
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
                self.updateRowStatus("RUNNING", mailIndex);
                $this.remove();
            });
        });

        gridContainer.find(".icon-view-log").click(function (event) {
            var $this = $(this);
            var mailIndex = parseInt($this.attr("mail-index"));
            self.viewMailLog(mailIndex);
        });
    };
    self.retrySendMail = function (mailIndex, afterRetryEvent) {
        sharedScriptManager.blockUI();
        var mail = self.cachedData.mailReport[mailIndex];
        var searchParams = {
            MailId: mail.Id
        };
        sharedScriptManager.callAjax(self,
            window.rootUrl + "MailStation/MailReport/RetrySendMail",
            searchParams,
            function (result) {
                var result = {
                    Code: 0,
                    Message: 'aaa'
                };
                sharedScriptManager.renderActionResult(result, afterRetryEvent);
            },
            null,
            sharedScriptManager.unblockUI);
    };
    self.renderActionResult = function (result) {
        sharedScriptManager.renderActionResult(
            result,
            function () {
                setTimeout(function () {
                    self.refreshMailReport();
                }, sharedScriptManager.cachedData.alertSuccessTime);
            });
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
        var mail = self.cachedData.mailReport[mailIndex];
        popUpManager.openPopup(
             window.rootUrl + 'MailStation/MailReport/MailDetail',
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
            window.rootUrl + 'MailStation/MailReport/MailLog',
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
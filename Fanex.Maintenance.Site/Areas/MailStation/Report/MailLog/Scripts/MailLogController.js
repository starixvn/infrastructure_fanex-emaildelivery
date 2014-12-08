$(document).ready(function () {
    mailLogController.initPage();
});

var MailLogController = function () {
    var self = this;
    self.initPage = function () {
        self.renderMailLog(mailLog);
    };
    self.renderMailLog = function (result) {
        result = self.dummyData();
        var mailLogLength = result.length;
        var bodyData = [];
        for (var i = 0; i < mailLogLength; i++) {
            var log = result[i];

            bodyData.push({
                Columns: [{ Name: "ColId", Value: log.Id },
                    { Name: "ColStatus", Value: log.Status },
                    { Name: "ColMessage", Value: log.Message },
                    { Name: "ColLogTime", Value: log.LogTime },
                    { Name: "ColStackTrace", Value: log.StackTrace}]
            });
        }

        var gridOptions = {
            columns: [{ HeaderText: viewResources.Symbol_No, Width: 40, Name: "ColId", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Status, Width: 120, Name: "ColStatus", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Message, Width: 400, Name: "ColMessage", HeaderAlign: "center", CellAlign: "left" },
                { HeaderText: viewResources.Log_Time, Width: 140, Name: "ColLogTime", HeaderAlign: "center", CellAlign: "center" },
                { HeaderText: viewResources.Stack_Trace, Width: 500, Name: "ColStackTrace", HeaderAlign: "center", CellAlign: "left"}],
            showTitle: false,
            bodyRows: bodyData,
            numberOfFrozenColumn: 0,
            fullWidth: true,
            fullHeight: false,
            gridHeight: 10000,
            showDistinctRow: false,
            showHighlightRow: true,
            headerColumnHeight: 29,
            bodyRowHeight: 30
        };
        $("#mail-log").removeClass("display-none").fanexGrid(gridOptions);
    };
    self.dummyData = function () {
        var result = [];
        result.push({
            Id: 1,
            Status: "Running",
            Message: 'Mail To can not be null or empty',
            LogTime: '11/3/2014 2:00:12 AM',
            StackTrace: 'asdsad sadasd '
        });
        result.push({
            Id: 2,
            Status: "Running",
            Message: 'Mail To can not be null or empty',
            LogTime: '11/3/2014 2:00:12 AM',
            StackTrace: 'asdsad sadasd '
        });

        for (var i = 0; i < 5; i++) {
            result.push(result[0]);
        }

        return result;
    };
};

var mailLogController = new MailLogController();






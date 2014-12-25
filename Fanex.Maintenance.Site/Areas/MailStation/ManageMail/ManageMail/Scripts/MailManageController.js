$(document).ready(function () {
    MailManageController.initPage();
});

var MailManageController = function () {
    var self = this;
    var DOMElements = {
        MailName: null,
        IsActive: null,
        MailGroupID: null,
        SiteID: null,
        TriggerType: null,
        Emails: null,
        btnSearch: null
    };

    self.reportParams = {
        MailName: null,
        IsActive: null,
        MailGroupID: null,
        SiteID: null,
        TriggerType: null,
        Emails: null,
        btnSearch: null,
        gridContainer: null
    };

    self.initPage = function () {
        self.cacheDOMElements();
        self.bindEvents();
        DOMElements.TriggerType.val(-1);
        contextMenuController.bindContextMenuEvents();
        DOMElements.btnSearch.click();
    };

    self.cacheDOMElements = function () {
        DOMElements.MailName = $("#MailName");
        DOMElements.IsActive = $("#IsActive");
        DOMElements.MailGroupID = $("#MailGroupID");
        DOMElements.SiteID = $("#SiteID");
        DOMElements.TriggerType = $("#TriggerType");
        DOMElements.Emails = $("#Emails");
        DOMElements.btnSearch = $('#button-search');
        DOMElements.gridContainer = $('#mail-list');
        DOMElements.btnSearch = $('#button-search');
    };

    self.bindEvents = function () {
        DOMElements.btnSearch.click(function () {
            self.buttonSearchClick();
        });
    };

    self.buttonSearchClick = function () {
        sharedScriptManager.blockUI();
        NAP.headlineMsgBox.close();
        self.reportParams = {
            MailName: DOMElements.MailName.val(),
            IsActive: DOMElements.IsActive.val(),
            MailGroupID: DOMElements.MailGroupID.val(),
            SiteID: DOMElements.SiteID.val(),
            TriggerType: DOMElements.TriggerType.val(),
            Emails: DOMElements.Emails.val()
        };

        sharedScriptManager.callAjax(self,
            window.rootUrl + "MailStation/MailManage/MailList",
            self.reportParams,
            self.renderMailList,
            null,
            sharedScriptManager.unblockUI);
    };

    self.renderMailList = function (result) {
        var mailListLength = result.length;
        var bodyData = [];
        for (var i = 0; i < mailListLength; i++) {
            var mail = result[i];
            var rowColumns = [];
            var mailIndex = ' mailid="' + mail.MailId + '"';
            rowColumns.push({ Name: "ColId", Value: mail.MailId });
            rowColumns.push({
                Name: "ColMailName",
                Value:
                        viewResources.hasEditRight ?
                            ('<a class="link mail-name" href="javascript:;"' + mailIndex + '>' +
                                mail.MailName +
                            '</a>') :
                             mail.MailName
            });

            rowColumns.push({ Name: "ColGroup", Value: mail.MailGroupName });
            rowColumns.push({ Name: "ColSite", Value: mail.SiteName });
            rowColumns.push({ Name: "ColTrigger", Value: self.getTriggerIcon(mail) });
            rowColumns.push({ Name: "ColTo", Value: mail.MailTo });
            rowColumns.push({ Name: "ColCC", Value: mail.MailCc });
            rowColumns.push({ Name: "ColBCC", Value: mail.MailBcc });
            rowColumns.push({ Name: "ColLastSent", Value: mail.LastSent });
            rowColumns.push({ Name: "ColActive", Value: mail.IsActive ? '<div class="ico-tick"></div>' : '' });
            if (viewResources.hasEditRight) {
                rowColumns.push({
                    Name: "ColAction",
                    Value: '<div class="action-wrp">\
                                    <span class="btn-action"' + mailIndex + '>Action</span>\
                                    <div class="relative"></div>\
                                </div>'
                });
            }

            bodyData.push({
                Columns: rowColumns
            });
        }

        var headerColumns = [];
        headerColumns.push({ HeaderText: viewResources.Id, Width: 20, Name: "ColId", HeaderAlign: "center", CellAlign: "center" });
        headerColumns.push({ HeaderText: viewResources.Mail_Name, Width: 250, Name: "ColMailName", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: viewResources.Group, Width: 100, Name: "ColGroup", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true });
        headerColumns.push({ HeaderText: viewResources.Site, Width: 100, Name: "ColSite", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true });
        headerColumns.push({ HeaderText: viewResources.Trigger, Width: 100, Name: "ColTrigger", HeaderAlign: "center", CellAlign: "center" });
        headerColumns.push({ HeaderText: viewResources.To, Width: 130, Name: "ColTo", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true });
        headerColumns.push({ HeaderText: viewResources.CC, Width: 130, Name: "ColCC", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true });
        headerColumns.push({ HeaderText: viewResources.BCC, Width: 130, Name: "ColBCC", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true });
        headerColumns.push({ HeaderText: viewResources.Last_Sent, Width: 150, Name: "ColLastSent", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "MMM DD, YYYY hh:mm:ss A", AllowTruncate: true });
        headerColumns.push({ HeaderText: viewResources.Active, Width: 40, Name: "ColActive", HeaderAlign: "center", CellAlign: "center" });
        if (viewResources.hasEditRight) {
            headerColumns.push({ HeaderText: viewResources.Action, Width: 50, Name: "ColAction", HeaderAlign: "center", CellAlign: "center" });
        }

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

        DOMElements.gridContainer.removeClass("display-none").fanexGrid(gridOptions);
        self.bindGridEvent();
    };

    self.bindGridEvent = function () {
        $(".btn-action").click(function (event) {
            var $this = $(this);
            var position = $this.offset();
            contextMenuController.showContextMenu(position);
            var mailIndex = parseInt($this.attr("mailid"));
            contextMenuController.bindEvents({
                editEvent: function () {
                    self.editMail(mailIndex);
                },
                deleteEvent: function () {
                    self.deleteMail(mailIndex);
                },
                copyEvent: function () {
                    self.copyMail(mailIndex);
                }
            });

            event.stopPropagation();
        });

        DOMElements.gridContainer.find(".link.mail-name").click(function () {
            var $this = $(this);
            var mailid = parseInt($this.attr("mailid"));
            self.editMail(mailid);
        });
    };

    self.refreshMailList = function () {
        sharedScriptManager.blockUI();
        NAP.headlineMsgBox.close();
        sharedScriptManager.callAjax(self,
            window.rootUrl + "MailStation/MailManage/MailList",
            self.reportParams,
            self.renderMailList,
            null,
            sharedScriptManager.unblockUI);
    };

    self.getTriggerIcon = function (mail) {
        switch (mail.TriggerType) {
            case 0:
                return '<div class="trigger-type trigger-function"></div>';
            case 1:
                return '<div class="trigger-type trigger-schedule"></div>';
            default:
                return '';
        }
    };

    self.deleteMail = function (mailid) {
        NAP.headlineMsgBox.confirm(viewResources.Confirm_DeleteMail, function () {
            sharedScriptManager.blockUI();
            NAP.headlineMsgBox.close();
            var deleteParams = {
                MailId: mailid
            };
            sharedScriptManager.callAjax(self,
                window.rootUrl + "MailStation/MailManage/DeleteMail",
                deleteParams,
                self.renderActionResult,
                null,
                null);
        });
    };

    self.editMail = function (mailId) {
        window.location.href = window.rootUrl + "MailStation/MailManage/UpdateMailView?" +
            $.param({
                mailId: mailId
            });
    };

    self.copyMail = function (mailId) {
        window.location.href = window.rootUrl + "MailStation/MailManage/CreateMailView?" +
            $.param({
                mailId: mailId
            });
    };

    self.renderActionResult = function (result) {
        var result = {
            Code: 0,
            Message: 'Mail is deleted successfully.'
        };
        sharedScriptManager.renderActionResult(
            result,
            function () {
                setTimeout(function () {
                    self.refreshMailList();
                }, sharedScriptManager.cachedData.alertSuccessTime);
            });
        };
    };
    self.initRecipientInfo = function (recipient) {

};

var MailManageController = new MailManageController();
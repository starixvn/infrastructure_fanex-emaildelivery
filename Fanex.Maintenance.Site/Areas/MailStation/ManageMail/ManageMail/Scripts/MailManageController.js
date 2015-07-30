$(document).ready(function () {
    MailManageController.initPage();
});

var MailManageController = function () {
    var self = this;
    var DOMElements = {
        MailName: null,
        IsActive: null,
        MailGroupId: null,
        MailGroup: null,
        SiteId: null,
        TriggerType: null,
        Emails: null,
        btnSearch: null
    };

    var cacheObject = {
        bodyData: null
    };

    self.reportParams = {
        MailName: null,
        IsActive: null,
        MailGroupId: null,
        SiteId: null,
        TriggerType: null,
        Emails: null,
        btnSearch: null,
        gridContainer: null
    };

    self.initPage = function () {
        self.cacheDOMElements();
        self.renderMailGroupFilter();
    };

    self.cacheDOMElements = function () {
        DOMElements.MailName = $("#MailName");
        DOMElements.IsActive = $("#IsActive");
        DOMElements.MailGroupId = $("#MailGroupId");
        DOMElements.SiteId = $("#SiteId");
        DOMElements.TriggerType = $("#TriggerType");
        DOMElements.Emails = $("#Emails");
        DOMElements.btnSearch = $('#button-search');
        DOMElements.gridContainer = $('#mail-list');
        DOMElements.btnSearch = $('#button-search');
        DOMElements.MailGroup = $('#dropdownMailGroup');
    };

    self.bindEvents = function () {
        DOMElements.btnSearch.click(function () {
            self.getMailList();
        });

        $([DOMElements.MailName, DOMElements.Emails]).each(function () {
            $(this).keypress(function (e) {
                if (e.which == 13) {
                    self.getMailList();
                }
            });
        });
    };

    self.renderMailGroupFilter = function (result) {
        sharedScriptManager.callAjax(self,
            window.rootUrl + "MailStation/MailManage/MailGroups",
            null,
            function (result) {
                var mailGroupID = parseInt(DOMElements.MailGroupId.val());
                DOMElements.MailGroup.empty();
                if (result && result.length > 0) {
                    var optionLength = result.length;
                    var ddlData = [];
                    var prefixId = 'ddlGroup_';
                    for (var i = 1; i < optionLength; i++) {
                        var info = result[i];
                        ddlData.push({
                            Id: prefixId + info.Value,
                            Value: info.Value,
                            Text: info.Text,
                            Checked: isNaN(mailGroupID) ? true : parseInt(info.Value) === mailGroupID,
                            Enable: true,
                            CssClass: "",
                            Items: []
                        });
                    }

                    DOMElements.MailGroup.multiSelect(
                    {
                        minWidth: 158,
                        maxHeight: 450,
                        shadow: true,
                        showHeader: true,
                        isCheckAll: isNaN(mailGroupID),
                        showFooter: false,
                        showTooltip: false,
                        checkAllText: "All",
                        isUsedCustomCheckAllText: true,
                        customCheckAllText: "All",
                        noItemText: "No Group is selected",
                        selectedList: 3,
                        listData: ddlData
                    });

                    DOMElements.MailGroup = $('#dropdownMailGroup');
                }

                self.bindEvents();
                DOMElements.TriggerType.val(-1);
                contextMenuController.bindContextMenuEvents();
                DOMElements.btnSearch.click();
            },
            null,
            sharedScriptManager.unblockUI);

    };

    self.getMailList = function () {
        sharedScriptManager.blockUI();
        NAP.headlineMsgBox.close();
        self.reportParams = {
            MailName: DOMElements.MailName.val(),
            IsActive: DOMElements.IsActive.val(),
            MailGroupId: DOMElements.MailGroup.multiSelect('getCheckedItems').join(','),
            SiteId: DOMElements.SiteId.val(),
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
                            ('<a title="' + mail.MailName + '" class="link mail-name" href="javascript:;"' + mailIndex + '>' +
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
            rowColumns.push({ Name: "ColLastSent", Value: mail.LastSent === null || mail.LastSent === '' ? 'N/A' : mail.LastSent });
            rowColumns.push({ Name: "ColActive", Value: mail.IsActive ? '<div class="ico-tick"></div>' : '' });
            if (viewResources.hasEditRight) {
                rowColumns.push({
                    Name: "ColAction",
                    Value: '<div class="action-wrp">\
                                    <span class="btn-action"' + mailIndex + '>Action</span>\
                                    <div class="relative"></div>\
                                </div>'
                });
                rowColumns.push({
                    Name: "ColRunNow",
                    Value: mail.TriggerType == 1 ? '<input type="submit" value="Run Now" class="button-run-now"' + mailIndex + '>' : ''
                });
            }

            bodyData.push({
                Columns: rowColumns
            });
        }

        var headerColumns = [];
        headerColumns.push({ HeaderText: viewResources.Id, Width: 20, Name: "ColId", HeaderAlign: "center", CellAlign: "center" });
        headerColumns.push({ HeaderText: viewResources.Mail_Name, Width: 215, Name: "ColMailName", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: viewResources.Group, Width: 90, Name: "ColGroup", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true });
        headerColumns.push({ HeaderText: viewResources.Site, Width: 100, Name: "ColSite", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true });
        headerColumns.push({ HeaderText: viewResources.Trigger, Width: 90, Name: "ColTrigger", HeaderAlign: "center", CellAlign: "center" });
        headerColumns.push({ HeaderText: viewResources.To, Width: 120, Name: "ColTo", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true });
        headerColumns.push({ HeaderText: viewResources.CC, Width: 110, Name: "ColCC", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true });
        headerColumns.push({ HeaderText: viewResources.BCC, Width: 110, Name: "ColBCC", HeaderAlign: "center", CellAlign: "left", AllowTruncate: true });
        headerColumns.push({ HeaderText: viewResources.Last_Sent, Width: 150, Name: "ColLastSent", HeaderAlign: "center", CellAlign: "center", FormatDateTime: "MMM DD, YYYY hh:mm:ss A", AllowTruncate: true });
        headerColumns.push({ HeaderText: viewResources.Active, Width: 40, Name: "ColActive", HeaderAlign: "center", CellAlign: "center" });
        if (viewResources.hasEditRight) {
            headerColumns.push({ HeaderText: viewResources.Action, Width: 50, Name: "ColAction", HeaderAlign: "center", CellAlign: "center" });
            headerColumns.push({ HeaderText: '', Width: 80, Name: "ColRunNow", HeaderAlign: "center", CellAlign: "center" });
        }

        var reRenderAfterPaged = function (page_index, itemsPerPage, jq) {
            gridOptions.pagerOption.itemsPerPage = itemsPerPage;
            gridOptions.pagerOption.currentPage = page_index;
            var startIndex = (page_index * itemsPerPage);
            var endIndex = ((page_index + 1) * itemsPerPage);
            var currentBodyRows = cacheObject.bodyData.slice(startIndex, endIndex);
            gridOptions.bodyRows = currentBodyRows;
            DOMElements.gridContainer.fanexGrid(gridOptions); 
            self.bindGridEvent();
            return false;
        };

        cacheObject.bodyData = bodyData;
        var startIndex = 0;
        var currentBodyRows = cacheObject.bodyData.slice(0, sharedScriptManager.cachedData.defaultItemsPerPage);
        var gridOptions = {
            columns: headerColumns,
            showTitle: false,
            bodyRows: currentBodyRows,
            showDistinctRow: false,
            showHighlightRow: true,
            showPager: true,
            customPager: true,
            timeZone: viewResources.TimeZone,
            gridExpandHeight: 350,
            pagerOption: {
                itemsPerPage: sharedScriptManager.cachedData.defaultItemsPerPage,
                currentPage: 0,
                itemsPerPageArray: sharedScriptManager.cachedData.pagingOptions,
                totalItem: cacheObject.bodyData.length,
                pagerSelectCallBack: reRenderAfterPaged
            }
        };

        DOMElements.gridContainer.removeClass("display-none").fanexGrid(gridOptions);
        self.bindGridEvent();
        $('.mail-name').parent().addClass('column-truncate');
        DOMElements.gridContainer.fanexGrid('resizeGrid');
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

        $(".button-run-now").click(function (event) {
            var $this = $(this);
            var mailIndex = parseInt($this.attr("mailid"));
            self.runNow(mailIndex, function () {
                setTimeout(function () {
                    NAP.headlineMsgBox.close();
                    // self.getMailList();
                }, sharedScriptManager.cachedData.alertSuccessTime);
            });
        });

        DOMElements.gridContainer.find(".link.mail-name").click(function () {
            var $this = $(this);
            var mailid = parseInt($this.attr("mailid"));
            self.editMail(mailid);
        });
    };

    self.runNow = function (mailIndex, callBack) {
        NAP.headlineMsgBox.confirm(viewResources.Confirm_RunNow, function () {
            sharedScriptManager.blockUI();
            var runNowParams = {
                mailId: mailIndex
            };
            sharedScriptManager.callAjax(self,
            window.rootUrl + "MailStation/MailManage/RunNow",
            runNowParams,
            function (result) {
                result = {
                    Code: 0,
                    Message: 'Job has been started.'
                };
                sharedScriptManager.renderActionResult(result, callBack);
            },
            null,
            sharedScriptManager.unblockUI);
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

var MailManageController = new MailManageController();
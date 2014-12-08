$(document).ready(function () {
    MailManageController.initPage();
});

var MailManageController = function () {
    var self = this;
    self.cachedData = {
        mailList: [],
        searchParams: null
    };

    self.initPage = function () {
        self.loadFilter();
        self.bindEvents();
        contextMenuController.bindContextMenuEvents();
        self.buttonSearchClick();
    };

    self.loadFilter = function () {
        self.renderDropdownStatus();
        sharedScriptManager.callAjax(self,
            window.rootUrl + "MailStation/MailManage/MailGroups",
            null,
            self.renderDropdownGroup,
            null,
            sharedScriptManager.unblockUI);

        sharedScriptManager.callAjax(self,
            window.rootUrl + "MailStation/MailManage/Sites",
            null,
            self.renderDropdownSite,
            null,
            sharedScriptManager.unblockUI);

        self.renderDropdownTrigger();
    };

    self.renderDropdownSite = function (sites) {
        var options = '';
        var siteLength = sites.length;
        sites.unshift({ Id: -1, Name: "All" });
        for (var i = 0; i < siteLength; i++) {
            var site = sites[i];
            options += '<option value="' + site.Id + '">' + site.Name + '</option>'
        }

        $("#site").empty().append(options);
    };

    self.renderDropdownTrigger = function () {
        var triggers = [{ Id: -1, Name: 'All Types' },
            { Id: 0, Name: 'By Schedule' },
            { Id: 1, Name: 'By Function'}];
        var options = '';
        var triggerLength = triggers.length;
        for (var i = 0; i < triggerLength; i++) {
            var trigger = triggers[i];
            options += '<option value="' + trigger.Id + '">' + trigger.Name + '</option>'
        }

        $("#trigger").empty().append(options);
    };

    self.renderDropdownStatus = function () {
        var statuses = [{ Id: -1, Name: 'All' },
            { Id: 0, Name: 'Active' },
            { Id: 1, Name: 'Inactive'}];
        var options = '';
        var statusLength = statuses.length;
        for (var i = 0; i < statusLength; i++) {
            var status = statuses[i];
            options += '<option value="' + status.Id + '">' + status.Name + '</option>'
        }

        $("#status").empty().append(options);
    };

    self.renderDropdownGroup = function (mailGroupList) {
        mailGroupList.unshift({ Id: -1, Name: "All" });
        var options = '';
        var groupLength = mailGroupList.length;
        for (var i = 0; i < groupLength; i++) {
            var group = mailGroupList[i];
            options += '<option value="' + group.Id + '">' + group.Name + '</option>'
        }

        $("#group").empty().append(options);

    };

    self.bindEvents = function () {
        $("#button-search").click(function () {
            self.buttonSearchClick();
        });
    };

    self.buttonSearchClick = function () {
        sharedScriptManager.blockUI();
        NAP.headlineMsgBox.close();
        var searchParams = {
            MailName: $("#mail-name").val(),
            Status: $("#status").val(),
            Group: $("#group").val(),
            Site: viewResources.hasSiteRight ? $("#site").val() : 0,
            TriggerType: $("#trigger").val(),
            Emails: $("#emails").val()
        };
        self.cachedData.searchParams = searchParams;
        sharedScriptManager.callAjax(self,
            window.rootUrl + "MailStation/MailManage/MailList",
            searchParams,
            self.renderMailList,
            null,
            sharedScriptManager.unblockUI);
    };

    self.renderMailList = function (result) {
        result = self.dummyData();
        self.cachedData.mailList = result;
        var mailListLength = result.length;
        var bodyData = [];
        for (var i = 0; i < mailListLength; i++) {
            var mail = result[i];
            var rowColumns = [];
            var mailIndex = ' mail-index="' + i + '"';
            rowColumns.push({ Name: "ColId", Value: mail.Id });
            rowColumns.push({
                Name: "ColMailName",
                Value:
                        viewResources.hasEditRight ?
                            ('<a class="link mail-name" href="javascript:;"' + mailIndex + '>' +
                                mail.MailName +
                            '</a>') :
                             mail.MailName
            });

            rowColumns.push({ Name: "ColGroup", Value: mail.Group });
            rowColumns.push({ Name: "ColSite", Value: mail.Site });
            rowColumns.push({ Name: "ColTrigger", Value: self.getTriggerIcon(mail) });
            rowColumns.push({ Name: "ColTo", Value: mail.To });
            rowColumns.push({ Name: "ColCC", Value: mail.CC });
            rowColumns.push({ Name: "ColBCC", Value: mail.BCC });
            rowColumns.push({ Name: "ColLastSent", Value: mail.LastSent });
            rowColumns.push({ Name: "ColActive", Value: mail.Active ? '<div class="ico-tick"></div>' : '' });
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
        headerColumns.push({ HeaderText: viewResources.Id, Width: 40, Name: "ColId", HeaderAlign: "center", CellAlign: "center" });
        headerColumns.push({ HeaderText: viewResources.Mail_Name, Width: 300, Name: "ColMailName", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: viewResources.Group, Width: 120, Name: "ColGroup", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: viewResources.Site, Width: 120, Name: "ColSite", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: viewResources.Trigger, Width: 120, Name: "ColTrigger", HeaderAlign: "center", CellAlign: "center" });
        headerColumns.push({ HeaderText: viewResources.To, Width: 120, Name: "ColTo", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: viewResources.CC, Width: 120, Name: "ColCC", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: viewResources.BCC, Width: 120, Name: "ColBCC", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: viewResources.Last_Sent, Width: 120, Name: "ColLastSent", HeaderAlign: "center", CellAlign: "center" });
        headerColumns.push({ HeaderText: viewResources.Active, Width: 120, Name: "ColActive", HeaderAlign: "center", CellAlign: "center" });
        if (viewResources.hasEditRight) {
            headerColumns.push({ HeaderText: viewResources.Action, Width: 120, Name: "ColAction", HeaderAlign: "center", CellAlign: "center" });
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
        $("#mail-list").removeClass("display-none").fanexGrid(gridOptions);
        self.bindGridEvent();
    };
    self.bindGridEvent = function () {
        var gridContainer = $("#mail-list");
        $(".btn-action").click(function (event) {
            var $this = $(this);
            var position = $this.offset();
            contextMenuController.showContextMenu(position);
            var mailIndex = parseInt($this.attr("mail-index"));
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

        gridContainer.find(".link.mail-name").click(function () {
            var $this = $(this);
            var mailIndex = parseInt($this.attr("mail-index"));
            self.editMail(mailIndex);
        });
    };

    self.refreshMailList = function () {
        sharedScriptManager.blockUI();
        NAP.headlineMsgBox.close();
        sharedScriptManager.callAjax(self,
            window.rootUrl + "MailStation/MailManage/MailList",
            self.cachedData.searchParams,
            self.renderMailList,
            null,
            sharedScriptManager.unblockUI);
    };

    self.getTriggerIcon = function (mail) {
        switch (mail.TriggerType) {
            case 'By Function':
                return '<div class="trigger-type trigger-function"></div>';
            case 'By Schedule':
                return '<div class="trigger-type trigger-schedule"></div>';
            default:
                return '';
        }
    };

    self.deleteMail = function (mailIndex) {
        NAP.headlineMsgBox.confirm(viewResources.Confirm_DeleteMail, function () {
            sharedScriptManager.blockUI();
            NAP.headlineMsgBox.close();
            var mail = self.cachedData.mailList[mailIndex];
            var deleteParams = {
                MailId: mail.Id
            };
            sharedScriptManager.callAjax(self,
                window.rootUrl + "MailStation/MailManage/DeleteMail",
                deleteParams,
                self.renderActionResult,
                null,
                null);
        });
    };

    self.editMail = function (mailIndex) {
        var mail = self.cachedData.mailList[mailIndex];
        var editParams = mail;
        window.location.href = window.rootUrl + "MailStation/MailManage/EditMail?" +
            $.param({
                mailId: mail.Id
            });
    };

    self.copyMail = function (mailIndex) {
        var mail = self.cachedData.mailList[mailIndex];
        var copyParams = mail;
        window.location.href = window.rootUrl + "MailStation/MailManage/CopyMail?" +
            $.param({
                mailId: mail.Id
            });
    };

    self.renderActionResult = function (result) {
        var result = {
            Code: 0,
            Message: 'aaa'
        };
        sharedScriptManager.renderActionResult(
            result,
            function () {
                setTimeout(function () {
                    self.refreshMailList();
                }, sharedScriptManager.cachedData.alertSuccessTime);
            });
    };

    self.dummyData = function () {
        var result = [];
        result.push({
            Id: 1,
            MailName: 'Name 1',
            Group: 'Licensee',
            Site: 'System Configuration',
            TriggerType: 'By Function',
            To: 'abc@gmail.com',
            CC: '',
            BCC: '',
            LastSent: 'Sep 24,2014 06:08 AM',
            Active: true
        });
        result.push({
            Id: 2,
            MailName: 'Name 2',
            Group: 'Licensee',
            Site: 'System Configuration',
            TriggerType: 'By Schedule',
            To: 'abc@gmail.com',
            CC: '',
            BCC: '',
            LastSent: 'Sep 24,2014 06:08 AM',
            Active: false
        });

        for (var i = 0; i < 5; i++) {
            result.push(result[0]);
        }

        return result;
    };
};

var MailManageController = new MailManageController();
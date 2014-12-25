$(document).ready(function () {
    manageRecipientController.initPage();
});

var ManageRecipientController = function () {
    var self = this;
    self.initPage = function () {
        self.loadFilter();
        self.bindEvents();
        self.buttonSearchClick();
        contextMenuController.bindContextMenuEvents();
    };
    self.cachedData = {
        searchParams: null
    };
    var DOMElements = {
        emails: null
    };

    self.loadFilter = function () {
        self.renderDropdownStatus();
    };

    self.renderDropdownStatus = function () {
        var statuses = [{ Id: -1, Name: 'All' },
            { Id: 1, Name: 'Active' },
            { Id: 0, Name: 'Inactive'}];
        var options = '';
        var statusLength = statuses.length;
        for (var i = 0; i < statusLength; i++) {
            var status = statuses[i];
            options += '<option value="' + status.Id + '">' + status.Name + '</option>'
        }

        $("#status").empty().append(options);
    };
    self.bindEvents = function () {
        $("#typeCust").attr("disabled", null);
        $("#button-search").click(function () {
            self.buttonSearchClick();
        });
    };
    self.buttonSearchClick = function () {
        sharedScriptManager.blockUI();
        var searchParams = {
            Username: $("#UserName").val(),
            Emails: $("#Emails").val(),
            Status: $("#Status").val()
        };
        self.cachedData.searchParams = searchParams;
        sharedScriptManager.callAjax(self,
            window.rootUrl + "MailStation/MailRecipient/RecipientList",
            searchParams,
            self.renderRecipientList,
            null,
            sharedScriptManager.unblockUI);
    };
    self.renderRecipientList = function (result) {
        //        result = self.dummyData();
        var recipientListLength = result.length;
        var bodyData = [];
        for (var i = 0; i < recipientListLength; i++) {
            var recipient = result[i];
            var rowColumns = [];
            var recipientIndex = ' recipient-index="' + recipient.RecipientId + '"';
            rowColumns.push({ Name: "ColId", Value: recipient.RecipientId });
            rowColumns.push({
                Name: "ColUsername",
                Value: viewResources.hasEditRight ?
                            ('<a class="link username" href="javascript:;"' + recipientIndex + '>' +
                               recipient.RecipientName +
                            '</a>') :
                             recipient.RecipientName
            });
            rowColumns.push({ Name: "ColNapUser", Value: recipient.IsNapUser ? '<div class="ico-tick"></div>' : '' });
            rowColumns.push({ Name: "ColEmail1", Value: recipient.Email1 });
            rowColumns.push({ Name: "ColEmail2", Value: recipient.Email2 });
            rowColumns.push({ Name: "ColEmail3", Value: recipient.Email3 });
            rowColumns.push({
                Name: "ColNoRelevantMail",
                Value: '<div class="ico-relevant-mail"' + recipientIndex + '>' + recipient.MailRelevant + '</div>'
            });
            rowColumns.push({ Name: "ColActive", Value: recipient.IsActive ? '<div class="ico-tick"></div>' : '' });
            if (viewResources.hasEditRight) {
                rowColumns.push({
                    Name: "ColAction",
                    Value: '<div class="action-wrp">\
                                    <span class="btn-action"' + recipientIndex + '>Action</span>\
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
        headerColumns.push({ HeaderText: viewResources.Username, Width: 300, Name: "ColUsername", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: viewResources.Nap_User, Width: 120, Name: "ColNapUser", HeaderAlign: "center", CellAlign: "center" });
        headerColumns.push({ HeaderText: viewResources.Email1, Width: 120, Name: "ColEmail1", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: viewResources.Email2, Width: 120, Name: "ColEmail2", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: viewResources.Email3, Width: 120, Name: "ColEmail3", HeaderAlign: "center", CellAlign: "left" });
        headerColumns.push({ HeaderText: viewResources.No_RelevantMail, Width: 120, Name: "ColNoRelevantMail", HeaderAlign: "center", CellAlign: "center" });
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
        $("#recipient-list").removeClass("display-none").fanexGrid(gridOptions);
        self.bindGridEvent();
    };

    self.bindGridEvent = function () {
        var gridContainer = $("#recipient-list");
        $(".btn-action").click(function (event) {
            var $this = $(this);
            var position = $this.offset();
            contextMenuController.showContextMenu(position);
            var recipientIndex = parseInt($this.attr("recipient-index"));
            contextMenuController.bindEvents({
                editEvent: function () {
                    self.editRecipient(recipientIndex);
                }
            });

            event.stopPropagation();
        });

        gridContainer.find(".link.username").click(function () {
            var $this = $(this);
            var recipientIndex = parseInt($this.attr("recipient-index"));
            self.editRecipient(recipientIndex);
        });

        gridContainer.find(".ico-relevant-mail").click(function () {
            var $this = $(this);
            var recipientIndex = parseInt($this.attr("recipient-index"));
            self.viewRelevantMail(recipientIndex);
        });
    };

    self.editRecipient = function (recipientIndex) {
        window.location.href = window.rootUrl + "MailStation/MailRecipient/UpdateRecipientView?" +
            $.param({
                recipientId: recipientIndex
            });
        };

    self.viewRelevantMail = function (recipientIndex) {
        window.location.href = window.rootUrl + 'MailStation/MailRecipient/ViewRelevantMail?' + $.param({
            recipientId: recipientIndex
        });
    };
};

var manageRecipientController = new ManageRecipientController();

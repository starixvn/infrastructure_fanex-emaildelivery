
function renderGrid() {
    var bodyData = [];
    bodyData.push({
        Columns: [{ Name: "ColId", Value: "1" },
            { Name: "ColMailName", Value: '<a class="link"  href="javascript:;"> Name 1 </a>' },
            { Name: "ColGroup", Value: "Licensee" },
            { Name: "ColSite", Value: "System Configuration" },
            { Name: "ColTrigger", Value: '<div class="trigger-type trigger-function"></div>' },
            { Name: "ColTo", Value: "abc@gmail.com" },
            { Name: "ColCC", Value: "" },
            { Name: "ColBCC", Value: "" },
            { Name: "ColLastSent", Value: "Sep 24,2014 06:08 AM" },
            { Name: "ColActive", Value: '<img class="ico-tick" />' },
            {
                Name: "ColAction",
                Value: '<div class="action-wrp">\
                            <span class="btn-action">Action</span>\
                            <div class="relative"></div>\
                        </div>'
            }]
    });
    bodyData.push({
        Columns: [{ Name: "ColId", Value: "1" },
            { Name: "ColMailName", Value: "Name 1" },
            { Name: "ColGroup", Value: "Licensee" },
            { Name: "ColSite", Value: "System Configuration" },
            { Name: "ColTrigger", Value: '<div class="trigger-type trigger-schedule"></div>' },
            { Name: "ColTo", Value: "abc@gmail.com" },
            { Name: "ColCC", Value: "" },
            { Name: "ColBCC", Value: "" },
            { Name: "ColLastSent", Value: "Sep 24,2014 06:08 AM" },
            { Name: "ColActive", Value: "" },
            { Name: "ColAction", Value: ""}]
    });

    for (var i = 0; i < 5; i++) {
        bodyData.push(bodyData[0]);
    }

    var gridOptions = {
        columns: [
            { HeaderText: "ID", Width: 40, Name: "ColId", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Mail Name", Width: 300, Name: "ColMailName", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Group", Width: 120, Name: "ColGroup", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Site", Width: 120, Name: "ColSite", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Trigger", Width: 120, Name: "ColTrigger", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "To", Width: 120, Name: "ColTo", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "CC", Width: 120, Name: "ColCC", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "BCC", Width: 120, Name: "ColBCC", HeaderAlign: "center", CellAlign: "left" },
            { HeaderText: "Last Sent", Width: 120, Name: "ColLastSent", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Active", Width: 120, Name: "ColActive", HeaderAlign: "center", CellAlign: "center" },
            { HeaderText: "Action", Width: 120, Name: "ColAction", HeaderAlign: "center", CellAlign: "center"}],
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
            itemsPerPage: 50,
            currentPage: 0,
            itemsPerPageArray: [50, 100, 200, 500],
            totalItem: 800
        }
    };

    $("#mail-list").removeClass("display-none").fanexGrid(gridOptions);
    $(".btn-action").click(function (event) {

        var $this = $(this);
        var position = $this.offset();

        var actionsGroup = $(".actions");
        actionsGroup.removeClass("display-none");
        actionsGroup.css({
            top: position.top + "px",
            left: (position.left - 200) + "px",
            display: "block"
        });

        event.stopPropagation()
    });
}

$(document).ready(function () {
    renderGrid();
    $(document).click(function () {
        var actionsGroup = $(".actions");
        actionsGroup.addClass("display-none");
    });
});
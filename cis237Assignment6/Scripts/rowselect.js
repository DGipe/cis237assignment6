function addRowHandlers() {
    var table = document.getElementById("tableId");
    var rows = table.getElementsByTagName("tr");
    for (i = 0; i < rows.length; i++) {
        var currentRow = table.rows[i];
        var createClickHandler =
            function (row) {
                return function () {

                   var _row = row.getElementsByTagName("td")[0];
                   var id = _row.innerHTML.trim();
                                   
                   window.location.href = 'Beverages/Edit/'+id;
                };
            };

        currentRow.onclick = createClickHandler(currentRow);
    }
}
window.onload = addRowHandlers();
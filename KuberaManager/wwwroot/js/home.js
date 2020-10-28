// Create timer & do first run on all tables on page load
$(document).ready(() => {
    refreshAll();
    setInterval(() => {
        refreshAll();
    }, 10000);
});

// Refreshes all data
function refreshAll() {
    loadAllBotStatus();
}


// --- Individual load functions --- //
function loadAllBotStatus() {
    $.ajax({
        url: "/api/ajax/AllBotStatus",
        cache: false,
        success: function (data) {
            var html = "";

            $.each(data, (k, row) => {
                html += "<tr>";

                // Account
                html += "<td>";
                if (row["isRunning"])
                    html += "<i class='nav-icon fas fa-circle text-success'></i>";
                else html += "<i class='nav-icon far fa-circle text-warning'></i>";
                html += " " + row.account + "</td>";


                // Session (progress)
                // startTime, stopTime exist if you can convert to ppl format
                html += "<td><div class='progress progress-xs mt-2'><div class='progress-bar progress-bar-info' style='width: " + row["percentageComplete"] + "%'></div></div></td>";

                // Job
                html += "<td>" + (row["activeJob"] == null ? "" : row["activeJob"]) + "</td>";

                // Host
                html += "<td>" + (row["host"] == null ? "" : row["host"]) + "</td>";

                // end row
                html += "</tr>";
            });

            // Replace table
            $("#tbodyAllBotStatus").empty();
            $("#tbodyAllBotStatus").html(html);
        }
    });
}
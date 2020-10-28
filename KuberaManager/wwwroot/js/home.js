// Create timer & do first run on all tables on page load
$(document).ready(() => {
    // Load data & start timer
    refreshAll();
    setInterval(() => {
        refreshAll();
    }, 60000);
});


// Refreshes all data
function refreshAll() {
    loadAllBotStatus();
}


// --- Variables --- //
var lastSelectedAccountId = 0;

// --- Individual load functions --- //
function loadAllBotStatus() {
    $.ajax({
        url: "/api/ajax/AllBotStatus",
        cache: false,
        success: function (data) {
            // Clear table
            $("#tbodyAllBotStatus").empty();

            // Add all rows
            $.each(data, (k, row) => {
                var html = "<tr class='botStatusRow' data-account='" + row.accountId +"' data-login='"+row.login+"'>";

                // Account
                html += "<td>";
                if (row["isRunning"])
                    html += "<i class='nav-icon fas fa-circle text-success'></i>";
                else html += "<i class='nav-icon far fa-circle text-warning'></i>";
                html += " " + row.login + "</td>";


                // Session (progress)
                // startTime, stopTime exist if you can convert to ppl format
                html += "<td><div class='progress progress-xs mt-2'><div class='progress-bar progress-bar-info' style='width: " +
                    row["percentageComplete"] + "%'></div></div></td>";

                // Job
                html += "<td>" + (row["activeJob"] == null ? "" : row["activeJob"]) + "</td>";

                // Host
                html += "<td>" + (row["host"] == null ? "" : row["host"]) + "</td>";

                // end row
                html += "</tr>";
                // Replace table
                $("#tbodyAllBotStatus").append(html);

                // Add clickable option
                $(".botStatusRow[data-account=" + row.accountId + "]").click((handler, eventData) => {
                    // Store active account
                    lastSelectedAccountId = row.accountId;

                    // Load the correct account levels tab
                    loadAccountLevels(row.accountId, row.login);
                });

                // Click the box if it was previously highlighted
                if (row.accountId == lastSelectedAccountId || lastSelectedAccountId == 0) {
                    $(".botStatusRow[data-account=" + row.accountId + "]").click();
                }
            });


        }
    });
}

//https://localhost:44393/api/ajax/getlevels/1
function loadAccountLevels(accountId, login) {
    // Remove previous highlight & highlight clicked box
    $(".botStatusRow.bg-info").removeClass("bg-info");
    $(".botStatusRow[data-account=" + accountId + "]").addClass("bg-info");

    // Get level data
    $.ajax({
        url: "/api/ajax/GetLevels/" + accountId,
        cache: false,
        success: function (data) {
            // Set the title
            $("#skillContainerTitle").html(login);

            // Prepare container contents
            $("#skillBoxContainer").empty();
            var html = "<div class='row'>";
            var totalLevel = 0;
            $.each(data, function (skill, level) {
                // skip account id, append skill
                if (skill != "accountId") {
                    totalLevel += level;
                    html += "<div class='col-md-4 border border-dark rounded'><img class='skill-img mr-2' src='/img/skill-icons/" + skill +
                        ".webp'><span class='skillLevel'>" + level + "</span></div>";
                }
            });

            // Append total level
            html += "<div class='col-md-4 border border-dark rounded'><i class='fas fa-arrow-alt-circle-up mr-2 skill-img'></i><strong>" + totalLevel + "</strong></div>"

            // Close row
            html += "</div>";

            // Set the skills
            $("#skillBoxContainer").html(html);
        }
    });
}
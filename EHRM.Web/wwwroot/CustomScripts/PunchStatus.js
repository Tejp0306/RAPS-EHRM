$(document).ready(function () {
    getPunchData();
});

function getPunchData() {
    debugger;
    const table = $('#punchTable').DataTable({
        ajax: {
            url: '/Calendar/GetPunchData', // Controller action that fetches punch data
            method: 'GET',
            dataSrc: function (json) {
                console.log("Received data from the server:", json); // Log the full response

                // Check if 'data' is directly available in the response
                if (json && json.data) {
                    return json.data; // Return the data inside 'data' to DataTable
                } else {
                    alert(json.Message || "No data available!"); // Show the error message if something went wrong
                    return []; // Return empty array if no data is found
                }
            }
        },

        columns: [
            { data: 'employeeName' },
            { data: 'punchDate' },
            { data: 'punchintime' },
            { data: 'punchouttime' },
            { data: 'totalhours' },
        ],
        pageLength: 5, // Default rows per page
        lengthMenu: [5, 10, 15, 20], // Options for rows per page
        language: {
            search: "Search",
            lengthMenu: "Show _MENU_ entries",
            info: "Showing _START_ to _END_ of _TOTAL_ Main Menu",
            infoEmpty: "No records available",
            paginate: {
                first: "First",
                last: "Last",
                next: "Next",
                previous: "Previous"
            }
        }
    });
}





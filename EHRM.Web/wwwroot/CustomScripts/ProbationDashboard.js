$(document).ready(function () {

    // Initialize DataTable and fetch data via AJAX
    const table = $('#EvaluationTable').DataTable({
        ajax: {
            url: `/Review/GetAllDetails`,  // API endpoint for the service
            method: 'GET',
            success: function (response) {
                // Check if the response indicates success
                if (response.success) {

                    table.clear().rows.add(response.data).draw();
                } else {
                    // Handle the case where the request was not successful
                    alert(response.message || "No data available.");
                }
            },
        },
        columns: [
            { data: 'id' },
            { data: 'firstName' },  // Accessing 'firstName' from the simplified object
            { data: 'lastName' },   // Accessing 'lastName' from the simplified object
            { data: 'recommendation' },
            { data: 'remarksConfirmation' }
        ],
        pageLength: 5, // Default rows per page
        lengthMenu: [5, 10, 15, 20], // Options for rows per page
        language: {
            search: "Search :",
            lengthMenu: "Show _MENU_ entries",
            paginate: {
                first: "First",
                last: "Last",
                next: "Next",
                previous: "Previous"
            }
        }
    });

});


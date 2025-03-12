$(document).ready(function () {
    // Get today's date in 'yyyy-mm-dd' format
    const today = new Date().toISOString().split('T')[0]; // Example: '2025-03-11'

    // Check if the DataTable is already initialized
    if ($.fn.dataTable.isDataTable('#punchTable')) {
        // Destroy the previous DataTable instance
        $('#punchTable').DataTable().clear().destroy();
    }
    debugger;
    // Initialize the DataTable
    const table = $('#punchTable').DataTable({
        ajax: {
            url: '/Calendar/GetPunchData', // Replace with your controller action
            method: 'GET',
            dataSrc: '', // Data source key, empty string assumes the root of the JSON array
            data: function (d) {
                // Add the selected date (today by default)
                d.punchDate = today;
            }
        },
        
        columns: [
       /*     { data: 'id' },*/
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
            info: "Showing _START_ to _END_ of _TOTAL_ entries",
            infoEmpty: "No records available",
            paginate: {
                first: "First",
                last: "Last",
                next: "Next",
                previous: "Previous"
            }
        }
    });

    // Add an event listener for a date range change (e.g., a date picker)
    $('#dateFilter').on('change', function () {
        const selectedDate = $(this).val(); // Get the selected date
        // Reload the table with the new date filter
        table.ajax.url('/Calendar/GetPunchData?punchDate=' + selectedDate).load();
    });
});



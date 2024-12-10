$(document).ready(function () {
 
    GetTeamsName();
    getHolidayData();

    // Handle View Button Click
    $('#holidayTable').on('click', '.view-btn', function () {

        const holidayID = $(this).data('id');
        EditHolidayDetails(holidayID)
    });

    // Handle Delete Button Click for NoticeBoard
    $('#holidayTable').on('click', '.delete-btn', function () {
        const holidayID = $(this).data('id');  // Get the Notice Board ID from the data-id attribute

        // Confirm before deleting
        if (confirm(`Are you sure you want to delete this Holiday Details?`)) {
            // Make AJAX request to delete the notice board
            $.ajax({
                url: `/Master/DeleteHoliday`,  // API endpoint to delete the notice board
                method: 'POST',
                data: { id: holidayID },
                success: function (response) {
                    if (response.success) {
                        // Remove the row from the table
                        // $(`#row-${noticeBoardID}`).remove(); // Assuming the table rows have IDs like 'row-1', 'row-2', etc.
                        // Optionally show a success message
                        alert(response.message);
                        location.reload();
                    } else {
                        // Handle error if the delete fails
                        alert(response.message || 'An error occurred while deleting the notice board.');
                    }
                },
                error: function (error) {
                    console.error('Error deleting notice board:', error);
                    alert('An error occurred while deleting the notice board.');
                }
            });
        }
    });
});


function EditHolidayDetails(holidayID) {
    $.ajax({
        url: '/Master/GetHolidayDetails/' + holidayID, // API endpoint
        method: 'GET',
        success: function (response) {
            // Log response for debugging
            console.log('Full Response:', response);
            console.log('Data:', response.data);
            //debugger;
            // Access the nested data object
            if (response.success && response.data && response.data.data) {
                const holidayData = response.data.data;

                $('#holidayId').val(holidayData.id || '');
                $('#Name').val(holidayData.name || '');
                $('#Description').val(holidayData.description || '');
                $('#HolidayDate').val(holidayData.holidayDate || '');
                $('#TeamId').val(holidayData.teamId);
                
            } else {
                console.error('Error: Invalid or missing data in response.');
            }
        },
        error: function (error) {
            console.error('AJAX Error:', error);
        }
    });
}


function GetTeamsName() {
    
    $.ajax({
        url: '/Master/GetAllTeamData', // Replace with your actual controller action
        method: 'GET',
        success: function (response) {
            
            if (response.success) {
                const teams = response.data; // Assuming Data contains the array of teams

                // Populate the dropdown
                const teamDropdown = $('#TeamId');
                teamDropdown.empty(); // Clear any existing options

                // Add a default option
                teamDropdown.append('<option value="" disabled selected>Select team</option>');
                teams.forEach(teams => {
                    teamDropdown.append(`<option value="${teams.id}">${teams.name}</option>`); // Adjust key names as needed
                });



                // Loop through the array and append options

            } else {
                console.error('Error:', response.Message);
            }
        },
        error: function (error) {
            console.error('AJAX Error:', error);
        }
    });

}

function getHolidayData() {
    
    
    const table = $('#holidayTable').DataTable({

        ajax: {
            url: '/Master/GetAllHolidayData', // Replace with your controller action
            method: 'GET',
            dataSrc: '' // Data source key, empty string assumes the root of the JSON array
        },
        columns: [
            
            { data: 'id' },
            { data: 'name' },
            { data: 'description' },
            { data: 'holidayDate' },
           
   
            {
                
                data: 'id',
                render: function (data) {
                    
                    //console.log(data);
                    return `
                                <div class="d-flex justify-content-center">
                                    <button class="btn btn-warning btn-sm mx-1 view-btn" data-id="${data}">
                                        <i class="bi bi-pencil"></i> Edit
                                    </button>
                                    <button class="btn btn-danger btn-sm mx-1 delete-btn" data-id="${data}">
                                        <i class="bi bi-trash"></i> Delete
                                    </button>
                                </div>
                            `;
                },
                orderable: false // Disable sorting on the Action column
            }
        ],
        pageLength: 5, // Default rows per page
        lengthMenu: [5, 10, 15, 20], // Options for rows per page
        language: {
            search: "Search",
            lengthMenu: "Show _MENU_ entries",
            info: "Showing _START_ to _END_ of _TOTAL_ Holiday",
            infoEmpty: "No prescribers available",
            paginate: {
                first: "First",
                last: "Last",
                next: "Next",
                previous: "Previous"
            }
        }
        
    });
}



$(document).ready(function () {
  
    getMainMenuData();

    //Logic for deleting row of Main menu Table
    $('#mainMenuTable').on('click', '.delete-btn', function () {
        const menuID = $(this).data('id');  // Get the Main menu ID from the data-id attribute

        // Confirm before deleting
        if (confirm(`Are you sure you want to delete this Main Menu?`)) {
            // Make AJAX request to delete the notice board
            $.ajax({
                url: `/Menu/DeleteMainMenu`,  // API endpoint to delete the notice board
                method: 'POST',
                data: { id: menuID },
                success: function (response) {
                    if (response.success) {
                        // Remove the row from the table
                        // $(`#row-${noticeBoardID}`).remove(); // Assuming the table rows have IDs like 'row-1', 'row-2', etc.
                        // Optionally show a success message
                        alert(response.message);
                        location.reload();
                    } else {
                        // Handle error if the delete fails
                        alert(response.message || 'An error occurred while deleting the Main Menu.');
                    }
                },
                error: function (error) {
                    console.error('Error deleting notice board:', error);
                    alert('An error occurred while deleting the notice board.');
                }
            });
        }
    });


    // EDit main menu

    $('#mainMenuTable').on('click', '.view-btn', function () {

        const mainmenuID = $(this).data('id');
        EditMainMenu(mainmenuID);
    });




});





function getMainMenuData() {


    const table = $('#mainMenuTable').DataTable({

        ajax: {
            url: '/Menu/GetAllMenuData', // Replace with your controller action
            method: 'GET',
            dataSrc: '' // Data source key, empty string assumes the root of the JSON array
        },
        columns: [

            { data: 'id' },
            { data: 'name' },
            { data: 'icon' },

            {

                data: 'id',
                render: function (data) {
                    console.log(data);
                    //debugger;
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
            info: "Showing _START_ to _END_ of _TOTAL_ Main Menu",
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


function EditMainMenu(mainmenuID) {
    $.ajax({
        url: '/Menu/GetMainMenuDetails/' + mainmenuID, // API endpoint
        method: 'GET',
        success: function (response) {
            // Log response for debugging
            console.log('Full Response:', response);
            //console.log('Data:', response.data);
            //debugger;
            // Access the nested data object
            if (response.success && response.data && response.data.data) {
                const mainmenuData = response.data.data;

                    $('#menuId').val(mainmenuData.id || '');
                    $('#Name').val(mainmenuData.name || '');
                    $('#Icon').val(mainmenuData.icon || '');

            } else {
                console.error('Error: Invalid or missing data in response.');
            }

            
        },
        error: function (error) {
            console.error('AJAX Error:', error);
        }
    });
}



$(document).ready(function () {
    // Initialize DataTable and fetch data via AJAX
    const table = $('#rolesTable').DataTable({
        ajax: {
            url: '/Master/GetAllRolesData', // Replace with your controller action
            method: 'GET',
            dataSrc: '' // Data source key, empty string assumes the root of the JSON array
        },
        columns: [
            { data: 'id' },
            { data: 'roleName' },
            { data: 'roleDescription' },
         
            {
                data: 'id',
                render: function (data) {
                    return `
                                <div class="d-flex justify-content-center">
                                    <button class="btn btn-info btn-sm mx-1 view-btn" data-id="${data}">
                                        <i class="bi bi-eye"></i> View
                                    </button>
                                    <button class="btn btn-warning btn-sm mx-1 edit-btn" data-id="${data}">
                                        <i class="bi bi-pencil"></i> Edit
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
            search: "Search Prescribers:",
            lengthMenu: "Show _MENU_ entries",
            info: "Showing _START_ to _END_ of _TOTAL_ prescribers",
            infoEmpty: "No prescribers available",
            paginate: {
                first: "First",
                last: "Last",
                next: "Next",
                previous: "Previous"
            }
        }
    });

    // Handle View Button Click
    $('#rolesTable').on('click', '.view-btn', function () {
        const roleID = $(this).data('id');
        alert(`View details for Role ID: ${roleID}`);
       // 
        // Add logic for viewing details in a modal or new page
    });

    // Handle Edit Button Click
    $('#rolesTable').on('click', '.edit-btn', function () {
        const roleID = $(this).data('id');
    
        editRoleDetails(roleID);
    });
});



// Separate method for viewing doctor details
function editRoleDetails(roleID) {
    $.ajax({
        url: '/Master/GetRoleDetails/' + roleID, // API endpoint
        method: 'GET',
        success: function (response) {
            // Log response for debugging
            console.log('Full Response:', response);
            console.log('Data:', response.data);

            // Access the nested data object
            if (response.success && response.data && response.data.data) {
                const roleData = response.data.data;

                $('#roleId').val(roleData.id || '');
                $('#roleName').val(roleData.roleName || '');
                $('#roleDescription').val(roleData.roleDescription || '');
            } else {
                console.error('Error: Invalid or missing data in response.');
            }
        },
        error: function (error) {
            console.error('AJAX Error:', error);
        }
    });
}




// Save changes when clicking "Save Changes"

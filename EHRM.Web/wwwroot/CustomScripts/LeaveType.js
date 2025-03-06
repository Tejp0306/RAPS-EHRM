$(document).ready(function () {
    getLeaveTypeData();

    //Handling LeaveType Delete
    $('#LeaveTypeTable').on('click', '.delete-btn', function () {
        const id = $(this).data('id');  // Get the Main menu ID from the data-id attribute
        debugger
        // Confirm before deleting
        if (confirm(`Are you sure you want to delete this Leave_Type?`)) {
            // Make AJAX request to delete the notice board
            $.ajax({
                url: `/Leave/DeleteLeaveType`,  // API endpoint to delete the notice board
                method: 'POST',
                data: { id: id },
                success: function (response) {
                    if (response.success) {

                        alert(response.message);
                        location.reload();
                    } else {
                        // Handle error if the delete fails
                        alert(response.message || 'An error occurred while deleting the Asset.');
                    }
                },
                error: function (error) {
                    console.error('Error deletingAsset:', error);
                    alert('An error occurred while deleting the Asset.');
                }
            });
        }
    });

    // EDit Asset

    $('#LeaveTypeTable').on('click', '.edit-btn', function () {

        const leaveTypeID = $(this).data('id');
        EditLeaveType(leaveTypeID);
    });
});

function getLeaveTypeData() {
    debugger;
    const table = $('#LeaveTypeTable').DataTable({
        ajax: {
            url: '/Leave/GetAllLeaveTypeData', // Replace with your controller action
            method: 'GET',
            dataSrc: '' // Data source key, empty string assumes the root of the JSON array
        },

        columns: [
            { data: 'id' },
            { data: 'leaveType' },
            { data: 'leaveDescription' },
            {
                data: 'id',
                render: function (data) {
                    return `
                        <div class="d-flex justify-content-center">
                            <button class="btn btn-success btn-sm mx-1 edit-btn" data-id="${data}">
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

function EditLeaveType(leaveTypeID) {
    $.ajax({
        url: '/Leave/GetLeaveTypeDetails/' + leaveTypeID, // API endpoint
        method: 'GET',
        success: function (response) {
            debugger;
            console.log('Full Response:', response);

            if (response.success && response.data && response.data.data) {
                const leaveTypeData = response.data.data;
                $('#leaveTypeID').val(leaveTypeData.id);
                $('#LeaveType').val(leaveTypeData.leaveType || '');
                $('#LeaveDescription').val(leaveTypeData.leaveDescription || '');

            }
            else {
                console.error('Error: Invalid or missing data in response.');
            }


        },
        error: function (error) {
            console.error('AJAX Error:', error);
        }
    });
}

$(document).ready(function () {
    GetTeamNames();
    getAssetData();

    //Handling Asset Delete
    $('#assetTable').on('click', '.delete-btn', function () {
        const assetID = $(this).data('id');  // Get the Main menu ID from the data-id attribute

        // Confirm before deleting
        if (confirm(`Are you sure you want to delete this Asset?`)) {
            // Make AJAX request to delete the notice board
            $.ajax({
                url: `/Asset/DeleteAsset`,  // API endpoint to delete the notice board
                method: 'POST',
                data: { id: assetID },
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

    $('#assetTable').on('click', '.edit-btn', function () {

        const assetID = $(this).data('id');
        EditAsset(assetID);
    });
});


function GetTeamNames() {

    $.ajax({
        url: '/Asset/GetAllTeamData', // Replace with your actual controller action
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
function GetEmployeeDetail(TeamId) {

    $.ajax({
        url: '/Asset/GetEmployeebyTeamId', // Replace with your actual controller action
        method: 'GET',
        data: { TeamId: TeamId },

        success: function (response) {

            if (response.success) {
                const emp = response.data.data; // Assuming data contains the array of employees

                // Populate the dropdown
                const userDropdown = $('#appointedTo');
                userDropdown.empty(); // Clear any existing options

                // Add a default option
                userDropdown.append('<option value="0" disabled selected>Select User</option>');

                // Check if emp is a valid array and iterate over it
                if (Array.isArray(emp) && emp.length > 0) {
                    emp.forEach(employee => {
                        userDropdown.append(`<option value="${employee.empId}">${employee.fullName}</option>`); // Adjust key names as needed
                    });
                } else {
                    // Optionally, handle cases where no employees are found
                    userDropdown.append('<option value="0" disabled>No Users Available</option>');
                }
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
function getAssetData() {
    const table = $('#assetTable').DataTable({
        ajax: {
            url: '/Asset/GetAllAssetData', // Replace with your controller action
            method: 'GET',
            dataSrc: '' // Data source key, empty string assumes the root of the JSON array
        },
        columns: [
            { data: 'id' },
            { data: 'name' },
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
                            <button class="btn btn-warning btn-sm mx-1 view-btn" data-id="${data}" onclick="AssetViewModal(${data})">
                               <i class="bi bi-pencil"></i> View
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

function EditAsset(assetID) {
    $.ajax({
        url: '/Asset/GetAssetDetails/' + assetID, // API endpoint
        method: 'GET',
        success: function (response) {

            console.log('Full Response:', response);

            if (response.success && response.data && response.data.data) {
                const assetData = response.data.data;
                
                $('#assetID').val(assetData.id),
                $('#assetName').val(assetData.name || '');
                $('#summary').val(assetData.Summary || '');
                $('#assetCategory').val(assetData.category || '');
                $('#assetValue').val(assetData.value || '');
                $('#assetStatus').val(assetData.Status || '');
                $('#appointedTo').val(assetData.empId || '');
                $('#TeamId').val(assetData.teamId || '');
                $('#IssueDate').val(assetData.Issuedate || '');

            } else {
                console.error('Error: Invalid or missing data in response.');
            }


        },
        error: function (error) {
            console.error('AJAX Error:', error);
        }
    });
}

function AssetViewModal(button) {
    // Get the asset ID from the button's data-id attribute
    var assetId = button;  // Access the 'data-id' attribute of the clicked button

    $.ajax({
        url: '/Asset/GetAssetDetails/'+assetId, // Replace with your actual controller action
        type: "GET",
        //data: { Id: assetId },
        success: function (response) {
            // Assuming response is an array with asset details
            if (response.data!=undefined) {
                // Set dynamic content for the modal
                $('#name').text(response.data.data.name);
                $('#category').text(response.data.data.category);
                $('#status').text(response.data.data.status); // Assuming status is correctly named in the response
                //$('#teamid').text(response.data.data.teamid); // Assuming status is correctly named in the response
                //$('#empid').text(response.data.data.empid); // Assuming status is correctly named in the response
                $('#value').text(response.data.data.value); // Assuming status is correctly named in the response
                const issueDate = new Date(response.data.data.issueDate).toLocaleDateString();
                $('#issuedate').text(issueDate); // Assuming status is correctly named in the response
                // Ensure that summary is handled correctly (e.g., use .html() if it contains HTML)
                $('#summarym').text(response.data.data.summary);  // Or use .html() if needed

                // Log to ensure data is being set
                console.log('Summary:', response.data.data.summary); // Assuming status is correctly named in the response

                // Open the modal using Bootstrap modal
                const modal = new bootstrap.Modal(document.getElementById('viewAssetModal'));
                modal.show();
            }
        },
        error: function (error) {
            console.error("Error fetching asset details:", error);
            alert("An error occurred while fetching asset details.");
        }
    });
}
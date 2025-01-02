$(document).ready(function () {
    getRole();
    getTeam();
    getEmployeeData();
});


function getRole() {

    $.ajax({
        url: '/Employee/GetRole', // Replace with your actual controller action
        method: 'GET',

        success: function (response) {



            if (response.success) {
                const role = response.data; // Assuming Data contains the array of teams

                // Populate the dropdown
                const roleDropdown = $('#role');
                roleDropdown.empty(); // Clear any existing options

                // Add a default option
                roleDropdown.append('<option value="" disabled selected>Select Role</option>');
                role.forEach(role => {
                    roleDropdown.append(`<option value="${role.id}">${role.name}</option>`); // Adjust key names as needed
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

function getTeam() {
    $.ajax({
        url: '/Employee/GetTeamData', // Replace with your actual controller action
        method: 'GET',
        success: function (response) {

            if (response.success) {
                const teams = response.data; // Assuming Data contains the array of teams

                // Populate the dropdown
                const teamDropdown = $('#teamName');
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

function getEmployeeData() {

    const table = $('#employeeTable').DataTable({

        ajax: {
            url: '/Employee/GetEmployeeData', // Replace with your controller action
            method: 'GET',
            dataSrc: '' // Data source key, empty string assumes the root of the JSON array
        },
        columns: [

            { data: 'id' },
            { data: 'name' },
            { data: 'email'},
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
                                    <button class="btn btn-success btn-sm mx-1 view-btn" data-id="${data}" >
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
            info: "Showing _START_ to _END_ of _TOTAL_ Employees",
            infoEmpty: "No Employees available",
            paginate: {
                first: "First",
                last: "Last",
                next: "Next",
                previous: "Previous"
            }
        }

    });
}

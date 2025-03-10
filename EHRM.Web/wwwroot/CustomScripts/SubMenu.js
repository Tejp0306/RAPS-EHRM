$(document).ready(function () {
    getMainMenuData();
    getRoleData();
    getSubMenuListData();

    //Delete SubMenu
    $('#subMenuTable').on('click', '.delete-btn', function () {
        const submenuID = $(this).data('id');  // Get the Main menu ID from the data-id attribute

        // Confirm before deleting
        if (confirm(`Are you sure you want to delete this Sub Menu?`)) {
            // Make AJAX request to delete the notice board
            $.ajax({
                url: `/Menu/DeleteSubMenu`,  // API endpoint to delete the notice board
                method: 'POST',
                data: { id: submenuID },
                success: function (response) {
                    if (response.success) {
                        
                        alert(response.message);
                        location.reload();
                    } else {
                        // Handle error if the delete fails
                        alert(response.message || 'An error occurred while deleting the Sub Menu.');
                    }
                },
                error: function (error) {
                    console.error('Error deleting notice board:', error);
                    alert('An error occurred while deleting the SubMenu.');
                }
            });
        }
    });


    // EDit Sub menu

    $('#subMenuTable').on('click', '.edit-btn', function () {

        const submenuID = $(this).data('id');
        EditSubMenu(submenuID);
    });
    
});


//Getting data for main menu dropdown
function getMainMenuData() {

    $.ajax({
        url: '/Menu/GetMainMenuId', // Replace with your actual controller action
        method: 'GET',

        success: function (response) {

            if (response.success) {
                const menu = response.data; // Assuming Data contains the array of teams

                // Populate the dropdown
                const menuDropdown = $('#MainMenuId');
                menuDropdown.empty(); // Clear any existing options

                // Add a default option
                menuDropdown.append('<option value="" disabled selected>Select MainMenu</option>');
                menu.forEach(menu => {
                    menuDropdown.append(`<option value="${menu.id}">${menu.name}</option>`); // Adjust key names as needed
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

//Getting data for role dropdown
function getRoleData() {

    $.ajax({
        url: '/Menu/GetRole', // Replace with your actual controller action
        method: 'GET',

        success: function (response) {

            

            if (response.success) {
                const role = response.data; // Assuming Data contains the array of teams

                // Populate the dropdown
                const roleDropdown = $('#RoleId');
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

//Getting data for User Dropdown
function GetEmployeeDetail(RoleId) {
    
    $.ajax({
        
        url: '/Menu/GetEmployeebyRoleId', // Replace with your actual controller action
        method: 'GET',
        data: { RoleId: RoleId },
        
        success: function (response) {
            
            if (response.success) {
                const emp = response.data.data; // Assuming data contains the array of employees
                
                // Populate the dropdown
                const userDropdown = $('#EmpId');
                userDropdown.empty(); // Clear any existing options

                // Add a default option
                userDropdown.append('<option value="" disabled selected>Select User</option>');

                // Check if emp is a valid array and iterate over it
                if (Array.isArray(emp) && emp.length > 0) {
                    emp.forEach(employee => {
                        userDropdown.append(`<option value="${employee.empId}">${employee.fullName}</option>`); // Adjust key names as needed
                    });
                } else {
                    // Optionally, handle cases where no employees are found
                    userDropdown.append('<option value="" disabled>No Users Available</option>');
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


function getSubMenuListData() {
    const table = $('#subMenuTable').DataTable({
        ajax: {
            url: '/Menu/GetSubMenuData', // Replace with your controller action
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
                            <button class="btn btn-warning btn-sm mx-1 view-btn" data-id="${data}" onclick="ViewModal(${data})">
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

// Function to open the modal
function ViewModal(id) {
    debugger;
    $.ajax({
        
        
        url: '/Menu/GetViewSubMenuData', // Replace with your actual controller action
        //method: 'GET',
        type:"get",
        data: {Id:id},

        success: function (response) {

            const modal = new bootstrap.Modal(document.getElementById('viewSubMenuModal'));

            modal.show();

            document.getElementById("action").innerText = response[0].action;
            document.getElementById("controller").innerText = response[0].controller;
            //document.getElementById("employeeId").innerText = response[0].employeeId;
            document.getElementById("employeeName").innerText = response[0].employeeName;
            //document.getElementById("id").innerText = response[0].id;
            //document.getElementById("mainMenuId").innerText = response[0].mainMenuId;
            document.getElementById("mainMenuName").innerText = response[0].mainMenuName;
            //document.getElementById("roleId").innerText = response[0].roleId;
            document.getElementById("roleName").innerText = response[0].roleName;
            document.getElementById("subMenuName").innerText = response[0].subMenuName;

            
            //document.getElementById('modalContent').textContent = `SubMenu ID: ${id}`;
            //const modal = new bootstrap.Modal(document.getElementById('viewSubMenuModal'));
            //modal.show();
        }


    });
    
}

//Edit SubMenu
function EditSubMenu(submenuID) {
    $.ajax({
        url: '/Menu/GetSubMenuDetails/' + submenuID, // API endpoint
        method: 'GET',
        success: function (response) {
            
            console.log('Full Response:', response);
            
            if (response.success && response.data && response.data.data) {
                const submenuData = response.data.data;
                
                $('#submenuId').val(submenuData.id),
                $('#Name').val(submenuData.name || '');
                $('#Controller').val(submenuData.controller || '');
                $('#Action').val(submenuData.action || '');
                $('#MainMenuId').val(submenuData.mainMenuId || '');
                $('#RoleId').val(submenuData.roleId || '');
                $('#EmpId').val(submenuData.empId || '');

            } else {
                console.error('Error: Invalid or missing data in response.');
            }


        },
        error: function (error) {
            console.error('AJAX Error:', error);
        }
    });
}





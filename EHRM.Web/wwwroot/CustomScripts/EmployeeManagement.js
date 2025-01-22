$(function () {
    getRole();
    getTeam();
    getEmployeeData();
    getManager();

    $('#employeeTable').on('click', '.activate-btn', function () {

        const employeeID = $(this).data('id');

        GetEmpForEmpCred(employeeID);
        
    });
});

function getManager() {

    $.ajax({
        url: '/Employee/GetManager', // Replace with your actual controller action
        method: 'GET',

        success: function (response) {



            if (response.success) {
                const res = response.data; // Assuming Data contains the array of teams

                // Populate the dropdown
                const resDropdown = $('#ManagerId');
                resDropdown.empty(); // Clear any existing options

                // Add a default option
                resDropdown.append('<option value="" disabled selected>Select Role</option>');
                res.forEach(res => {
                    resDropdown.append(`<option value="${res.empId}">${res.name}</option>`); // Adjust key names as needed
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

function getRole() {

    $.ajax({
        url: '/Employee/GetRole', // Replace with your actual controller action
        method: 'GET',

        success: function (response) {



            if (response.success) {
                const role = response.data; // Assuming Data contains the array of teams
                debugger
                // Populate the dropdown
                const roleDropdown = $('#RoleId');
                //roleDropdown.empty(); // Clear any existing options

                // Add a default option
                //roleDropdown.append('<option value="" disabled selected>Select Role</option>');
                var currentRole = $('#RoleId').val();
                role.forEach(role => {
                    console.log(role.id + ' -- ' + currentRole);
                    var newroleid = role.id;
                    if (newroleid != currentRole) {
                        roleDropdown.append(`<option value="${role.id}">${role.name}</option>`); // Adjust key names as needed
                    }
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
                const teamDropdown = $('#TeamId');
                //teamDropdown.empty(); // Clear any existing options

                // Add a default option
                //teamDropdown.append('<option value="" disabled selected>Select team</option>');

                var currentteam = $('#TeamId').val();

                teams.forEach(teams => {
                    console.log(teams.id + '--' + currentteam);
                    var newteamid = teams.id;
                    if (newteamid != currentteam) {
                        teamDropdown.append(`<option value="${teams.id}">${teams.name}</option>`); // Adjust key names as needed
                    }
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
            { data: 'email' },
            {
                data: 'profileStatus',
                render: function (data) {
                    // Check profile status and apply color
                    if (data === 'Profile Incomplete') {
                        return `<span class="badge bg-danger">${data}</span>`; // Red for incomplete
                    } else if (data === 'Profile Complete') {
                        return `<span class="badge bg-success">${data}</span>`; // Green for complete
                    }
                    return data; // Default case (if any unexpected value)
                }
            },
            {
                data: 'id',
                render: function (data) {
                    return `
                        <div class="d-flex justify-content-center">
                            <button class="btn btn-warning btn-sm mx-1 edit-btn" data-id="${data}">
                                <i class="bi bi-pencil"></i> Edit/Complete
                            </button>
                            <button class="btn btn-success btn-sm mx-1 activate-btn" data-id="${data}">
                                <i class="bi bi-check-circle"></i> Activate Account
                            </button>

                            <button class="btn btn-primary btn-sm mx-1 view-btn" data-id="${data}">
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
    $('#employeeTable').on('click', '.edit-btn', function () {
        const EmpId = $(this).data('id');
        GetAllEmployeeDetailsData(EmpId);
        // 
        // Add logic for viewing details in a modal or new page
    });
}

// Handle View Button Click
function GetAllEmployeeDetailsData(EmpId) {
    // Construct the URL based on EmpId
    const url = EmpId ? `/Employee/AddEmployee/${EmpId}` : '/Employee/AddEmployee';
    console.log('Navigating to:', url);

    // Redirect to the URL
    window.location.href = url;
}

function ClAge() {
    var dateBirth = $('#DateOfBirth').val(); // Get the date of birth from the input field
    // Manually set the URL path
    var url = '/Employee/GetAge'; // Adjust this path if needed, make sure it's correct
    $.ajax({
        url: url,
        type: 'POST', // POST method
        data: { dateBirth: dateBirth }, // Send dateBirth in the request body
        success: function (response) {
            debugger;
            if (response.success) {
                var age = response.age; // Extract age from the JSON response

                if (age >= 18) {
                    $('#Age').val(age); // Set the age in the input field
                } else {
                    alert("The applicant's age must be 18 years or above.");
                    location.reload();
                }
            } else {
                alert(response.message || "Unable to calculate age. Please check the date of birth format.");
            }
        },
        error: function () {
            alert("An error occurred while calculating age. Please try again.");
        }
    });
}

//This is for Generate LoginId Based on First Name

function CheckExistingEmpId() {
    var empId = $('#EmpId').val(); // Get the EmpId from the input field
    var url = '/Employee/CheckExistingEmpId'; // Correct the URL to match your action method

    $.ajax({
        url: url,
        type: 'POST', // POST method
        data: { empId: empId }, // Send EmpId in the request body
        success: function (response) {
            debugger;
            if (response.flag > 0) { // Check if the flag is greater than 0
                $('#lblEmpIdError').text("EmpId is already in use. Please use another EmpId.☒"); 
                $('#EmpId').val(""); // Clear the input field
            }
            else {
                $('#lblEmpIdError').text("");
            }
        },
        error: function () {
            alert("An error occurred while checking the EmpId. Please try again.");
        }
    });
}

function GenerateLoginId() {
    var firstName = $('#FirstName').val(); // Get the first name from the input field
    var url = '/Employee/GenerateLogin';;
    $.ajax({
        url: url,
        type: 'POST', // POST method
        data: { firstName: firstName }, // Send firstName in the request body
        success: function (response) {
            debugger;
            if (response.success) {
                var loginId = response.loginId;
                var password = response.password;
                $('#LoginId').val(loginId); // Set the loginId in the corresponding input field
                $('#Password').val(password);
            }
        },
        error: function () {
            alert("An error occurred while generating the login ID. Please try again.");
        }
    });
}

function ValidateEmail() {
    var email = $('#EmailAddress').val(); // Get email from the input field
    var lblError = document.getElementById("lblError");
    lblError.innerHTML = ""; // Clear any previous error message

    // Corrected email regex pattern
/*    var expr = /^[\w-\.]+@([\w-]+\.)+[a-zA-Z]{2,7}$/;*/
    var expr = /^[\w-\.]+@([\w-]+\.)+[a-zA-Z]{2,6}$/;

    if (!expr.test(email)) {
        lblError.innerHTML = "Invalid email address.";
    }
}


function validate_ConfirmEmail() {
    /*  alert("test")*/
    var pass = document.getElementById('EmailAddress').value;
    var confirm_pass = document.getElementById('emailC').value;
    if (pass != confirm_pass) {
        document.getElementById('wrong_email_alert').style.color = 'red';
        document.getElementById('wrong_email_alert').innerHTML
            = '☒ Use same Email';
        document.getElementById('btnsubmit').disabled = true;
        document.getElementById('btnsubmit').style.opacity = (0.4);
    } else {
        document.getElementById('wrong_email_alert').style.color = 'green';
        document.getElementById('wrong_email_alert').innerHTML =
            '🗹 Email Matched';
        document.getElementById('btnsubmit').disabled = false;
        document.getElementById('btnsubmit').style.opacity = (1);
    }
}


function calculateServiceDuration(AppointmentDate) {
    debugger;
    const appointmentDateObj = new Date(AppointmentDate);
    const currentDate = new Date();

    // Calculate the difference in years, months, and days
    let yearsDifference = currentDate.getFullYear() - appointmentDateObj.getFullYear();
    let monthsDifference = currentDate.getMonth() - appointmentDateObj.getMonth();
    let daysDifference = currentDate.getDate() - appointmentDateObj.getDate();

    // If the month difference is negative, adjust the year difference
    if (monthsDifference < 0) {
        monthsDifference += 12;
        yearsDifference--;
    }

    // If the days difference is negative, adjust the month difference
    if (daysDifference < 0) {
        const previousMonth = new Date(currentDate.getFullYear(), currentDate.getMonth() - 1, 0);
        const daysInPreviousMonth = previousMonth.getDate();
        daysDifference += daysInPreviousMonth;
        monthsDifference--;
    }

    // Prepare result in years, months, and days
    let result = `${yearsDifference} year${yearsDifference !== 1 ? "s" : ""}, ${monthsDifference} month${monthsDifference !== 1 ? "s" : ""}, ${daysDifference} day${daysDifference !== 1 ? "s" : ""}`;

    // Update the appointedService input with calculated time difference
    const appointedServiceInput = document.getElementById("AppointedService");
    appointedServiceInput.value = result;
}


//Activate employee and add employee details in employee cred table

function GetEmpForEmpCred(EmpId) {
    debugger;
    // Construct the URL based on EmpId
    const url = EmpId ? `/Employee/GetEmployeeForCred/${EmpId}` : '/Employee/GetEmployeeForCred';
    console.log('Navigating to:', url);

    // Redirect to the URL
    window.location.href = url;

}


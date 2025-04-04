$(function () {
    getRole();
    getTeam();
    getEmployeeData();
    getManager();

    $('#employeeTable').on('click', '.activate-btn', function () {

        const employeeID = $(this).data('id');

        GetEmpForEmpCred(employeeID);
        
    });
    
    $('#employeeTable').on('click', '.danger-btn', function () {

        const employeeID = $(this).data('id');

        GetEmpForEmpCred(employeeID);

    });

    $('#addExperienceBtn').on('click', function () {
        const index = $('.experience-entry').length;
        const experienceHtml = `
            <div class="row mb-3 experience-entry">
                <div class="col-md-6">
                    <label class="form-label">Organisation Name</label>
                    <input name="WorkExperiences[${index}].OrganisationName" class="form-control" />
                </div>
                <div class="col-md-4">
                    <label class="form-label">Years of Experience</label>
                    <input name="WorkExperiences[${index}].YearsOfExperience" class="form-control" type="number" step="0.01" />
                </div>
                <div class="col-md-2 d-flex align-items-end">
                    <button type="button" class="btn btn-danger remove-experience">Remove</button>
                </div>
            </div>`;
        $('#experienceSection').append(experienceHtml);
});

    // Remove Work Experience
    $(document).on('click', '.remove-experience', function () {
        $(this).closest('.experience-entry').remove();
    });

    // Add Dependent
    $('#addDependentBtn').on('click', function () {
        const index = $('.dependent-entry').length;
        const dependentHtml = `
            <div class="row mb-3 dependent-entry">
                <div class="col-md-4">
                    <label class="form-label">Dependent Name</label>
                    <input name="Dependents[${index}].DependentName" class="form-control" />
                </div>
                <div class="col-md-4">
                    <label class="form-label">Relationship</label>
                    <input name="Dependents[${index}].DependentRelationship" class="form-control" />
                </div>
                <div class="col-md-4">
                    <label class="form-label">Date of Birth</label>
                    <input name="Dependents[${index}].DependentDOB" class="form-control" type="date" />
                </div>
            </div>`;
        $('#dependentSection').append(dependentHtml);
    });

    // Add Education
    $('#addEducationBtn').on('click', function () {
        const index = $('.education-entry').length;
        const educationHtml = `
            <div class="row mb-3 education-entry">
                <div class="col-md-4">
                    <label class="form-label">Institution Name</label>
                    <input name="EducationDetails[${index}].InstitutionName" class="form-control" />
                </div>
                <div class="col-md-4">
                    <label class="form-label">Degree Name</label>
                    <input name="EducationDetails[${index}].DegreeName" class="form-control" />
                </div>
                <div class="col-md-4">
                    <label class="form-label">Passing Year</label>
                    <input name="EducationDetails[${index}].PassingYear" class="form-control" type="number" />
                </div>
            </div>`;
        $('#educationSection').append(educationHtml);
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
                //resDropdown.empty(); // Clear any existing options

                // Add a default option
                //resDropdown.append('<option value="" disabled selected>Select Role</option>');
                var currentManager = $('#ManagerId').val();

                res.forEach(res => {
                    var newmanagerid = res.empId;
                    if (newmanagerid != currentManager) {
                        resDropdown.append(`<option value="${res.empId}">${res.name}</option>`); // Adjust key names as needed
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

function getRole() {

    $.ajax({
        url: '/Employee/GetRole', // Replace with your actual controller action
        method: 'GET',

        success: function (response) {



            if (response.success) {
                const role = response.data; // Assuming Data contains the array of teams
      
                // Populate the dropdown
                const roleDropdown = $('#RoleId');
                //roleDropdown.empty(); // Clear any existing options

                // Add a default option
                //roleDropdown.append('<option value="" disabled selected>Select Role</option>');
                var currentRole = $('#RoleId').val();
                role.forEach(role => {
           
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
            url: '/Employee/GetEmployeeData', // Replace with your endpoint
            method: 'GET',
            dataSrc: ''
        },
        columns: [
            { data: 'id' },
            { data: 'name' },
            { data: 'email' },
            {
                data: 'profileStatus',
                render: function (data) {
                    if (data === false) {
                        return `<span class="badge bg-danger">Incomplete</span>`;
                    } else {
                        return `<span class="badge bg-success">Completed</span>`;
                    }
                }
            },
            {
                data: 'employmentStatus',
                render: function (data) {
                    if (data === false) {
                        return `<span class="badge bg-danger">InActive</span>`;
                    } else {
                        return `<span class="badge bg-success">Active</span>`;
                    }
                }
            },
            {
                data: null,
                render: function (rowData) {
                    // Check if profileStatus is incomplete
                    const isProfileIncomplete = rowData.profileStatus === false;
                    const isActive = rowData.employmentStatus === true;

                    return `
                        <div class="d-flex justify-content-center">
                            <button class="btn btn-warning btn-sm mx-1 edit-btn" data-id="${rowData.id}">
                                <i class="bi bi-pencil"></i> Edit
                            </button>
                            <button class="btn btn-success btn-sm mx-1 activate-btn" 
                                data-id="${rowData.id}" 
                                ${isActive || isProfileIncomplete ? 'disabled' : ''}>
                                <i class="bi bi-check-circle"></i> Activate
                            </button>
                            <button class="btn btn-danger btn-sm mx-1 danger-btn" 
                                data-id="${rowData.id}" 
                                ${!isActive || isProfileIncomplete ? 'disabled' : ''}>
                                <i class="bi bi-x-circle"></i> Deactivate
                            </button>
                        </div>`;
                },
                orderable: false
            }

        ],
        pageLength: 5,
        lengthMenu: [5, 10, 15, 20],
        language: { /* ... */ }
    });

    // Click handler for Activate/Deactivate buttons
    $('#employeeTable').on('click', '.activate-btn, .danger-btn', function () {

        const button = $(this);
        const EmpId = button.data('id');
        const isActivate = button.hasClass('activate-btn');

        // Disable the clicked button immediately
        button.prop('disabled', true);

        // Example: Send AJAX request to update status
        $.ajax({
            url: isActivate ? '/Employee/Activate' : '/Employee/ ',
            method: 'POST',
            data: { id: EmpId },
            success: function () {
                // Refresh table data to reflect new state
                table.ajax.reload();
            },
            error: function () {
                // Re-enable the button if the request fails
                button.prop('disabled', false);
            }
        });
    });

    // Edit button handler (unchanged)
    $('#employeeTable').on('click', '.edit-btn', function () {
        const EmpId = $(this).data('id');
        GetAllEmployeeDetailsData(EmpId);
    });
}
    
 //Handle View Button Click

function GetAllEmployeeDetailsData(EmpId) {
    // Construct the URL based on EmpId
    const url = EmpId ? `/Employee/AddEmployee/${EmpId}` : '/Employee/AddEmployee';


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


function CheckExistingEmail() {
    var EmailAddress = $('#EmailAddress').val(); // Get the EmpId from the input field
    var url = '/Employee/CheckExistingEmail'; // Correct the URL to match your action method

    $.ajax({
        url: url,
        type: 'POST', // POST method
        data: { EmailAddress: EmailAddress }, // Send EmpId in the request body
        success: function (response) {

            if (response.flag > 0) { // Check if the flag is greater than 0
                $('#lblErrorexistingmail').text("Email is already registered. Please use another Email Id");
                $('#EmailAddress').val(""); // Clear the input field
            }
            else {
                $('#lblErrorexistingmail').text("");
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

    // Construct the URL based on EmpId
    const url = EmpId ? `/Employee/GetEmployeeForCred/${EmpId}` : '/Employee/GetEmployeeForCred';


    // Redirect to the URL
    window.location.href = url;

}





function validatepersonalinfoForm() {
    const form = document.forms["personal-info"];
    const fieldsToValidate = [
        { input: form["FirstName"], errorSpan: "FirstNameError", errorMessage: "First name is required." },
        { input: form["LastName"], errorSpan: "LastNameError", errorMessage: "Last name is required." },
        { input: form["gender"], errorSpan: "GenderError", errorMessage: "Gender is required." },
        { input: form["DateOfBirth"], errorSpan: "DOBError", errorMessage: "DOB is required." },
        { input: form["phone"], errorSpan: "cellError", errorMessage: "Mobile Number is required." },
        { input: form["EmailAddress"], errorSpan: "EmailError", errorMessage: "Email is required." },
        { input: form["RoleId"], errorSpan: "RoleError", errorMessage: "Role is required." },
        //{ input: form["AadharNumber"], errorSpan: "AadharError", errorMessage: "Aadhar Number is required." },
        { input: form["TeamId"], errorSpan: "TeamError", errorMessage: "Team name is required." },
        { input: form["Street"], errorSpan: "streetError", errorMessage: "Street Name is required." },
        { input: form["Country"], errorSpan: "countryError", errorMessage: "Country name is required." },
        { input: form["City"], errorSpan: "CityError", errorMessage: "City name is required." },
        { input: form["ProfileImg"], errorSpan: "ImageError", errorMessage: "Profile Image is required." }
    ];

    let isValid = true;

    fieldsToValidate.forEach(({ input, errorSpan, errorMessage }) => {
        const errorElement = document.getElementById(errorSpan);

        if (input.value.trim() === "" || input.value==="0") {
            showError(errorElement, errorMessage);
            if (isValid) input.focus(); // Focus the first invalid field
            isValid = false;
        } else {
            hideError(errorElement);
        }
    });

    return isValid;

    // Helper functions
    function showError(errorElement, message) {
        errorElement.textContent = message;
        errorElement.style.display = "inline"; // Show the error message
        errorElement.style.color = "red"; // Ensure consistent styling
    }

    function hideError(errorElement) {
        errorElement.textContent = ""; // Clear the error message
        errorElement.style.display = "none"; // Hide the error message
    }
}

function validateemploymentinfoForm() {

    const form = document.forms["employment-info"];
    const fieldsToValidate = [
        { input: form["EmpType"], errorSpan: "etypeError", errorMessage: "Employee Type is required." },
        { input: form["AppointmentDate"], errorSpan: "dateError", errorMessage: "Appointed Date is required." },
        { input: form["startdate"], errorSpan: "startdateError", errorMessage: "Start Date is required." },
        { input: form["startdate"], errorSpan: "startdateError", errorMessage: "Start Date is required." },
        { input: form["EmploymentStatusId"], errorSpan: "employmentstatuserror", errorMessage: "Employment Status is required." },
        { input: form["ManagerId"], errorSpan: "managererror", errorMessage: "Manager must be selected" },   
    ];

    let isValid = true;

    fieldsToValidate.forEach(({ input, errorSpan, errorMessage }) => {
        const errorElement = document.getElementById(errorSpan);

        if (input.value.trim() === "" || input.value === "0" ) {
            showError(errorElement, errorMessage);
            if (isValid) input.focus(); // Focus the first invalid field
            isValid = false;
        } else {
            hideError(errorElement);
        }
    });

    return isValid;

    // Helper functions
    function showError(errorElement, message) {
        errorElement.textContent = message;
        errorElement.style.display = "inline"; // Show the error message
        errorElement.style.color = "red"; // Ensure consistent styling
    }

    function hideError(errorElement) {
        errorElement.textContent = ""; // Clear the error message
        errorElement.style.display = "none"; // Hide the error message
    }

}

function validatequalificationForm() {

    const form = document.forms["qualification-education-info"];
    const fieldsToValidate = [
        { input: form["CourseName"], errorSpan: "coursenameerror", errorMessage: "Course name is required." },
        { input: form["InstitutionName"], errorSpan: "instituteerror", errorMessage: "Institute is Required." },
        { input: form["PassedDate"], errorSpan: "passeddateerror", errorMessage: "Passed date is Required." },
  
    ];

    let isValid = true;

    fieldsToValidate.forEach(({ input, errorSpan, errorMessage }) => {
        const errorElement = document.getElementById(errorSpan);

        if (input.value.trim() === "" ) {
            showError(errorElement, errorMessage);
            if (isValid) input.focus(); // Focus the first invalid field
            isValid = false;
        } else {
            hideError(errorElement);
        }
    });

    return isValid;

    // Helper functions
    function showError(errorElement, message) {
        errorElement.textContent = message;
        errorElement.style.display = "inline"; // Show the error message
        errorElement.style.color = "red"; // Ensure consistent styling
    }

    function hideError(errorElement) {
        errorElement.textContent = ""; // Clear the error message
        errorElement.style.display = "none"; // Hide the error message
    }
}


function validatesalaryForm() {

    const form = document.forms["salary-grade-info"];
    const fieldsToValidate = [
        { input: form["Ctc"], errorSpan: "CTCerror", errorMessage: "CTC is required." },
    ];

    let isValid = true;

    fieldsToValidate.forEach(({ input, errorSpan, errorMessage }) => {
        const errorElement = document.getElementById(errorSpan);

        if (input.value.trim() === "" ) {
            showError(errorElement, errorMessage);
            if (isValid) input.focus(); // Focus the first invalid field
            isValid = false;
        } else {
            hideError(errorElement);
        }
    });

    return isValid;

    // Helper functions
    function showError(errorElement, message) {
        errorElement.textContent = message;
        errorElement.style.display = "inline"; // Show the error message
        errorElement.style.color = "red"; // Ensure consistent styling
    }

    function hideError(errorElement) {
        errorElement.textContent = ""; // Clear the error message
        errorElement.style.display = "none"; // Hide the error message
    }
}

function validatedeclarationForm() {

    const form = document.forms["declaration-info"];
    const fieldsToValidate = [
        { input: form["HrRepresentativeName"], errorSpan: "hrnameerror", errorMessage: "HR Representative Name is required." },
        { input: form["HrRepresentativeDesignation"], errorSpan: "hrdesignationerror", errorMessage: "Designation is required." },
        { input: form["HrContactInfo"], errorSpan: "hrcontacterror", errorMessage: "Contact Info is required." },
        { input: form["Signature"], errorSpan: "signatureerror", errorMessage: "Signature is required." },
    ];

    let isValid = true;

    fieldsToValidate.forEach(({ input, errorSpan, errorMessage }) => {
        const errorElement = document.getElementById(errorSpan);

        if (input.value.trim() === "" ) {
            showError(errorElement, errorMessage);
            if (isValid) input.focus(); // Focus the first invalid field
            isValid = false;
        } else {
            hideError(errorElement);
        }
    });

    return isValid;

    // Helper functions
    function showError(errorElement, message) {
        errorElement.textContent = message;
        errorElement.style.display = "inline"; // Show the error message
        errorElement.style.color = "red"; // Ensure consistent styling
    }

    function hideError(errorElement) {
        errorElement.textContent = ""; // Clear the error message
        errorElement.style.display = "none"; // Hide the error message
    }


}

function validateAadhar() {
    const aadharInput = document.getElementById('AadharNumber');
    const errorSpan = document.getElementById('AadharError');

    // Regular expression to match exactly 12 digits
    const aadharRegex = /^\d{12}$/;

    if (aadharRegex.test(aadharInput.value)) {
        errorSpan.style.display = 'none';
        aadharInput.classList.remove('is-invalid');
    } else {
        errorSpan.style.display = 'block';
        aadharInput.classList.add('is-invalid');
    }
}


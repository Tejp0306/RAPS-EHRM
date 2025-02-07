
$(document).ready(function () {
    GetDetail();
});
function GetDetail() {

    $.ajax({
        url: '/Review/GetEmployeeDetailsByManager', // Replace with your actual controller action
        method: 'GET',
        success: function (response) {
            if (response.success) {
                var emp = response.data; // Assuming data contains the array of employees

                // Populate the dropdown
                const userDropdown = $('#EmpId');
                userDropdown.empty(); // Clear any existing options

                // Add a default option
                userDropdown.append('<option value="0" disabled selected>Select User</option>');

                // Check if emp is a valid array and iterate over it
                if (Array.isArray(emp) && emp.length > 0) {
                    emp.forEach(employee => {
                        userDropdown.append(`<option value="${employee.empId}">${employee.firstName}</option>`); // Adjust key names as needed
                    });
                } else {
                    // Optionally, handle cases where no employees are found
                    userDropdown.append('<option value="0" disabled>No Users Available</option>');
                }
                // Loop through the array and append options

            } else {
                console.error('Error:');
            }
        },
        error: function (error) {
            console.error('AJAX Error:', error);
        }
    });


}

function ProbationForm() {
    const form = document.forms["evaluationForm"];
    const fieldsToValidate = [
        { input: form["ApplicationDate"], errorSpan: "dateError", errorMessage: "Date is required." },
        { input: form["Recommendation"], errorSpan: "recommendationError", errorMessage: "Recommendation is required." },
        { input: form["RemarksConfirmation"], errorSpan: "remarksError", errorMessage: "Remarks required." },
        { input: form["RemarksConfirmation"], errorSpan: "remarksError", errorMessage: "Remarks required." }
       
    ];

    let isValid = true;

    fieldsToValidate.forEach(({ input, errorSpan, errorMessage }) => {
        const errorElement = document.getElementById(errorSpan);

        if (input.value.trim() === "") {
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

function Form() {
    const form = document.forms["info"];
    const fieldsToValidate = [
        { input: form["EmpId"], errorSpan: "empIdError", errorMessage: "Emp-Id is required." },
        { input: form["employeeName"], errorSpan: "employeeNameError", errorMessage: "First name is required." },
        { input: form["designation"], errorSpan: "destinationError", errorMessage: "Destination required." },
        { input: form["DateOfJoining"], errorSpan: "dateOfJoiningError", errorMessage: "Joining Date is required." },
        { input: form["location"], errorSpan: "locationError", errorMessage: "Location is required." },
        { input: form["DateOfBirth"], errorSpan: "dobError", errorMessage: "DOB is required." },
        { input: form["age"], errorSpan: "ageError", errorMessage: "Age is required." },
        { input: form["officialContact"], errorSpan: "officialContactError", errorMessage: "Official-Conatct required." },
        { input: form["personalContact"], errorSpan: "personalContactError", errorMessage: "Personal-Contact required." },
        { input: form["officialEmail"], errorSpan: "officialEmailError", errorMessage: "Official-Email required." },
        { input: form["personalEmail"], errorSpan: "personalEmailError", errorMessage: "Personal-Email required." },
        { input: form["priorWorkExperience"], errorSpan: "priorWorkError", errorMessage: "Prior work-ex required." },
        { input: form["totalWorkExperience"], errorSpan: "totalWorkError", errorMessage: "Total work-ex required." },
        { input: form["dependent1Name"], errorSpan: "dependentNameError", errorMessage: "Dependent name is required." },
        { input: form["dependent1Relationship"], errorSpan: "dependentRelationshipError", errorMessage: "Dependent Relation is required." },
        { input: form["emergencyName1"], errorSpan: "emergencyNameError", errorMessage: "Emergency name is required." },
        { input: form["emergencyContact1"], errorSpan: "emergencyContactError", errorMessage: "Emergency Contact is required." },
        { input: form["emergencyRelationship1"], errorSpan: "emergencyRelationshipError", errorMessage: "Emergency Relation is required." },
        { input: form["bachelorCompleteYear"], errorSpan: "bachelorError", errorMessage: "Passing year is required." },
        { input: form["bachelorDegrees"], errorSpan: "bachelorDegreeError", errorMessage: "Degree name is required." },
        { input: form["masterCompleteYear"], errorSpan: "masterError", errorMessage: "Passing year is required." },
        { input: form["uanNo"], errorSpan: "uanError", errorMessage: "Number required." },
        { input: form["adharNo"], errorSpan: "adharError", errorMessage: "Number required." },
        { input: form["panCardNo"], errorSpan: "panCardError", errorMessage: "Number required." },
        { input: form["bankName"], errorSpan: "bankNameError", errorMessage: "Bank name is required." },
        { input: form["accountNumber"], errorSpan: "accountNumberError", errorMessage: "Number required." },
        { input: form["ifscCode"], errorSpan: "ifscError", errorMessage: "Code required." },
        { input: form["permanentAddress"], errorSpan: "permanentAddressError", errorMessage: "Address required." },
        { input: form["postalAddress"], errorSpan: "postalAddressError", errorMessage: "Postal Address required." },
        { input: form["filingPerson"], errorSpan: "filingPersonError", errorMessage: "Name is required." },
        { input: form["resignationDate"], errorSpan: "resignationDateError", errorMessage: "Date is required." },
        { input: form["exitDate"], errorSpan: "exitDateError", errorMessage: "Date is required." },
        { input: form["reasonForLeaving"], errorSpan: "leavingError", errorMessage: "Reason is required." }
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
//$("#submitTimesheetButton").click(function (event) {
//    event.preventDefault();
//    let signatureDate = new Date($("#signatureDate").val());
//    let formattedDate = signatureDate.toISOString().split('T')[0]; // Formats to 'YYYY-MM-DD'


//    let submissionDate = new Date($("#submissionDate").val());
//    let formattedSubmission = submissionDate.toISOString().split('T')[0]; // Formats to 'YYYY-MM-DD'

//    let timesheetData = {
//        EmpName: $("#employeeName").val(),
//        ClientName: $("#clientName").val(),
//        Position: $("#position").val(),
//        ProjectName: $("#projectName").val(),
//        EmployeeSignature: $("#employeeSignature").val(),
//        ManagerSignature: $("#managerSignature").val(),
//        SignatureDate: formattedDate,
//        SubmissionDate: formattedSubmission,
//        Note: $("#note").val(),
//        TotalHours: $("#totalHours").val(),
//        PresentMonth: $("#monthSelector").val(),
//        DailyEntries: []
//    };

//    $("#timesheet-rows tr").each(function () {
//        var dayDate = $(this).find(".day-date").text();
//        let dayOfWeek = $(this).find(".day-of-week").text();
//        let hoursWorked = $(this).find(".hours-input").val();
//        let assignmentDesc = $(this).find(".assignment-desc").val();
//        let remarks = $(this).find(".remarks").val();

//        timesheetData.DailyEntries.push({
//            DayDate: dayDate,
//            DayOfWeek: dayOfWeek,
//            HoursWorked: hoursWorked,
//            AssignmentDesc: assignmentDesc,
//            Remarks: remarks
//        });
//    });


//    $.ajax({
//        url: "/Self/SubmitTimeSheet",
//        type: "POST",
//        contentType: "application/json",
//        data: JSON.stringify(timesheetData),
//        success: function (result) {
//            debugger;
//            if (result.success) {
//                alert(result.message || "Timesheet submitted successfully.");
//                window.location.href = "/Self/TimeSheet";
//            } else {
//                alert(result.message || "Error saving the timesheet.");
//            }
//        },
//        error: function (xhr, status, error) {
//            alert("An error occurred while submitting the timesheet. Please try again.");
//            console.log(xhr.responseText);
//        }
//    });

//});
// Function to generate timesheet rows based on the selected month
function generateTimesheetRows() {
    const timesheetRows = document.getElementById('timesheet-rows');
    const monthSelector = document.getElementById('monthSelector');
    const selectedMonth = parseInt(monthSelector.value); // Get selected month from dropdown
    const currentYear = new Date().getFullYear(); // Get current year

    // Get the number of days in the selected month
    const daysInMonth = new Date(currentYear, selectedMonth + 1, 0).getDate();

    // Clear existing rows
    timesheetRows.innerHTML = '';

    for (let day = 1; day <= daysInMonth; day++) {
        const date = new Date(currentYear, selectedMonth, day);
        const dayOfWeek = date.toLocaleString('en-us', { weekday: 'long' });
        const formattedDate = `${day}-${selectedMonth + 1}-${currentYear}`;

        const row = document.createElement('tr');

        // Weekend background
        const weekendStyle = (dayOfWeek === "Saturday" || dayOfWeek === "Sunday") ? 'background-color: yellow;' : '';

        row.innerHTML = `
                <td class="day-date">${formattedDate}</td>
                <td class="day-of-week">${dayOfWeek}</td>
                <td><input type="number" class="form-control hours-input" min="0" step="1" value=""/></td>
                <td><input type="text" class="form-control assignment-desc" value="" /></td>
                <td><input type="text" class="form-control remarks" value=""/></td>
        `;

        row.style = weekendStyle;

        timesheetRows.appendChild(row);
    }

    const hoursInputs = document.querySelectorAll('.hours-input');
    hoursInputs.forEach(input => {
        input.addEventListener('input', updateTotalHours);
    });
}

// Function to calculate and update the total hours worked
function updateTotalHours() {
    let totalHours = 0;
    const hoursInputs = document.querySelectorAll('.hours-input');
    hoursInputs.forEach(input => {
        const value = parseFloat(input.value) || 0;
        totalHours += value;
    });

    document.getElementById('totalHours').value = totalHours;
}

window.onload = generateTimesheetRows;

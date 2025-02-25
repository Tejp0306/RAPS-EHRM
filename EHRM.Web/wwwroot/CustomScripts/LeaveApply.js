$(document).ready(function () {

    GetLeaveTypeName();
    getLeaveData();
});

function getLeaveData() {
    debugger;
    const table = $('#LeaveDataTable').DataTable({
        ajax: {
            url: '/Leave/GetLeaveDetails',
            method: 'GET',
            dataSrc: '' // Data source key, empty string assumes the root of the JSON array
        },
        columns: [
            { data: 'id' },
            { data: 'employeeName' },
            { data: 'applyDate' },
            { data: 'leaveType' },
            { data: 'leaveFrom' },
            { data: 'leaveTo' },
            { data: 'description' },
            {
                data: 'status.leaveStatus',
                render: function (data) {
                    // Conditional styling based on leave status
                    if (data === "Approved") {
                        return `<span class="approved-status">${data}</span>`;
                    } else if (data === "Rejected") {
                        return `<span class="rejected-status">${data}</span>`;
                    } else {
                        return `<span class="pending-status">${data}</span>`;
                    }
                }
            },
            {
                data: 'status.managerRemark',
                render: function (data) {
                    return data ? data : "N/A"; // Display "N/A" for missing remarks
                }
            }
        ],
        pageLength: 5,
        lengthMenu: [5, 10, 15, 20],
        language: {
            search: "Search",
            lengthMenu: "Show _MENU_ entries",
            info: "Showing _START_ to _END_ of _TOTAL_ entries",
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

function GetLeaveTypeName() {
    debugger
    $.ajax({
        url: '/Leave/GetLeaveTypeData', // Replace with your actual controller action
        method: 'GET',
        success: function (response) {

            if (response.success) {
                const teams = response.data; // Assuming Data contains the array of teams

                // Populate the dropdown
                const teamDropdown = $('#LeaveType');
                teamDropdown.empty(); // Clear any existing options

                // Add a default option
                teamDropdown.append('<option value="" disabled selected>Select Leave Type</option>');
                teams.forEach(teams => {
                    teamDropdown.append(`<option value="${teams.name}">${teams.name}</option>`); // Adjust key names as needed
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

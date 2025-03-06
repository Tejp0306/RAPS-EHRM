$(document).ready(function () {

    GetLeaveTypeName();
    getLeaveData();
});

function getLeaveData() {
    debugger;
    $('#LeaveDataTable').DataTable({
        destroy: true, // Ensures DataTable is reinitialized correctly
        processing: true,
        serverSide: false, // Change to true if implementing server-side pagination
        ajax: {
            url: '/Leave/GetLeaveDetails',
            method: 'GET',
            dataSrc: '' // Assumes JSON response is an array
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
                render: function (data, type, row) {
                    debugger
                    console.log(data);
                    if (!data) return '<span class="pending-status">Pending</span>';
                    let statusClass = data === "Approved" ? "approved-status"
                        : data === "Rejected" ? "rejected-status"
                            : "pending-status";
                    return `<span class="${statusClass}">${data}</span>`;
                }
            },
            {
                data: 'status.managerRemark',
                render: function (data) {
                    return data ? `<span class="remark-text">${data}</span>` : "N/A";
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

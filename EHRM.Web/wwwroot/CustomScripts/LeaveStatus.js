$(document).ready(function () {
    $('#leaveTable').DataTable({
        "paging": true,          // Enables pagination
        "searching": true,       // Enables search box
        "ordering": true,        // Enables sorting
        "info": true             // Shows "Showing X of Y entries"
    });
});




document.addEventListener("DOMContentLoaded", function () {
    document.querySelectorAll(".view-btn").forEach(button => {
        button.addEventListener("click", function () {
            const leaveId = this.getAttribute("data-leave-id");
            const employeeName = this.getAttribute("data-employee-name");
            const leaveStatus = this.getAttribute("data-leave-status");
            const remark = this.getAttribute("data-remark");

            document.getElementById("LeaveApplyId").value = leaveId;
            document.getElementById("EmployeeName").value = employeeName;
            document.getElementById("LeaveStatus").value = leaveStatus;
            document.getElementById("Remark").value = remark;
        });
    });
});




//$(document).ready(function () {
//    getLeaveTypeData();

//    // Open Modal and Populate Data
//    $('#leaveDetailsModal').on('shown.bs.modal', function (event) {
//        var button = $(event.relatedTarget); // Button that triggered the modal
//        var employeeName = button.data('employee-name'); // Extract data from the button
//        var leaveStatus = button.data('leave-status');
//        var remark = button.data('remark');

//        // Fill in the modal fields with the data
//        $('#EmployeeName').val(employeeName); // Set Employee Name (readonly)

//        // Set Leave Status
//        if (leaveStatus) {
//            $('#LeaveStatus').val(leaveStatus);  // Set Leave Status
//        } else {
//            $('#LeaveStatus').val(""); // Ensure the placeholder is visible when there's no value
//        }

//        // Set Remark
//        $('#Remark').val(remark || ""); // If remark is undefined or null, it will be empty
//    });


//});

//function getLeaveTypeData() {
//    debugger;
//    const table = $('#leaveTable').DataTable({
//        ajax: {
//            url: '/Leave/GetLeaveData', // Replace with your controller action
//            method: 'GET',
//            dataSrc: '' // Data source key, empty string assumes the root of the JSON array
//        },

//        columns: [
//            { data: 'id' },
//            { data: 'employeeName' },
//            { data: 'leaveType' },
//            { data: 'leaveFrom' },
//            { data: 'leaveTo' },
//            {
//                data: 'id',
//                render: function (data, type, row) {
//                    return `
//                        <div class="d-flex justify-content-center">
//                            <button class="btn btn-primary btn-sm mx-1 view-btn" data-bs-toggle="modal" data-bs-target="#leaveDetailsModal"
//                                data-employee-name="${row.employeeName}"
//                                data-leave-status="${row.leaveStatus}"
//                                data-remark="${row.remark}">
//                                <i class="bi bi-eye"></i> View
//                            </button>
//                        </div>
//                    `;
//                },
//                orderable: false // Disable sorting on the Action column
//            }
//        ],

//        pageLength: 5, // Default rows per page
//        lengthMenu: [5, 10, 15, 20], // Options for rows per page

//        language: {
//            search: "Search",
//            lengthMenu: "Show _MENU_ entries",
//            info: "Showing _START_ to _END_ of _TOTAL_ Main Menu",
//            infoEmpty: "No records available",
//            paginate: {
//                first: "First",
//                last: "Last",
//                next: "Next",
//                previous: "Previous"
//            }
//        }
//    });
//}

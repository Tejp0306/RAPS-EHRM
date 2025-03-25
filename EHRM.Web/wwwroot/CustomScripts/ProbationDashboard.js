//$(document).ready(function () {

//    // Initialize DataTable and fetch data via AJAX
//    const table = $('#EvaluationTable').DataTable({
//        ajax: {
//            url: `/Review/GetAllDetails`,  // API endpoint for the service
//            method: 'GET',
//            success: function (response) {
//                // Check if the response indicates success
//                if (response.success) {

//                    table.clear().rows.add(response.data).draw();
//                } else {
//                    // Handle the case where the request was not successful
//                    alert(response.message || "No data available.");
//                }
//            },
//        },
//        columns: [
//            { data: 'id' },
//            { data: 'questionId' },
//            { data: 'firstName' },  // Accessing 'firstName' from the simplified object
//            { data: 'lastName' },   // Accessing 'lastName' from the simplified object
//            { data: 'recommendation' },
//            { data: 'remarksConfirmation' }
//        ],
//        pageLength: 5, // Default rows per page
//        lengthMenu: [5, 10, 15, 20], // Options for rows per page
//        language: {
//            search: "Search :",
//            lengthMenu: "Show _MENU_ entries",
//            paginate: {
//                first: "First",
//                last: "Last",
//                next: "Next",
//                previous: "Previous"
//            }
//        }
//    });

//});

$(document).ready(function () {
    // Initialize DataTable
    const table = $('#EvaluationTable').DataTable({
        ajax: {
            url: `/Review/GetAllDetails`, // API endpoint
            method: 'GET',
            dataSrc: function (response) {
                if (!response.success) {
                    alert(response.message || "No data available.");
                    return [];
                }

                let seenEmpIds = new Set();

                // Filter to ensure only one entry per employee
                let modifiedData = response.data.filter(item => {
                    let empIdentifier = item.firstName + " " + item.lastName;
                    if (seenEmpIds.has(empIdentifier)) {
                        return false; // Skip duplicate employees
                    } else {
                        seenEmpIds.add(empIdentifier);
                        return true; // Keep first occurrence
                    }
                });

                return modifiedData;
            }
        },
        columns: [
            { data: 'firstName', title: "First Name" },
            { data: 'lastName', title: "Last Name" },
            { data: 'totalAverage', title: "Total Average Marks" },
            { data: 'recommendation', title: "Recommendation" },
            { data: 'remarksConfirmation', title: "Remarks" }
        ],
        pageLength: 5, // Default rows per page
        lengthMenu: [5, 10, 15, 20],
        language: {
            search: "Search :",
            lengthMenu: "Show _MENU_ entries",
            paginate: {
                first: "First",
                last: "Last",
                next: "Next",
                previous: "Previous"
            }
        }
    });
});


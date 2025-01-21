$(document).ready(function () {
    getDeclarationData();

    // Handle activate button click
    $('#declarationTable').on('click', '.activate-btn', function () {
        var empId = $(this).data('id'); // Get the employee ID from the button's data-id attribute

        // Send an AJAX request to activate the employee account
        $.ajax({
            url: '/Review/ActivateAccount', // Replace with your actual controller URL
            type: 'POST',
            data: { empId: empId }, // Send the employee ID to the server
            success: function (response) {
                if (response.success) {
                    alert('Employee activated successfully!');
                    // Optionally, you can refresh the table or update the row to reflect the change
                    // For example, you can update the 'IsActive' field in the table row here
                    // Reload the table or update specific row:
                    $('#declarationTable').DataTable().ajax.reload();
                } else {
                    alert('Error: ' + response.message);
                }
            },
            error: function (error) {
                alert('An error occurred: ' + error.responseText);
            }
        });
    });
});

// Fetch declaration data and populate the table
function getDeclarationData() {

    const table = $('#declarationTable').DataTable({

        ajax: {
            url: '/Review/GetAllDetailsData', // Replace with your controller action
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
                           
                            <button class="btn btn-success btn-sm mx-1 activate-btn" data-id="${data}">
                                <i class="bi bi-check-circle"></i> Activate Account
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
}
// Declaration Status Chart (Corrected)
var declarationStatusChart = new Chart(document.getElementById('declarationStatusChart'), {
    type: 'pie',
    data: {
        labels: ['Approved', 'Total Approved'],  // Fixed typo, 'Total Appproved' -> 'Total Approved'
        datasets: [{
            data: [40, 60], // Modify with actual data
            backgroundColor: ['#1e90ff', '#32cd32'],
            borderColor: '#ffffff',
            borderWidth: 2
        }]
    },
    options: {
        responsive: true,
        plugins: {
            legend: {
                position: 'top',
            },
            tooltip: {
                callbacks: {
                    label: function (tooltipItem) {
                        return tooltipItem.label + ': ' + tooltipItem.raw + '%';  // Tooltip fix
                    }
                }
            }
        }
    }
});

// Another Chart (Example 2)
var weeklyDeclarationsChart = new Chart(document.getElementById('weeklyDeclarationsChart'), {
    type: 'pie',
    data: {
        labels: ['Week 1', 'Week 2', 'Week 3', 'Week 4'],
        datasets: [{
            data: [30, 50, 20], // Modify with actual data
            backgroundColor: ['#ff1493', '#ff8c00', '#8a2be2', '#32cd32'],
            borderColor: '#ffffff',
            borderWidth: 2
        }]
    },
    options: {
        responsive: true,
        plugins: {
            legend: {
                position: 'top',
            },
            tooltip: {
                callbacks: {
                    label: function (tooltipItem) {
                        return tooltipItem.label + ': ' + tooltipItem.raw + '%';
                    }
                }
            }
        }
    }
});

function GetDeclarationForEmpDec(EmpId) {
    debugger;
    // Construct the URL based on EmpId
    const url = EmpId ? `/Review/GetDeclarationDetails/${EmpId}` : '/Review/GetDeclarationDetails';
    console.log('Navigating to:', url);

    // Redirect to the URL
    window.location.href = url;

}
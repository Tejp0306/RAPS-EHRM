$(function () {
    getDocumentData();

    //handling view button
    $(document).on('click', '.view-btn', function () {
        const id = $(this).data('id'); // Extracting the actual ID value
        console.log("Document ID:", id); // Debugging

        $.ajax({
            url: `/Document/ShowFile`,  // API endpoint to fetch the file
            method: 'GET',
            data: { id: id }, // Send only the ID value
            success: function (response) {
                if (response.success) {
                    //console.log(response);

                    var filePath = response.data.data.filePath; // Ensure the API response contains filePath
                    //console.log(filePath);
                    var fileExtension = filePath.split('.').pop().toLowerCase(); // Get file extension

                    // Open file in a new tab based on extension
                    if (['jpg', 'jpeg', 'png', 'gif', 'bmp'].includes(fileExtension)) {
                        window.open(filePath, '_blank');
                    } else if (fileExtension === 'pdf') {
                        window.open(filePath, '_blank');
                    } else if (['doc', 'docx'].includes(fileExtension)) {
                        window.open(filePath, '_blank');
                    } else {
                        alert('This file format is not supported for preview.');
                    }
                } else {
                    alert("File not found.");
                }
            },
            error: function (error) {
                console.error('Error fetching file:', error);
                alert("An error occurred while fetching the file.");
            }
        });
    });

    // Handle Edit Button Click
    $('#DocumentTable').on('click', '.edit-btn', function () {
        const id = $(this).data('id');
        editDocument(id)
    });

});

function getDocumentData() {
    debugger;
    const table = $('#DocumentTable').DataTable({
        ajax: {
            url: '/Document/GetAllDocumentData', // Replace with your controller action
            method: 'GET',
            dataSrc: '' // Assuming data is a JSON array at the root
        },

        columns: [
            //{ data: 'documentId' },  // Ensure your API returns this field
            { data: 'documentType' },
            { data: 'description' },
            { data: 'uploadedAt' },
            {
                data: 'documentId',  // Use the correct property instead of 'id'
                render: function (data) {
                    return `
                        <div class="d-flex justify-content-center">
                            <button class="btn btn-info btn-sm mx-1 view-btn" data-id="${data}">
                                <i class="bi bi-eye"></i> View
                            </button>
                            <button class="btn btn-warning btn-sm mx-1 edit-btn" data-id="${data}">
                                <i class="bi bi-pencil"></i> Edit
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
            search: "Search Documents:",
            lengthMenu: "Show _MENU_ entries",
            info: "Showing _START_ to _END_ of _TOTAL_ documents",
            infoEmpty: "No documents available",
            paginate: {
                first: "First",
                last: "Last",
                next: "Next",
                previous: "Previous"
            }
        }
    });
}


function editDocument(id) {
    $.ajax({
        url: '/Document/GetDocumentDetails/' + id, // API endpoint
        method: 'GET',
        data: { id: id },
        success: function (response) {
            // Log response for debugging
            console.log('Full Response:', response);

            if (response.success && response.data && response.data.data) {
                const documentData = response.data.data;

                debugger;

                // Populate form fields with retrieved data
                $('#DocumentId').val(documentData.documentId);
                $('#DocumentType').val(documentData.documentType || '');
                $('#documentDescription').val(documentData.description || '');
                //$("#file").val(documentData.filePath || '');
                
            } else {
                console.error('Error: Invalid or missing data in response.');
            }
        },
        error: function (error) {
            console.error('AJAX Error:', error);
            alert('There was an error processing your request.');
        }
    });
}

﻿$(document).ready(function () {
    // Initialize DataTable and fetch data via AJAX
    const table = $('#NoticeBoardTable').DataTable({
        ajax: {
            url: '/Master/GetAllAddNoticeBoard',
            method: 'GET',
            dataSrc: ''
        },
        columns: [
            {
                render: function (data, type, row, meta) {
                    return meta.row + 1;
                }
            },
            { data: 'headingName' },
            { data: 'description' },
            {
                data: 'expiryDate',
                render: function (data) {
                    if (!data) return '-';
                    const date = new Date(data);
                    return date.toLocaleDateString(); // e.g. 5/16/2025
                }
            },
            {
                data: 'id',
                render: function (data) {
                    return `
                    <div class="d-flex justify-content-center">
                        <button class="btn btn-info btn-sm mx-1 view-btn" data-id="${data}">
                            <i class="bi bi-eye"></i> View
                        </button>
                        <button class="btn btn-danger btn-sm mx-1 delete-btn" data-id="${data}">
                            <i class="bi bi-trash"></i> Delete
                        </button>
                        <button class="btn btn-warning btn-sm mx-1 edit-btn" data-id="${data}">
                            <i class="bi bi-pencil"></i> Edit
                        </button>
                    </div>
                `;
                },
                orderable: false
            }
        ],
        pageLength: 5,
        lengthMenu: [5, 10, 15, 20],
        language: {
            search: "Search Prescribers:",
            lengthMenu: "Show _MENU_ entries",
            info: "Showing _START_ to _END_ of _TOTAL_ prescribers",
            infoEmpty: "No prescribers available",
            paginate: {
                first: "First",
                last: "Last",
                next: "Next",
                previous: "Previous"
            }
        }
    });


   
    // Handle Delete Button Click for NoticeBoard
    $('#NoticeBoardTable').on('click', '.delete-btn', function () {
        const noticeBoardID = $(this).data('id');  // Get the Notice Board ID from the data-id attribute

        // Confirm before deleting
        if (confirm(`Are you sure you want to delete this Notice ?`)) {
            // Make AJAX request to delete the notice board
            $.ajax({
                url: `/Master/DeleteAddNoticeBoard`,  // API endpoint to delete the notice board
                method: 'POST',
                data: { id: noticeBoardID },
                success: function (response) {
                    if (response.success) {
                        // Remove the row from the table
                       // $(`#row-${noticeBoardID}`).remove(); // Assuming the table rows have IDs like 'row-1', 'row-2', etc.
                        
                        // Optionally show a success message
                        alert(response.message);
                        location.reload();
                    } else {
                        // Handle error if the delete fails
                        alert(response.message || 'An error occurred while deleting the notice board.');
                    }
                },
                error: function (error) {
                    console.error('Error deleting notice board:', error);
                    alert('An error occurred while deleting the notice board.');
                }
            });
        }
    });

    // Handle Edit Button Click
    $('#NoticeBoardTable').on('click', '.edit-btn', function () {
        const NoticeBoardID = $(this).data('id');
        editNoticeBoardDetails(NoticeBoardID)
    });

    $('#NoticeBoardTableold').on('click', '.view-btn', function () {
        const id = $(this).data('id');
        $.ajax({
            url: `/Master/ShowFile`,  // API endpoint to delete the notice board
            method: 'GET',
            data: { id: id },
            success: function (response) {
                if (response.success) {
                    const FileData = response.data.data;
                    var FilePath = FileData.filePath
                    // Handle image files


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
    //handling view button
    $('#NoticeBoardTable').on('click', '.view-btn', function () {
        const id = $(this).data('id');
        $.ajax({
            url: `/Master/ShowFile`,  // API endpoint to fetch the file
            method: 'GET',
            data: { id: id },
            success: function (response) {
                if (response.success) {
                    const FileData = response.data.data;
                    var FilePath = FileData.filePath;
                    var fileExtension = FilePath.split('.').pop().toLowerCase(); // Get file extension

                    // Check for image files
                    if (fileExtension === 'jpg' || fileExtension === 'jpeg' || fileExtension === 'png' || fileExtension === 'gif' || fileExtension === 'bmp') {
                        // Open image file directly in a new tab, keeping the URL clean
                        window.open(FilePath, '_blank');
                    }
                    // Check for PDF files
                    else if (fileExtension === 'pdf') {
                        // Open PDF in a new tab
                        window.open(FilePath, '_blank');
                    }
                    // Check for DOC/DOCX files
                    else if (fileExtension === 'doc' || fileExtension === 'docx') {
                        // Open DOC file in a new tab
                        window.open(FilePath, '_blank');
                    }
                    // Handle unsupported file types
                    else {
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


});

function editNoticeBoardDetails(NoticeBoardID) {
    $.ajax({
        url: '/Master/GetNoticeBoardDetails/' + NoticeBoardID, // API endpoint
        method: 'GET',
        data: { NoticeBoardID: NoticeBoardID },
        success: function (response) {
            // Log response for debugging
            console.log('Full Response:', response);

            if (response.success && response.data && response.data.data) {
                const noticeBoardData = response.data.data;

                // Populate form fields with retrieved data
                $('#NoticeBoardId').val(noticeBoardData.id || '');
                $('#HeadingName').val(noticeBoardData.headingName || '');
                $('#Description').val(noticeBoardData.description || '');
                $('#ExpiryDate').val(noticeBoardData.expiryDate || '');
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


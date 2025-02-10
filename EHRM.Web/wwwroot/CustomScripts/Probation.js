$(document).ready(function () {  
    GetProbationData();

    $('#AddReviewTable').on('click', '.edit-btn', function () {

        const questionID = $(this).data('id');

        EditQuestion(questionID);
    });

    // Handle Delete Button Click for NoticeBoard
    $('#AddReviewTable').on('click', '.delete-btn', function () {
        const questionID = $(this).data('id');  // Get the Notice Board ID from the data-id attribute

        // Confirm before deleting
        if (confirm(`Are you sure you want to delete this Question?`)) {
            // Make AJAX request to delete the notice board
            $.ajax({
                url: `/Review/DeleteReview`,  // API endpoint to delete the notice board
                method: 'POST',
                data: { id: questionID },
                success: function (response) {
                    if (response.success) {
                        //alert(response.message);
                        location.reload();
                    } else {
                        // Handle error if the delete fails
                        alert(response.message || 'An error occurred while deleting the review question.');
                    }
                },
                error: function (error) {
                    console.error('Error deleting Question:', error);
                    alert('An error occurred while deleting the review question.');
                }
            });
        }
    });
});

function GetProbationData() {


    const table = $('#AddReviewTable').DataTable({

        ajax: {
            url: '/Review/GetAllProbationData', // Replace with your controller action
            method: 'GET',
            dataSrc: '' // Data source key, empty string assumes the root of the JSON array
        },
        columns: [

            { data: 'id' },
            { data: 'question' },
            {

                data: 'id',
                render: function (data) {

                    //console.log(data);
                    return `
                                <div class="d-flex justify-content-center">
                                    <button class="btn btn-warning btn-sm mx-1 edit-btn" data-id="${data}" data-bs-toggle="modal" data-bs-target="#addQuestionModal" >
                                        <i class="bi bi-pencil"></i> Edit
                                    </button>
                                    <button class="btn btn-danger btn-sm mx-1 delete-btn" data-id="${data}">
                                        <i class="bi bi-trash"></i> Delete
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
            info: "Showing _START_ to _END_ of _TOTAL_ Question",
            infoEmpty: "No prescribers available",
            paginate: {
                first: "First",
                last: "Last",
                next: "Next",
                previous: "Previous"
            }
        }

    });
}

function EditQuestion(questionID) {
    $.ajax({
        url: '/Review/GetQuestionData/' + questionID, // API endpoint
        method: 'GET',
        success: function (response) {
        
            // Access the nested data object
            if (response.success && response.data && response.data.data) {

                const questionData = response.data.data;

                $('#Id').val(questionData.id);
                $('#Question').val(questionData.question);
    

            } else {
            alert("alert")
            }
        },
        error: function (error) {
           
        }
    });
}


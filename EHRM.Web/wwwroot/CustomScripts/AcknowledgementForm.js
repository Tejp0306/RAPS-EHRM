$(document).ready(function () {
    getAcknowledgeData();
});

// Function to fetch acknowledgment data and populate the DataTable
function getAcknowledgeData() {
    $('#acknowledgementTable').DataTable({
        destroy: true, // Ensures reloading without duplication
        ajax: {
            url: '/PostJoining/GetAllAcknowledgeForm', // API to get all acknowledgment forms
            method: 'GET',
            dataSrc: '' // Assuming JSON array response
        },
        columns: [
            { data: 'employeeName' },
            { data: 'signatureDate' },
            {
                data: 'id',
                render: function (data) {
                    return `
                        <div class="d-flex justify-content-center">
                            <button class="btn btn-info btn-sm mx-1 view-btn" data-id="${data}">
                                <i class="bi bi-eye"></i> View & Download
                            </button>
                        </div>
                    `;
                },
                orderable: false
            }
        ],
        pageLength: 5,
        lengthMenu: [5, 10, 15, 20]
    });

    // Bind click event for dynamically added buttons
    $('#acknowledgementTable tbody').on('click', '.view-btn', function () {
        const id = $(this).data('id');
        openAcknowledgementModal(id);
    });
}

// Function to open the modal and load acknowledgment details
function openAcknowledgementModal(acknowlegementFormID) {
    $.ajax({
        url: `/PostJoining/GetAcknowlegementFormDetails/${acknowlegementFormID}`,
        method: 'GET',
        success: function (response) {
            if (response.success) {
                console.log("API Response:", response); // Debugging log

                const data = response.data.data || response.data;

                // Populate modal fields
                $('#EmployeeName').html(`<strong>${data.employeeName || "N/A"}</strong>`);
                $('#SignatureDate0').val(data.signatureDate || "N/A");
                $('#EmployeeSignature').val(data.employeeSignature || "N/A");

                // Update acknowledgment statement
                $('#acknowledgementText').html(`
                    I, <strong>${data.employeeName || "N/A"}</strong>, hereby acknowledge that I have read and fully understood all the policies mentioned in the Employee Handbook provided to me. I agree to abide by these policies and will not violate any of them.
                `);

                // Set the download button action
                $('#downloadBtn').off('click').on('click', function () {
                    downloadAcknowledgement(data);
                });

                // Show the modal
                $('#acknowledgementModal').modal('show');
            } else {
                alert("Error: " + response.message);
            }
        },
        error: function () {
            alert("An error occurred while fetching the data.");
        }
    });
}

// Function to generate and download PDF
function downloadAcknowledgement(data) {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();

    // Extract modal content
    const employeeName = data.employeeName || "N/A";
    const signatureDate = data.signatureDate || "N/A";
    const employeeSignature = data.employeeSignature || "N/A";
    const acknowledgementText = `I, ${employeeName}, hereby acknowledge that I have read and fully understood all the policies mentioned in the Employee Handbook provided to me. I agree to abide by these policies and will not violate any of them.`;

    // Set PDF Title
    doc.setFont("helvetica", "bold");
    doc.setFontSize(16);
    doc.text("Acknowledgement Form", 20, 20);

    // Set Content
    doc.setFont("helvetica", "normal");
    doc.setFontSize(12);
    doc.text(`Employee Name: ${employeeName}`, 20, 40);
    doc.text(`Signature Date: ${signatureDate}`, 20, 50);
    doc.text(`Employee Signature: ${employeeSignature}`, 20, 60);

    // Acknowledgment Text
    doc.setFont("helvetica", "italic");
    doc.setFontSize(12);
    doc.text("Acknowledgement Statement:", 20, 80);

    doc.setFont("helvetica", "normal");
    doc.text(acknowledgementText, 20, 90, { maxWidth: 170 });

    // Save as PDF
    doc.save(`Acknowledgement_Form_${data.id}.pdf`);
}



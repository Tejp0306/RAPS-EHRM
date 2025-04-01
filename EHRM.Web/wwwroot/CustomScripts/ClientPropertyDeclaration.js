$(document).ready(function () {
    getClientDeclaration();
});

// Function to fetch acknowledgment data and populate the DataTable
function getClientDeclaration() {
    $('#clientDeclarationTable').DataTable({
        destroy: true,
        ajax: {
            url: '/PostJoining/GetAllClientPropertyDeclaration',
            method: 'GET',
            dataSrc: ''
        },
        columns: [
            { data: 'employeeName' },
            { data: 'clientName' },
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
    $('#clientDeclarationTable tbody').on('click', '.view-btn', function () {
        const id = $(this).data('id');
        openClientDeclarationModal(id);
    });
}

function openClientDeclarationModal(Id) {
    $.ajax({
        url: `/PostJoining/GetClientDeclarationById/${Id}`,
        method: 'GET',
        success: function (response) {
            if (response.success) {
                console.log("API Response:", response);

                const data = response.data.data || response.data;
                debugger;
                // Populate modal fields
                $('#EmployeeNameone').val(data?.employeeName || "N/A");
                $('#ClientNameone').val(data?.clientName || "N/A");
                $('#ReceivedDateone').val(data?.receivedDate || "N/A");
                $('#ItemsReceivedone').val(data?.itemsReceived || "N/A");
                $('#Signatureone').val(data?.signature || "N/A");
                $('#ConfirmationDateone').val(data?.confirmationDate || "N/A");

                // Set Employee Name Confirmation inside the <p> tag
                $('#EmployeeNameConfirmone').html(data?.employeeName || "N/A");

                // Ensure download button exists
                if ($('#downloadBtn').length === 0) {
                    $('#clientDeclarationModal .modal-footer').append(
                        `<button id="downloadBtn" class="btn btn-primary">Download PDF</button>`
                    );
                }

                // Attach click event (ensure no duplicate events)
                $('#downloadBtn').off('click').on('click', function () {
                    downloadClientDeclaration(data);
                });

                // Show the modal
                $('#clientDeclarationModal').modal('show');
            } else {
                alert("Error: " + response.message);
            }
        },
        error: function () {
            alert("An error occurred while fetching the data.");
        }
    });
}

function downloadClientDeclaration(data) {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();

    // Extract modal content
    const employeeName = data?.employeeName || "N/A";
    const clientName = data?.clientName || "N/A";
    const receivedDate = data?.receivedDate || "N/A";
    const itemsReceived = data?.itemsReceived || "N/A";
    const employeeSignature = data?.signature || "N/A";
    const confirmationDate = data?.confirmationDate || "N/A";

    // Acknowledgment Text
    const acknowledgementText = `I, ${employeeName}, confirm that if I am unable to return the property, I authorize that a suitable amount of the value of the property can be deducted from my final settlement amount.`;

    // Set PDF Title
    doc.setFont("helvetica", "bold");
    doc.setFontSize(16);
    doc.text("Client Property Declaration Form", 20, 20);

    // Add Employee & Client Information
    doc.setFont("helvetica", "normal");
    doc.setFontSize(12);
    doc.text(`Full Name of Employee: ${employeeName}`, 20, 40);
    doc.text(`Client Name: ${clientName}`, 20, 50);
    doc.text(`Date of Receipt: ${receivedDate}`, 20, 60);

    // Items Received (Multiline)
    doc.text("Items Received:", 20, 70);
    doc.setFont("helvetica", "italic");
    doc.text(itemsReceived, 20, 80, { maxWidth: 170 });

    // Reset font to normal
    doc.setFont("helvetica", "normal");

    // Declaration Text
    doc.text(
        "I confirm that I will take good care of the above property and ensure it is always kept in good working condition...",
        20, 100,
        { maxWidth: 170 }
    );

    // Acknowledgment Statement
    doc.setFont("helvetica", "bold");
    doc.text("Acknowledgment Statement:", 20, 120);

    doc.setFont("helvetica", "normal");
    doc.text(acknowledgementText, 20, 130, { maxWidth: 170 });

    // Signature and Confirmation Date
    doc.text(`Signature of Employee: ${employeeSignature}`, 20, 150);
    doc.text(`Confirmation Date: ${confirmationDate}`, 20, 160);

    // Save the PDF
    doc.save(`Client_Declaration_${data.id || "Form"}.pdf`);
}


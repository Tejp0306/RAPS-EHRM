var empIdFromSession = parseInt('@empIdFromSession');

function openClientDeclarationModal(empId) {
    $.ajax({
        url: `/PostJoining/GetClientDeclarationById/${empId}`,
        method: 'GET',
        success: function (response) {
            if (response.success) {

                const data = response.data.data || response.data;

                // Populate modal fields
                $('#EmployeeName').val(data?.employeeName || "N/A");
                $('#ClientName').val(data?.clientName || "N/A");
                $('#ReceivedDate').val(data?.receivedDate || "N/A");
                $('#ItemsReceived').val(data?.itemsReceived || "N/A");
                $('#SignatureEmployee').val(data?.signature || "N/A");
                $('#ConfirmationDate').val(data?.confirmationDate || "N/A");

                // Set Employee Name Confirmation inside the <p> tag
                $('#EmployeeNameConfirm').html(data?.employeeName || "N/A");

                // Ensure download button exists
                if ($('#downloadBtn').length === 0) {
                    $('#clientDeclarationModal .modal-footer').append(
                        `<button id="downloadBtn" class="btn btn-primary">Download PDF</button>`
                    );
                }

                

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

function downloadClientDecla(empId) {
    $.ajax({
        url: `/PostJoining/GetClientDeclarationById/${empId}`,
        method: 'GET',
        success: function (response) {
            if (response.success) {


                const data = response.data.data || response.data;
                downloadClientDeclaration(data)
              
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


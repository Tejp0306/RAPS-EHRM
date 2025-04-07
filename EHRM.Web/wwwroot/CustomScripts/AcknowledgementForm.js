var empIdFromSession = parseInt('@empIdFromSession');

// Function to open the modal and load acknowledgment details
function openAcknowledgementModal(empId) {
    $.ajax({
        url: `/PostJoining/GetAcknowlegementFormDetails/${empId}`,
        method: 'GET',
        success: function (response) {
            if (response.success) {

                const data = response.data.data || response.data;
                // Populate modal fields
                $('#EmployeeNameone').html(`<strong>${data.employeeName || "N/A"}</strong>`);
                $('#SignDate').val(data.signatureDate || "N/A");
                $('#EmpSignature').val(data.employeeSignature || "N/A");

                // Update acknowledgment statement
                $('#acknowledgementText').html(`
                    I, <strong>${data.employeeName || "N/A"}</strong>, hereby acknowledge that I have read and fully understood all the policies mentioned in the Employee Handbook provided to me. I agree to abide by these policies and will not violate any of them.
                `);


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

function downloadAcknowledgementForm(empId) {
    $.ajax({
        url: `/PostJoining/GetAcknowlegementFormDetails/${empId}`,
        method: 'GET',
        success: function (response) {
            if (response.success) {

                const data = response.data.data || response.data;
                downloadAcknowledgement(data)
            } else {
                alert("Error: " + response.message);
            }
        },
        error: function () {
            alert("An error occurred while fetching the data.");
        }
    });
}


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



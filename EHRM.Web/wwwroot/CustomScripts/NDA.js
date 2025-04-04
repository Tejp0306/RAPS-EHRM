var empIdFromSession = parseInt('@empIdFromSession');

function ndaModal(empId) {
    $.ajax({
        url: `/PostJoining/GetNDAFormById/${empId}`,
        method: 'GET',
        success: function (response) {
            if (response.success) {

                const data = response.data.data || response.data;

                // Populate input fields
                $('#EmployeeNameone').val(data?.employeeName || "");
                $('#AgreementDateone').val(data?.agreementDate || "");
                $('#Signatureone').val(data?.signature || "");

                // Show the modal
                $('#ndaModal').modal('show');


            } else {
                alert("Error: " + response.message);
            }
        },
        error: function () {
            alert("An error occurred while fetching the NDA data.");
        }
    });
}

function ndaDownload(empId) {
    $.ajax({
        url: `/PostJoining/GetNDAFormById/${empId}`,
        method: 'GET',
        success: function (response) {
            if (response.success) {

                const data = response.data.data || response.data;
                downloadNDAForm(data);

            } else {
                alert("Error: " + response.message);
            }
        },
        error: function () {
            alert("An error occurred while fetching the NDA data.");
        }
    });
}
function downloadNDAForm(data) {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();

    // Extract modal content
    const employeeName = data?.employeeName || "N/A";
    const agreementDate = data?.agreementDate || "N/A";
    const employeeSignature = data?.signature || "N/A";

    // Set PDF Title
    doc.setFont("helvetica", "bold");
    doc.setFontSize(16);
    doc.text("Non-Disclosure Agreement (NDA)", 20, 20);

    // NDA Content
    doc.setFont("helvetica", "normal");
    doc.setFontSize(12);

    // Initial text
    doc.text("In performing your duties, you may have access to company data, including but not limited to:", 20, 40, { maxWidth: 170 });

    doc.text("• Information about RAPS Consulting Inc. (RAPS) employees, other private/proprietary/confidential data", 20, 55, { maxWidth: 170 });
    doc.text("• Technical information, as well as financial, contractor, or organizational data.", 20, 65, { maxWidth: 170 });

    doc.text("You are charged with the responsibility of accessing only the files/information you need to perform your job and of safeguarding and maintaining the confidentiality of that information.", 20, 80, { maxWidth: 170 });

    // Safeguarding section
    doc.text("Safeguarding includes:", 20, 100, { maxWidth: 160 });

    doc.text("• Protecting user IDs, passwords, storage media, online screens, printouts, magnetic tapes, personal computer hard disks, laptops, and technical information from unauthorized access.", 25, 115, { maxWidth: 160 });
    doc.text("• Not removing information from RAPS property other than for temporary periods as necessary.", 25, 135, { maxWidth: 160 });
    doc.text("• Maintaining confidentiality by not disclosing information to anyone except authorized personnel.", 25, 150, { maxWidth: 160 });

    // Final statement
    doc.text("A copy of this agreement will be maintained in your employment file. Violation of these requirements may result in disciplinary action, up to and including termination of employment.", 20, 170, { maxWidth: 170 });

    // Employee Details
    doc.setFont("helvetica", "bold");
    doc.text("Employee Details:", 20, 190);
    doc.setFont("helvetica", "normal");
    doc.text(`Full Name: ${employeeName}`, 20, 200);
    doc.text(`Date: ${agreementDate}`, 20, 210);
    doc.text(`Signature: ${employeeSignature}`, 20, 220);

    // Save the PDF
    doc.save(`NDA_Agreement_${data.id || "Form"}.pdf`);

}

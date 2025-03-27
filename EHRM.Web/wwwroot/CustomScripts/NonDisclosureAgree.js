$(document).ready(function () {
    getNDA();
});

// Function to fetch acknowledgment data and populate the DataTable
function getNDA() {
    $('#ndaTable').DataTable({
        destroy: true,
        ajax: {
            url: '/PostJoining/GetAllNDAForm',
            method: 'GET',
            dataSrc: ''
        },
        columns: [
            { data: 'employeeName' },
            { data: 'agreementDate' },
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
    $('#ndaTable tbody').on('click', '.view-btn', function () {
        const id = $(this).data('id');
        ndaModal(id);
    });
}

function ndaModal(Id) {
    $.ajax({
        url: `/PostJoining/GetNDAFormById/${Id}`,
        method: 'GET',
        success: function (response) {
            if (response.success) {
                console.log("API Response:", response);

                const data = response.data.data || response.data;

                // Populate input fields
                $('#EmployeeNameone').val(data?.employeeName || "");
                $('#AgreementDateone').val(data?.agreementDate || "");
                $('#Signatureone').val(data?.signature || "");

                // Show the modal
                $('#ndaModal').modal('show');

                // Attach event to download button
                $('#downloadNDA').off('click').on('click', function () {
                    downloadNDAForm(data);
                });

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
    doc.text("In performing your duties, you may have access to company data, including but not limited to:", 20, 40, { maxWidth: 170 });

    doc.text("- Personal information about RAPS Consulting Inc. (RAPS) employees.", 25, 50, { maxWidth: 160 });
    doc.text("- Other private/proprietary/confidential data or technical information.", 25, 60, { maxWidth: 160 });
    doc.text("- Financial, contractor, or organizational data.", 25, 70, { maxWidth: 160 });

    doc.text("By signing this document, you agree to:", 20, 90, { maxWidth: 170 });
    doc.text("- Protect user credentials and sensitive information.", 25, 100, { maxWidth: 160 });
    doc.text("- Not disclose information to unauthorized personnel.", 25, 110, { maxWidth: 160 });
    doc.text("- Ensure confidentiality and safeguard all company data.", 25, 120, { maxWidth: 160 });

    // Employee Details
    doc.setFont("helvetica", "bold");
    doc.text("Employee Details:", 20, 140);
    doc.setFont("helvetica", "normal");
    doc.text(`Full Name: ${employeeName}`, 20, 150);
    doc.text(`Date: ${agreementDate}`, 20, 160);
    doc.text(`Signature: ${employeeSignature}`, 20, 170);

    // Save the PDF
    doc.save(`NDA_Agreement_${data.id || "Form"}.pdf`);
}



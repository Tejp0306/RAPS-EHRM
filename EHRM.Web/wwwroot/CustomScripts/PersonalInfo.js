$(document).ready(function () {
    getPersonalInfoData();
});

// Function to fetch acknowledgment data and populate the DataTable
function getPersonalInfoData() {
    $('#personalInfoTable').DataTable({
        destroy: true,
        ajax: {
            url: '/PostJoining/GetAllPersonalInfoForm',
            method: 'GET',
            dataSrc: ''
        },
        columns: [
            { data: 'employeeName' },
            { data: 'personalEmail' },
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
    $('#personalInfoTable tbody').on('click', '.view-btn', function () {
        const id = $(this).data('id');
        openPersonalInfoModal(id);
    });
}

function openPersonalInfoModal(personalInfoID) {
    $.ajax({
        url: `/PostJoining/GetPersonalInfoById/${personalInfoID}`,
        method: 'GET',
        success: function (response) {
            if (response.success) {
                console.log("API Response:", response);

                const data = response.data.data || response.data;

                // Populate modal fields
                $('#EmployeeNameOne').val(data?.employeeName || "N/A");
                $('#PersonalEmailone').val(data?.personalEmail || "N/A");
                $('#PermanentAddressone').val(data?.permanentAddress || "N/A");
                $('#CurrentAddressone').val(data?.currentAddress || "N/A");
                $('#MobilePhoneone').val(data?.mobilePhone || "N/A");

                // Emergency Contacts
                $('#EmergencyContact1Nameone').val(data?.emergencyContact1Name || "N/A");
                $('#EmergencyContact1Relationshipone').val(data?.emergencyContact1Relationship || "N/A");
                $('#EmergencyContact1Phoneone').val(data?.emergencyContact1Phone || "N/A");

                $('#EmergencyContact2Nameone').val(data?.emergencyContact2Name || "N/A");
                $('#EmergencyContact2Relationshipone').val(data?.emergencyContact2Relationship || "N/A");
                $('#EmergencyContact2Phoneone').val(data?.emergencyContact2Phone || "N/A");

                // Signature and Date
                $('#Signatureone').val(data?.signature || "N/A");
                $('#FormDateOne').val(data?.formDate || "N/A");

                // Ensure download button exists
                if ($('#downloadBtn').length === 0) {
                    $('#personalInfoModal .modal-footer').append(`
                        <button id="downloadBtn" class="btn btn-primary">Download PDF</button>
                    `);
                }

                // Attach click event (ensure no duplicate events)
                $('#downloadBtn').off('click').on('click', function () {
                    downloadPersonalInfo(data);
                });

                // Show the modal
                $('#personalInfoModal').modal('show');
            } else {
                alert("Error: " + response.message);
            }
        },
        error: function () {
            alert("An error occurred while fetching the data.");
        }
    });
}


function downloadPersonalInfo(data) {
    const jsPDF = window.jspdf.jsPDF;
    const doc = new jsPDF();

    // Extract modal content
    const employeeName = $('#EmployeeNameOne').val() || "N/A";
    const personalEmail = $('#PersonalEmailone').val() || "N/A";
    const permanentAddress = $('#PermanentAddressone').val() || "N/A";
    const currentAddress = $('#CurrentAddressone').val() || "N/A";
    const mobilePhone = $('#MobilePhoneone').val() || "N/A";
    const emergencyContact1Name = $('#EmergencyContact1Nameone').val() || "N/A";
    const emergencyContact1Relationship = $('#EmergencyContact1Relationshipone').val() || "N/A";
    const emergencyContact1Phone = $('#EmergencyContact1Phoneone').val() || "N/A";
    const emergencyContact2Name = $('#EmergencyContact2Nameone').val() || "N/A";
    const emergencyContact2Relationship = $('#EmergencyContact2Relationshipone').val() || "N/A";
    const emergencyContact2Phone = $('#EmergencyContact2Phoneone').val() || "N/A";
    const signature = $('#Signatureone').val() || "N/A";
    const formDate = $('#FormDateOne').val() || "N/A";

    // Set PDF Title
    doc.setFont("helvetica", "bold");
    doc.setFontSize(16);
    doc.text("Personal Information Form", 20, 20);

    // Set Content
    doc.setFont("helvetica", "normal");
    doc.setFontSize(12);
    doc.text(`Employee Name: ${employeeName}`, 20, 40);
    doc.text(`Personal Email ID: ${personalEmail}`, 20, 50);
    doc.text(`Permanent Address: ${permanentAddress}`, 20, 60);
    doc.text(`Current Address: ${currentAddress}`, 20, 70);
    doc.text(`Mobile Phone #: ${mobilePhone}`, 20, 80);

    // Emergency Contact Information
    doc.text("Emergency Contact 1:", 20, 100);
    doc.text(`Name: ${emergencyContact1Name}`, 20, 110);
    doc.text(`Relationship: ${emergencyContact1Relationship}`, 20, 120);
    doc.text(`Phone #: ${emergencyContact1Phone}`, 20, 130);

    doc.text("Emergency Contact 2:", 20, 140);
    doc.text(`Name: ${emergencyContact2Name}`, 20, 150);
    doc.text(`Relationship: ${emergencyContact2Relationship}`, 20, 160);
    doc.text(`Phone #: ${emergencyContact2Phone}`, 20, 170);

    // Signature and Date
    doc.text(`Signature: ${signature}`, 20, 180);
    doc.text(`Date: ${formDate}`, 20, 190);

    // Save as PDF
    doc.save(`Personal_Info_Form_${data.id || "Unknown"}.pdf`);
}




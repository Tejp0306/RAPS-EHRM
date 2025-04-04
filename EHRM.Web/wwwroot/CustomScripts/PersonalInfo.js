var empIdFromSession = parseInt('@empIdFromSession');


function openPersonalInfoModal(empId) {
    $.ajax({
        url: `/PostJoining/GetPersonalInfoById/${empId}`,
        method: 'GET',
        success: function (response) {
            if (response.success) {

                const data = response.data.data || response.data;

                // Populate modal fields
                $('#EmpName').val(data?.employeeName || "N/A");
                $('#PersEmail').val(data?.personalEmail || "N/A");
                $('#PermanentAdd').val(data?.permanentAddress || "N/A");
                $('#CurrentAdd').val(data?.currentAddress || "N/A");
                $('#MobilePhn').val(data?.mobilePhone || "N/A");

                // Emergency Contacts
                $('#EmergencyCont1Name').val(data?.emergencyContact1Name || "N/A");
                $('#EmergencyCont1Relationship').val(data?.emergencyContact1Relationship || "N/A");
                $('#EmergencyCont1Phone').val(data?.emergencyContact1Phone || "N/A");

                $('#EmergencyCont2Name').val(data?.emergencyContact2Name || "N/A");
                $('#EmergencyCont2Relationship').val(data?.emergencyContact2Relationship || "N/A");
                $('#EmergencyCont2Phone').val(data?.emergencyContact2Phone || "N/A");

                // Signature and Date
                $('#Sign').val(data?.signature || "N/A");
                $('#FormDateO').val(data?.formDate || "N/A");

                // Ensure download button exists
                if ($('#downloadBtn').length === 0) {
                    $('#personalInfoModal .modal-footer').append(`
                        <button id="downloadBtn" class="btn btn-primary">Download PDF</button>
                    `);
                }


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

function downloadPersonalInformation(empId) {
    $.ajax({
        url: `/PostJoining/GetPersonalInfoById/${empId}`,
        method: 'GET',
        success: function (response) {
            if (response.success) {

                const data = response.data.data || response.data;
                downloadPersonalInfo(data);
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
    debugger;
    // Extract modal content
    const employeeName = data?.employeeName || "N/A";
    const personalEmail = data?.personalEmail || "N/A";
    const permanentAddress = data?.permanentAddress || "N/A";
    const currentAddress = data?.currentAddress || "N/A";
    const mobilePhone = data?.mobilePhone || "N/A";
    const emergencyContact1Name = data?.emergencyContact1Name || "N/A";
    const emergencyContact1Relationship = data?.emergencyContact1Relationship || "N/A";
    const emergencyContact1Phone = data?.emergencyContact1Phone || "N/A";
    const emergencyContact2Name = data?.emergencyContact2Name || "N/A";
    const emergencyContact2Relationship = data?.emergencyContact2Relationship || "N/A";
    const emergencyContact2Phone = data?.emergencyContact2Phone || "N/A";
    const signature = data?.signature || "N/A";
    const formDate = data?.formDate || "N/A";

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




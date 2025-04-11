var empIdFromSession = parseInt('@empIdFromSession');

$(document).ready(function () {

    getPersonalInfoDataDetail();

});

// Function to fetch acknowledgment data and populate the DataTable

function getPersonalInfoDataDetail() {
    $.ajax({
        url: '/PostJoining/GetBackGroundFormDetails',
        method: 'GET',
        dataType: 'json',
        success: function (response) {
            console.log("Full API Response:", response); // ✅ Correct variable

            $('#AllBackgroundVerificationData').DataTable({
                destroy: true,
                processing: true,
                data: response.bgvForms,
                columns: [
                    { data: 'empId', title: 'Employee Id' },
                    {
                        title: 'Employee Name',
                        render: function (data, type, row) {
                            const first = row.firstName || '';
                            const middle = row.middleName || '';
                            return `${first} ${middle}`.trim();
                        }
                    },
                    { data: 'email', title: 'Email Address' },
                    {
                        data: 'empId',
                        title: 'Actions',
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

        },
        error: function (xhr, status, error) {
            console.error("AJAX Error:", xhr.responseText || error);
        }
    });


    // Bind click events properly
    $('#AllBackgroundVerificationData tbody').off("click").on('click', '.view-btn', function () {
        const id = $(this).data('id');
        //console.log("Button clicked for ID:", empId);
        bgvformModalDetail(id);
    });
}




function bgvformModalDetail(EmpId) {
    if (!EmpId) {
        console.error("Error: Employee ID is missing.");
        alert("Invalid Employee ID.");
        return;
    }

    $.ajax({
        url: `/PostJoining/GetBGVDataByEmpId/${EmpId}`,
        method: 'GET',
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                console.log(response.data);
                renderBGVData(response.data);
            } else {
                $('#formPreviewContent').html('<p class="text-danger">No data found for the selected employee.</p>');
            }
        },
        error: function () {
            $('#formPreviewContent').html('<p class="text-danger">Error loading form data.</p>');
        },
        error: function (xhr, status, error) {
            console.error("AJAX Error:", xhr.responseText || error);
            alert("An error occurred while fetching the NDA data.");
        }
    });
}



function renderBGVData(data) {
    if (!data) {
        console.warn("No data provided for rendering BGV details.");
        return;
    }

    let formContent = `
        <h5>Employee Information</h5>
        <table class="table table-bordered">
            <tr><th>First Name</th><td>${data.firstName || '-'}</td></tr>
            <tr><th>Middle Name</th><td>${data.middleName || '-'}</td></tr>
            <tr><th>Last Name</th><td>${data.lastName || '-'}</td></tr>
            <tr><th>Father's First Name</th><td>${data.fatherFirstName || '-'}</td></tr>
            <tr><th>Father's Middle Name</th><td>${data.fatherMiddleName || '-'}</td></tr>
            <tr><th>Father's Last Name</th><td>${data.fatherLastName || '-'}</td></tr>
            <tr><th>Date of Birth</th><td>${formatDate(data.dateOfBirth)}</td></tr>
            <tr><th>Place of Birth</th><td>${data.placeOfBirth || '-'}</td></tr>
            <tr><th>Gender</th><td>${data.gender || '-'}</td></tr>
            <tr><th>Marital Status</th><td>${data.maritalStatus || '-'}</td></tr>
            <tr><th>Nationality</th><td>${data.nationality || '-'}</td></tr>
            <tr><th>Mobile Number</th><td>${data.mobileNumber || '-'}</td></tr>
            <tr><th>Alternate Number</th><td>${data.alternateNumber || '-'}</td></tr>
            <tr><th>Email</th><td>${data.email || '-'}</td></tr>
            <tr><th>Complete Address</th><td>${data.completeAddress || '-'}</td></tr>
            <tr><th>Nearest Landmark</th><td>${data.nearestLandmark || '-'}</td></tr>
            <tr><th>Course Name</th><td>${data.courseName || '-'}</td></tr>
            <tr><th>Program Type</th><td>${data.programType || '-'}</td></tr>
            <tr><th>College Name</th><td>${data.collegeName || '-'}</td></tr>
            <tr><th>University/Board Name</th><td>${data.universityBoardName || '-'}</td></tr>
            <tr><th>Passing Year</th><td>${data.passingYear || '-'}</td></tr>
            <tr><th>Proof Type</th><td>${data.proofType || '-'}</td></tr>
            <tr><th>Reference Name</th><td>${data.referenceName || '-'}</td></tr>
            <tr><th>Reference Contact</th><td>${data.referenceContact || '-'}</td></tr>
            <tr><th>Consent Given</th><td>${data.consentGiven ? 'Yes' : 'No'}</td></tr>
        </table>

        <h5 class="mt-4">Previous Employments</h5>
        <div class="table-responsive" style="max-height: 300px; overflow-x: auto; white-space: nowrap;">
            <table class="table table-bordered">
                <thead class="table-light">
                    <tr>
                        <th>Nature of Employment</th>
                        <th>Designation</th>
                        <th>Department</th>
                        <th>Official Title</th>
                        <th>Company Name</th>
                        <th>Organization Address</th>
                        <th>From Date</th>
                        <th>To Date</th>
                        <th>Employee Code</th>
                        <th>CTC Per Annum</th>
                        <th>Key Responsibility</th>
                        <th>Employment Tenure</th>
                        <th>Reason for Leaving</th>
                        <th>Reporting Manager Name</th>
                        <th>Manager Designation</th>
                        <th>Is Manager Still in Company</th>
                        <th>Company Landline</th>
                        <th>Personal Mobile No.</th>
                        <th>Best Time to Reach</th>
                    </tr>
                </thead>
                <tbody>`;

    if (data.previousEmployments && Array.isArray(data.previousEmployments) && data.previousEmployments.length > 0) {
        data.previousEmployments.forEach(emp => {
            formContent += `
                <tr>
                    <td>${emp.natureOfEmployment || '-'}</td>
                    <td>${emp.currentDesignation || '-'}</td>
                    <td>${emp.department || '-'}</td>
                    <td>${emp.officialTitle || '-'}</td>
                    <td>${emp.payrollCompanyName || '-'}</td>
                    <td>${emp.organizationAddress || '-'}</td>
                    <td>${formatDate(emp.fromDate)}</td>
                    <td>${formatDate(emp.toDate)}</td>
                    <td>${emp.employeeCode || '-'}</td>
                <td>${emp.ctcPerAnnum != null ? emp.ctcPerAnnum : '-'}</td>
                    <td>${emp.keyResponsibility || '-'}</td>
                    <td>${emp.employmentTenure || '-'}</td>
                    <td>${emp.reasonForLeaving || '-'}</td>
                    <td>${emp.reportingManagerName || '-'}</td>
                    <td>${emp.reportingManagerDesignation || '-'}</td>
                <td>${emp.isReportingManagerStillInCompany === 'Yes' ? 'Yes' : 'No'}</td>
                    <td>${emp.companyLandline || '-'}</td>
                    <td>${emp.personalMobileNo || '-'}</td>
                    <td>${emp.bestTimeToReach || '-'}</td>
                </tr>`;
        });
    } else {
        formContent += `<tr><td colspan="19" class="text-center">No previous employment details found.</td></tr>`;
    }


    formContent += `</tbody></table></div>`;

    // ✅ Render the final content in the modal
    $('#formPreviewContentModal').html(formContent);

    // ✅ Show the modal
    $('#previewModal').modal('show');

     //✅ Bind download button event (if needed)
     $('#downloadBtn').off('click').on('click', function () {
         downloadBGVForm(data);
     });
}

// ✅ Function to Format Date Properly
function formatDate(dateString) {
    if (!dateString) return '-';
    const date = new Date(dateString);
    return date.toLocaleDateString('en-GB'); // Format: DD/MM/YYYY
}

function downloadBGVForm(data) {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();

    doc.setFont("helvetica", "bold");
    doc.setFontSize(16);
    doc.text("Employee Background Verification Form", 20, 20);
    let y = 30;

    const addSectionTitle = (title) => {
        doc.setFontSize(12);
        doc.setFont("helvetica", "bold");
        doc.text(title, 20, y);
        y += 8;
    };

    const addTable = (headers, rows) => {
        doc.autoTable({
            startY: y,
            head: [headers],
            body: rows,
            theme: "striped",
            styles: { font: "helvetica", fontSize: 10, cellPadding: 3 },
            headStyles: { fillColor: [50, 50, 50], textColor: [255, 255, 255] }
        });
        y = doc.lastAutoTable.finalY + 10;
    };

    addSectionTitle("Employee Personal Information");
    addTable(
        ["Field", "Value", "Field", "Value"],
        [
            ["First Name", data.firstName, "Middle Name", data.middleName],
            ["Last Name", data.lastName, "Gender", data.gender],
            ["Date of Birth", formatDate(data.dateOfBirth), "Place of Birth", data.placeOfBirth],
            ["Father's First Name", data.fatherFirstName, "Father's Last Name", data.fatherLastName],
            ["Marital Status", data.maritalStatus, "Nationality", data.nationality],
            ["Mobile Number", data.mobileNumber, "Alternate Number", data.alternateNumber],
            ["Email", data.email, "Nearest Landmark", data.nearestLandmark],
            ["Complete Address", data.completeAddress, "", ""]
        ]
    );

    addSectionTitle("Education Information");
    addTable(
        ["Field", "Value", "Field", "Value"],
        [
            ["Course Name", data.courseName, "Program Type", data.programType],
            ["College Name", data.collegeName, "University/Board Name", data.universityBoardName],
            ["Passing Year", data.passingYear, "Proof Type", data.proofType]
        ]
    );

    addSectionTitle("Reference Information");
    addTable(
        ["Field", "Value", "Field", "Value"],
        [
            ["Reference Name", data.referenceName, "Reference Contact", data.referenceContact],
            ["Consent Given", data.consentGiven ? "Yes" : "No", "", ""]
        ]
    );

    addSectionTitle("Previous Employment Details");

    const previousEmployments = data.previousEmployments || [];

    if (previousEmployments.length > 0) {
        let employmentRows = [];

        previousEmployments.forEach((emp, index) => {
            employmentRows.push([`Employment #${index + 1}`, "", "", ""]);
            employmentRows.push(["Nature of Employment", emp.natureOfEmployment || "N/A", "Designation", emp.currentDesignation || "N/A"]);
            employmentRows.push(["Department", emp.department || "N/A", "Official Title", emp.officialTitle || "N/A"]);
            employmentRows.push(["Company Name", emp.payrollCompanyName || "N/A", "Organization Address", emp.organizationAddress || "N/A"]);
            employmentRows.push(["From Date", formatDate(emp.fromDate), "To Date", formatDate(emp.toDate)]);
            employmentRows.push(["Employee Code", emp.employeeCode || "N/A", "CTC Per Annum", emp.ctcPerAnnum || "N/A"]);
            employmentRows.push(["Key Responsibility", emp.keyResponsibility || "N/A", "Employment Tenure", emp.employmentTenure || "N/A"]);
            employmentRows.push(["Reason for Leaving", emp.reasonForLeaving || "N/A", "", ""]);
            employmentRows.push(["Reporting Manager", emp.reportingManagerName || "N/A", "Manager Designation", emp.reportingManagerDesignation || "N/A"]);
            employmentRows.push(["Still in Company", emp.isReportingManagerStillInCompany || "N/A", "Best Time to Reach", emp.bestTimeToReach || "N/A"]);
            employmentRows.push(["Company Landline", emp.companyLandline || "N/A", "Personal Mobile No.", emp.personalMobileNo || "N/A"]);
            employmentRows.push(["", "", "", ""]); // Spacer
        });

        addTable(["Field", "Value", "Field", "Value"], employmentRows);
    } else {
        addTable(["Message", ""], [["No previous employment details found.", ""]]);
    }


    doc.save(`Employee_BGV_${data.firstName || "N/A"}_${data.lastName || "N/A"}.pdf`);
}





// ✅ Get EmpId from Session in JavaScript
var empIdFromSession = parseInt('@empIdFromSession');

//✅ Automatically Load BGV Form with Employee Data in Modal
$(document).ready(function () {
    if (typeof empIdFromSession !== "undefined" && empIdFromSession) {
        loadEmployeeForm(empIdFromSession);
        loadMasterSheetData(empIdFromSession);
    }
});


// ✅ Load Employee Form with Data(on bgv preview)
function loadEmployeeForm(empId) {
    $('#formPreviewContent').html('<p class="text-center text-muted">Loading form details...</p>');

    // ✅ AJAX Call to Fetch Data for BGV Form
    $.ajax({
        url: '/PostJoining/GetBGVDetails',
        type: 'GET',
        data: { EmpId: empId },
        success: function (response) {
            if (response.success) {
                console.log(response.data);
                renderEmployeeData(response.data);
            } else {
                $('#formPreviewContent').html('<p class="text-danger">No data found for the selected employee.</p>');
            }
        },
        error: function () {
            $('#formPreviewContent').html('<p class="text-danger">Error loading form data.</p>');
        }
    });
}

// ✅ Load Employee Form with Data(on bgv download)
function loadDownloadForm(empId) {


    // ✅ AJAX Call to Fetch Data for BGV Form
    $.ajax({
        url: '/PostJoining/GetBGVDetails',
        type: 'GET',
        data: { EmpId: empId },
        success: function (response) {
            if (response.success) {
                console.log(response.data);
                downloadEmployeeForm(response.data);
            } else {
                $('#formPreviewContent').html('<p class="text-danger">No data found for the selected employee.</p>');
            }
        },
        error: function () {
            $('#formPreviewContent').html('<p class="text-danger">Error loading form data.</p>');
        }
    });
}

function GetDownloadMasterForm(empId) {


    // ✅ AJAX Call to Fetch Data for BGV Form
    $.ajax({
        url: '/PostJoining/GetMasterSheetData',
        type: 'GET',
        data: { EmpId: empId },
        success: function (response) {
            if (response.success) {
                console.log(response.data);
                DownloadMasterForm(response.data);
            } else {
                $('#formPreviewContent').html('<p class="text-danger">No data found for the selected employee.</p>');
            }
        },
        error: function () {
            $('#formPreviewContent').html('<p class="text-danger">Error loading form data.</p>');
        }
    });
}


//download master data
function DownloadMasterForm(data) {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();

    // Load autoTable plugin
    doc.autoTableSetDefaults({
        styles: { font: "helvetica", fontSize: 10, cellPadding: 3 },
        headStyles: { fillColor: [50, 50, 50], textColor: [255, 255, 255] }
    });

    // Utility function to format dates
    const formatDate = (dateStr) => {
        if (!dateStr) return "-";
        const date = new Date(dateStr);
        return !isNaN(date.getTime()) ? date.toLocaleDateString() : "-";
    };

    // Set PDF Title
    doc.setFont("helvetica", "bold");
    doc.setFontSize(16);
    doc.text("Employee Master Sheet", 20, 20);

    let y = 30;

    const addSectionTitle = (title) => {
        doc.setFontSize(12);
        doc.setFont("helvetica", "bold");
        doc.text(title, 20, y);
        y += 6;
    };

    const addTable = (rows, headers = null) => {
        doc.autoTable({
            startY: y,
            head: headers ? [headers] : undefined,
            body: rows,
            theme: "striped",
            styles: { cellWidth: "wrap" }
        });
        y = doc.lastAutoTable.finalY + 10;
    };

    // ✅ Employee Information
    addSectionTitle("Employee Information");
    addTable([
        ["First Name", data.masterEmployee.firstName, "Middle Name", data.masterEmployee.middleName],
        ["Last Name", data.masterEmployee.lastName, "Gender", data.masterEmployee.gender],
        ["Date of Birth", formatDate(data.masterEmployee.dateOfBirth), "Age", data.masterEmployee.age],
        ["Marital Status", data.masterEmployee.maritalStatus, "Date of Joining", formatDate(data.masterEmployee.dateOfJoining)],
        ["Band Level", data.masterEmployee.bandLevel, "Designation", data.masterEmployee.designation],
        ["Location", data.masterEmployee.location, "Department", data.masterEmployee.department],
        ["Function/Project", data.masterEmployee.functionProject, "Probation Status", data.masterEmployee.probationConfirmationStatus],
        ["Probation Confirmation Date", formatDate(data.masterEmployee.probationConfirmationDate), "Tenure in RAPS", data.masterEmployee.tenureInRAPS],
        ["Years in RAPS", data.masterEmployee.yearsInRAPS, "Total Work Experience", data.masterEmployee.totalWorkExperience]
    ]);

    // ✅ Contact Information
    addSectionTitle("Contact Information");
    addTable([
        ["Official Contact No", data.masterContactDetails.officialContactNo, "Personal Contact No", data.masterContactDetails.personalContactNo],
        ["Official Email ID", data.masterContactDetails.officialEmailId, "Personal Email ID", data.masterContactDetails.personalEmailId],
        ["Emergency Contact Name", data.masterContactDetails.emergencyContactName, "Emergency Contact No", data.masterContactDetails.emergencyContactNumber],
        ["Emergency Relationship", data.masterContactDetails.emergencyRelationship, "", ""]
    ]);

    // ✅ Address Details
    addSectionTitle("Address Details");
    addTable([
        ["Permanent Address", data.masterAddress.permanentAddress],
        ["Postal Address", data.masterAddress.postalAddress]
    ]);

    // ✅ Education Details
    addSectionTitle("Education Details");
    addTable([
        ["Xth Institution", data.masterEducation.xthInstitution, "Xth Passing Year", data.masterEducation.xthPassingYear],
        ["XIIth Institution", data.masterEducation.xiIthInstitution, "XIIth Passing Year", data.masterEducation.xiIthPassingYear],
        ["Bachelor Institution", data.masterEducation.bachelorInstitution, "Bachelor Degree", data.masterEducation.bachelorDegree],
        ["Bachelor Completion Year", data.masterEducation.bachelorCompletionYear, "Master Institution", data.masterEducation.masterInstitution],
        ["Master Degree", data.masterEducation.masterDegree, "Master Completion Year", data.masterEducation.masterCompletionYear],
        ["Post Doctorate Institution", data.masterEducation.postDoctorateInstitution, "Post Doctorate Degree", data.masterEducation.postDoctorateDegree],
        ["Post Doctorate Completion Year", data.masterEducation.postDoctorateCompletionYear, "Professional Course Institution", data.masterEducation.professionalCoursesInstitution],
        ["Professional Course Degree", data.masterEducation.professionalCoursesDegree, "Professional Course Completion Year", data.masterEducation.professionalCoursesCompletionYear]
    ]);

    // ✅ Work Experience
    addSectionTitle("Work Experience");

    const experiences = data.masterWorkExperience || [];

    if (experiences.length > 0) {
        const experienceRows = experiences.map((exp, index) => ([
            index + 1,
            exp.organisationName || "N/A",
            exp.designation || "N/A",
            exp.yearsOfExperience || "N/A",
            formatDate(exp.fromDate),
            formatDate(exp.toDate),
            exp.reasonForLeaving || "N/A"
        ]));

        doc.autoTable({
            startY: y,
            head: [[
                "SNo", "Organization Name", "Designation", "Years", "From Date", "To Date", "Reason for Leaving"
            ]],
            body: experienceRows,
            theme: "striped",
            styles: { cellWidth: 'wrap' },
            columnStyles: {
                0: { cellWidth: 20 },   // S.No
                1: { cellWidth: 35 },   // Org Name
                2: { cellWidth: 30 },   // Designation
                3: { cellWidth: 20 },   // Years
                4: { cellWidth: 25 },   // From Date
                5: { cellWidth: 25 },   // To Date
                6: { cellWidth: 50 }    // Reason for Leaving
            },
            useCss: true
        });

        y = doc.lastAutoTable.finalY + 10;
    } else {
        addTable(["Message"], [["No Work Experience Found"]]);
    }


    // ✅ Bank Details
    addSectionTitle("Bank Details");
    addTable([
        ["Bank Name", data.masterBankDetails.bankName, "Account Number", data.masterBankDetails.accountNumber],
        ["IFSC Code", data.masterBankDetails.ifscCode, "", ""]
    ]);

    // ✅ Emergency Contact
    addSectionTitle("Emergency Contact");
    addTable([
        ["Emergency Contact Name", data.masterEmergencyContactViewModel.emergencyName, "Emergency Contact Number", data.masterEmergencyContactViewModel.emergencyContactNumber],
        ["Relationship", data.masterEmergencyContactViewModel.relationship, "", ""]
    ]);

    // ✅ Reporting Details
    addSectionTitle("Reporting Details");
    addTable([
        ["Direct Reporting", data.masterReportingDetails.directReporting, "Dotted Reporting", data.masterReportingDetails.dottedReporting],
        ["Skip Reporting", data.masterReportingDetails.skipReporting, "", ""]
    ]);

    // ✅ Family & Dependent Details
    addSectionTitle("Family & Dependent Details");
    addTable([
        ["Family Member Name", data.masterFamilyDetails.name, "Relation with Employee", data.masterFamilyDetails.relationWithEmployee || "N/A"],
        ["Date of Birth", formatDate(data.masterFamilyDetails.dateOfBirth), "Dependent Name", data.masterDependentDetails.dependentName],
        ["Dependent Relationship", data.masterDependentDetails.relationship, "Dependent DOB", formatDate(data.masterDependentDetails.dateOfBirth)]
    ]);

    // ✅ Save as PDF
    doc.save(`Employee_MasterSheet_${data.masterEmployee.firstName}_${data.masterEmployee.lastName}.pdf`);
}


// ✅ Helper Function to Format Date
function formatDate(dateStr) {
    if (!dateStr) return "N/A";
    let date = new Date(dateStr);
    return date.toISOString().split("T")[0];
}


//preview bgv
function renderEmployeeData(data) {

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

    if (data.previousEmployments && data.previousEmployments.length > 0) {
        data.previousEmployments.forEach(emp => {
            formContent += `
                <tr>
                    <td>${emp.natureOfEmployment || '-'}</td>
                    <td>${emp.currentDesignation || '-'}</td>
                    <td>${emp.department || '-'}</td>
                    <td>${emp.officialTitle || '-'}</td>
                    <td>${emp.payrollCompanyName || '-'}</td>
                    <td>${emp.organizationAddress || '-'}</td>
            <td>${emp.fromDate && emp.fromDate !== '0001-01-01' ? formatDate(emp.fromDate) : '-'}</td>
            <td>${emp.toDate && emp.toDate !== '0001-01-01' ? formatDate(emp.toDate) : '-'}</td>
                    <td>${emp.employeeCode || '-'}</td>
                    <td>${emp.ctcPerAnnum || '-'}</td>
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
        formContent += `
                <tr><td colspan="19" class="text-center">No previous employment details found.</td></tr>`;
    }


    formContent += `
                </tbody>
            </table>
        </div>`;

    // ✅ Render the final content in the preview modal
    $('#formPreviewContent').html(formContent);

    $('#downloadBtn').off('click').on('click', function () {
        downloadEmployeeForm(data);
    });

}

//bgv download
function downloadEmployeeForm(data) {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();

    // Set PDF Title
    doc.setFont("helvetica", "bold");
    doc.setFontSize(16);
    doc.text("Employee Background Verification Form", 20, 20);
    let y = 30;

    // Helper: Add Section Title
    const addSectionTitle = (title) => {
        doc.setFontSize(12);
        doc.setFont("helvetica", "bold");
        doc.text(title, 20, y);
        y += 8;
    };

    // Helper: Add Tables
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

    // Personal Info
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

    // Education Info
    addSectionTitle("Education Information");
    addTable(
        ["Field", "Value", "Field", "Value"],
        [
            ["Course Name", data.courseName, "Program Type", data.programType],
            ["College Name", data.collegeName, "University/Board Name", data.universityBoardName],
            ["Passing Year", data.passingYear, "Proof Type", data.proofType]
        ]
    );

    // Reference Info
    addSectionTitle("Reference Information");
    addTable(
        ["Field", "Value", "Field", "Value"],
        [
            ["Reference Name", data.referenceName, "Reference Contact", data.referenceContact],
            ["Consent Given", data.consentGiven ? "Yes" : "No", "", ""]
        ]
    );

    // Previous Employment Info
    addSectionTitle("Previous Employment Details");

    const previousEmployments = data.previousEmployments || [];

    if (previousEmployments.length > 0) {
        let employmentRows = previousEmployments.map((emp, index) => [
            [`Employment #${index + 1}`, "", "", ""],
            ["Nature of Employment", emp.natureOfEmployment || "N/A", "Designation", emp.currentDesignation || "N/A"],
            ["Department", emp.department || "N/A", "Official Title", emp.officialTitle || "N/A"],
            ["Company Name", emp.payrollCompanyName || "N/A", "Organization Address", emp.organizationAddress || "N/A"],
            ["From Date", (emp.fromDate && emp.fromDate !== "0001-01-01") ? formatDate(emp.fromDate) : "N/A", "To Date", (emp.toDate && emp.toDate !== "0001-01-01") ? formatDate(emp.toDate) : "N/A"],
            ["Employee Code", emp.employeeCode || "N/A", "CTC Per Annum", emp.ctcPerAnnum || "N/A"],
            ["Key Responsibility", emp.keyResponsibility || "N/A", "Employment Tenure", emp.employmentTenure || "N/A"],
            ["Reason for Leaving", emp.reasonForLeaving || "N/A", "", ""],
            ["Reporting Manager", emp.reportingManagerName || "N/A", "Manager Designation", emp.reportingManagerDesignation || "N/A"],
            ["Still in Company", emp.isReportingManagerStillInCompany === "Yes" ? "Yes" : "No", "Best Time to Reach", emp.bestTimeToReach || "N/A"],
            ["Company Landline", emp.companyLandline || "N/A", "Personal Mobile No.", emp.personalMobileNo || "N/A"],
            ["", "", "", ""] // Spacer row
        ]).flat();

        addTable(
            ["Field", "Value", "Field", "Value"],
            employmentRows
        );
    } else {
        addTable(["Message", ""], [["No previous employment details found.", ""]]);
    }

    // Save
    doc.save(`Employee_BGV_${data.firstName || "N/A"}_${data.lastName || "N/A"}.pdf`);
}



//master preview
function loadMasterForm(empId) {

    // ✅ AJAX Call to Fetch Data for BGV Form
    $.ajax({
        url: '/PostJoining/GetMasterSheetData',
        type: 'GET',
        data: { EmpId: empId },
        success: function (response) {
            if (response.success) {

                renderMasterData(response.data);
            } else {
                $('#formPreviewContent').html('<p class="text-danger">No data found for the selected employee.</p>');
            }
        },
        error: function () {
            $('#formPreviewContent').html('<p class="text-danger">Error loading form data.</p>');
        }
    });
}

//render master
function renderMasterData(data) {
    if (!data || !data.masterEmployee) {
        $('#formPreviewContent').html('<p class="text-danger">No data available for this employee.</p>');
        return;
    }

    let masterEmployee = data.masterEmployee;
    let masterAddress = data.masterAddress || {};
    let masterEducation = data.masterEducation || {};
    let masterBankDetails = data.masterBankDetails || {};
    let masterContactDetails = data.masterContactDetails || {};
    let masterDependentDetails = data.masterDependentDetails || {};
    let masterFamilyDetails = data.masterFamilyDetails || {};
    let masterReportingDetails = data.masterReportingDetails || {};
    let masterEmergencyContactViewModel = data.masterEmergencyContactViewModel || {};
    let masterWorkExperience = Array.isArray(data.masterWorkExperience) ? data.masterWorkExperience : [];


    let formContent = `
        <h5>Employee Information</h5>
        <table class="table table-bordered">
            <tr><th>First Name</th><td>${masterEmployee.firstName || '-'}</td></tr>
            <tr><th>Middle Name</th><td>${masterEmployee.middleName || '-'}</td></tr>
            <tr><th>Last Name</th><td>${masterEmployee.lastName || '-'}</td></tr>
            <tr><th>Gender</th><td>${masterEmployee.gender || '-'}</td></tr>
            <tr><th>Date of Birth</th><td>${formatDate(masterEmployee.dateOfBirth)}</td></tr>
            <tr><th>Age</th><td>${masterEmployee.age || '-'}</td></tr>
            <tr><th>Marital Status</th><td>${masterEmployee.maritalStatus || '-'}</td></tr>
            <tr><th>Date of Joining</th><td>${formatDate(masterEmployee.dateOfJoining)}</td></tr>
            <tr><th>Band Level</th><td>${masterEmployee.bandLevel || '-'}</td></tr>
            <tr><th>Designation</th><td>${masterEmployee.designation || '-'}</td></tr>
            <tr><th>Location</th><td>${masterEmployee.location || '-'}</td></tr>
            <tr><th>Department</th><td>${masterEmployee.department || '-'}</td></tr>
            <tr><th>Function/Project</th><td>${masterEmployee.functionProject || '-'}</td></tr>
            <tr><th>Probation Status</th><td>${masterEmployee.probationConfirmationStatus || '-'}</td></tr>
            <tr><th>Probation Confirmation Date</th><td>${formatDate(masterEmployee.probationConfirmationDate)}</td></tr>
            <tr><th>Tenure in RAPS</th><td>${masterEmployee.tenureInRAPS || '-'}</td></tr>
            <tr><th>Years in RAPS</th><td>${masterEmployee.yearsInRAPS || '-'}</td></tr>
            <tr><th>Total Work Experience</th><td>${masterEmployee.totalWorkExperience || '-'}</td></tr>
            <tr><th>UAN Number</th><td>${masterEmployee.uanNumber || '-'}</td></tr>
            <tr><th>Aadhar Number</th><td>${masterEmployee.aadharNumber || '-'}</td></tr>
            <tr><th>PAN Card Number</th><td>${masterEmployee.panCardNumber || '-'}</td></tr>
            <tr><th>CTC Per Annum on DOJ</th><td>${masterEmployee.ctcPerAnnumOnDOJ || '-'}</td></tr>
            <tr><th>Filing Person</th><td>${masterEmployee.filingPerson || '-'}</td></tr>
            <tr><th>Filing Recheck</th><td>${masterEmployee.filingRecheck || '-'}</td></tr>
            <tr><th>Remarks</th><td>${masterEmployee.remark || '-'}</td></tr>
        </table>

        <h5 class="mt-4">Bank Details</h5>
        <table class="table table-bordered">
            <tr><th>Bank Name</th><td>${masterBankDetails.bankName || '-'}</td></tr>
            <tr><th>Account Number</th><td>${masterBankDetails.accountNumber || '-'}</td></tr>
            <tr><th>IFSC Code</th><td>${masterBankDetails.ifscCode || '-'}</td></tr>
        </table>

        <h5 class="mt-4">Address</h5>
        <table class="table table-bordered">
            <tr><th>Permanent Address</th><td>${masterAddress.permanentAddress || '-'}</td></tr>
            <tr><th>Postal Address</th><td>${masterAddress.postalAddress || '-'}</td></tr>
        </table>

        <h5 class="mt-4">Contact Details</h5>
        <table class="table table-bordered">
            <tr><th>Personal Contact</th><td>${masterContactDetails.personalContactNo || '-'}</td></tr>
            <tr><th>Personal Email</th><td>${masterContactDetails.personalEmailId || '-'}</td></tr>
            <tr><th>Official Contact</th><td>${masterContactDetails.officialContactNo || '-'}</td></tr>
            <tr><th>Official Email</th><td>${masterContactDetails.officialEmailId || '-'}</td></tr>
            <tr><th>Emergency Name</th><td>${masterContactDetails.emergencyContactName || '-'}</td></tr>
            <tr><th>Emergency Contact Number</th><td>${masterContactDetails.emergencyContactNumber || '-'}</td></tr>
            <tr><th>Relation</th><td>${masterContactDetails.emergencyRelationship || '-'}</td></tr>
        </table>

        <h5 class="mt-4">Education</h5>
        <table class="table table-bordered">
            <tr><th>Xth Institution</th><td>${masterEducation.xthInstitution || '-'}</td></tr>
            <tr><th>Xth Passing Year</th><td>${masterEducation.xthPassingYear || '-'}</td></tr>
            <tr><th>XIIth Institution</th><td>${masterEducation.xiIthInstitution || '-'}</td></tr>
            <tr><th>XIIth Passing Year</th><td>${masterEducation.xiIthPassingYear || '-'}</td></tr>
    
            <tr><th>Bachelor Institution</th><td>${masterEducation.bachelorInstitution || '-'}</td></tr>
            <tr><th>Bachelor Degree</th><td>${masterEducation.bachelorDegree || '-'}</td></tr>
            <tr><th>Bachelor Completion Year</th><td>${masterEducation.bachelorCompletionYear || '-'}</td></tr>

            <tr><th>Master Institution</th><td>${masterEducation.masterInstitution || '-'}</td></tr>
            <tr><th>Master Degree</th><td>${masterEducation.masterDegree || '-'}</td></tr>
            <tr><th>Master Completion Year</th><td>${masterEducation.masterCompletionYear || '-'}</td></tr>

            <tr><th>Post Doctorate Institution</th><td>${masterEducation.postDoctorateInstitution || '-'}</td></tr>
            <tr><th>Post Doctorate Degree</th><td>${masterEducation.postDoctorateDegree || '-'}</td></tr>
            <tr><th>Post Doctorate Completion Year</th><td>${masterEducation.postDoctorateCompletionYear || '-'}</td></tr>

            <tr><th>Professional Courses Institution</th><td>${masterEducation.professionalCoursesInstitution || '-'}</td></tr>
            <tr><th>Professional Courses Degree</th><td>${masterEducation.professionalCoursesDegree || '-'}</td></tr>
            <tr><th>Professional Courses Completion Year</th><td>${masterEducation.professionalCoursesCompletionYear || '-'}</td></tr>
        </table>


        <h5 class="mt-4">Emergency Contact</h5>
        <table class="table table-bordered">
            <tr><th>Emergency Name</th><td>${masterEmergencyContactViewModel.emergencyName || '-'}</td></tr>
            <tr><th>Emergency Contact</th><td>${masterEmergencyContactViewModel.emergencyContactNumber || '-'}</td></tr>
            <tr><th>Relationship</th><td>${masterEmergencyContactViewModel.relationship || '-'}</td></tr>
        </table>

        <h5 class="mt-4">Previous Employments</h5>
        <div class="table-responsive" style="max-height: 300px; overflow-x: auto; white-space: nowrap;">
            <table class="table table-bordered">
                <thead class="table-light">
                    <tr>
                        <th>Organisation Name</th>
                        <th>Designation</th>
                        <th>Reason for Leaving</th>
                        <th>Work Start Date</th>
                        <th>Work End Date</th>
                        <th>Years of Experience</th>
                    </tr>
                </thead>
                <tbody>`;

    if (masterWorkExperience.length > 0) {
        masterWorkExperience.forEach(emp => {
            formContent += `
                <tr>
                    <td>${emp.organisationName || '-'}</td>
                    <td>${emp.designation || '-'}</td>
                    <td>${emp.reasonForLeaving || '-'}</td>
                    <td>${formatDate(emp.fromDate)}</td>
                    <td>${emp.toDate ? formatDate(emp.toDate) : '-'}</td>
                    <td>${emp.yearsOfExperience || '-'}</td>
                </tr>`;
        });
    } else {
        formContent += `<tr><td colspan="6" class="text-center">No previous employment details found.</td></tr>`;
    }

    formContent += `
                </tbody>
            </table>
        </div>`;

    // Render final HTML
    $('#formPreviewContent').html(formContent);

    $('#downloadBtn').off('click').on('click', function () {
        DownloadMasterForm(data);
    });
}








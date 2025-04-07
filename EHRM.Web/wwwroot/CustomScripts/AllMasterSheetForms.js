var empIdFromSession = parseInt('@empIdFromSession');

$(document).ready(function () {

    getMasterSheetDataDetail();

});

// Function to fetch acknowledgment data and populate the DataTable

function getMasterSheetDataDetail() {
    $.ajax({
        url: '/PostJoining/GetMasterSheetFormDetails',
        method: 'GET',
        dataType: 'json',
        success: function (json) {
            console.log("Full API Response:", json); // Debugging API response

            // Check if response contains valid data
            if (
                !json ||
                !json.success ||
                !json.data ||
                !Array.isArray(json.data.bgvForms) ||
                json.data.bgvForms.length === 0
            ) {
                console.warn("Invalid or missing BgvForms data.");
                return;
            }



            console.log("Processed Data for Table:", json.data.bgvForms);

            $('#AllMasterSheetData').DataTable({
                destroy: true,
                processing: true, // Show loading indicator
                data: json.data.bgvForms, // ✅ Updated to use actual array

                columns: [
                    { data: 'empId', title: 'Employee Id' },
                    { data: 'employeeName', title: 'Employee Name' },
                    { data: 'emailAddress', title: 'Email Address' },
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
    $('#AllMasterSheetData tbody').off("click").on('click', '.view-btn', function () {
        const id = $(this).data('id');
        //console.log("Button clicked for ID:", empId);
        masterformModalDetail(id);
    });
}


function masterformModalDetail(EmpId) {
    if (!EmpId) {
        console.error("Error: Employee ID is missing.");
        alert("Invalid Employee ID.");
        return;
    }

    $.ajax({
        url: `/PostJoining/GetMasterSheetDataByEmpId/${EmpId}`,
        method: 'GET',
        dataType: 'json',
        success: function (response) {
            if (response.success) {
                console.log(response.data);
                renderMasterData(response.data);
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

        <h5 class="mt-4">Work Experience</h5>
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

    if (data.masterWorkExperience && data.masterWorkExperience.length > 0) {
        data.masterWorkExperience.forEach(emp => {
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

    // ✅ Render the final content in the preview modal
    $('#formPreviewContentModal').html(formContent);

    // ✅ Show the modal
    $('#previewModal').modal('show');

    $('#downloadBtn').off('click').on('click', function () {
        DownloadMasterForm(data);
    });
}

// ✅ Function to Format Date Properly
function formatDate(dateString) {
    if (!dateString) return '-';
    const date = new Date(dateString);
    return date.toLocaleDateString('en-GB'); // Format: DD/MM/YYYY
}

function DownloadMasterForm(data) {
    const { jsPDF } = window.jspdf;
    const doc = new jsPDF();

    // Load autoTable plugin
    doc.autoTableSetDefaults({
        styles: { font: "helvetica", fontSize: 10, cellPadding: 3 },
        headStyles: { fillColor: [50, 50, 50], textColor: [255, 255, 255] }
    });

    // Set PDF Title
    doc.setFont("helvetica", "bold");
    doc.setFontSize(16);
    doc.text("Employee Master Sheet", 20, 20);

    let y = 30; // Start position

    const addSectionTitle = (title) => {
        doc.setFontSize(12);
        doc.setFont("helvetica", "bold");
        doc.text(title, 20, y);
        y += 6;
    };

    const addTable = (rows) => {
        doc.autoTable({
            startY: y,
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
    addSectionTitle("Previous Work Experience");

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


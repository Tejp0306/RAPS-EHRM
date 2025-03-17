$(document).ready(function () {
    // Initialize the calendar
    initializeCalendar();
    getLeaveBalance();
});

function initializeCalendar() {
    // Find the calendar element by ID
    var calendarEl = document.getElementById('calendar');

    // Get EmpId from cookies
    let empId = getCookie("EmpId");

    if (!empId) {
        console.error("EmpId not found in cookies. Ensure the cookie is set correctly.");
        return; // Exit if empId is not available
    }

    if (calendarEl) { // Check if the calendar element exists
        var calendar = new FullCalendar.Calendar(calendarEl, {
            contentHeight: 600,

            initialView: 'dayGridMonth', // Default view
            headerToolbar: {
                left: 'prev,next today',
                center: 'title',
                right: 'dayGridMonth,timeGridWeek,timeGridDay'
            },
            events: function (fetchInfo, successCallback, failureCallback) {

  
                const url = `/Calendar/GetCombinedEvents?empId=${empId}`;

                const request = new Request(url, {
                    method: 'GET',
                    headers: {
                        'Content-Type': 'application/json'
                    }
                });

                // Make an AJAX request to fetch events
                fetch(request)
                    .then(response => {
                        if (!response.ok) {
                            throw new Error("Failed to fetch events: " + response.statusText);
                        }
                        return response.json();
                    })
                    .then(data => {
                        successCallback(data); // Pass both punch and holiday data to FullCalendar
                    })
                    .catch(error => {
                        console.error("Error fetching events:", error); // Debug: Log fetch errors
                        failureCallback(error); // Notify FullCalendar of failure
                    });
            },
            editable: true, // Allow drag-and-drop
            selectable: true, // Allow selecting dates

            eventContent: function (arg) {
                let eventObj = arg.event;
                let punchInTime = eventObj.extendedProps.punchInTime || 'N/A';
                let punchOutTime = eventObj.extendedProps.punchOutTime || 'N/A';
                let totalHours = eventObj.extendedProps.totalHours || 'N/A';

                // Create the HTML content for Punch Details
                let punchInHtml = `<b>Punch In:</b> ${punchInTime}`;
                let punchOutHtml = `<b>Punch Out:</b> ${punchOutTime}`;
                let totalHoursHtml = `<b>Total Hours:</b> ${totalHours}`;

                // Styles based on the Total Hours conditions
                let totalHoursStyle = '';

                if (totalHours === 'N/A') {
                    totalHoursStyle = 'background-color: orange; color: white; padding: 2px 5px;';
                } else if (parseFloat(totalHours) < 7.5) {
                    totalHoursStyle = 'background-color: red; color: white; padding: 2px 5px;';
                } else if (parseFloat(totalHours) >= 9.5) {
                    totalHoursStyle = 'background-color: green; color: white; padding: 2px 5px;';
                }

                // Customize event display based on type
                if (eventObj.title === "Punch Details") {
                    return {
                        html: `
                            <div>${punchInHtml}</div>
                            <div>${punchOutHtml}</div>
                            <div style="${totalHoursStyle}">${totalHoursHtml}</div>
                        `
                    };
                } else {
                    // If it's a holiday event, show the holiday name
                    return {
                        html: `<b>Holiday:</b> ${eventObj.title}<br>`
                    };
                }
            },

            // When a date is clicked, show the modal
            //dateClick: function (info) {
            //    $('#eventModalLabel').text("Create New Event");
            //    $('#eventDate').val(info.dateStr);
            //    $('#eventTitle').val("");
            //    $('#eventModal').modal('show');
            //},

            // When an event is clicked, show its details in the modal
            eventClick: function (info) {
                $('#eventModalLabel').text("Event Details");
                $('#eventDate').val(info.event.startStr);
                $('#eventTitle').val(info.event.title);
                $('#eventModal').modal('show');
            }
        });

        // Render the calendar
        calendar.render();
    } else {
        console.error("Calendar element not found. Ensure an element with ID 'calendar' exists.");
    }

    // Helper function to get a cookie value by name
    function getCookie(name) {
        let cookieArr = document.cookie.split(';');
        for (let i = 0; i < cookieArr.length; i++) {
            let cookie = cookieArr[i].trim();
            if (cookie.startsWith(name + '=')) {
                return cookie.substring(name.length + 1);
            }
        }
        return null;
    }
}


    getLeaveBalance();

    $.ajax({
        url: "/Leave/GetLeaveRecord", // Update with your actual controller name
        type: "GET",
        dataType: "json",
        success: function (data) {
            let tbody = $("#leaveRecords");
            tbody.empty(); // Clear existing rows

            if (data.length === 0) {
                tbody.append("<tr><td colspan='5' class='text-center'>No leave records found</td></tr>");
            } else {
                data.forEach((leave,index) => {
                    let row = `<tr>
                            <td>${index+1}</td>
                            <td>${leave.employeeName}</td>
                            <td>${leave.leaveType}</td>
                            <td>${leave.leaveFrom}</td>
                            <td>${leave.tillDate}</td>
                            <td>${leave.leaveStatus}</td>
                        </tr>`;
                    tbody.append(row);
                });
            }
        },
        error: function () {
            alert("Failed to load leave records.");
        }
    });



function getLeaveBalance() {
    $.ajax({
        url: "/Leave/GetLeaveBalance",
        method: "GET",
        success: function (data) {
            if (data) {
                $("#earnedLeave").text(data.earnedLeave);
                $("#sickLeave").text(data.sickLeave);
                $("#casualLeave").text(data.casualLeave);
                $("#totalLeave").text(data.totalLeave);
            } else {
                $("#leaveBalanceContainer").html("<p>No leave balance data found.</p>");
            }
        },
        error: function () {
            $("#leaveBalanceContainer").html("<p>Error fetching leave balance data.</p>");
        }
    });
    document.addEventListener('DOMContentLoaded', function () {
        var calendarEl = document.getElementById('calendar');
        var calendar = new FullCalendar.Calendar(calendarEl, {
            initialView: 'dayGridMonth',

            dateClick: function (info) {
                // Show modal and update the content dynamically
                document.getElementById('modalDate').innerText = info.dateStr;
                document.getElementById('modalEventTitle').innerText = "No event"; // Since it's just a date click

                var modal = new bootstrap.Modal(document.getElementById('eventModal'));
                modal.show();
            },

            eventClick: function (info) {
                // Show modal and update the content dynamically
                document.getElementById('modalDate').innerText = info.event.start.toISOString().split('T')[0];
                document.getElementById('modalEventTitle').innerText = info.event.title;

                var modal = new bootstrap.Modal(document.getElementById('eventModal'));
                modal.show();
            }
        });

        calendar.render();
    });

}

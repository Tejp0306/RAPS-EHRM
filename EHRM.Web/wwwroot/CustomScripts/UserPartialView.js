// Ensure FullCalendar is loaded after the DOM is ready
$(document).ready(function () {
    // Find the calendar element by ID
    var calendarEl = document.getElementById('calendar');

    // Get EmpId from cookies
    let empId = getCookie("EmpId");
    //console.log("EmpId:", empId); // Debug: Check if empId is retrieved correctly

    if (!empId) {
        console.error("EmpId not found in cookies. Ensure the cookie is set correctly.");
        return; // Exit if empId is not available
    }

    if (calendarEl) { // Check if the calendar element exists
        var calendar = new FullCalendar.Calendar(calendarEl, {
            initialView: 'dayGridMonth', // Default view

            headerToolbar: {
                left: 'prev,next today',
                center: 'title',
                right: 'dayGridMonth,timeGridWeek,timeGridDay'
            },
            

            events: function (fetchInfo, successCallback, failureCallback) {
                // Debug: Log the fetch request details
                //console.log("Fetching events for empId:", empId);

                const url = `/Calendar/GetEvents?empId=${empId}`;
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
                        //console.log("Fetched events:", data); // Debug: Log fetched data
                        successCallback(data); // Pass data to FullCalendar
                    })
                    .catch(error => {
                        console.error("Error fetching events:", error); // Debug: Log fetch errors
                        failureCallback(error); // Notify FullCalendar of failure
                    });
            },
            editable: true, // Allow drag-and-drop
            selectable: true, // Allow selecting dates
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

    getLeaveBalance();

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


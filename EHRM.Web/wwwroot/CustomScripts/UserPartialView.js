// Ensure FullCalendar is loaded after the DOM is ready
$(document).ready(function () {
    debugger;

    // Find the calendar element by ID
    var calendarEl = document.getElementById('calendar');

    // Get EmpId from cookies
    let empId = getCookie("EmpId");
    console.log("EmpId:", empId); // Debug: Check if empId is retrieved correctly

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
            innerHeight: '70%',
            innerWidth: '30%',

            events: function (fetchInfo, successCallback, failureCallback) {
                // Debug: Log the fetch request details
                console.log("Fetching events for empId:", empId);

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
                        console.log("Fetched events:", data); // Debug: Log fetched data
                        successCallback(data); // Pass data to FullCalendar
                    })
                    .catch(error => {
                        console.error("Error fetching events:", error); // Debug: Log fetch errors
                        failureCallback(error); // Notify FullCalendar of failure
                    });
            },
            editable: true, // Allow drag-and-drop
            selectable: true, // Allow selecting dates
            dateClick: function (info) {
                alert('Clicked on: ' + info.dateStr);
            },
            eventClick: function (info) {
                alert('Event: ' + info.event.title);
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
});

document.addEventListener('DOMContentLoaded', function () {
    const basketURL = "https://localhost:7157/home/getbasketitems";
    const flightURL = "https://localhost:7046/api/flights/";
    const airportURL = "https://localhost:7046/api/airports/";  // Adjust to correct airport API endpoint
    const planeURL = "https://localhost:7046/api/planes/";  // Adjust to correct plane API endpoint

    function fetchBasketItems() {
        fetch(basketURL)
            .then(response => response.json())
            .then(basketItems => {
                basketItems.forEach(item => {
                    // Fetch flight details using flightId
                    fetchFlightDetails(item.flightId);
                });
            });
    }

    function fetchFlightDetails(flightId) {
        fetch(`${flightURL}${flightId}`)
            .then(response => response.json())
            .then(data => {
                const flight = data.data;
                // Fetch related data (plane and airport details)
                fetchRelatedDetails(flight);
            })
            .catch(error => {
                console.error('Error fetching flight details:', error);
            });
    }

    function fetchRelatedDetails(flight) {
        // Fetch departure and arrival airports, and plane details
        const departureAirportPromise = fetch(`${airportURL}${flight.departureAirportId}`).then(res => res.json());
        const arrivalAirportPromise = fetch(`${airportURL}${flight.arrivalAirportId}`).then(res => res.json());
        const planePromise = fetch(`${planeURL}${flight.planeId}`).then(res => res.json());

        Promise.all([departureAirportPromise, arrivalAirportPromise, planePromise])
            .then(([departureAirport, arrivalAirport, plane]) => {
                displayFlight(flight, departureAirport.data, arrivalAirport.data, plane.data);
            })
            .catch(error => {
                console.error('Error fetching related details:', error);
            });
    }

    function displayFlight(flight, departureAirport, arrivalAirport, plane) {
        const rowDiv = document.getElementById("rowDiv");
        const eachRow = document.createElement('div');

        eachRow.innerHTML = `
            <main style="margin-top: 20px" class="ticket-system">
                <div class="receipts-wrapper">
                    <div class="receipts">
                        <div class="receipt">
                            <div style="display: flex; justify-content: space-between">
                                <div>
                                    <i class="fa-solid fa-plane-departure"></i>
                                    <a style="font-family: cursive; font-size: large; margin-left: 3px">
                                        ${plane.airlineName}
                                    </a>
                                </div>
                                <div>
                                    <a>${new Date(flight.departureTime).toLocaleDateString()}</a>
                                </div>
                            </div>
                            <div class="route">
                                <h2>${departureAirport.city}</h2>
                                <svg class="plane-icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 510 510">
                                    <path fill="#3f32e5" d="M497.25 357v-51l-204-127.5V38.25C293.25 17.85 275.4 0 255 0s-38.25 17.85-38.25 38.25V178.5L12.75 306v51l204-63.75V433.5l-51 38.25V510L255 484.5l89.25 25.5v-38.25l-51-38.25V293.25l204 63.75z" />
                                </svg>
                                <h2>${arrivalAirport.city}</h2>
                            </div>
                            <div class="details">
                                <div class="item">
                                    <span>Departure</span>
                                    <h3 style="font-family: Arial; font-size: larger">${new Date(flight.departureTime).toLocaleTimeString()}</h3>
                                </div>
                                <div class="item">
                                    <span>Flight No.</span>
                                    <h3 style="font-family: Arial; font-size: larger">${flight.flightNumber}</h3>
                                </div>
                                <div class="item">
                                    <span>Arrival</span>
                                    <h3 style="font-family: Arial; font-size: larger">${new Date(flight.arrivalTime).toLocaleTimeString()}</h3>
                                </div>
                                <div class="item">
                                    <span>Plane</span>
                                    <h3 style="font-family: Arial; font-size: large">${plane.name}</h3>
                                </div>
                                <div class="item">
                                    <span>Price (Eco)</span>
                                    <h3 style="font-family: Arial; font-size: larger">$${flight.seatPrice}</h3>
                                </div>
                                <div class="item">
                                    <span>Total Seats</span>
                                    <h3 style="font-family: Arial; font-size: larger">${flight.seats.length}</h3>
                                </div>
                            </div>
                        </div>
                        <div class="receipt qr-code">
                            <a class="btn btn-outline-primary">Book Now<i style="margin-left: 3px" class="fas fa-arrow-right"></i></a>
                        </div>
                    </div>
                </div>
            </main>
        `;

        rowDiv.appendChild(eachRow);
    }

    fetchBasketItems();
});

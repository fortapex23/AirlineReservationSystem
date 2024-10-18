document.addEventListener('DOMContentLoaded', function () {
	console.log('JavaScript loaded successfully');

	const airportCities = {
		NewYork_City: 0,
		Los_Angeles: 1,
		London: 2,
		Paris: 3,
		Tokyo: 4,
		Dubai: 5,
		Sydney: 6,
		Singapore: 7,
		Beijing: 8,
		Hong_Kong: 9,
		Madrid: 10,
		Rome: 11,
		Moscow: 12,
		Istanbul: 13,
		Berlin: 14,
		Toronto: 15,
		São_Paulo: 16,
		Buenos_Aires: 17,
		Mexico_City: 18,
		Bangkok: 19,
		Cairo: 20,
		Delhi: 21,
		Mumbai: 22,
		Johannesburg: 23,
		Zurich: 24,
		Amsterdam: 25,
		Munich: 26,
		Chicago: 27,
		San_Francisco: 28,
		Washington_D_C: 29,
		Miami: 30,
		Boston: 31,
		Seattle: 32,
		Vancouver: 33,
		Rio_de_Janeiro: 34,
		Doha: 35,
		Barcelona: 36,
		Vienna: 37,
		Seoul: 38,
		Jakarta: 39,
		Kuala_Lumpur: 40,
		Manila: 41,
		Athens: 42,
		Brussels: 43,
		Warsaw: 44,
		Oslo: 45,
		Stockholm: 46,
		Lisbon: 47,
		Helsinki: 48,
		Copenhagen: 49,
		Prague: 50,
		Budapest: 51,
		Bucharest: 52,
		Dublin: 53,
		Milan: 54,
		Florence: 55,
		Venice: 56,
		Naples: 57,
		Lyon: 58,
		Marseille: 59,
		Nice: 60,
		Geneva: 61,
		Frankfurt: 62,
		Hamburg: 63,
		Stuttgart: 64,
		Düsseldorf: 65,
		Cologne: 66,
		Leipzig: 67,
		Antwerp: 68,
		Basel: 69,
		Bern: 70,
		Reykjavik: 71,
		Casablanca: 72,
		Marrakech: 73,
		Lagos: 74,
		Nairobi: 75,
		Addis_Ababa: 76,
		Cape_Town: 77,
		Durban: 78,
		Melbourne: 79,
		Brisbane: 80,
		Perth: 81,
		Adelaide: 82,
		Canberra: 83,
		Auckland: 84,
		Wellington: 85,
		Christchurch: 86,
		Lima: 87,
		Santiago: 88,
		Bogota: 89,
		Caracas: 90,
		Havana: 91,
		Santo_Domingo: 92,
		San_Juan: 93,
		Panama_City: 94,
		San_Salvador: 95,
		Guatemala_City: 96,
		Tegucigalpa: 97,
		San_José: 98,
		Montevideo: 99,
		La_Paz: 100,
		Quito: 101,
		Asunción: 102,
		Kingston: 103,
		Port_of_Spain: 104,
		Bridgetown: 105,
		Georgetown: 106,
		Belmopan: 107,
		Amman: 108,
		Beirut: 109,
		Tel_Aviv: 110,
		Kuwait_City: 111,
		Muscat: 112,
		Sana_a: 113,
		Tbilisi: 114,
		Baku: 115,
		Yerevan: 116,
		Tehran: 117,
		Islamabad: 118,
		Lahore: 119,
		Karachi: 120,
		Kabul: 121,
		Colombo: 122,
		Male: 123,
		Dhaka: 124,
		Kathmandu: 125,
		Thimphu: 126,
		Almaty: 127,
		Bishkek: 128,
		Tashkent: 129,
		Ashgabat: 130,
		Ulaanbaatar: 131,
		Yangon: 132,
		Ho_Chi_Minh_City: 133,
		Hanoi: 134,
		Vientiane: 135,
		Phnom_Penh: 136,
		Luang_Prabang: 137,
		Taipei: 138,
		Kaohsiung: 139,
		Naha: 140,
		Osaka: 141,
		Kyoto: 142,
		Fukuoka: 143,
		Sapporo: 144,
		Nagoya: 145,
		Sendai: 146,
		Busan: 147,
		Jeju: 148,
		Chiang_Mai: 149,
		Phuket: 150,
		Siem_Reap: 151,
		Penang: 152,
		Langkawi: 153,
		Kota_Kinabalu: 154,
		Brunei: 155,
		Macau: 156,
		Guangzhou: 157,
		Shanghai: 158,
		Shenzhen: 159,
		Chengdu: 160,
		Xian: 161,
		Chongqing: 162,
		Wuhan: 163,
		Kunming: 164,
		Nanjing: 165,
		Changsha: 166,
		Harbin: 167,
		Shenyang: 168,
		Hangzhou: 169,
		Suzhou: 170,
		Tianjin: 171,
		Urumqi: 172,
		Dalian: 173,
		Hefei: 174,
		Fuzhou: 175,
		Xiamen: 176,
		Guiyang: 177,
		Lhasa: 178,
		Pyongyang: 179,
		Vladivostok: 180,
		Novosibirsk: 181,
		Saint_Petersburg: 182,
		Kaliningrad: 183,
		Yekaterinburg: 184,
		Irkutsk: 185,
		Sochi: 186,
		Krasnodar: 187,
		Samara: 188,
		Nizhny_Novgorod: 189,
		Volgograd: 190,
		RostovonDon: 191
	};


	const basketURL = "https://localhost:7157/home/getbasketitems";
	const flightURL = "https://localhost:7046/api/flights/";
	const airportURL = "https://localhost:7046/api/airports/";
	const planeURL = "https://localhost:7046/api/planes/";
	const removeFromBasketURL = "https://localhost:7157/wishlist/removefrombasket";

	function fetchBasketItems() {
		fetch(basketURL)
			.then(response => response.json())
			.then(basketItems => {
				basketItems.forEach(item => {
					fetchFlightDetails(item.flightId);
				});
			});
	}

	function fetchFlightDetails(flightId) {
		fetch(`${flightURL}${flightId}`)
			.then(response => response.json())
			.then(data => {
				const flight = data.data;
				fetchRelatedDetails(flight);
			})
			.catch(error => {
				console.error('Error fetching flight details:', error);
			});
	}

	function fetchRelatedDetails(flight) {
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

		const departureCityName = departureAirport.city;
		const arrivalCityName = arrivalAirport.city;

		eachRow.innerHTML = `
            <main style="margin-top: 20px" class="ticket-system">
    <div class="receipts-wrapper">
        <div class="receipts">
            <div class="receipt">
                <div style="display: flex; justify-content: space-between">
                    <div style="display: flex; flex-direction:column">
                        <div>
                            <i class="fa-solid fa-plane-departure"></i>
                            <a style="font-family: cursive; font-size: medium; margin-left: 3px">${plane.airlineName}</a>
                        </div>
                        <div>
                            <i class="fa-solid fa-plane"></i>
                            <a style="font-family: cursive; font-size: medium; margin-left: 3px">${plane.name}</a>
                        </div>
                    </div>
                    <div>
                        <i style="color: red; font-size: 25px; margin-top: 10px" class="fa-solid fa-xmark"></i>
                    </div>
                </div>
                <div class="route">
                    <h2>${departureCityName}</h2>
                    <svg class="plane-icon" xmlns="http://www.w3.org/2000/svg" viewBox="0 0 510 510">
                        <path fill="#3f32e5" d="M497.25 357v-51l-204-127.5V38.25C293.25 17.85 275.4 0 255 0s-38.25 17.85-38.25 38.25V178.5L12.75 306v51l204-63.75V433.5l-51 38.25V510L255 484.5l89.25 25.5v-38.25l-51-38.25V293.25l204 63.75z" />
                    </svg>
                    <h2>${arrivalCityName}</h2>
                </div>
                <div class="details">
                    <div style="display: flex; align-items: center; justify-content: center">
                        <div class="item">
                            <span>Departure time</span>
                            <h3 style="font-family: Arial; font-size: larger; margin-left: 15px">${new Date(flight.departureTime).toLocaleDateString()}</h3>
                        </div>
                        <div style="margin-left: 30px">
                            <i style="font-size:large" class="fa-solid fa-arrow-right"></i>
                        </div>
                        <div style="margin-left: 50px" class="item">
                            <span>Arrival time</span>
                            <h3 style="font-family: Arial; font-size: larger; margin-left: 8px">${new Date(flight.arrivalTime).toLocaleTimeString()}</h3>
                        </div>
                    </div>
                    <div class="item">
                        <span>Flight No.</span>
                        <h3 style="font-family: Arial; font-size: larger">${flight.flightNumber}</h3>
                    </div>
                    <div class="item">
                        <span>Economy Seat</span>
                        <h3 style="font-family: Arial; font-size: larger">$${flight.EconomySeatPrice}</h3>
                    </div>
                    <div class="item">
                        <span>Business Seat</span>
                        <h3 style="font-family: Arial; font-size: larger">$${flight.EconomySeatPrice}</h3>
                    </div>
                    <div class="item">
                        <span>Economy Seats</span>
                        <h3 style="font-family: Arial; font-size: larger">${flight.seats.length}</h3>
                    </div>
                    <div class="item">
                        <span>Business Seats</span>
                        <h3 style="font-family: Arial; font-size: larger">${flight.seats.length}</h3>
                    </div>
                </div>
            </div>
            <div style="display:flex;flex-direction:column" class="receipt qr-code">
                <a class="btn btn-outline-primary">Book Economy Seat $${flight.economySeatPrice}<i style="margin-left: 3px" class="fas fa-arrow-right"></i></a>
                <a style="margin-top: 10px" class="btn btn-outline-warning">Book Business Seat $${flight.businessSeatPrice}<i style="margin-left: 6px" class="fas fa-arrow-right"></i></a>
            </div>
        </div>
    </div>
</main>
        `;

		rowDiv.appendChild(eachRow);
	}

	function removeFromBasket(flightId) {
		fetch(`${removeFromBasketURL}?flightId=${flightId}`, {
			method: 'POST'
		})
			.then(response => {
				if (response.ok) {
					alert('Flight removed from basket successfully.');
					location.reload();
				} else {
					alert('Failed to remove flight from basket.');
				}
			})
			.catch(error => {
				console.error('Error removing flight from basket:', error);
			});
	}

	fetchBasketItems();
});
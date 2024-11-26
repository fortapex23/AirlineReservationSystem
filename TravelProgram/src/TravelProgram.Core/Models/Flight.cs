﻿namespace TravelProgram.Core.Models
{
	public class Flight : BaseEntity
	{
        public string FlightNumber { get; set; }
        public decimal EconomySeatPrice { get; set; }
        public decimal BusinessSeatPrice { get; set; }
        public int DepartureAirportId { get; set; }
		public int ArrivalAirportId { get; set; }
        public int PlaneId { get; set; }

        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

		public Airport DepartureAirport { get; set; }
		public Airport ArrivalAirport { get; set; }
        public Plane Plane { get; set; }

        public ICollection<Booking> Bookings { get; set; }
        public ICollection<Seat> Seats { get; set; }
    }
}

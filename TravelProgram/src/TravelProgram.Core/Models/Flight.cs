﻿namespace TravelProgram.Core.Models
{
	public class Flight : BaseEntity
	{
        public int FlightNumber { get; set; }
		public int DepartureAirportId { get; set; }
		public int ArrivalAirportId { get; set; }
		public int AirlineId { get; set; }
        public int PlaneId { get; set; }

		public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

		public Airport DepartureAirport { get; set; }
		public Airport ArrivalAirport { get; set; }
        public Airline Airline { get; set; }
        public Plane Plane { get; set; }
    }
}
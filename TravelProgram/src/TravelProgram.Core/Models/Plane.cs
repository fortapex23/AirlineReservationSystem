namespace TravelProgram.Core.Models
{
	public class Plane : BaseEntity
	{
        public string Name { get; set; }
        public int EconomySeats { get; set; }
        public int BusinessSeats { get; set; }
        public int AirlineId { get; set; }

        public Airline Airline { get; set; }
        public ICollection<Flight> Flights { get; set; }
        public ICollection<Seat> Seats { get; set; }
	}
}

namespace TravelProgram.Core.Models
{
	public class Airline : BaseEntity
	{
		public string Name { get; set; }
        public string Country { get; set; }

        public ICollection<Plane> Planes { get; set; }
        public ICollection<Flight> Flights { get; set; }
	}
}

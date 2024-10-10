using TravelProgram.Core.Enum;

namespace TravelProgram.Core.Models
{
	public class Airport : BaseEntity
	{
        public string Name { get; set; }
        public AiportCities City { get; set; }

		public ICollection<Flight> DepartingFlights { get; set; }
		public ICollection<Flight> ArrivingFlights { get; set; }
	}
}

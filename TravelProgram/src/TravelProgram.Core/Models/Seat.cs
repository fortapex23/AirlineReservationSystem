using TravelProgram.Core.Enum;

namespace TravelProgram.Core.Models
{
	public class Seat : BaseEntity
	{
        public int PlaneId { get; set; }
        public int? FlightId { get; set; }
		public int SeatNumber { get; set; }
        public decimal Price { get; set; }
        public SeatClassType ClassType { get; set; }
        public bool IsAvailable { get; set; }

        public Plane Plane { get; set; }
        public Flight? Flight { get; set; }
	}
}

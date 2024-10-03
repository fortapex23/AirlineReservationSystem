using TravelProgram.Core.Enum;

namespace TravelProgram.Core.Models
{
	public class Booking : BaseEntity
	{
        public int FlightId { get; set; }
        public string AppUserId { get; set; }
        public string BookingNumber { get; set; }
        public int SeatNumber { get; set; }
		public BookStatus Status { get; set; }

		public Flight Flight { get; set; }
        public AppUser AppUser { get; set; }
    }
}

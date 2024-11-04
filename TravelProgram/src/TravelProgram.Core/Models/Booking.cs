using TravelProgram.Core.Enum;

namespace TravelProgram.Core.Models
{
	public class Booking : BaseEntity
	{
        public int FlightId { get; set; }
        public int SeatId { get; set; }
        public string AppUserId { get; set; }
        public string BookingNumber { get; set; }

		public Flight Flight { get; set; }
		public Seat Seat { get; set; }
		public AppUser AppUser { get; set; }

        //public ICollection<OrderItem> OrderItems { get; set; }
    }
}

using TravelProgram.MVC.Enums;

namespace TravelProgram.MVC.ViewModels.BookingVMs
{
    public class BookingCreateVM
    {
        public int FlightId { get; set; }
        public string AppUserId { get; set; }
        public string BookingNumber { get; set; }
        public int SeatId { get; set; }
        public BookStatus Status { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}

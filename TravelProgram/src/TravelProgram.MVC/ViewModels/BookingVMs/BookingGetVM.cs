using TravelProgram.MVC.Enums;

namespace TravelProgram.MVC.ViewModels.BookingVMs
{
    public class BookingGetVM
    {
        public int Id { get; set; }
        public int FlightId { get; set; }
        public string FlightNumber { get; set; }
        public string Destination { get; set; }
        public string AppUserId { get; set; }
        public string AppUserName { get; set; }
		public string BookingNumber { get; set; }
        public int SeatId { get; set; }
        public int SeatNumber { get; set; }
        public string SeatClass { get; set; }
        //public OrderStatus Status { get; set; }
        public DateTime CreatedTime { get; set; }
    }
}

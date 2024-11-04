using TravelProgram.MVC.Enums;

namespace TravelProgram.MVC.Areas.Admin.ViewModels.SeatVMs
{
    public class SeatCreateVM
    {
        //public int PlaneId { get; set; }
        public int FlightId { get; set; }
        public int SeatNumber { get; set; }
        public decimal Price { get; set; }
        public SeatClassType ClassType { get; set; }
        public bool IsAvailable { get; set; }
    }
}

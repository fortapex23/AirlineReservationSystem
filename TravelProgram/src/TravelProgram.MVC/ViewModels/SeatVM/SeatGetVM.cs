using TravelProgram.MVC.Enums;
using TravelProgram.MVC.ViewModels.FlightVMs;
using TravelProgram.MVC.ViewModels.PlaneVMs;

namespace TravelProgram.MVC.ViewModels.SeatVM
{
    public class SeatGetVM
    {
        public int Id { get; set; }
        public int PlaneId { get; set; }
        public string PlaneName { get; set; }
        public int? FlightId { get; set; }
        public string? FlightNumber { get; set; }
        public int SeatNumber { get; set; }
        public decimal Price { get; set; }
        public SeatClassType ClassType { get; set; }
        public bool IsAvailable { get; set; }

        public PlaneGetVM Plane { get; set; }
        public FlightGetVM Flight { get; set; }
    }
}

namespace TravelProgram.MVC.Areas.Admin.ViewModels.FlightVMs
{
    public class FlightUpdateVM
    {
        public int FlightNumber { get; set; }
        public decimal EconomySeatPrice { get; set; }
        public decimal BusinessSeatPrice { get; set; }
        public int DepartureAirportId { get; set; }
        public int ArrivalAirportId { get; set; }
        public int PlaneId { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }
    }
}

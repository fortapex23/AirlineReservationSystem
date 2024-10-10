using TravelProgram.MVC.ViewModels.AirlineVMs;
using TravelProgram.MVC.ViewModels.AirportVMs;
using TravelProgram.MVC.ViewModels.BookingVMs;
using TravelProgram.MVC.ViewModels.PlaneVMs;
using TravelProgram.MVC.ViewModels.SeatVM;

namespace TravelProgram.MVC.ViewModels.FlightVMs
{
    public class FlightGetVM
    {
        public int Id { get; set; }
        public int FlightNumber { get; set; }
        public decimal SeatPrice { get; set; }
        public int DepartureAirportId { get; set; }
        public string DepartureAirportCity { get; set; }
        public int ArrivalAirportId { get; set; }
        public string ArrivalAirportCity { get; set; }
        public int AirlineId { get; set; }
        public string AirlineName { get; set; }
        public string AirlineCountry { get; set; }
        public int PlaneId { get; set; }
        public string PlaneName { get; set; }
        public DateTime DepartureTime { get; set; }
        public DateTime ArrivalTime { get; set; }

        public AirportGetVM DepartureAirport { get; set; }
        public AirportGetVM ArrivalAirport { get; set; }
        public AirlineGetVM Airline { get; set; }
        public PlaneGetVM Plane { get; set; }

        public ICollection<BookingGetVM> Bookings { get; set; }
        public ICollection<SeatGetVM> Seats { get; set; }
    }
}

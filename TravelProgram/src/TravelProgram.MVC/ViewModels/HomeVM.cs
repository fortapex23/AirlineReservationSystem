using TravelProgram.MVC.ViewModels.AirlineVMs;
using TravelProgram.MVC.ViewModels.AirportVMs;
using TravelProgram.MVC.ViewModels.BookingVMs;
using TravelProgram.MVC.ViewModels.FlightVMs;
using TravelProgram.MVC.ViewModels.PlaneVMs;
using TravelProgram.MVC.ViewModels.SeatVM;

namespace TravelProgram.MVC.ViewModels
{
    public class HomeVM
    {
        public List<FlightGetVM> Flights { get; set; }
        public List<AirlineGetVM> Airlines { get; set; }
        public List<AirportGetVM> Airports { get; set; }
        public List<BookingGetVM> Bookings { get; set; }
        public List<PlaneGetVM> Planes { get; set; }
        public List<SeatGetVM> Seats { get; set; }
        public string FullName { get; set; }
        
    }
}

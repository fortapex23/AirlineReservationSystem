using TravelProgram.MVC.ViewModels.AirlineVMs;
using TravelProgram.MVC.ViewModels.FlightVMs;
using TravelProgram.MVC.ViewModels.SeatVM;

namespace TravelProgram.MVC.ViewModels.PlaneVMs
{
    public class PlaneGetVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TotalSeats { get; set; }
        public int AirlineId { get; set; }
        public string AirlineName { get; set; }

        public AirlineGetVM Airline { get; set; }
        public ICollection<FlightGetVM> Flights { get; set; }
        public ICollection<SeatGetVM> Seats { get; set; }
    }
}

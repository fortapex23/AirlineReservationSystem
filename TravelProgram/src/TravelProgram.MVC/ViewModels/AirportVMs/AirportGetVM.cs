using TravelProgram.MVC.Enums;
using TravelProgram.MVC.ViewModels.FlightVMs;

namespace TravelProgram.MVC.ViewModels.AirportVMs
{
    public class AirportGetVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public AiportCities City { get; set; }

        public ICollection<FlightGetVM> DepartingFlights { get; set; }
        public ICollection<FlightGetVM> ArrivingFlights { get; set; }
    }
}

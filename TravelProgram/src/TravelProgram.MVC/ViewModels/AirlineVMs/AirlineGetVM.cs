using TravelProgram.MVC.Enums;
using TravelProgram.MVC.ViewModels.FlightVMs;
using TravelProgram.MVC.ViewModels.PlaneVMs;

namespace TravelProgram.MVC.ViewModels.AirlineVMs
{
    public class AirlineGetVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Countries Country { get; set; }

        public ICollection<PlaneGetVM> Planes { get; set; }
        public ICollection<FlightGetVM> Flights { get; set; }
    }
}

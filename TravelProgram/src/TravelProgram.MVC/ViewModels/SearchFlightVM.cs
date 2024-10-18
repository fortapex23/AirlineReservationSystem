using System.ComponentModel.DataAnnotations;
using TravelProgram.MVC.ViewModels.FlightVMs;

namespace TravelProgram.MVC.ViewModels
{
    public class SearchFlightVM
    {
        [Required(ErrorMessage = "Departure city is required.")]
        public string DepartureCity { get; set; }

        [Required(ErrorMessage = "Destination city is required.")]
        public string DestinationCity { get; set; }

        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime? DepartureTime { get; set; }

        public List<FlightGetVM>? Flights { get; set; }
    }
}

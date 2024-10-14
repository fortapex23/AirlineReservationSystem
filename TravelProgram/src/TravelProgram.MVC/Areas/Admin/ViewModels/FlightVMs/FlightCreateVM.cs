using System.ComponentModel.DataAnnotations;

namespace TravelProgram.MVC.Areas.Admin.ViewModels.FlightVMs
{
    public class FlightCreateVM
    {
        [Required(ErrorMessage = "Flight Number is required")]
        public string FlightNumber { get; set; }

        [Required(ErrorMessage = "Seat Price is required")]
        [Range(1, double.MaxValue, ErrorMessage = "Seat Price must be greater than 0")]
        public decimal SeatPrice { get; set; }

        [Required(ErrorMessage = "Departure Airport is required")]
        public int DepartureAirportId { get; set; }

        [Required(ErrorMessage = "Arrival Airport is required")]
        public int ArrivalAirportId { get; set; }

        [Required(ErrorMessage = "Plane is required")]
        public int PlaneId { get; set; }

        [Required(ErrorMessage = "Departure Time is required")]
        public DateTime DepartureTime { get; set; }

        [Required(ErrorMessage = "Arrival Time is required")]
        public DateTime ArrivalTime { get; set; }
    }
}

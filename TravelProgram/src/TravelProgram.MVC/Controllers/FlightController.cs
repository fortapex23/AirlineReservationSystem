using Microsoft.AspNetCore.Mvc;
using TravelProgram.MVC.Services.Interfaces;
using TravelProgram.MVC.ViewModels.AirlineVMs;
using TravelProgram.MVC.ViewModels.AirportVMs;
using TravelProgram.MVC.ViewModels.FlightVMs;
using TravelProgram.MVC.ViewModels.PlaneVMs;

namespace TravelProgram.MVC.Controllers
{
    public class FlightController : BaseController
    {
        private readonly ICrudService _crudService;

        public FlightController(ICrudService crudService)
        {
            _crudService = crudService;
        }
        public async Task<IActionResult> Index()
        {
            SetFullName();

            var flights = await _crudService.GetAllAsync<List<FlightGetVM>>("/flights");
            var airports = await _crudService.GetAllAsync<List<AirportGetVM>>("/airports");
            var airlines = await _crudService.GetAllAsync<List<AirlineGetVM>>("/airlines");
            var planes = await _crudService.GetAllAsync<List<PlaneGetVM>>("/planes");

            foreach (var flight in flights)
            {
                var departureAirport = airports.FirstOrDefault(x => x.Id == flight.DepartureAirportId);
                var arrivalAirport = airports.FirstOrDefault(x => x.Id == flight.ArrivalAirportId);
                var plane = planes.FirstOrDefault(x => x.Id == flight.PlaneId);

                if (plane != null)
                {
                    if (flight.Plane == null)
                    {
                        flight.Plane = new PlaneGetVM();
                    }

                    flight.PlaneName = plane.Name;

                    var airline = airlines.FirstOrDefault(x => x.Id == plane.AirlineId);
                    if (airline != null)
                    {
                        flight.Plane.AirlineName = airline.Name;
                    }
                }

                if (departureAirport != null)
                {
                    flight.DepartureAirportCity = departureAirport.City.ToString();
                }

                if (arrivalAirport != null)
                {
                    flight.ArrivalAirportCity = arrivalAirport.City.ToString();
                }
            }

            return View(flights);
        }
    }
}

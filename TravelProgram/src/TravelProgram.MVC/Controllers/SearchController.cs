using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TravelProgram.MVC.Services.Interfaces;
using TravelProgram.MVC.ViewModels;
using TravelProgram.MVC.ViewModels.AirlineVMs;
using TravelProgram.MVC.ViewModels.AirportVMs;
using TravelProgram.MVC.ViewModels.FlightVMs;
using TravelProgram.MVC.ViewModels.PlaneVMs;

namespace TravelProgram.MVC.Controllers
{
    public class SearchController : BaseController
    {
        private readonly IConfiguration _configuration;
        private readonly ICrudService _crudService;

        public SearchController(IConfiguration configuration, ICrudService crudService)
        {
            _configuration = configuration;
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
                    else
                    {
                        flight.Plane.AirlineName = "Airline Not Available";
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

            var searchVM = new SearchFlightVM()
            {
                Flights = flights
            };

            return View(searchVM);
        }

        [HttpGet]
        public async Task<IActionResult> Search(SearchFlightVM searchVM)
        {
            SetFullName();

            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Error --> {error.ErrorMessage}");
                    }
                }
                return View("Index", searchVM);
            }

            string searchUrl = string.Empty;

            if (searchVM.DepartureTime is not null)
            {
                searchUrl = $"{_configuration.GetSection("Api:URL").Value}/Flights/search?departureCity={searchVM.DepartureCity}&destinationCity={searchVM.DestinationCity}&departureTime={searchVM.DepartureTime}";
            }
            else
            {
                searchUrl = $"{_configuration.GetSection("Api:URL").Value}/Flights/search?departureCity={searchVM.DepartureCity}&destinationCity={searchVM.DestinationCity}";
            }

            try
            {
                var searchResults = await _crudService.GetAllAsync<List<FlightGetVM>>(searchUrl);

                if (searchResults == null || !searchResults.Any())
                {
                    ModelState.AddModelError("", "No flights found for the given criteria");
                    return View("Index", searchVM);
                }

                var airports = await _crudService.GetAllAsync<List<AirportGetVM>>("/airports");
                var airlines = await _crudService.GetAllAsync<List<AirlineGetVM>>("/airlines");
                var planes = await _crudService.GetAllAsync<List<PlaneGetVM>>("/planes");

                foreach (var flight in searchResults)
                {
                    var departureAirport = airports.FirstOrDefault(x => x.Id == flight.DepartureAirportId);
                    var arrivalAirport = airports.FirstOrDefault(x => x.Id == flight.ArrivalAirportId);
                    var plane = planes.FirstOrDefault(x => x.Id == flight.PlaneId);

                    if (plane != null)
                    {
                        //if (flight.Plane == null)
                        //{
                        //    flight.Plane = new PlaneGetVM();
                        //}

                        flight.PlaneName = plane.Name;

                        var airline = airlines.FirstOrDefault(x => x.Id == plane.AirlineId);
                        if (airline != null)
                        {
                            flight.Plane.AirlineName = airline.Name;
                        }
                        //else
                        //{
                        //    flight.Plane.AirlineName = "Airline not";
                        //}
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

                searchVM.Flights = searchResults;

                return View("Index", searchVM);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);
                Console.WriteLine("details -> " + ex.ToString());
                return RedirectToAction("Index", "Home");
            }
        }


    }
}

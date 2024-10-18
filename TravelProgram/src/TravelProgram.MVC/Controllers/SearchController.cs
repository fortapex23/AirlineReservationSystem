using System.Net.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using TravelProgram.MVC.ViewModels;
using TravelProgram.MVC.ViewModels.FlightVMs;

namespace TravelProgram.MVC.Controllers
{
    public class SearchController : Controller
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public SearchController(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Search(SearchFlightVM searchVM)
        {
            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        Console.WriteLine($"Error in {state.Key}: {error.ErrorMessage}");
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
                var response = await _httpClient.GetStringAsync(searchUrl);
                var searchResults = JsonConvert.DeserializeObject<List<FlightGetVM>>(response);

                if (searchResults == null || !searchResults.Any())
                {
                    ModelState.AddModelError("", "No flights found for the given criteria.");
                    return View("Search", searchVM);
                }

                searchVM.Flights = searchResults;

                return View("SearchResults", searchVM);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"An error occurred while searching: {ex.Message}");
                return View("Index", searchVM);
            }
        }


    }
}

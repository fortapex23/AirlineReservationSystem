using Microsoft.AspNetCore.Mvc;
using TravelProgram.MVC.Areas.Admin.ViewModels.FlightVMs;
using TravelProgram.MVC.Services.Interfaces;
using TravelProgram.MVC.ViewModels.AirlineVMs;
using TravelProgram.MVC.ViewModels.AirportVMs;
using TravelProgram.MVC.ViewModels.FlightVMs;
using TravelProgram.MVC.ViewModels.PlaneVMs;

namespace TravelProgram.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class FlightController : Controller
    {
        private readonly ICrudService _crudService;

        public FlightController(ICrudService crudService)
        {
            _crudService = crudService;
        }

        public async Task<IActionResult> Index()
        {
            var flights = await _crudService.GetAllAsync<List<FlightGetVM>>("/Flights");

            var airports = await _crudService.GetAllAsync<List<AirportGetVM>>("/airports");

            var planes = await _crudService.GetAllAsync<List<PlaneGetVM>>("/planes");

            foreach (var flight in flights)
            {
				var arrAirport = airports.FirstOrDefault(x => x.Id == flight.ArrivalAirportId);
				var depAirport = airports.FirstOrDefault(x => x.Id == flight.DepartureAirportId);
				var plane = planes.FirstOrDefault(x => x.Id == flight.PlaneId);

				flight.DepartureAirportCity = depAirport.City.ToString();
				flight.ArrivalAirportCity = arrAirport.City.ToString();
				flight.PlaneName = plane.Name;
			}

            return View(flights);
        }

        public async Task<IActionResult> Create()
        {
            ViewBag.Airports = await _crudService.GetAllAsync<List<AirportGetVM>>("/airports");
            ViewBag.Planes = await _crudService.GetAllAsync<List<PlaneGetVM>>("/planes");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(FlightCreateVM vm)
        {
            ViewBag.Airports = await _crudService.GetAllAsync<List<AirportGetVM>>("/airports");
            ViewBag.Planes = await _crudService.GetAllAsync<List<PlaneGetVM>>("/planes");

            try
            {
                await _crudService.Create("/Flights", vm);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "something went wrong");
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _crudService.Delete<object>($"/Flights/{id}", id);
            }
            catch (Exception)
            {
                TempData["Err"] = "not found";
                return View("Error");
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            ViewBag.Airports = await _crudService.GetAllAsync<List<AirportGetVM>>("/airports");
            ViewBag.Planes = await _crudService.GetAllAsync<List<PlaneGetVM>>("/planes");

            FlightUpdateVM data = null;

            try
            {
                data = await _crudService.GetByIdAsync<FlightUpdateVM>($"/Flights/{id}", id);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Entity not found, changes will not be saved");
                return View(data);
            }

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, FlightUpdateVM vm)
        {
            ViewBag.Airports = await _crudService.GetAllAsync<List<AirportGetVM>>("/airports");
            ViewBag.Planes = await _crudService.GetAllAsync<List<PlaneGetVM>>("/planes");

            try
            {
                await _crudService.Update($"/Flights/{id}", vm);
            }
            //catch (ModelStateException ex)
            //{
            //	ModelState.AddModelError(ex.PropertyName, ex.Message);
            //	return View();
            //}
            //catch (BadrequestException ex)
            //{
            //	TempData["Err"] = ex.Message;
            //	return View("Error");
            //}
            //catch (ModelNotFoundException ex)
            //{
            //	TempData["Err"] = ex.Message;
            //	return View("Error");
            //}
            catch (Exception ex)
            {
                TempData["Err"] = ex.Message;
                return View("Error");
            }

            return RedirectToAction("Index");
        }
    }
}

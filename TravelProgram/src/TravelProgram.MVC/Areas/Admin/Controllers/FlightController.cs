using Microsoft.AspNetCore.Mvc;
using TravelProgram.MVC.Areas.Admin.ViewModels.FlightVMs;
using TravelProgram.MVC.Controllers;
using TravelProgram.MVC.Services.Interfaces;
using TravelProgram.MVC.ViewModels.AirlineVMs;
using TravelProgram.MVC.ViewModels.AirportVMs;
using TravelProgram.MVC.ViewModels.FlightVMs;
using TravelProgram.MVC.ViewModels.PlaneVMs;

namespace TravelProgram.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
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

            if (ViewBag.Role is null)
            {
                return RedirectToAction("AdminLogin", "Auth", new { area = "Admin" });
            }

            var flights = await _crudService.GetAllAsync<List<FlightGetVM>>("/Flights");

            var planes = await _crudService.GetAllAsync<List<PlaneGetVM>>("/planes");

            var airports = await _crudService.GetAllAsync<List<AirportGetVM>>("/airports");

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
            SetFullName();

            if (ViewBag.Role is null)
            {
                return RedirectToAction("AdminLogin", "Auth", new { area = "Admin" });
            }

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
            catch (Exception ex)
            {
                if (ex.Message.Contains("same number"))
                {
                    ModelState.AddModelError("", "Flight with same number already exists");
                    return View(vm);
                }
                if (ex.Message.Contains("prices"))
                {
                    ModelState.AddModelError("BusinessSeatPrice", "Invalid seat price");
                    return View(vm);
                }
                if (ex.Message.Contains("plane"))
                {
                    ModelState.AddModelError("PlaneId", "Plane does not exist");
                    return View(vm);
                }
                if (ex.Message.Contains("departure airport"))
                {
                    ModelState.AddModelError("DepartureAirportId", "airport does not exist");
                    return View(vm);
                }
                if (ex.Message.Contains("arrival airport"))
                {
                    ModelState.AddModelError("ArrivalAirportId", "airport does not exist");
                    return View(vm);
                }
                if (ex.Message.Contains("Departure time"))
                {
                    ModelState.AddModelError("DepartureTime", "Invalid Departure time");
                    return View(vm);
                }
                if (ex.Message.Contains("Arrival time"))
                {
                    ModelState.AddModelError("ArrivalTime", "Arrival time must be > departure time");
                    return View(vm);
                }
                else
                {
                    ModelState.AddModelError("", "Something went wrong");
                    return View();
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _crudService.Delete<object>($"/Flights/{id}", id);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("book"))
                {
                    TempData["Err"] = "this flight has been booked. You cant delete it";
                }
                else
                {
                    TempData["Err"] = "Flight not found";
                }

                return RedirectToAction("Index");
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(int id)
        {
            SetFullName();

            if (ViewBag.Role is null)
            {
                return RedirectToAction("AdminLogin", "Auth", new { area = "Admin" });
            }

            ViewBag.Airports = await _crudService.GetAllAsync<List<AirportGetVM>>("/airports");
            ViewBag.Planes = await _crudService.GetAllAsync<List<PlaneGetVM>>("/planes");

            FlightUpdateVM data = null;

            try
            {
                data = await _crudService.GetByIdAsync<FlightUpdateVM>($"/Flights/{id}", id);
            }
            catch (Exception)
            {
                TempData["Err"] = "Flight not found";
                return RedirectToAction("Index");
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
            catch (Exception ex)
            {
                if (ex.Message.Contains("same number"))
                {
                    ModelState.AddModelError("", "Flight with same number already exists");
                    return View(vm);
                }
                if (ex.Message.Contains("prices"))
                {
                    ModelState.AddModelError("BusinessSeatPrice", "Invalid seat price");
                    return View(vm);
                }
                if (ex.Message.Contains("plane"))
                {
                    ModelState.AddModelError("PlaneId", "Plane does not exist");
                    return View(vm);
                }
                if (ex.Message.Contains("departure airport"))
                {
                    ModelState.AddModelError("DepartureAirportId", "airport does not exist");
                    return View(vm);
                }
                if (ex.Message.Contains("arrival airport"))
                {
                    ModelState.AddModelError("ArrivalAirportId", "airport does not exist");
                    return View(vm);
                }
                if (ex.Message.Contains("Departure time"))
                {
                    ModelState.AddModelError("DepartureTime", "Invalid Departure time");
                    return View(vm);
                }
                if (ex.Message.Contains("Arrival time"))
                {
                    ModelState.AddModelError("ArrivalTime", "Arrival time must be > departure time");
                    return View(vm);
                }
                else
                {
                    ModelState.AddModelError("", "Something went wrong");
                    return View();
                }
            }

            return RedirectToAction("Index");
        }
    }
}

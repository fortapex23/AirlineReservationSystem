using Microsoft.AspNetCore.Mvc;
using TravelProgram.MVC.Areas.Admin.ViewModels.FlightVMs;
using TravelProgram.MVC.Areas.Admin.ViewModels.SeatVMs;
using TravelProgram.MVC.Controllers;
using TravelProgram.MVC.PaginatedLists;
using TravelProgram.MVC.Services.Interfaces;
using TravelProgram.MVC.ViewModels.AirportVMs;
using TravelProgram.MVC.ViewModels.FlightVMs;
using TravelProgram.MVC.ViewModels.PlaneVMs;
using TravelProgram.MVC.ViewModels.SeatVM;

namespace TravelProgram.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SeatController : BaseController
    {
        private readonly ICrudService _crudService;

        public SeatController(ICrudService crudService)
        {
            _crudService = crudService;
        }
        public async Task<IActionResult> Index(int page = 1)
        {
            SetFullName();

            if (ViewBag.Role is null)
            {
                return RedirectToAction("AdminLogin", "Auth", new { area = "Admin" });
            }

            var seats = await _crudService.GetAllAsync<List<SeatGetVM>>("/seats");

            var flights = await _crudService.GetAllAsync<List<FlightGetVM>>("/Flights");

            //var planes = await _crudService.GetAllAsync<List<PlaneGetVM>>("/planes");

            foreach (var seat in seats)
            {
                //var planename = planes.FirstOrDefault(x => x.Id == seat.Flight.PlaneId);
                var flightnum = flights.FirstOrDefault(x => x.Id == seat.FlightId);

                //seat.PlaneName = planename.Name;
                seat.FlightNumber = flightnum?.FlightNumber;
            }

            int pageSize = 5;
            var paginatedSeats = PaginatedList<SeatGetVM>.Create(seats.AsQueryable(), page, pageSize);

            return View(paginatedSeats);
        }

        public async Task<IActionResult> Create()
        {
            SetFullName();

            if (ViewBag.Role is null)
            {
                return RedirectToAction("AdminLogin", "Auth", new { area = "Admin" });
            }

            ViewBag.Flights = await _crudService.GetAllAsync<List<FlightGetVM>>("/flights");
            //ViewBag.Planes = await _crudService.GetAllAsync<List<PlaneGetVM>>("/planes");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(SeatCreateVM vm)
        {
			ViewBag.Flights = await _crudService.GetAllAsync<List<FlightGetVM>>("/flights");
			//ViewBag.Planes = await _crudService.GetAllAsync<List<PlaneGetVM>>("/planes");

            try
            {
                await _crudService.Create("/seats", vm);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("No flight"))
                {
                    ModelState.AddModelError("FlightId", "Invalid flight id");
                    return View(vm);
                }
                if (ex.Message.Contains("same number"))
                {
                    ModelState.AddModelError("SeatNumber", "seat with same number already exists");
                    return View(vm);
                }
                if (ex.Message.Contains("Seat number"))
                {
                    ModelState.AddModelError("SeatNumber", "seat number cant be <= 0");
                    return View(vm);
                }
                if (ex.Message.Contains("Price"))
                {
                    ModelState.AddModelError("Price", "Price cant be below 0");
                    return View(vm);
                }
                else
                {
                    ModelState.AddModelError("", "Something went wrong");
                    return View(vm);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _crudService.Delete<object>($"/seats/{id}", id);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("booked"))
                {
                    TempData["Err"] = "this seat has been booked. You cant delete it";
                }
                else
                {
                    TempData["Err"] = "Seat not found";
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

            ViewBag.Flights = await _crudService.GetAllAsync<List<FlightGetVM>>("/flights");
            //ViewBag.Planes = await _crudService.GetAllAsync<List<PlaneGetVM>>("/planes");

            SeatUpdateVM data = null;

            try
            {
                data = await _crudService.GetByIdAsync<SeatUpdateVM>($"/seats/{id}", id);
            }
            catch (Exception)
            {
                TempData["Err"] = "Seat not found";
                return RedirectToAction("Index");
            }

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, SeatUpdateVM vm)
        {
            ViewBag.Flights = await _crudService.GetAllAsync<List<FlightGetVM>>("/flights");
            //ViewBag.Planes = await _crudService.GetAllAsync<List<PlaneGetVM>>("/planes");

            try
            {
                await _crudService.Update($"/seats/{id}", vm);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("No flight"))
                {
                    ModelState.AddModelError("FlightId", "Invalid flight id");
                    return View(vm);
                }
                if (ex.Message.Contains("same number"))
                {
                    ModelState.AddModelError("SeatNumber", "seat with same number already exists");
                    return View(vm);
                }
                if (ex.Message.Contains("Seat number cant"))
                {
                    ModelState.AddModelError("SeatNumber", "seat number cant be <= 0");
                    return View(vm);
                }
                if (ex.Message.Contains("Price cant"))
                {
                    ModelState.AddModelError("Price", "Price cant be below 0");
                    return View(vm);
                }
                else
                {
                    ModelState.AddModelError("", "Something went wrong");
                    return View(vm);
                }
            }

            return RedirectToAction("Index");
        }
    }
}

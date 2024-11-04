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
                await _crudService.Delete<object>($"/seats/{id}", id);
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
                ModelState.AddModelError("", "Entity not found, changes will not be saved");
                return View(data);
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

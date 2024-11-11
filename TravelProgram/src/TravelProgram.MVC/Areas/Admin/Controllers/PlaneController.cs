using Microsoft.AspNetCore.Mvc;
using TravelProgram.MVC.Areas.Admin.ViewModels.PlaneVMs;
using TravelProgram.MVC.Controllers;
using TravelProgram.MVC.Services.Interfaces;
using TravelProgram.MVC.ViewModels.AirlineVMs;
using TravelProgram.MVC.ViewModels.PlaneVMs;

namespace TravelProgram.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PlaneController : BaseController
    {
        private readonly ICrudService _crudService;

        public PlaneController(ICrudService crudService)
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

            var planes = await _crudService.GetAllAsync<List<PlaneGetVM>>("/Planes");

            var airlines = await _crudService.GetAllAsync<List<AirlineGetVM>>("Airlines");

            foreach (var pl in planes)
            {
                var airline = airlines.FirstOrDefault(x => x.Id == pl.AirlineId);

                pl.AirlineName = airline.Name;
            }

            return View(planes);
        }
        
        public async Task<IActionResult> Create()
        {
            SetFullName();

            if (ViewBag.Role is null)
            {
                return RedirectToAction("AdminLogin", "Auth", new { area = "Admin" });
            }

            ViewBag.Airlines = await _crudService.GetAllAsync<List<AirlineGetVM>>("/airlines");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PlaneCreateVM vm)
        {
            ViewBag.Airlines = await _crudService.GetAllAsync<List<AirlineGetVM>>("/airlines");

            try
            {
                await _crudService.Create("/Planes", vm);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("length"))
                {
                    ModelState.AddModelError("", "Plane name length must be < 100");
                    return View(vm);
                }
                if (ex.Message.Contains("same name"))
                {
                    ModelState.AddModelError("", "PLane with same name already exists");
                    return View(vm);
                }
                if (ex.Message.Contains("count"))
                {
                    ModelState.AddModelError("BusinessSeats", "Invalid plane seat count");
                    return View(vm);
                }
                if (ex.Message.Contains("airline"))
                {
                    ModelState.AddModelError("AirlineId", "Invalid AirlineId");
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
                await _crudService.Delete<object>($"/Planes/{id}", id);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("flights"))
                {
                    TempData["Err"] = "Plane has flights. You cant delete it";
                }
                else
                {
                    TempData["Err"] = "Plane not found";
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

            ViewBag.Airlines = await _crudService.GetAllAsync<List<AirlineGetVM>>("/airlines");

            PlaneUpdateVM data = null;

            try
            {
                data = await _crudService.GetByIdAsync<PlaneUpdateVM>($"/Planes/{id}", id);
            }
            catch (Exception)
            {
                TempData["Err"] = "Plane not found";
                return RedirectToAction("Index");
            }

            return View(data);
        }

        [HttpPost]
        public async Task<IActionResult> Update(int id, PlaneUpdateVM vm)
        {
            ViewBag.Airlines = await _crudService.GetAllAsync<List<AirlineGetVM>>("/airlines");

            try
            {
                await _crudService.Update($"/Planes/{id}", vm);
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("length"))
                {
                    ModelState.AddModelError("", "Plane name length must be < 100");
                    return View(vm);
                }
                if (ex.Message.Contains("same name"))
                {
                    ModelState.AddModelError("", "Plane with same name already exists");
                    return View(vm);
                }
                if (ex.Message.Contains("count"))
                {
                    ModelState.AddModelError("BusinessSeats", "Invalid plane seat count");
                    return View(vm);
                }
                if (ex.Message.Contains("airline"))
                {
                    ModelState.AddModelError("AirlineId", "Invalid AirlineId");
                    return View(vm);
                }
                else
                {
                    TempData["Err"] = ex.Message;
                    return View("Error");
                }
            }

            return RedirectToAction("Index");
        }
    }
}

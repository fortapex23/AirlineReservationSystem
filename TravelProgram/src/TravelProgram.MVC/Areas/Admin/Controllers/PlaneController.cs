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
            catch (Exception)
            {
                ModelState.AddModelError("", "cant be null");
                return View();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _crudService.Delete<object>($"/Planes/{id}", id);
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

            ViewBag.Airlines = await _crudService.GetAllAsync<List<AirlineGetVM>>("/airlines");

            PlaneUpdateVM data = null;

            try
            {
                data = await _crudService.GetByIdAsync<PlaneUpdateVM>($"/Planes/{id}", id);
            }
            catch (Exception)
            {
                ModelState.AddModelError("", "Entity not found, changes will not be saved");
                return View(data);
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

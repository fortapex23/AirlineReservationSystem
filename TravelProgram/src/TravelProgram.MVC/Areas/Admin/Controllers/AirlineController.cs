using Microsoft.AspNetCore.Mvc;
using TravelProgram.MVC.Areas.Admin.ViewModels.AirlineVMs;
using TravelProgram.MVC.Controllers;
using TravelProgram.MVC.Services.Interfaces;
using TravelProgram.MVC.ViewModels.AirlineVMs;

namespace TravelProgram.MVC.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AirlineController : BaseController
	{
		private readonly ICrudService _crudService;

		public AirlineController(ICrudService crudService)
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

            var datas = await _crudService.GetAllAsync<List<AirlineGetVM>>("/Airlines");

			return View(datas);
		}

		public IActionResult Create()
		{
            SetFullName();

            if (ViewBag.Role is null)
            {
                return RedirectToAction("AdminLogin", "Auth", new { area = "Admin" });
            }

            return View();
		}

		[HttpPost]
		public async Task<IActionResult> Create(AirlineCreateVM vm)
		{
			try
			{
				await _crudService.Create("/Airlines", vm);
			}
			catch (Exception ex)
			{
                if (ex.Message.Contains("same name"))
                {
                    ModelState.AddModelError("", "Airline with same name already exists");
                    return View(vm);
                }
                if (ex.Message.Contains("length"))
                {
                    ModelState.AddModelError("", "Airline name length must be < 100");
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
				await _crudService.Delete<object>($"/Airlines/{id}", id);
			}
            catch (Exception ex)
            {
                if (ex.Message.Contains("plane"))
                {
                    TempData["Err"] = "Airline has planes. You cant delete it";
                }
                else
                {
                    TempData["Err"] = "Airline not found";
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

            AirlineUpdateVM data = null;

			try
			{
				data = await _crudService.GetByIdAsync<AirlineUpdateVM>($"/Airlines/{id}", id);
			}
            catch (Exception)
            {
                TempData["Err"] = "Airline not found";
                return RedirectToAction("Index");
            }

            return View(data);
		}

		[HttpPost]
		public async Task<IActionResult> Update(int id, AirlineUpdateVM vm)
		{
			try
			{
				await _crudService.Update($"/Airlines/{id}", vm);
			}
			catch (Exception ex)
			{
                if (ex.Message.Contains("same name"))
                {
                    ModelState.AddModelError("", "Airline with same name already exists");
                    return View(vm);
                }
                if (ex.Message.Contains("length"))
                {
                    ModelState.AddModelError("", "Airline name length must be < 100");
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

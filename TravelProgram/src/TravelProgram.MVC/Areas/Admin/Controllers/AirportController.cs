using Microsoft.AspNetCore.Mvc;
using TravelProgram.MVC.Areas.Admin.ViewModels.AirportVMs;
using TravelProgram.MVC.Controllers;
using TravelProgram.MVC.Services.Interfaces;
using TravelProgram.MVC.ViewModels.AirportVMs;

namespace TravelProgram.MVC.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AirportController : BaseController
	{
		private readonly ICrudService _crudService;

		public AirportController(ICrudService crudService)
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

            var datas = await _crudService.GetAllAsync<List<AirportGetVM>>("/Airports");

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
		public async Task<IActionResult> Create(AirportCreateVM vm)
		{
			try
			{
				await _crudService.Create("/Airports", vm);
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("same name"))
				{
					ModelState.AddModelError("", "Airport with same name already exists");
					return View(vm);
				}
				if (ex.Message.Contains("length"))
				{
					ModelState.AddModelError("", "Airport name length must be < 100");
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
				await _crudService.Delete<object>($"/Airports/{id}", id);
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("flights"))
				{
					TempData["Err"] = "Airport has flights to departure and arrive. You cant delete it";
				}
				else
				{
					TempData["Err"] = "Airport not found";
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

            AirportUpdateVM data = null;

            try
            {
                data = await _crudService.GetByIdAsync<AirportUpdateVM>($"/Airports/{id}", id);
                //if (data == null)
                //{
                //    TempData["Err"] = "Airport not found";
                //    return RedirectToAction("Index");
                //}
            }
            catch (Exception)
            {
                TempData["Err"] = "Airport not found";
                return RedirectToAction("Index");
            }

            return View(data);
        }


        [HttpPost]
		public async Task<IActionResult> Update(int id, AirportUpdateVM vm)
		{
			try
			{
				await _crudService.Update($"/Airports/{id}", vm);
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("same name"))
				{
					ModelState.AddModelError("", "Aiport with same name already exists");
					return View(vm);
				}
                if (ex.Message.Contains("length"))
                {
                    ModelState.AddModelError("", "Airport name length must be < 100");
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

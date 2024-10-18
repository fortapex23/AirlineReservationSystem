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

			var datas = await _crudService.GetAllAsync<List<AirlineGetVM>>("/Airlines");

			return View(datas);
		}

		//public async Task<IActionResult> Detail(int id)
		//{
		//	AirlineGetVM data = null;
		//	try
		//	{
		//		data = await _crudService.GetByIdAsync<AirlineGetVM>($"/Airlines/{id}", id);
		//	}
		//	//catch (BadrequestException ex)
		//	//{
		//	//	TempData["Err"] = ex.Message;
		//	//	return View("Error");
		//	//}
		//	//catch (ModelNotFoundException ex)
		//	//{
		//	//	TempData["Err"] = ex.Message;
		//	//	return View("Error");
		//	//}
		//	catch (Exception ex)
		//	{
		//		TempData["Err"] = ex.Message;
		//		return View("Error");
		//	}

		//	return View(data);
		//}

		public IActionResult Create()
		{
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
				ModelState.AddModelError("", "cant be null");
				return View();
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
				TempData["Err"] = "not found";
				return View("Error");
			}

			return RedirectToAction(nameof(Index));
		}

		public async Task<IActionResult> Update(int id)
		{
			AirlineUpdateVM data = null;

			try
			{
				data = await _crudService.GetByIdAsync<AirlineUpdateVM>($"/Airlines/{id}", id);
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", "Entity not found, changes will not be saved");
				return View(data);
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

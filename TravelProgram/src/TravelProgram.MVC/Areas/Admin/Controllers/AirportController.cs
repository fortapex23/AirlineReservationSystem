using Microsoft.AspNetCore.Mvc;
using TravelProgram.MVC.Areas.Admin.ViewModels.AirportVMs;
using TravelProgram.MVC.Services.Interfaces;
using TravelProgram.MVC.ViewModels.AirportVMs;

namespace TravelProgram.MVC.Areas.Admin.Controllers
{
	[Area("Admin")]
	public class AirportController : Controller
	{
		private readonly ICrudService _crudService;

		public AirportController(ICrudService crudService)
		{
			_crudService = crudService;
		}

		public async Task<IActionResult> Index()
		{
			var datas = await _crudService.GetAllAsync<List<AirportGetVM>>("/Airports");

			return View(datas);
		}

		//public async Task<IActionResult> Detail(int id)
		//{
		//	AirportGetVM data = null;
		//	try
		//	{
		//		data = await _crudService.GetByIdAsync<AirportGetVM>($"/Airports/{id}", id);
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
		public async Task<IActionResult> Create(AirportCreateVM vm)
		{
			try
			{
				await _crudService.Create("/Airports", vm);
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
				await _crudService.Delete<object>($"/Airports/{id}", id);
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
			AirportUpdateVM data = null;

			try
			{
				data = await _crudService.GetByIdAsync<AirportUpdateVM>($"/Airports/{id}", id);
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", "Entity not found, changes will not be saved");
				return View(data);
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

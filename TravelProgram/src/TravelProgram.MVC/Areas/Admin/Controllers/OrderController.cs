using Microsoft.AspNetCore.Mvc;
using TravelProgram.MVC.Controllers;
using TravelProgram.MVC.Enums;
using TravelProgram.MVC.Services.Interfaces;
using TravelProgram.MVC.ViewModels.AuthVMs;
using TravelProgram.MVC.ViewModels.OrderVMs;

namespace TravelProgram.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : BaseController
    {
        private readonly ICrudService _crudService;

        public OrderController(ICrudService crudService)
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

			var orders = await _crudService.GetAllAsync<List<OrderGetVM>>("/orders");

			//var flights = await _crudService.GetAllAsync<List<FlightGetVM>>("/flights");

			//var seats = await _crudService.GetAllAsync<List<SeatGetVM>>("/seats");

			var users = await _crudService.GetAllAsync<List<AuthGetVM>>("/Auth");

            foreach (var book in orders)
            {
                var bUserName = users.FirstOrDefault(x => x.AppUserId == book.AppUserId);
                //var bookFlghtNum = flights.FirstOrDefault(x => x.Id == book.FlightId);
                //var bookSeatNum = seats.FirstOrDefault(x => x.Id == book.SeatId);

                //book.SeatNumber = bookSeatNum.SeatNumber;
                book.AppUserName = bUserName?.FullName ?? "User";
                //book.FlightNumber = bookFlghtNum.FlightNumber;
            }

            return View(orders);
        }

		public async Task<IActionResult> ApproveOrder(int id)
		{
			SetFullName();

			if (ViewBag.Role is null)
			{
				return RedirectToAction("AdminLogin", "Auth", new { area = "Admin" });
			}

			var order = await _crudService.GetByIdAsync<OrderGetVM>($"/orders/{id}", id);

			if (order == null)
			{
				return NotFound();
			}

			order.Status = OrderStatus.Completed;

			try
			{
				await _crudService.Update($"/orders/{id}", order);
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("already booking "))
				{
					TempData["Err"] = "seat from this order has already been booked. You should cancel order";
				}
			}
			return RedirectToAction("Index");
		}

		public async Task<IActionResult> CancelOrder(int id)
		{
			SetFullName();

			if (ViewBag.Role is null)
			{
				return RedirectToAction("AdminLogin", "Auth", new { area = "Admin" });
			}

			var order = await _crudService.GetByIdAsync<OrderGetVM>($"/orders/{id}", id);

			if (order == null)
			{
				return NotFound();
			}

			order.Status = OrderStatus.Canceled;

			try
			{
				await _crudService.Update($"/orders/{id}", order);
			}
			catch (Exception ex)
			{
				if (ex.Message.Contains("You cant change"))
				{
					TempData["Err"] = "You cant reject completed order!";
				}
			}

			return RedirectToAction("Index");
		}


	}
}

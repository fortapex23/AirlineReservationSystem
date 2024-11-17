using Microsoft.AspNetCore.Mvc;
using TravelProgram.MVC.Controllers;
using TravelProgram.MVC.Services.Interfaces;
using TravelProgram.MVC.ViewModels.FlightVMs;
using TravelProgram.MVC.ViewModels.OrderItemVMs;
using TravelProgram.MVC.ViewModels.SeatVM;

namespace TravelProgram.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderItemController : BaseController
    {
        private readonly ICrudService _crudService;

        public OrderItemController(ICrudService crudService)
        {
            _crudService = crudService;
        }

        public async Task<IActionResult> Index(int orderId)
        {
            SetFullName();

            if (ViewBag.Role is null)
            {
                return RedirectToAction("AdminLogin", "Auth", new { area = "Admin" });
            }

			var orderItems = await _crudService.GetAllAsync<List<OrderItemGetVM>>($"/OrderItems/GetByOrderId?orderid={orderId}");

			var seats = await _crudService.GetAllAsync<List<SeatGetVM>>("/seats");

			var flights = await _crudService.GetAllAsync<List<FlightGetVM>>("/flights");

            foreach (var item in orderItems)
            {
                var seat = seats.FirstOrDefault(x => x.Id == item.SeatId);

                var flight = flights.FirstOrDefault(f => f.Id == seat.FlightId);

				item.SeatNumber = seat.SeatNumber;
                item.FlightNumber = flight.FlightNumber;
			}

			if (orderItems == null)
			{
				//ViewBag.Message = "No items";
				return View();
			}


			return View(orderItems);
        }
    }
}

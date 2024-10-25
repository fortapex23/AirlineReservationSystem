using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using TravelProgram.MVC.Services.Interfaces;
using TravelProgram.MVC.ViewModels.OrderVMs;

namespace TravelProgram.MVC.Controllers
{
    public class OrderController : BaseController
    {
        private readonly ICrudService _crudService;

        public OrderController(ICrudService crudService)
        {
            _crudService = crudService;
        }
        [HttpGet]
        public IActionResult CreateOrder()
        {
            var orderData = TempData["OrderCreateVM"] as string;
            if (orderData == null) return RedirectToAction("Index", "Home");

            var orderCreateVM = JsonSerializer.Deserialize<OrderCreateVM>(orderData);
            return View(orderCreateVM);
        }


        [HttpPost]
        public async Task<IActionResult> ConfirmOrder(OrderCreateVM orderCreateVm)
        {
            if (!ModelState.IsValid) return View("CreateOrder", orderCreateVm);

            await _crudService.Create("/orders", orderCreateVm);
            return RedirectToAction("OrderConfirmation");
        }


        public IActionResult OrderConfirmation()
        {
            return View();
        }
    }
}

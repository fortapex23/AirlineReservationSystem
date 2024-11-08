using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using TravelProgram.MVC.ApiResponseMessages;
using TravelProgram.MVC.Enums;
using TravelProgram.MVC.Services.Interfaces;
using TravelProgram.MVC.ViewModels;
using TravelProgram.MVC.ViewModels.OrderItemVMs;
using TravelProgram.MVC.ViewModels.OrderVMs;
using TravelProgram.MVC.ViewModels.SeatVM;

namespace TravelProgram.MVC.Controllers
{
    public class OrderController : BaseController
    {
        private readonly ICrudService _crudService;

        public OrderController(ICrudService crudService)
        {
            _crudService = crudService;
        }

        public IActionResult Index()
        {

            return View();
        }

        public IActionResult Checkout()
        {
            SetFullName();

            if (ViewBag.FullName is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Checkout(int cardNumber)
        {
            string token = HttpContext.Request.Cookies["token"];
            string appUserId = null;

            if (!string.IsNullOrEmpty(token))
            {
                var secretKey = "sdfgdf-463dgdfsd j-fdvnji2387nTravel";
                var key = Encoding.ASCII.GetBytes(secretKey);
                var tokenHandler = new JwtSecurityTokenHandler();

                try
                {
                    var claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    }, out SecurityToken validatedToken);

                    appUserId = claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                    if (string.IsNullOrEmpty(appUserId))
                    {
                        return Unauthorized("User identity not found in the token.");
                    }
                }
                catch (SecurityTokenException)
                {
                    return Unauthorized("Invalid token.");
                }
            }
            else
            {
                return Unauthorized("Token is required.");
            }

            var basketItems = await _crudService.GetAllAsync<List<BasketItemVM>>($"basketitem?appUserId={appUserId}");

            if (basketItems == null || !basketItems.Any())
            {
                ModelState.AddModelError("", "Your basket is empty.");
                return View("Order");
            }

            //var orderItems = new List<OrderItemCreateVM>();

            //foreach (var item in basketItems)
            //{
            //    var seatDetails = await _crudService.GetByIdAsync<SeatGetVM>($"seats/{item.SeatId}", item.SeatId);

            //    if (seatDetails == null)
            //    {
            //        ModelState.AddModelError("", $"Seat with ID {item.SeatId} not found.");
            //        return View("Order");
            //    }

            //    orderItems.Add(new OrderItemCreateVM
            //    {
            //        SeatId = item.SeatId,
            //        Price = seatDetails.Price
            //    });
            //}

            //var totalAmount = orderItems.Sum(item => item.Price);
            //var orderCreateVm = new OrderCreateVM
            //{
            //    AppUserId = appUserId,
            //    CardNumber = cardNumber,
            //    OrderItems = orderItems,
            //    TotalAmount = totalAmount,
            //};

            //var seatIds = basketItems.Select(item => item.SeatId).ToList();

            //var orderCreateVm = new OrderCreateVM
            //{
            //    AppUserId = appUserId,
            //    CardNumber = cardNumber,
            //    SeatIds = seatIds
            //};

            await _crudService.Create("orders", orderCreateVm);

			foreach (var item in basketItems)
			{
				try
				{
					await _crudService.DeleteItem<object>($"BasketItem/remove?appUserId={appUserId}&seatId={item.SeatId}");
				}
				catch (Exception ex)
				{
					Console.WriteLine($"Failed to delete BasketItem for SeatId {item.SeatId}: {ex.Message}");
				}
			}

			return RedirectToAction("Confirmation");
        }

        public IActionResult Confirmation()
        {
            return View();
        }

    }
}

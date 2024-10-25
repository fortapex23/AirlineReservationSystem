using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using NuGet.Common;
using TravelProgram.MVC.ApiResponseMessages;
using TravelProgram.MVC.Enums;
using TravelProgram.MVC.Services.Implementations;
using TravelProgram.MVC.Services.Interfaces;
using TravelProgram.MVC.ViewModels;
using TravelProgram.MVC.ViewModels.BookingVMs;
using TravelProgram.MVC.ViewModels.OrderItemVMs;
using TravelProgram.MVC.ViewModels.OrderVMs;
using TravelProgram.MVC.ViewModels.SeatVM;

namespace TravelProgram.MVC.Controllers
{
    public class BookingController : BaseController
    {
        private readonly ICrudService _crudService;
        private readonly IConfiguration _configuration;

        public BookingController(ICrudService crudService, IConfiguration configuration)
        {
            _crudService = crudService;
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            SetFullName();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BookSeats(int flightId, SeatClassType seatClass, List<int> SelectedSeats)
        {
            if (SelectedSeats == null || SelectedSeats.Count == 0)
            {
                return BadRequest("No seats selected.");
            }

            string token = HttpContext.Request.Cookies["token"];
            string appUserId = null;
            ClaimsPrincipal claimsPrincipal = null;

            if (!string.IsNullOrEmpty(token))
            {
                var secretKey = "sdfgdf-463dgdfsd j-fdvnji2387nTravel";
                var key = Encoding.ASCII.GetBytes(secretKey);
                var tokenHandler = new JwtSecurityTokenHandler();

                try
                {
                    claimsPrincipal = tokenHandler.ValidateToken(token, new TokenValidationParameters()
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key)
                    }, out SecurityToken validatedToken);

                    appUserId = claimsPrincipal?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
                }
                catch (SecurityTokenException)
                {
                    return Unauthorized("Invalid token.");
                }
            }

            if (!string.IsNullOrEmpty(appUserId))
            {
                var orderItems = new List<OrderItemCreateVM>();

                foreach (var seatId in SelectedSeats)
                {
                    var bookingCreateDto = new BookingCreateVM
                    {
                        AppUserId = appUserId,
                        FlightId = flightId,
                        SeatId = seatId,
                        BookingNumber = Guid.NewGuid().ToString().Substring(0, 10),
                        CreatedTime = DateTime.Now
                    };

                    //var bookingResponse = await _crudService.Create("/bookings", bookingCreateDto);

                    //orderItems.Add(new OrderItemCreateVM
                    //{
                    //    BookingId = bookingResponse.I,
                    //    Price = bookingResponse.SeatPrice
                    //});
                }

                var orderCreateVm = new OrderCreateVM
                {
                    AppUserId = appUserId,
                    TotalAmount = orderItems.Sum(x => x.Price),
                    OrderItems = orderItems
                };

                TempData["OrderCreateVM"] = JsonSerializer.Serialize(orderCreateVm);

                return RedirectToAction("CreateOrder", "Order");
            }

            return BadRequest("Failed to proceed with the booking.");
        }


    }
}

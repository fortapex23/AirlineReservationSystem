using Microsoft.AspNetCore.Mvc;
using TravelProgram.MVC.Controllers;
using TravelProgram.MVC.Services.Interfaces;
using TravelProgram.MVC.ViewModels.AirportVMs;
using TravelProgram.MVC.ViewModels.AuthVMs;
using TravelProgram.MVC.ViewModels.BookingVMs;
using TravelProgram.MVC.ViewModels.FlightVMs;
using TravelProgram.MVC.ViewModels.PlaneVMs;
using TravelProgram.MVC.ViewModels.SeatVM;

namespace TravelProgram.MVC.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BookingController : BaseController
    {
        private readonly ICrudService _crudService;

        public BookingController(ICrudService crudService)
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

            var bookings = await _crudService.GetAllAsync<List<BookingGetVM>>("/bookings");

            var flights = await _crudService.GetAllAsync<List<FlightGetVM>>("/flights");

            var seats = await _crudService.GetAllAsync<List<SeatGetVM>>("/seats");

            var users = await _crudService.GetAllAsync<List<AuthGetVM>>("/Auth");

            foreach (var book in bookings)
            {
                var bUserName = users.FirstOrDefault(x => x.AppUserId == book.AppUserId);
                var bookFlghtNum = flights.FirstOrDefault(x => x.Id == book.FlightId);
                var bookSeatNum = seats.FirstOrDefault(x => x.Id == book.SeatId);

                book.SeatNumber = bookSeatNum.SeatNumber;
                book.AppUserName = bUserName?.FullName ?? "Unknown";
                book.FlightNumber = bookFlghtNum.FlightNumber;
            }

            return View(bookings);
        }

        

    }
}

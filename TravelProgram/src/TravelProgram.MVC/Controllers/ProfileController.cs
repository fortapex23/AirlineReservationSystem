using Microsoft.AspNetCore.Mvc;
using TravelProgram.MVC.Services.Interfaces;
using TravelProgram.MVC.ViewModels.AirportVMs;
using TravelProgram.MVC.ViewModels.AuthVMs;
using TravelProgram.MVC.ViewModels.BookingVMs;
using TravelProgram.MVC.ViewModels.FlightVMs;
using TravelProgram.MVC.ViewModels.OrderVMs;
using TravelProgram.MVC.ViewModels.SeatVM;

namespace TravelProgram.MVC.Controllers
{
    public class ProfileController : BaseController
    {
        private readonly ICrudService _crudService;

        public ProfileController(ICrudService crudService)
        {
            _crudService = crudService;
        }

        public async Task<IActionResult> Index()
        {
            SetFullName();

            if (ViewBag.FullName is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var id = ViewBag.Id;

            var user = await _crudService.GetByStringIdAsync<AuthGetVM>($"/auth/{id}", id);

            return View(user);
        }

        public async Task<IActionResult> Bookings()
        {
            SetFullName();

            if (ViewBag.FullName is null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var id = ViewBag.Id;

            var user = await _crudService.GetByStringIdAsync<AuthGetVM>($"/auth/{id}", id);

            var bookings = await _crudService.GetAllAsync<List<BookingGetVM>>("/bookings");

            var userBookings = bookings.Where(b => b.AppUserId == id).ToList();

            var airports = await _crudService.GetAllAsync<List<AirportGetVM>>("/airports");

            var flights = await _crudService.GetAllAsync<List<FlightGetVM>>("/flights");

            var seats = await _crudService.GetAllAsync<List<SeatGetVM>>("/seats");

            foreach (var item in userBookings)
            {
                var flight = flights.FirstOrDefault(x => x.Id == item.FlightId);
				var arrAirport = airports.FirstOrDefault(x => x.Id == flight.ArrivalAirportId);
                var seat = seats.FirstOrDefault(x => x.Id == item.SeatId);

                flight.ArrivalAirportCity = arrAirport.City.ToString();
                item.SeatNumber = seat.SeatNumber;
                item.SeatClass = seat.ClassType.ToString();
                item.Destination = flight.ArrivalAirportCity;
            }

            return View(userBookings);
        }

        public async Task<IActionResult> Orders()
        {
            SetFullName();

            if (ViewBag.FullName is null)
            {
                return RedirectToAction("Login", "Auth");
            }
            var id = ViewBag.Id;

            var orders = await _crudService.GetAllAsync<List<OrderGetVM>>("/orders");

            var userOrders = orders.Where(b => b.AppUserId == id).ToList();

            return View(userOrders);
        }

        public async Task<IActionResult> EditProfile()
        {
            SetFullName();

            if (ViewBag.FullName is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            var id = ViewBag.Id;
            var user = await _crudService.GetByStringIdAsync<AuthEditVM>($"/auth/{id}", id);

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditProfile(AuthEditVM vm)
        {
            SetFullName();

            if (ViewBag.FullName is null)
            {
                return RedirectToAction("Login", "Auth");
            }

            if (!ModelState.IsValid)
            {
                return View(vm);
            }

            var id = ViewBag.Id;

            await _crudService.Update($"/auth/{id}", vm);

            return RedirectToAction("Index");
        }


    }
}

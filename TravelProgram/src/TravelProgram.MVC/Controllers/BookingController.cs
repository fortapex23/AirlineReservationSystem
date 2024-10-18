using Microsoft.AspNetCore.Mvc;
using TravelProgram.MVC.ApiResponseMessages;
using TravelProgram.MVC.Enums;
using TravelProgram.MVC.Services.Implementations;
using TravelProgram.MVC.Services.Interfaces;
using TravelProgram.MVC.ViewModels.BookingVMs;

namespace TravelProgram.MVC.Controllers
{
    public class BookingController : BaseController
    {
        private readonly ICrudService _crudService;

        public BookingController(ICrudService crudService)
        {
            _crudService = crudService;
        }

        public IActionResult Index()
        {
            SetFullName();

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> BookSeats(int FlightId, SeatClassType SeatClassType, List<int> SelectedSeats)
        {
            if (SelectedSeats == null || SelectedSeats.Count == 0)
            {
                return BadRequest("No seats selected.");
            }

            foreach (var seatId in SelectedSeats)
            {
                var bookingCreateDto = new BookingCreateVM
                {
                    FlightId = FlightId,
                    SeatId = seatId,
                    BookingNumber = GenerateBookingNumber(),
                    CreatedTime = DateTime.Now
                };

                try
                {
                    await _crudService.Create("/bookings", bookingCreateDto);
                }
                catch (Exception ex)
                {
                    return BadRequest($"Failed to book seat {seatId}: {ex.Message}");
                }
            }

            return RedirectToAction("Index", "Booking");
        }

        [HttpGet]
        public async Task<IActionResult> GetBookingById(int id)
        {
            try
            {
                var booking = await _crudService.GetByIdAsync<BookingGetVM>("/bookings", id);
                return View(booking);
            }
            catch (Exception ex)
            {
                return NotFound($"Booking not found: {ex.Message}");
            }
        }

        //[HttpDelete]
        //public async Task<IActionResult> DeleteBooking(int id)
        //{
        //    try
        //    {
        //        await _crudService.Delete<object>("/bookings", id);
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"Error deleting booking: {ex.Message}");
        //    }
        //}

        //[HttpPut]
        //public async Task<IActionResult> UpdateBooking(int id, BookingUpdateDto dto)
        //{
        //    try
        //    {
        //        await _crudService.Update("/bookings", dto);
        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest($"Error updating booking: {ex.Message}");
        //    }
        //}

        private string GenerateBookingNumber()
        {
            return Guid.NewGuid().ToString();
        }
    }
}

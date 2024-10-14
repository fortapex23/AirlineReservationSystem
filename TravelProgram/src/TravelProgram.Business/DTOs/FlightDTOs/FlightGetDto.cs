using Microsoft.AspNetCore.Http;
using TravelProgram.Business.DTOs.BookingDTOs;
using TravelProgram.Business.DTOs.SeatDTOs;

namespace TravelProgram.Business.DTOs.FlightDTOs
{
	public record FlightGetDto(int Id, int FlightNumber, int DepartureAirportId, int ArrivalAirportId,
							int PlaneId, decimal SeatPrice, DateTime DepartureTime, DateTime ArrivalTime,
							ICollection<BookingGetDto> Bookings, ICollection<SeatGetDto> Seats);
}

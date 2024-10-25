using TravelProgram.Business.DTOs.FlightDTOs;
using TravelProgram.Core.Enum;

namespace TravelProgram.Business.DTOs.BookingDTOs
{
	public record BookingGetDto(int Id, int FlightId, string AppUserId, string BookingNumber,
			int SeatId, BookStatus Status, DateTime CreatedTime, DateTime UpdatedTime);

}

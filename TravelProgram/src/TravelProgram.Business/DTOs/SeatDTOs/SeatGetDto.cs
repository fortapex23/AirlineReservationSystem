using TravelProgram.Core.Enum;

namespace TravelProgram.Business.DTOs.SeatDTOs
{
	public record SeatGetDto(int Id, int PlaneId, int FlightId, int Price, int SeatNumber, SeatClassType ClassType, bool IsAvailable);
}

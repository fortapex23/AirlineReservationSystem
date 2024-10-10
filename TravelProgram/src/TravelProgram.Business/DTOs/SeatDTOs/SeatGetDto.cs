using TravelProgram.Core.Enum;

namespace TravelProgram.Business.DTOs.SeatDTOs
{
	public record SeatGetDto(int Id, int PlaneId, int FlightId, int SeatNumber, SeatClassType ClassType, bool IsAvailable);
}

using TravelProgram.Business.DTOs.FlightDTOs;
using TravelProgram.Business.DTOs.SeatDTOs;

namespace TravelProgram.Business.DTOs.PlaneDTOs
{
	public record PlaneGetDto(int Id, string Name, string TotalSeats, int AirlineId, 
				ICollection<FlightGetDto> Flights, ICollection<SeatGetDto> Seats);
}

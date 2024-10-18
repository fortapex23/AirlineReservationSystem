using TravelProgram.Business.DTOs.FlightDTOs;
using TravelProgram.Business.DTOs.SeatDTOs;

namespace TravelProgram.Business.DTOs.PlaneDTOs
{
	public record PlaneGetDto(int Id, string Name, int EconomySeats, int BusinessSeats, int AirlineId, 
				ICollection<FlightGetDto> Flights, ICollection<SeatGetDto> Seats);
}

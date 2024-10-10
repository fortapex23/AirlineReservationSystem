using TravelProgram.Business.DTOs.FlightDTOs;
using TravelProgram.Core.Enum;

namespace TravelProgram.Business.DTOs.AirportDTOs
{
	public record AirportGetDto(int Id, string Name, AiportCities City, ICollection<FlightGetDto> DepartingFlights, 
					ICollection<FlightGetDto> ArrivingFlights);
}

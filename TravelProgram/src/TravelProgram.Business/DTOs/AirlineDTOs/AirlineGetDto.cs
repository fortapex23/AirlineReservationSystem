using TravelProgram.Business.DTOs.FlightDTOs;
using TravelProgram.Business.DTOs.PlaneDTOs;
using TravelProgram.Core.Enum;

namespace TravelProgram.Business.DTOs.AirlineDTOs
{
	public record AirlineGetDto(int Id, string Name, Countries Country, ICollection<PlaneGetDto> Planes, 
				ICollection<FlightGetDto> Flights);
}

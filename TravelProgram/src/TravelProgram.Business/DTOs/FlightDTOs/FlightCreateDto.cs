using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace TravelProgram.Business.DTOs.FlightDTOs
{
	public record FlightCreateDto(int FlightNumber, int DepartureAirportId, int ArrivalAirportId, int AirlineId, 
							int PlaneId, decimal SeatPrice, DateTime DepartureTime, DateTime ArrivalTime);
	public class FlightCreateDtoValidator : AbstractValidator<FlightCreateDto>
	{
		public FlightCreateDtoValidator()
		{
			RuleFor(x=>x.FlightNumber).NotNull().NotEmpty();

			RuleFor(x=>x.DepartureAirportId).NotNull().NotEmpty();

			RuleFor(x=>x.ArrivalAirportId).NotNull().NotEmpty();

			RuleFor(x=>x.AirlineId).NotNull().NotEmpty();

			RuleFor(x=>x.SeatPrice).NotNull().NotEmpty();

			RuleFor(x=>x.PlaneId).NotNull().NotEmpty();

			RuleFor(x => x.DepartureTime).NotNull().NotEmpty()
				.GreaterThan(DateTime.Now).WithMessage("Departure time must be greater than the current time.");

			RuleFor(x => x.ArrivalTime).NotNull().NotEmpty()
				.GreaterThan(x => x.DepartureTime).WithMessage("Arrival time must be greater than departure time.");
		}
	}
}

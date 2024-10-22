using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace TravelProgram.Business.DTOs.FlightDTOs
{
	public record FlightUpdateDto(string FlightNumber, int DepartureAirportId, int ArrivalAirportId,
							int PlaneId, decimal EconomySeatPrice, decimal BusinessSeatPrice, DateTime DepartureTime, DateTime ArrivalTime);
	public class FlightUpdateDtoValidator : AbstractValidator<FlightUpdateDto>
	{
		public FlightUpdateDtoValidator()
		{
			RuleFor(x => x.FlightNumber).NotNull().NotEmpty();

			RuleFor(x => x.DepartureAirportId).NotNull().NotEmpty();

			RuleFor(x => x.ArrivalAirportId).NotNull().NotEmpty();

			RuleFor(x => x.EconomySeatPrice).NotNull().NotEmpty();

			RuleFor(x => x.BusinessSeatPrice).NotNull().NotEmpty();

            RuleFor(x => x.PlaneId).NotNull().NotEmpty();

			RuleFor(x => x.DepartureTime).NotNull().NotEmpty()
				.GreaterThan(DateTime.Now).WithMessage("Departure time must be greater than the current time.");

			RuleFor(x => x.ArrivalTime).NotNull().NotEmpty()
				.GreaterThan(x => x.DepartureTime).WithMessage("Arrival time must be greater than departure time.");
		}
	}
}

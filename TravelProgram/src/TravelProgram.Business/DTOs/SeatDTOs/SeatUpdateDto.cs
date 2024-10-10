using FluentValidation;
using TravelProgram.Core.Enum;

namespace TravelProgram.Business.DTOs.SeatDTOs
{
	public record SeatUpdateDto(int PlaneId, int? FlightId, int SeatNumber, SeatClassType ClassType, bool IsAvailable);

	public class SeatUpdateDtoValidator : AbstractValidator<SeatUpdateDto>
	{
		public SeatUpdateDtoValidator()
		{
			RuleFor(x => x.PlaneId).NotNull().NotEmpty();

			RuleFor(x => x.FlightId).NotEmpty();

			RuleFor(x => x.SeatNumber).NotNull().NotEmpty();

			RuleFor(x => x.ClassType).NotNull();

			RuleFor(x => x.IsAvailable).NotNull();
		}
	}

}

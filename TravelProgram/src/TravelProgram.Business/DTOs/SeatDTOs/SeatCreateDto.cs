using FluentValidation;
using TravelProgram.Core.Enum;

namespace TravelProgram.Business.DTOs.SeatDTOs
{
	public record SeatCreateDto(int FlightId, decimal Price, int SeatNumber, SeatClassType ClassType, bool IsAvailable);

	public class SeatCreateDtoValidator : AbstractValidator<SeatCreateDto>
	{
        public SeatCreateDtoValidator()
        {
            //RuleFor(x=>x.PlaneId).NotNull().NotEmpty();

            RuleFor(x=>x.Price).NotNull().NotEmpty();

            RuleFor(x=>x.FlightId).NotNull().NotEmpty();

            RuleFor(x=>x.SeatNumber).NotNull().NotEmpty();

            RuleFor(x=>x.ClassType).NotNull();

            RuleFor(x=>x.IsAvailable).NotNull();
		}
	}
}

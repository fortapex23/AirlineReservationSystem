using FluentValidation;

namespace TravelProgram.Business.DTOs.PlaneDTOs
{
	public record PlaneUpdateDto(string Name, int TotalSeats, int AirlineId);

	public class PlaneUpdateDtoValidator : AbstractValidator<PlaneUpdateDto>
	{
		public PlaneUpdateDtoValidator()
		{
			RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(100);

			RuleFor(x => x.TotalSeats).NotNull().NotEmpty();

			RuleFor(x => x.AirlineId).NotNull().NotEmpty();
		}
	}
}

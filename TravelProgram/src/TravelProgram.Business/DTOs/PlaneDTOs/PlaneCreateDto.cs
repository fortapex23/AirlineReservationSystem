using FluentValidation;

namespace TravelProgram.Business.DTOs.PlaneDTOs
{
	public record PlaneCreateDto(string Name, int TotalSeats, int AirlineId);

	public class PlaneCreateDtoValidator : AbstractValidator<PlaneCreateDto>
	{
		public PlaneCreateDtoValidator()
		{
			RuleFor(x=>x.Name).NotNull().NotEmpty().MaximumLength(100);

			RuleFor(x=>x.TotalSeats).NotNull().NotEmpty();

			RuleFor(x=>x.AirlineId).NotNull().NotEmpty();
		}
	}
}

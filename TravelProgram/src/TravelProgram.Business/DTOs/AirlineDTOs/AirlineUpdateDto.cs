using FluentValidation;
using TravelProgram.Core.Enum;

namespace TravelProgram.Business.DTOs.AirlineDTOs
{
	public record AirlineUpdateDto(string Name, Countries Country);

	public class AirlineUpdateDtoValidator : AbstractValidator<AirlineUpdateDto>
	{
		public AirlineUpdateDtoValidator()
		{
			RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(100);

			RuleFor(x => x.Country).NotNull();
		}
	}
}

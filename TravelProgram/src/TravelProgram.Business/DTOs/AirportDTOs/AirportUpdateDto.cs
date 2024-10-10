using FluentValidation;
using TravelProgram.Core.Enum;

namespace TravelProgram.Business.DTOs.AirportDTOs
{
	public record AirportUpdateDto(string Name, AiportCities City);

	public class AirportUpdateDtoValidator : AbstractValidator<AirportUpdateDto>
	{
		public AirportUpdateDtoValidator()
		{
			RuleFor(x => x.Name).NotNull().NotEmpty().MaximumLength(100);

			RuleFor(x => x.City).NotNull();
		}
	}
}

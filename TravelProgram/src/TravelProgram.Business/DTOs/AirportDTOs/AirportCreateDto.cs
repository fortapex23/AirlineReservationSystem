using FluentValidation;
using TravelProgram.Core.Enum;

namespace TravelProgram.Business.DTOs.AirportDTOs
{
	public record AirportCreateDto(string Name, AiportCities City);

	public class AirportCreateDtoValidator : AbstractValidator<AirportCreateDto>
	{
		public AirportCreateDtoValidator()
		{
			RuleFor(x=>x.Name).NotNull().NotEmpty().MaximumLength(100);

			RuleFor(x => x.City).NotNull();
		}
	}
}

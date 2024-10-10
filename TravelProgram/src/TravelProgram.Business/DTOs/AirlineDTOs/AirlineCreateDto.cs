using FluentValidation;
using TravelProgram.Core.Enum;

namespace TravelProgram.Business.DTOs.AirlineDTOs
{
	public record AirlineCreateDto(string Name, Countries Country);

	public class AirlineCreateDtoValidator : AbstractValidator<AirlineCreateDto>
	{
		public AirlineCreateDtoValidator()
		{
			RuleFor(x=>x.Name).NotNull().NotEmpty().MaximumLength(100);

			RuleFor(x=>x.Country).NotNull();
		}
	}
}

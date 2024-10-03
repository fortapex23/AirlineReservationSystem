using FluentValidation;
using TravelProgram.Core.Enum;

namespace TravelProgram.Business.DTOs.UserDTOs
{
	public record UserRegisterDto(string FullName, string Email, string Password, string PassportNumber,
		string ConfirmPassword, string PhoneNumber, DateTime BirthDate, GenderType Gender);

	public class UserRegisterDtoValidator : AbstractValidator<UserRegisterDto>
	{
        public UserRegisterDtoValidator()
        {
			RuleFor(x => x.FullName).NotNull().NotEmpty().MaximumLength(60);

			RuleFor(x => x.Password).MinimumLength(8).MaximumLength(40);

			RuleFor(x => x.PassportNumber).NotNull().NotEmpty().MaximumLength(100);

			RuleFor(x => x.Email).NotNull().NotEmpty();

			RuleFor(x => x.BirthDate).NotEmpty().WithMessage("cant be empty")
				.GreaterThan(x => DateTime.Now).WithMessage("wrong birthdate");



			RuleFor(x => x).Custom((x, context) =>
				{
					if (x.Password != x.ConfirmPassword)
					{
						context.AddFailure("ConfirmPassword", "pw and confirm pw dont match");
					}
				});
		}
	}
}

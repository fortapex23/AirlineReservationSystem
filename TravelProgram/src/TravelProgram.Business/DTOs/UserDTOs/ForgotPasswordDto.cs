using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;

namespace TravelProgram.Business.DTOs.UserDTOs
{
	public record ForgotPasswordDto(string Email, string FullName, string Password, string ConfirmPassword);

	public class ForgotPasswordDtoValidator : AbstractValidator<ForgotPasswordDto>
	{
        public ForgotPasswordDtoValidator()
        {
			RuleFor(x => x.Password).MinimumLength(8).MaximumLength(40);


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

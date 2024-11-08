using System.ComponentModel.DataAnnotations;

namespace TravelProgram.MVC.ViewModels.AuthVMs
{
	public class ForgotPasswordVM
	{
        public string FullName { get; set; }

        public string Email { get; set; }

        public string Password { get; set; }

        [Compare("Password")]
        public string ConfirmPassword { get; set; }
	}
}

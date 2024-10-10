using System.ComponentModel.DataAnnotations;
using TravelProgram.MVC.Enums;

namespace TravelProgram.MVC.ViewModels.AuthVMs
{
    public class UserRegisterVM
    {
        [Required]
        public string FullName { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        public DateTime BirthDate { get; set; }
        [Required]
        public GenderType Gender { get; set; }
        [Required]
        public string PassportNumber { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        [MinLength(8, ErrorMessage = "Password length must be > 8")]
        public string Password { get; set; }
        [Required]
        [Compare("Password", ErrorMessage = "The password and confrim password dont match.")]
        public string ConfirmPassword { get; set; }
    }
}

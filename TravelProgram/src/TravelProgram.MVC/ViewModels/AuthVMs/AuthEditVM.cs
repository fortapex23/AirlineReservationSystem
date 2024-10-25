using TravelProgram.MVC.Enums;

namespace TravelProgram.MVC.ViewModels.AuthVMs
{
    public record AuthEditVM(string FullName, string Email, string PassportNumber, string PhoneNumber,
                            DateTime BirthDate, GenderType Gender);
}

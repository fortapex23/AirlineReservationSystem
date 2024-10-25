using TravelProgram.MVC.Enums;
using TravelProgram.MVC.ViewModels.BookingVMs;

namespace TravelProgram.MVC.ViewModels.AuthVMs
{
    public record AuthGetVM(string AppUserId, string FullName, string Email, string PassportNumber, string PhoneNumber,
                            DateTime BirthDate, GenderType Gender, ICollection<BookingGetVM> Bookings);
}

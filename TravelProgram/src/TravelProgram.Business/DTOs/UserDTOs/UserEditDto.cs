using TravelProgram.Core.Enum;

namespace TravelProgram.Business.DTOs.UserDTOs
{
    public record UserEditDto(string FullName, string Email, string PassportNumber, string PhoneNumber,
                                DateTime BirthDate, GenderType Gender);
}

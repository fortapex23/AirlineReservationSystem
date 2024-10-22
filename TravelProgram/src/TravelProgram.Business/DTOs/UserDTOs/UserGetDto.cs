using TravelProgram.Core.Enum;

namespace TravelProgram.Business.DTOs.UserDTOs
{
    public record UserGetDto(string AppUserId, string FullName, string Email, string PassportNumber, string PhoneNumber, 
                            DateTime BirthDate, GenderType Gender);
}

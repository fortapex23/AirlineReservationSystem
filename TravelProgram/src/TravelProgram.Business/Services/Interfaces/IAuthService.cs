using TravelProgram.Business.DTOs.TokenDTOs;
using TravelProgram.Business.DTOs.UserDTOs;

namespace TravelProgram.Business.Services.Interfaces
{
	public interface IAuthService
	{
		Task Register(UserRegisterDto dto);
		Task<TokenResponseDto> Login(UserLoginDto dto);
		Task<TokenResponseDto> AdminLogin(UserLoginDto dto);
        Task<ICollection<UserGetDto>> GetAllUsersAsync();
        Task<UserGetDto> GetById(string id);
        Task UpdateUserAsync(string id, UserEditDto dto);
		Task ForgotPassword(ForgotPasswordDto dto);

		//Task<TokenResponseDto> LoginWithCode(UserLoginWithCodeDto dto);
	}
}

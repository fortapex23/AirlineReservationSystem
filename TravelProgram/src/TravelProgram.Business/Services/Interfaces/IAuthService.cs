using TravelProgram.Business.DTOs.TokenDTOs;
using TravelProgram.Business.DTOs.UserDTOs;

namespace TravelProgram.Business.Services.Interfaces
{
	public interface IAuthService
	{
		Task Register(UserRegisterDto dto);

		//Task<TokenResponseDto> LoginWithCode(UserLoginWithCodeDto dto);
	}
}

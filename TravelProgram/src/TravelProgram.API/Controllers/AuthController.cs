using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TravelProgram.Business.DTOs.UserDTOs;
using TravelProgram.Business.Services.Interfaces;

namespace TravelProgram.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;

		public AuthController(IAuthService authService)
        {
			_authService = authService;
		}

		public async Task<IActionResult> Register(UserRegisterDto dto)
		{
			try
			{
				await _authService.Register(dto);
			}
			catch (NullReferenceException)
			{
				return BadRequest();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}

			return Ok();
		}

  //      [HttpPost("login-with-code")]
		//public async Task<IActionResult> LoginWithCode(UserLoginWithCodeDto dto)
		//{
		//	try
		//	{
		//		var token = await _authService.LoginWithCode(dto);
		//		return Ok(token);
		//	}
		//	catch (Exception ex)
		//	{
		//		return BadRequest(ex.Message);
		//	}
		//}

	}
}

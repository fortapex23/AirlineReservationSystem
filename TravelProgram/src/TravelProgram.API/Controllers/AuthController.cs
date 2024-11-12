using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravelProgram.API.ApiResponses;
using TravelProgram.Business.DTOs.AirportDTOs;
using TravelProgram.Business.DTOs.TokenDTOs;
using TravelProgram.Business.DTOs.UserDTOs;
using TravelProgram.Business.Services.Implementations;
using TravelProgram.Business.Services.Interfaces;
using TravelProgram.Core.Models;

namespace TravelProgram.API.Controllers
{
    [Route("api/[controller]")]
	[ApiController]
	public class AuthController : ControllerBase
	{
		private readonly IAuthService _authService;
		private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<AppUser> _userManager;

        public AuthController(IAuthService authService, RoleManager<IdentityRole> roleManager, UserManager<AppUser> userManager)
		{
			_authService = authService;
			_roleManager = roleManager;
            _userManager = userManager;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(new ApiResponse<ICollection<UserGetDto>>
            {
                Data = await _authService.GetAllUsersAsync(),
                ErrorMessage = null,
                StatusCode = StatusCodes.Status200OK
            });
        }

		[HttpPost("[action]")]
		public async Task<IActionResult> ForgotPassword(ForgotPasswordDto dto)
		{
			try
			{
				await _authService.ForgotPassword(dto);

				return Ok(new ApiResponse<string>
				{
					Data = "Password reset successfully",
					ErrorMessage = null,
					StatusCode = StatusCodes.Status200OK
				});
			}
			catch (Exception ex)
			{
				return BadRequest(new ApiResponse<string>
				{
					Data = null,
					ErrorMessage = ex.Message,
					StatusCode = StatusCodes.Status400BadRequest
				});
			}
		}

		[HttpGet("{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            UserGetDto dto = null;
            try
            {
				dto = await _authService.GetById(id);
            }
            catch (Exception ex)
            {
                return NotFound(ex.Message);
            }

            return Ok(new ApiResponse<UserGetDto>
            {
                Data = dto,
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, UserEditDto dto)
        {
            try
            {
                await _authService.UpdateUserAsync(id, dto);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse<UserEditDto>
                {
                    StatusCode = StatusCodes.Status400BadRequest,
                    ErrorMessage = ex.Message,
                    Data = null
                });
            }
            return NoContent();
        }

        [HttpPost("[action]")]
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

		[HttpPost("[action]")]
		public async Task<IActionResult> Login(UserLoginDto dto)
		{
			TokenResponseDto rDto = null;

			try
			{
				rDto = await _authService.Login(dto);
			}
            catch (Exception ex) when (ex.Message == "Invalid credentials")
            {
                return BadRequest(new ApiResponse<string>
                {
                    Data = null,
                    ErrorMessage = "Invalid email or password",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            catch (Exception)
            {
                return BadRequest("An error occurred");
            }

            return Ok(rDto);
		}

        [HttpPost("[action]")]
        public async Task<IActionResult> AdminLogin(UserLoginDto dto)
        {
            TokenResponseDto rDto = null;
            try
            {
                rDto = await _authService.AdminLogin(dto);
                return Ok(rDto);
            }
            catch (Exception ex) when (ex.Message == "Invalid credentials")
            {
                return BadRequest(new ApiResponse<string>
                {
                    Data = null,
                    ErrorMessage = "Invalid email or password",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            catch (Exception)
            {
                return BadRequest("An error occurred");
            }
        }

		//[HttpGet("")]
		//public async Task<IActionResult> CreateAdmin()
		//{
		//	AppUser appUser = await _userManager.FindByEmailAsync("admin@gmail.com");

		//	await _userManager.AddToRoleAsync(appUser, "Admin");

		//	return Ok();
		//}


		//[HttpGet("")]
		//public async Task<IActionResult> CreateRole()
		//{
		//	IdentityRole role2 = new IdentityRole("Admin");
		//	IdentityRole role3 = new IdentityRole("Member");

		//	await _roleManager.CreateAsync(role2);
		//	await _roleManager.CreateAsync(role3);

		//	return Ok();
		//}

	}
}

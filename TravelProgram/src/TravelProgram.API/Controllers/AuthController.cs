using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TravelProgram.Business.DTOs.TokenDTOs;
using TravelProgram.Business.DTOs.UserDTOs;
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
			catch (NullReferenceException)
			{
				return BadRequest();
			}
			catch (Exception)
			{
				return BadRequest();
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
            catch (NullReferenceException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return BadRequest();
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

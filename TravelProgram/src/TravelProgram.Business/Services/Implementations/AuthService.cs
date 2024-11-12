using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TravelProgram.Business.DTOs.TokenDTOs;
using TravelProgram.Business.DTOs.UserDTOs;
using TravelProgram.Business.Services.Interfaces;
using TravelProgram.Core.Models;

namespace TravelProgram.Business.Services.Implementations
{
    public class AuthService : IAuthService
	{
		private readonly UserManager<AppUser> _userManager;
		private readonly SignInManager<AppUser> _signInManager;
		private readonly IConfiguration _configuration;

		public AuthService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IConfiguration configuration)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_configuration = configuration;
		}

		public async Task ForgotPassword(ForgotPasswordDto dto)
		{
			if (dto.Password != dto.ConfirmPassword)
			{
				throw new Exception("New password and confirpassword do not match");
			}

			var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);

			if (user == null)
			{
				throw new NullReferenceException("User email does not exist");
			}

			if (!string.Equals(user.FullName.ToLower().Trim(), dto.FullName.ToLower().Trim()))
			{
				throw new Exception("No suchh a fullname");
			}

			var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);

			var result = await _userManager.ResetPasswordAsync(user, resetToken, dto.Password);

			if (!result.Succeeded)
			{
				throw new Exception("Failed to reset password");
			}
		}

		public async Task<ICollection<UserGetDto>> GetAllUsersAsync()
        {
            var users = await _userManager.Users.ToListAsync();

            var userDtos = users.Select(user => new UserGetDto(
                user.Id,
                user.FullName,
                user.Email,
                user.PassportNumber,
                user.PhoneNumber,
                user.BirthDate,
                user.Gender
            )).ToList();

            return userDtos;
        }

        public async Task UpdateUserAsync(string id, UserEditDto dto)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is null)
            {
                throw new NullReferenceException($"{id} not found.");
            }

            user.FullName = dto.FullName;
            user.Email = dto.Email;
            user.PassportNumber = dto.PassportNumber;
            user.PhoneNumber = dto.PhoneNumber;
            user.BirthDate = dto.BirthDate;
            user.Gender = dto.Gender;

			if(user.BirthDate > DateTime.Now)
			{
				throw new Exception("Invalid date time");
			}

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
			{
                throw new Exception($"Failed to update");
            }
        }


        public async Task<UserGetDto> GetById(string id)
        {
            var user = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id.ToString());

            if (user == null)
            {
                throw new NullReferenceException($"{id} not found");
            }

            var userDto = new UserGetDto(
                user.Id,
                user.FullName,
                user.Email,
                user.PassportNumber,
                user.PhoneNumber,
                user.BirthDate,
                user.Gender
            );

            return userDto;
        }

        public async Task<TokenResponseDto> Login(UserLoginDto dto)
		{
			AppUser appUser = null;

			appUser = await _userManager.FindByEmailAsync(dto.Email);

			if (appUser == null)
			{
				throw new Exception("Invalid credentials");
			}

			var result = await _signInManager.CheckPasswordSignInAsync(appUser, dto.Password, dto.RememberMe);

			if (!result.Succeeded)
			{
				throw new Exception("Invalid credentials");
			}

			var roles = await _userManager.GetRolesAsync(appUser);

			List<Claim> claims = new List<Claim>()
			{
				new Claim(ClaimTypes.NameIdentifier, appUser.Id),
				new Claim(ClaimTypes.Name, appUser.FullName),
			};

			claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));
			DateTime expiredt = DateTime.UtcNow.AddHours(1);
			string secretkey = _configuration.GetSection("JWT:secretKey").Value;

			SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey));
			SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

			JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
				signingCredentials: signingCredentials,
				claims: claims,
				audience: _configuration.GetSection("JWT:audience").Value,
				issuer: _configuration.GetSection("JWT:issuer").Value,
				expires: expiredt,
				notBefore: DateTime.UtcNow
				);

			string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

			return new TokenResponseDto(token, expiredt);
		}

		public async Task Register(UserRegisterDto dto)
		{
			AppUser appUser = new AppUser()
			{
				Email = dto.Email,
				BirthDate = dto.BirthDate,
				PassportNumber = dto.PassportNumber,
				FullName = dto.FullName,
				PhoneNumber = dto.PhoneNumber,
				UserName = dto.Email
			};

			if (appUser.BirthDate > DateTime.Now)
				throw new Exception("Invalid birth date");

			var result = await _userManager.CreateAsync(appUser, dto.Password);

			if (!result.Succeeded)
			{
				throw new NullReferenceException();
			}

			var member = await _userManager.FindByEmailAsync(dto.Email);

			if (member is not null)
			{
				await _userManager.AddToRoleAsync(appUser, "Member");
			}
		}

		public async Task<TokenResponseDto> AdminLogin(UserLoginDto dto)
		{
			AppUser appUser = null;

			appUser = await _userManager.FindByEmailAsync(dto.Email);

			if (appUser == null)
			{
				throw new Exception("Invalid credentials");
			}

			var result = await _signInManager.CheckPasswordSignInAsync(appUser, dto.Password, dto.RememberMe);

			if (!result.Succeeded)
			{
				throw new Exception("Invalid credentials");
			}

			var roles = await _userManager.GetRolesAsync(appUser);

			if (!roles.Contains("Admin"))
			{
				throw new Exception("You need to be an Admin to log in");
			}

			List<Claim> claims = new List<Claim>()
			{
				new Claim(ClaimTypes.NameIdentifier, appUser.Id),
				new Claim(ClaimTypes.Name, appUser.FullName),
			};

			if (roles.Contains("Admin"))
			{
				claims.Add(new Claim(ClaimTypes.Role, "Admin"));
			}

			claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));
			DateTime expiredt = DateTime.UtcNow.AddMinutes(30);
			string secretkey = _configuration.GetSection("JWT:secretKey").Value;

			SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey));
			SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

			JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
				signingCredentials: signingCredentials,
				claims: claims,
				audience: _configuration.GetSection("JWT:audience").Value,
				issuer: _configuration.GetSection("JWT:issuer").Value,
				expires: expiredt,
				notBefore: DateTime.UtcNow
				);

			string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

			return new TokenResponseDto(token, expiredt);
		}


    }

}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TravelProgram.Business.DTOs.AirportDTOs;
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
				throw new NullReferenceException();
			}

			var result = await _signInManager.CheckPasswordSignInAsync(appUser, dto.Password, dto.RememberMe);

			if (!result.Succeeded)
			{
				throw new NullReferenceException();
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
				FullName = dto.FullName,
				PhoneNumber = dto.PhoneNumber,
				UserName = dto.Email
			};

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
				throw new NullReferenceException();
			}

			var result = await _signInManager.CheckPasswordSignInAsync(appUser, dto.Password, dto.RememberMe);

			if (!result.Succeeded)
			{
				throw new NullReferenceException();
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


        //public async Task<TokenResponseDto> LoginWithCode(UserLoginWithCodeDto dto)
        //{
        //	AppUser appUser = await _userManager.FindByEmailAsync(dto.Email);

        //	if (appUser == null)
        //	{
        //		throw new NullReferenceException("User not found");
        //	}

        //	var storedCode = await _userManager.GetAuthenticationTokenAsync(appUser, "Default", "LoginCode");

        //	if (storedCode == null || storedCode != dto.Code)
        //	{
        //		throw new Exception("Invalid or expired login code");
        //	}

        //	List<Claim> claims = new List<Claim>()
        //	{
        //		new Claim(ClaimTypes.NameIdentifier, appUser.Id),
        //		new Claim(ClaimTypes.Name, appUser.UserName)
        //	};

        //	var roles = await _userManager.GetRolesAsync(appUser);
        //	claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));

        //	var secretKey = _configuration.GetSection("JWT:secretKey").Value;
        //	var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        //	var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        //	var jwtSecurityToken = new JwtSecurityToken(
        //		signingCredentials: signingCredentials,
        //		claims: claims,
        //		audience: _configuration.GetSection("JWT:audience").Value,
        //		issuer: _configuration.GetSection("JWT:issuer").Value,
        //		expires: DateTime.UtcNow.AddMinutes(10),
        //		notBefore: DateTime.UtcNow
        //	);

        //	string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        //	return new TokenResponseDto(token, DateTime.UtcNow.AddMinutes(10));
        //}

        //private async Task SendLoginCodeEmail(string email, string code)
        //{
        //	var smtpClient = new SmtpClient(_configuration["Email:smtpServer"])
        //	{
        //		Port = int.Parse(_configuration["Email:smtpPort"]),
        //		Credentials = new NetworkCredential(
        //			_configuration["Email:gmail"],
        //			_configuration["Email:password"]),
        //		EnableSsl = bool.Parse(_configuration["Email:enableSsl"]),
        //	};

        //	var mailMessage = new MailMessage
        //	{
        //		From = new MailAddress(_configuration["Email:gmail"]),
        //		Subject = "Your Login Code",
        //		Body = $"Your login code is {code}",
        //		IsBodyHtml = true,
        //	};

        //	mailMessage.To.Add(email);

        //	await smtpClient.SendMailAsync(mailMessage);
        //}


    }

}

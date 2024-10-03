using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;
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

		public async Task Register(UserRegisterDto dto)
		{
			AppUser appUser = new AppUser()
			{
				Email = dto.Email,
				FullName = dto.FullName,
				PhoneNumber = dto.PhoneNumber,
			};

			var result = await _userManager.CreateAsync(appUser, dto.Password);

			if (!result.Succeeded)
			{
				throw new NullReferenceException();
			}

			//var member = await _userManager.FindByNameAsync(dto.FullName);

			//if (member is not null)
			//{
			//	await _userManager.AddToRoleAsync(appUser, "Member");
			//}
		}

		//public async Task<TokenResponseDto> AdminLogin(UserLoginDto dto)
		//{
		//	AppUser appUser = null;

		//	appUser = await _userManager.FindByNameAsync(dto.UserName);

		//	if (appUser == null)
		//	{
		//		throw new NullReferenceException();
		//	}

		//	var result = await _signInManager.CheckPasswordSignInAsync(appUser, dto.Password, dto.RememberMe);

		//	if (!result.Succeeded)
		//	{
		//		throw new NullReferenceException();
		//	}

		//	var roles = await _userManager.GetRolesAsync(appUser);

		//	if (!roles.Contains("Admin") && !roles.Contains("SuperAdmin"))
		//	{
		//		throw new Exception("You need to be an Admin to log in");
		//	}

		//	List<Claim> claims = new List<Claim>()
		//	{
		//		new Claim(ClaimTypes.NameIdentifier, appUser.Id),
		//		new Claim(ClaimTypes.Name, appUser.UserName),
		//	};

		//	if (roles.Contains("SuperAdmin"))
		//	{
		//		claims.Add(new Claim(ClaimTypes.Role, "SuperAdmin"));
		//	}

		//	claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));
		//	DateTime expiredt = DateTime.UtcNow.AddMinutes(30);
		//	string secretkey = _configuration.GetSection("JWT:secretKey").Value;

		//	SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey));
		//	SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

		//	JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
		//		signingCredentials: signingCredentials,
		//		claims: claims,
		//		audience: _configuration.GetSection("JWT:audience").Value,
		//		issuer: _configuration.GetSection("JWT:issuer").Value,
		//		expires: expiredt,
		//		notBefore: DateTime.UtcNow
		//		);

		//	string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

		//	return new TokenResponseDto(token, expiredt);
		//}

		//public async Task<TokenResponseDto> Login(UserLoginDto dto)
		//{
		//	AppUser appUser = null;

		//	appUser = await _userManager.FindByNameAsync(dto.UserName);

		//	if (appUser == null)
		//	{
		//		throw new NullReferenceException();
		//	}

		//	var result = await _signInManager.CheckPasswordSignInAsync(appUser, dto.Password, dto.RememberMe);

		//	if (!result.Succeeded)
		//	{
		//		throw new NullReferenceException();
		//	}

		//	var roles = await _userManager.GetRolesAsync(appUser);

		//	List<Claim> claims = new List<Claim>()
		//	{
		//		new Claim(ClaimTypes.NameIdentifier, appUser.Id),
		//		new Claim(ClaimTypes.Name, appUser.UserName),
		//	};

		//	claims.AddRange(roles.Select(x => new Claim(ClaimTypes.Role, x)));
		//	DateTime expiredt = DateTime.UtcNow.AddMinutes(10);
		//	string secretkey = _configuration.GetSection("JWT:secretKey").Value;

		//	SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretkey));
		//	SigningCredentials signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

		//	JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
		//		signingCredentials: signingCredentials,
		//		claims: claims,
		//		audience: _configuration.GetSection("JWT:audience").Value,
		//		issuer: _configuration.GetSection("JWT:issuer").Value,
		//		expires: expiredt,
		//		notBefore: DateTime.UtcNow
		//		);

		//	string token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

		//	return new TokenResponseDto(token, expiredt);
		//}

		//public async Task Register(UserRegisterDto dto)
		//{
		//	AppUser appUser = new AppUser()
		//	{
		//		Email = dto.Email,
		//		UserName = dto.Email,
		//		PhoneNumber = dto.PhoneNumber,
		//		FullName = dto.FullName,
		//		PassportNumber = dto.PassportNumber,
		//		BirthDate = dto.BirthDate,
		//		Gender = dto.Gender
		//	};

		//	var result = await _userManager.CreateAsync(appUser, dto.Password);

		//	if (!result.Succeeded)
		//	{
		//		throw new NullReferenceException("User creation failed.");
		//	}

		//	var loginCode = new Random().Next(100000, 999999).ToString();

		//	await _userManager.SetAuthenticationTokenAsync(appUser, "Default", "LoginCode", loginCode);

		//	await SendLoginCodeEmail(appUser.Email, loginCode);

		//	await _userManager.AddToRoleAsync(appUser, "Member");
		//}

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
		//	var smtpClient = new SmtpClient("smtp.your-email-provider.com")
		//	{
		//		Port = 587,
		//		Credentials = new NetworkCredential(_configuration.GetSection("Email:gmail").Value, _configuration.GetSection("Email:pw").Value),
		//		EnableSsl = true,
		//	};

		//	var mailMessage = new MailMessage
		//	{
		//		From = new MailAddress(_configuration.GetSection("Email:gmail").Value),
		//		Subject = "Your Login Code",
		//		Body = $"Your login code is {code}",
		//		IsBodyHtml = true,
		//	};

		//	mailMessage.To.Add(email);

		//	await smtpClient.SendMailAsync(mailMessage);
		//}

	}
}

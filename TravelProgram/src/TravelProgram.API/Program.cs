using System.Text;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using TravelProgram.Business;
using TravelProgram.Business.DTOs.AirlineDTOs;
using TravelProgram.Business.DTOs.UserDTOs;
using TravelProgram.Business.MappingProfiles;
using TravelProgram.Core.Models;
using TravelProgram.Data;
using TravelProgram.Data.DAL;

namespace TravelProgram.API
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var builder = WebApplication.CreateBuilder(args);

			builder.Services.AddCors(options =>
			{
				options.AddPolicy("AllowAll",
					builder =>
					{
						builder.AllowAnyOrigin()
							   .AllowAnyMethod()
							   .AllowAnyHeader();
					});
			});

			builder.Services.AddControllers().AddFluentValidation(op =>
			{
				op.RegisterValidatorsFromAssembly(typeof(AirlineCreateDtoValidator).Assembly);
			}).AddNewtonsoftJson(options =>
				options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

			builder.Services.AddAutoMapper(op =>
			{
				op.AddProfile<MapProfile>();
			});

			builder.Services.AddIdentity<AppUser, IdentityRole>(opt =>
			{
				opt.Password.RequiredLength = 8;
				opt.Password.RequireNonAlphanumeric = true;
				opt.Password.RequiredUniqueChars = 1;
				opt.Password.RequireUppercase = true;
				opt.Password.RequireLowercase = true;
				opt.Password.RequireDigit = true;

				opt.User.RequireUniqueEmail = true;
			}).AddEntityFrameworkStores<AppDbContext>().AddDefaultTokenProviders();

			builder.Services.AddAuthentication(opt =>
			{
				opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
				opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
				opt.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
			}).AddJwtBearer(op =>
			{
				op.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
				{
					ValidateIssuer = true,
					ValidateAudience = true,
					ValidIssuer = builder.Configuration.GetSection("JWT:audience").Value,
					ValidAudience = builder.Configuration.GetSection("JWT:issuer").Value,
					ValidateLifetime = true,
					ClockSkew = TimeSpan.Zero,
					IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration.GetSection("JWT:secretKey").Value))
				};
			});

			builder.Services.AddRepositories(builder.Configuration.GetConnectionString("default"));
			builder.Services.AddServices();
			builder.Services.AddEndpointsApiExplorer();
			builder.Services.AddSwaggerGen();

			var app = builder.Build();

			if (app.Environment.IsDevelopment())
			{
				app.UseSwagger();
				app.UseSwaggerUI();
			}

			app.UseCors("AllowAll");
			app.UseHttpsRedirection();
			app.UseAuthentication();
			app.UseAuthorization();


			app.MapControllers();

			app.Run();
		}
	}
}

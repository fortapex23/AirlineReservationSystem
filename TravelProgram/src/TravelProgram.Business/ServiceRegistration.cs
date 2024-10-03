using Microsoft.Extensions.DependencyInjection;
using TravelProgram.Business.Services.Implementations;
using TravelProgram.Business.Services.Interfaces;

namespace TravelProgram.Business
{
	public static class ServiceRegistration
	{
		public static void AddServices(this IServiceCollection services)
		{
			services.AddScoped<IAuthService, AuthService>();
		}
	}
}

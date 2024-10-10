using TravelProgram.MVC.Services.Implementations;
using TravelProgram.MVC.Services.Interfaces;

namespace TravelProgram.MVC
{
    public static class ServiceRegistration
    {
        public static void AddRegisterService(this IServiceCollection services)
        {
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<ICrudService, CrudService>();
        }
    }
}

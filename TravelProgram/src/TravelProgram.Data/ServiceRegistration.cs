using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TravelProgram.Core.Repositories;
using TravelProgram.Data.DAL;
using TravelProgram.Data.Repositories;

namespace TravelProgram.Data
{
	public static class ServiceRegistration
	{
		public static void AddRepositories(this IServiceCollection services, string connectionstring)
		{
			services.AddScoped<IAirlineRepository, AirlineRepository>();
			services.AddScoped<IAirportRepository, AirportRepository>();
			services.AddScoped<IBookingRepository, BookingRepository>();
			services.AddScoped<IFlightRepository, FlightRepository>();
			services.AddScoped<IPlaneRepository, PlaneRepository>();
			services.AddScoped<ISeatRepository, SeatRepository>();

            services.AddDbContext<AppDbContext>(op =>
			{
				op.UseSqlServer(connectionstring);
			});
		}
	}
}

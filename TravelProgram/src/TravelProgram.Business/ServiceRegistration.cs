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
			services.AddScoped<IAirlineService, AirlineService>();
			services.AddScoped<IAirportService, AirportService>();
			services.AddScoped<IPlaneService, PlaneService>();
			services.AddScoped<IFlightService, FlightService>();
			services.AddScoped<ISeatService, SeatService>();
			services.AddScoped<IBookingService, BookingService>();
			services.AddScoped<IOrderService, OrderService>();
			services.AddScoped<IOrderItemService, OrderItemService>();
            services.AddScoped<IBasketItemService, BasketItemService>();
        }
	}
}

using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TravelProgram.Core.Models;
using TravelProgram.Data.Configurations;

namespace TravelProgram.Data.DAL
{
	public class AppDbContext : IdentityDbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<Airline> Airlines { get; set; }
		public DbSet<Airport> Airports { get; set; }
		public DbSet<AppUser> AppUsers { get; set; }
		public DbSet<Booking> Bookings { get; set; }
		public DbSet<Flight> Flights { get; set; }
		public DbSet<Plane> Planes { get; set; }
		public DbSet<Seat> Seats { get; set; }
		public DbSet<BasketItem> BasketItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
		{
			builder.ApplyConfigurationsFromAssembly(typeof(AirlineConfiguration).Assembly);

			base.OnModelCreating(builder);
		}
	}
}

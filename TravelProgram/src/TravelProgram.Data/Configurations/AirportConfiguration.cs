using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelProgram.Core.Models;

namespace TravelProgram.Data.Configurations
{
	public class AirportConfiguration : IEntityTypeConfiguration<Airport>
	{
		public void Configure(EntityTypeBuilder<Airport> builder)
		{
			builder.Property(a => a.Name).IsRequired().HasMaxLength(100);
			builder.Property(a => a.City).IsRequired();

			builder.HasMany(ap => ap.DepartingFlights)
			   .WithOne(f => f.DepartureAirport)
			   .HasForeignKey(f => f.DepartureAirportId)
			   .OnDelete(DeleteBehavior.Restrict);

			builder.HasMany(ap => ap.ArrivingFlights)
				   .WithOne(f => f.ArrivalAirport)
				   .HasForeignKey(f => f.ArrivalAirportId)
				   .OnDelete(DeleteBehavior.Restrict);
		}
	}
}

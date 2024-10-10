using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TravelProgram.Core.Models;

namespace TravelProgram.Data.Configurations
{
	public class BookingConfiguration : IEntityTypeConfiguration<Booking>
	{
		public void Configure(EntityTypeBuilder<Booking> builder)
		{
			builder.Property(b => b.BookingNumber).IsRequired().HasMaxLength(50);
			builder.Property(b => b.Status).IsRequired();
			builder.Property(b => b.SeatId).IsRequired();

			builder.HasOne(b => b.Flight)
				   .WithMany(b => b.Bookings)
				   .HasForeignKey(b => b.FlightId);

			builder.HasOne(b => b.Seat)
				   .WithOne()
				   .HasForeignKey<Booking>(b => b.SeatId);

			builder.HasOne(b => b.AppUser)
				   .WithMany(b => b.Bookings)
				   .HasForeignKey(b => b.AppUserId);
		}
	}
}

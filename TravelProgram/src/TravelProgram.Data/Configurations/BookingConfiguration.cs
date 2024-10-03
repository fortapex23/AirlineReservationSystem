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
			builder.Property(b => b.SeatNumber).IsRequired();
			builder.Property(b => b.Status).IsRequired();

			builder.HasOne(b => b.Flight)
				   .WithMany()
				   .HasForeignKey(b => b.FlightId);

			builder.HasOne(b => b.AppUser)
				   .WithMany()
				   .HasForeignKey(b => b.AppUserId);
		}
	}
}

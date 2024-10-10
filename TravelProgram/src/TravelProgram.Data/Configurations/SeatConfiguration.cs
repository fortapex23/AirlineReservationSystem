using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TravelProgram.Core.Models;

namespace TravelProgram.Data.Configurations
{
	public class SeatConfiguration : IEntityTypeConfiguration<Seat>
	{
		public void Configure(EntityTypeBuilder<Seat> builder)
		{
			builder.HasKey(s => s.Id);

			builder.Property(s => s.SeatNumber).IsRequired();
			builder.Property(s => s.ClassType).IsRequired();
			builder.Property(s => s.IsAvailable).IsRequired();

			builder.HasOne(s => s.Plane)
				   .WithMany(p => p.Seats)
				   .HasForeignKey(s => s.PlaneId)
				   .OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(s => s.Flight)
				   .WithMany(s => s.Seats)
				   .HasForeignKey(s => s.FlightId)
				   .OnDelete(DeleteBehavior.Restrict);
		}
	}
}

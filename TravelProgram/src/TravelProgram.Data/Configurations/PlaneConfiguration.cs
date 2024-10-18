using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TravelProgram.Core.Models;

namespace TravelProgram.Data.Configurations
{
	public class PlaneConfiguration : IEntityTypeConfiguration<Plane>
	{
		public void Configure(EntityTypeBuilder<Plane> builder)
		{
			builder.Property(p => p.Name).IsRequired().HasMaxLength(100);
			builder.Property(p => p.EconomySeats).IsRequired();
			builder.Property(p => p.BusinessSeats).IsRequired();

			builder.HasOne(p => p.Airline)
				   .WithMany(a => a.Planes)
				   .HasForeignKey(p => p.AirlineId);
		}
	}
}

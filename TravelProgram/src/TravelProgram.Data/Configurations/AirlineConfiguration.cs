﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelProgram.Core.Models;

namespace TravelProgram.Data.Configurations
{
	public class AirlineConfiguration : IEntityTypeConfiguration<Airline>
	{
		public void Configure(EntityTypeBuilder<Airline> builder)
		{
			builder.Property(a => a.Name).IsRequired().HasMaxLength(100);
			builder.Property(a => a.Country).IsRequired();

			builder.HasMany(a => a.Planes)
				   .WithOne(p => p.Airline)
				   .HasForeignKey(p => p.AirlineId)
				   .OnDelete(DeleteBehavior.Restrict);

		}
	}
}

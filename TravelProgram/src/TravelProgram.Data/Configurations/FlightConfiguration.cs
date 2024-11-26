﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelProgram.Core.Models;

namespace TravelProgram.Data.Configurations;

public class FlightConfiguration : IEntityTypeConfiguration<Flight>
{
	public void Configure(EntityTypeBuilder<Flight> builder)
	{
		builder.Property(f => f.FlightNumber).IsRequired().HasMaxLength(50);
		builder.Property(f => f.DepartureTime).IsRequired();
		builder.Property(f => f.ArrivalTime).IsRequired();

		builder.HasOne(f => f.DepartureAirport)
			   .WithMany(f => f.DepartingFlights)
			   .HasForeignKey(f => f.DepartureAirportId)
			   .OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(f => f.ArrivalAirport)
			   .WithMany(f => f.ArrivingFlights)
			   .HasForeignKey(f => f.ArrivalAirportId)
			   .OnDelete(DeleteBehavior.Restrict);

		builder.HasOne(f => f.Plane)
			   .WithMany(p => p.Flights)
			   .HasForeignKey(f => f.PlaneId)
			   .OnDelete(DeleteBehavior.Restrict);
	}
}

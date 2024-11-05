using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TravelProgram.Core.Models;

namespace TravelProgram.Data.Configurations
{
    public class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
    {
        public void Configure(EntityTypeBuilder<OrderItem> builder)
        {
            builder.Property(x => x.Price).IsRequired();

            builder.HasOne(x => x.Order)
                   .WithMany(o => o.OrderItems)
                   .HasForeignKey(x => x.OrderId)
                   .OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(oi => oi.Seat)
				   .WithMany(oi => oi.OrderItems)
				   .HasForeignKey(oi => oi.SeatId)
				   .OnDelete(DeleteBehavior.Restrict);
		}
    }
}

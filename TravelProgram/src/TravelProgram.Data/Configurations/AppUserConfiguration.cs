using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using TravelProgram.Core.Models;

namespace TravelProgram.Data.Configurations
{
	public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
	{
		public void Configure(EntityTypeBuilder<AppUser> builder)
		{
			builder.Property(u => u.FullName).IsRequired().HasMaxLength(100);
			builder.Property(u => u.PassportNumber).IsRequired().HasMaxLength(20);
			builder.Property(u => u.Gender).IsRequired();
		}
	}
}

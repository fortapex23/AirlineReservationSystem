using Microsoft.AspNetCore.Identity;
using TravelProgram.Core.Enum;

namespace TravelProgram.Core.Models
{
	public class AppUser : IdentityUser
	{
		public string FullName { get; set; }
		public string PassportNumber { get; set; }
		public GenderType Gender { get; set; }
        public DateTime BirthDate { get; set; }

        public ICollection<Booking> Bookings { get; set; }
    }
}

using TravelProgram.Core.Models;
using TravelProgram.Core.Repositories;
using TravelProgram.Data.DAL;

namespace TravelProgram.Data.Repositories
{
	public class BookingRepository : GenericRepository<Booking>, IBookingRepository
	{
		public BookingRepository(AppDbContext context) : base(context)
		{
		}
	}
}

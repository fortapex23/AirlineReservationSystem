using TravelProgram.Core.Models;
using TravelProgram.Core.Repositories;
using TravelProgram.Data.DAL;

namespace TravelProgram.Data.Repositories
{
	public class SeatRepository : GenericRepository<Seat>, ISeatRepository
	{
		public SeatRepository(AppDbContext context) : base(context)
		{
		}
	}
}

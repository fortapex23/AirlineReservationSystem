using TravelProgram.Core.Models;
using TravelProgram.Core.Repositories;
using TravelProgram.Data.DAL;

namespace TravelProgram.Data.Repositories
{
	public class AirportRepository : GenericRepository<Airport>, IAirportRepository
	{
		public AirportRepository(AppDbContext context) : base(context)
		{
		}
	}
}

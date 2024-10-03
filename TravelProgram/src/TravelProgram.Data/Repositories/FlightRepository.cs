using TravelProgram.Core.Models;
using TravelProgram.Core.Repositories;
using TravelProgram.Data.DAL;

namespace TravelProgram.Data.Repositories
{
	public class FlightRepository : GenericRepository<Flight>, IFlightRepository
	{
		public FlightRepository(AppDbContext context) : base(context)
		{
		}
	}
}

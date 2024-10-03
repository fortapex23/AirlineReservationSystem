using TravelProgram.Core.Models;
using TravelProgram.Core.Repositories;
using TravelProgram.Data.DAL;

namespace TravelProgram.Data.Repositories
{
	public class AirlineRepository : GenericRepository<Airline>, IAirlineRepository
	{
		public AirlineRepository(AppDbContext context) : base(context)
		{
		}
	}
}

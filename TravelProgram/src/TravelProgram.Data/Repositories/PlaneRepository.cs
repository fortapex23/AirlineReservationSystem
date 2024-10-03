using TravelProgram.Core.Models;
using TravelProgram.Core.Repositories;
using TravelProgram.Data.DAL;

namespace TravelProgram.Data.Repositories
{
	public class PlaneRepository : GenericRepository<Plane>, IPlaneRepository
	{
		public PlaneRepository(AppDbContext context) : base(context)
		{
		}
	}
}

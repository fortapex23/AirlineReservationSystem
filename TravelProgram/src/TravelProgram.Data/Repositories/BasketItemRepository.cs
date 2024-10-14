using TravelProgram.Core.Models;
using TravelProgram.Core.Repositories;
using TravelProgram.Data.DAL;

namespace TravelProgram.Data.Repositories
{
    public class BasketItemRepository : GenericRepository<BasketItem>, IBasketItemRepository
    {
        public BasketItemRepository(AppDbContext context) : base(context)
        {
        }
    }
}

using TravelProgram.Business.DTOs.BasketItemDTOs;
using TravelProgram.Core.Models;

namespace TravelProgram.Business.Services.Interfaces
{
    public interface IBasketItemService
    {
        Task AddToBasketAsync(string appUserId, int flightId);
        IQueryable<BasketItemDTO> GetBasketItems(string appUserId);
        Task RemoveFromBasketAsync(string appUserId, int flightId);
    }

}

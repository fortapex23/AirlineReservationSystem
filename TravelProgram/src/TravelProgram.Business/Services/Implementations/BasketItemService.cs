using System.Linq;
using Microsoft.EntityFrameworkCore;
using TravelProgram.Business.DTOs.BasketItemDTOs;
using TravelProgram.Business.Services.Interfaces;
using TravelProgram.Core.Models;
using TravelProgram.Core.Repositories;

namespace TravelProgram.Business.Services.Implementations
{
    public class BasketItemService : IBasketItemService
    {
        private readonly IBasketItemRepository _basketItemRepository;
        private readonly ISeatRepository _seatRepository;

        public BasketItemService(IBasketItemRepository basketItemRepository, ISeatRepository seatRepository)
        {
            _basketItemRepository = basketItemRepository;
            _seatRepository = seatRepository;
        }

        public async Task AddToBasketAsync(string appUserId, int seatId)
        {
            var seat = await _seatRepository.GetByIdAsync(seatId);
            if (seat == null)
            {
                throw new Exception("Seat not found.");
            }

            var existingBasketItem = _basketItemRepository
                .GetByExpression(false, b => b.AppUserId == appUserId && b.SeatId == seatId)
                .FirstOrDefault();

            if (existingBasketItem != null)
            {
                return;
            }

            var basketItem = new BasketItem
            {
                AppUserId = appUserId,
                SeatId = seatId,
                CreatedTime = DateTime.UtcNow,
                UpdatedTime = DateTime.UtcNow
            };

            await _basketItemRepository.CreateAsync(basketItem);
            await _basketItemRepository.CommitAsync();
        }

        public async Task RemoveFromBasketAsync(string appUserId, int seatId)
        {
            var basketItem = _basketItemRepository
                .GetByExpression(false, b => b.AppUserId == appUserId && b.SeatId == seatId)
                .FirstOrDefault();

            if (basketItem != null)
            {
                _basketItemRepository.Delete(basketItem);
                await _basketItemRepository.CommitAsync();
            }
        }


        public IQueryable<BasketItemDTO> GetBasketItems(string appUserId)
        {
            return _basketItemRepository
                .GetByExpression(false, b => b.AppUserId == appUserId, "Seat")
                .Select(b => new BasketItemDTO(b.SeatId, b.AppUserId));
        }
    }
}

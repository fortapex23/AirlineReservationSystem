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
        private readonly IFlightRepository _flightRepository;

        public BasketItemService(IBasketItemRepository basketItemRepository, IFlightRepository flightRepository)
        {
            _basketItemRepository = basketItemRepository;
            _flightRepository = flightRepository;
        }

        public async Task AddToBasketAsync(string appUserId, int flightId)
        {
            var flight = await _flightRepository.GetByIdAsync(flightId);
            if (flight == null)
            {
                throw new Exception("Flight not found.");
            }

            var existingBasketItem = _basketItemRepository
                .GetByExpression(false, b => b.AppUserId == appUserId && b.FlightId == flightId)
                .FirstOrDefault();

            if (existingBasketItem != null)
            {
                return;
            }

            var basketItem = new BasketItem
            {
                AppUserId = appUserId,
                FlightId = flightId,
                CreatedTime = DateTime.UtcNow,
                UpdatedTime = DateTime.UtcNow
            };

            await _basketItemRepository.CreateAsync(basketItem);
            await _basketItemRepository.CommitAsync();
        }

        public IQueryable<BasketItemDTO> GetBasketItems(string appUserId)
        {
            return _basketItemRepository
                .GetByExpression(false, b => b.AppUserId == appUserId, "Flight")
                .Select(b => new BasketItemDTO(b.FlightId, b.AppUserId));
        }
    }
}

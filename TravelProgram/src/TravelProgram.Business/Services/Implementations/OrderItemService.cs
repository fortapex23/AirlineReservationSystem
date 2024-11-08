using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using TravelProgram.Business.DTOs.OrderItemDTOs;
using TravelProgram.Business.Services.Interfaces;
using TravelProgram.Core.Models;
using TravelProgram.Core.Repositories;

namespace TravelProgram.Business.Services.Implementations
{
    public class OrderItemService : IOrderItemService
    {
        private readonly IOrderItemRepository _orderItemRepository;
        private readonly IMapper _mapper;
		private readonly ISeatRepository _seatRepository;

		public OrderItemService(IOrderItemRepository orderItemRepository, IMapper mapper, ISeatRepository seatRepository)
        {
            _orderItemRepository = orderItemRepository;
            _mapper = mapper;
			_seatRepository = seatRepository;
		}

        public async Task<OrderItemGetDto> CreateAsync(OrderItemCreateDto dto)
        {
			var seat = await _seatRepository.GetByIdAsync(dto.SeatId);
			if (seat == null || !seat.IsAvailable)
				throw new Exception("Seat not found or is not available.");

			var orderItem = _mapper.Map<OrderItem>(dto);
            orderItem.Price = seat.Price;
            orderItem.CreatedTime = DateTime.Now;
            orderItem.UpdatedTime = DateTime.Now;

            await _orderItemRepository.CreateAsync(orderItem);
            await _orderItemRepository.CommitAsync();

            return _mapper.Map<OrderItemGetDto>(orderItem);
        }

        public async Task DeleteAsync(int id)
        {
            var orderItem = await _orderItemRepository.GetByIdAsync(id);
            if (orderItem == null) throw new Exception("Order item not found");

            _orderItemRepository.Delete(orderItem);
            await _orderItemRepository.CommitAsync();
        }

        public async Task<ICollection<OrderItemGetDto>> GetByExpression(bool asNoTracking = false, Expression<Func<OrderItem, bool>>? expression = null, params string[] includes)
        {
            var orderItems = await _orderItemRepository.GetByExpression(asNoTracking, expression, includes).ToListAsync();
            return _mapper.Map<ICollection<OrderItemGetDto>>(orderItems);
        }

        public async Task<OrderItemGetDto> GetById(int id)
        {
            var orderItem = await _orderItemRepository.GetByIdAsync(id);
            if (orderItem == null) throw new Exception("Order item not found");

            return _mapper.Map<OrderItemGetDto>(orderItem);
        }

        //public async Task<OrderItemGetDto> GetByOrderId(int orderid)
        //{
        //    var orderItem = _orderItemRepository.GetByExpression(false, x => x.OrderId == orderid);
        //    if (orderItem == null) throw new Exception("Order item not found");

        //    return _mapper.Map<OrderItemGetDto>(orderItem);
        //}

        //public async Task UpdateAsync(int id, OrderItemUpdateDto dto)
        //{
        //    var orderItem = await _orderItemRepository.GetByIdAsync(id);
        //    if (orderItem == null) throw new Exception("Order item not found");

        //    _mapper.Map(dto, orderItem);
        //    await _orderItemRepository.CommitAsync();
        //}
    }
}

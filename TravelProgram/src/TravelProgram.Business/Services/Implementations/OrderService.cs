using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelProgram.Business.DTOs.OrderDTOs;
using TravelProgram.Business.Services.Interfaces;
using TravelProgram.Core.Enum;
using TravelProgram.Core.Models;
using TravelProgram.Core.Repositories;
using TravelProgram.Data.Repositories;

namespace TravelProgram.Business.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;
        private readonly IOrderItemRepository _orderItemRepository;
		private readonly ISeatRepository _seatRepository;
		private readonly IBookingRepository _bookingRepository;

		public OrderService(IOrderRepository orderRepository, IMapper mapper, IOrderItemRepository orderItemRepository,
                            ISeatRepository seatRepository, IBookingRepository bookingRepository)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _orderItemRepository = orderItemRepository;
			_seatRepository = seatRepository;
			_bookingRepository = bookingRepository;
		}

        public Task<bool> IsExist(Expression<Func<Order, bool>> expression)
        {
            return _orderRepository.Table.AnyAsync(expression);
        }

		public async Task<OrderGetDto> CreateAsync(OrderCreateDto dto)
		{
			var order = _mapper.Map<Order>(dto);
			order.CreatedTime = DateTime.Now;
			order.UpdatedTime = DateTime.Now;

			await _orderRepository.CreateAsync(order);
			//await _orderRepository.CommitAsync();

			decimal totalAmount = 0;

			if (dto.OrderItems != null)
			{
				foreach (var item in dto.OrderItems)
				{
					var seat = await _seatRepository.GetByIdAsync(item.SeatId);
					if (seat == null || !seat.IsAvailable)
						throw new Exception("Seat not available or not found");

					var orderItem = new OrderItem
					{
						OrderId = order.Id,
						SeatId = item.SeatId,
						Price = seat.Price,
						CreatedTime = DateTime.Now,
						UpdatedTime = DateTime.Now
					};

					await _orderItemRepository.CreateAsync(orderItem);
					totalAmount += orderItem.Price;
				}
				await _orderItemRepository.CommitAsync();
			}

			order.TotalAmount = totalAmount;
			await _orderRepository.CommitAsync();

			return _mapper.Map<OrderGetDto>(order);
		}

        public async Task DeleteAsync(int id)
        {
            var order = await _orderRepository.Table
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                throw new Exception("Order not found");

            if (order.OrderItems != null && order.OrderItems.Any())
            {
                foreach (var orderItem in order.OrderItems.ToList())
                {
                    _orderItemRepository.Delete(orderItem);
                }
                await _orderItemRepository.CommitAsync();
            }

            _orderRepository.Delete(order);
            await _orderRepository.CommitAsync();
        }


        public async Task<ICollection<OrderGetDto>> GetByExpression(bool asNoTracking = false, Expression<Func<Order, bool>>? expression = null, params string[] includes)
        {
            var orders = await _orderRepository.GetByExpression(asNoTracking, expression, includes).ToListAsync();
            return _mapper.Map<ICollection<OrderGetDto>>(orders);
        }

        public async Task<OrderGetDto> GetById(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) throw new Exception("Order not found");

            return _mapper.Map<OrderGetDto>(order);
        }

		public async Task UpdateAsync(int id, OrderUpdateDto dto)
		{
			var order = await _orderRepository.Table
				.Include(o => o.OrderItems)
				.FirstOrDefaultAsync(o => o.Id == id);

			if (order == null) throw new Exception("Order not found");

			_mapper.Map(dto, order);
			order.UpdatedTime = DateTime.Now;

			if (dto.Status == OrderStatus.Completed)
			{
				foreach (var orderItem in order.OrderItems)
				{
					var existingBooking = await _bookingRepository
						.GetByExpression(false, x => x.SeatId == orderItem.SeatId)
						.FirstOrDefaultAsync();

					if (existingBooking != null)
						throw new Exception($"already booking for seat {orderItem.SeatId}");

					var seat = await _seatRepository.GetByIdAsync(orderItem.SeatId);
					if (seat == null || !seat.IsAvailable)
						throw new Exception("Seat not found or booked");

					seat.IsAvailable = false;

					var booking = new Booking
					{
						FlightId = seat.FlightId,
						SeatId = orderItem.SeatId,
						AppUserId = order.AppUserId,
						BookingNumber = Guid.NewGuid().ToString().Substring(0, 10).ToUpper(),
						CreatedTime = DateTime.Now,
						UpdatedTime = DateTime.Now
					};

					await _bookingRepository.CreateAsync(booking);
				}

				await _seatRepository.CommitAsync();
			}

			await _orderRepository.CommitAsync();
		}


	}
}

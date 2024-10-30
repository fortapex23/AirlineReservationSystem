using System.Linq.Expressions;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using TravelProgram.Business.DTOs.OrderDTOs;
using TravelProgram.Business.Services.Interfaces;
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

        public OrderService(IOrderRepository orderRepository, IMapper mapper, IOrderItemRepository orderItemRepository)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
            _orderItemRepository = orderItemRepository;
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
            await _orderRepository.CommitAsync();

            if (dto.OrderItems != null)
            {
                foreach (var item in dto.OrderItems)
                {
                    var orderItem = _mapper.Map<OrderItem>(item);
                    orderItem.OrderId = order.Id;
                    orderItem.CreatedTime = DateTime.Now;
                    orderItem.UpdatedTime = DateTime.Now;
					await _orderItemRepository.CreateAsync(orderItem);
                }
                await _orderItemRepository.CommitAsync();
            }

            return _mapper.Map<OrderGetDto>(order);
        }


        public async Task DeleteAsync(int id)
        {
            var order = await _orderRepository.GetByIdAsync(id);
            if (order == null) throw new Exception("Order not found");

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

        //public async Task UpdateAsync(int id, OrderUpdateDto dto)
        //{
        //    var order = await _orderRepository.GetByIdAsync(id);
        //    if (order == null) throw new Exception("Order not found");

        //    _mapper.Map(dto, order);

        //    order.TotalAmount = dto.OrderItems.Sum(item => item.Price);
        //    order.UpdatedTime = DateTime.Now;

        //    await _orderRepository.CommitAsync();
        //}
    }
}

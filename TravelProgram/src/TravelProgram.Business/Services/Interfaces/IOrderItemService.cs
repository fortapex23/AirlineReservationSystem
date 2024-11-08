using System.Linq.Expressions;
using TravelProgram.Business.DTOs.OrderItemDTOs;
using TravelProgram.Core.Models;

namespace TravelProgram.Business.Services.Interfaces
{
    public interface IOrderItemService
    {
        Task<OrderItemGetDto> CreateAsync(OrderItemCreateDto dto);
        Task DeleteAsync(int id);
        Task<ICollection<OrderItemGetDto>> GetByExpression(bool asNoTracking = false, Expression<Func<OrderItem, bool>>? expression = null, params string[] includes);
        Task<OrderItemGetDto> GetById(int id);
        //Task<OrderItemGetDto> GetByOrderId(int orderid);
        //Task UpdateAsync(int id, OrderItemUpdateDto dto);
    }
}

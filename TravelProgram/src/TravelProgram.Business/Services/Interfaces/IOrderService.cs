using System.Linq.Expressions;
using TravelProgram.Business.DTOs.OrderDTOs;
using TravelProgram.Core.Models;

namespace TravelProgram.Business.Services.Interfaces
{
    public interface IOrderService
    {
        Task<bool> IsExist(Expression<Func<Order, bool>> expression);
        Task<OrderGetDto> CreateAsync(OrderCreateDto dto);
        Task DeleteAsync(int id);
        Task<ICollection<OrderGetDto>> GetByExpression(bool asNoTracking = false, Expression<Func<Order, bool>>? expression = null, params string[] includes);
        Task<OrderGetDto> GetById(int id);
        Task UpdateAsync(int id, OrderUpdateDto dto);
    }
}

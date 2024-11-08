using TravelProgram.Business.DTOs.OrderItemDTOs;
using TravelProgram.Core.Enum;

namespace TravelProgram.Business.DTOs.OrderDTOs
{
    public record OrderCreateDto(string AppUserId, decimal TotalAmount, int CardNumber, OrderStatus Status,
							ICollection<OrderItemCreateDto> OrderItems);
}

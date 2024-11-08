using TravelProgram.Business.DTOs.OrderItemDTOs;
using TravelProgram.Core.Enum;

namespace TravelProgram.Business.DTOs.OrderDTOs
{
    public record OrderGetDto(int Id, string AppUserId, decimal TotalAmount, DateTime CreatedTime, int CardNumber, 
                            OrderStatus Status, ICollection<OrderItemGetDto> OrderItems);
}

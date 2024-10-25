using TravelProgram.Business.DTOs.OrderItemDTOs;

namespace TravelProgram.Business.DTOs.OrderDTOs
{
    public record OrderGetDto(int Id, string AppUserId, decimal TotalAmount, DateTime CreatedTime, 
                                ICollection<OrderItemGetDto> OrderItems);
}

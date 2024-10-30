using TravelProgram.Business.DTOs.OrderItemDTOs;

namespace TravelProgram.Business.DTOs.OrderDTOs
{
    public record OrderCreateDto(string AppUserId, decimal TotalAmount, int CardNumber, List<OrderItemCreateDto> OrderItems);
}

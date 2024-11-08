namespace TravelProgram.Business.DTOs.OrderItemDTOs
{
    public record OrderItemCreateDto(int SeatId, decimal Price, DateTime CreatedTime, DateTime UpdatedTime);
}

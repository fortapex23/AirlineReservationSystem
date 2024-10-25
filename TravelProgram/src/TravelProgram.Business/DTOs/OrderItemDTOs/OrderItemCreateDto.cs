namespace TravelProgram.Business.DTOs.OrderItemDTOs
{
    public record OrderItemCreateDto(int OrderId, int BookingId, decimal Price);
}

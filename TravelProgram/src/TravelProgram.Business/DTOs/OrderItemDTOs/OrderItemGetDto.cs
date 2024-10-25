namespace TravelProgram.Business.DTOs.OrderItemDTOs
{
    public record OrderItemGetDto(int Id, int OrderId, int BookingId, decimal Price);
}

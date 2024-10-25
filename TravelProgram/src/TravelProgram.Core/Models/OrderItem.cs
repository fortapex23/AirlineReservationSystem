namespace TravelProgram.Core.Models
{
    public class OrderItem : BaseEntity
    {
        public int OrderId { get; set; }
        public int BookingId { get; set; }
        public decimal Price { get; set; }

        public Order Order { get; set; }
        public Booking Booking { get; set; }
    }
}

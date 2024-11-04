namespace TravelProgram.Core.Models
{
    public class BasketItem : BaseEntity
    {
        public string AppUserId { get; set; }
        public int? SeatId { get; set; }

        public Seat Seat { get; set; }
        public AppUser AppUser { get; set; }
    }
}

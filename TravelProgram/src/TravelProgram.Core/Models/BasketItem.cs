namespace TravelProgram.Core.Models
{
    public class BasketItem : BaseEntity
    {
        public string AppUserId { get; set; }
        public int? FlightId { get; set; }

        public Flight Flight { get; set; }
        public AppUser AppUser { get; set; }
    }
}

namespace TravelProgram.Core.Models
{
    public class Order : BaseEntity
    {
        public string AppUserId { get; set; }
        public decimal TotalAmount { get; set; }
        public int CardNumber { get; set; }
        
        public AppUser AppUser { get; set; }
        public ICollection<OrderItem> OrderItems { get; set; }
    }
}

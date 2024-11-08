namespace TravelProgram.MVC.ViewModels.OrderItemVMs
{
    public class OrderItemCreateVM
    {
        public DateTime CreatedTime { get; set; }
        public DateTime UpdatedTime { get; set; }
        public int OrderId { get; set; }
        public int SeatId { get; set; }
        public decimal Price { get; set; }
    }
}

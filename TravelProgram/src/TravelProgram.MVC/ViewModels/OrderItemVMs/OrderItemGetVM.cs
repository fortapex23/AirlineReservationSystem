namespace TravelProgram.MVC.ViewModels.OrderItemVMs
{
    public class OrderItemGetVM
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int BookingId { get; set; }
        public decimal Price { get; set; }
    }
}

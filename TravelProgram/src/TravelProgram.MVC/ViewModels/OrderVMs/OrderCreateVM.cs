using TravelProgram.MVC.ViewModels.OrderItemVMs;

namespace TravelProgram.MVC.ViewModels.OrderVMs
{
    public class OrderCreateVM
    {
        //public int FlightId { get; set; }
        public string AppUserId { get; set; }
        public List<OrderItemCreateVM> OrderItems { get; set; }
        public int CardNumber { get; set; }
        public decimal TotalAmount { get; set; }
    }
}

using TravelProgram.MVC.ViewModels.OrderItemVMs;

namespace TravelProgram.MVC.ViewModels.OrderVMs
{
    public class OrderCreateVM
    {
        public string AppUserId { get; set; }
        public int CardNumber { get; set; }
        public decimal TotalAmount { get; set; }

        public List<OrderItemCreateVM> OrderItems { get; set; }
    }
}

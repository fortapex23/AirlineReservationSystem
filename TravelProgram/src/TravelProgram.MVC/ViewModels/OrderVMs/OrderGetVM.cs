using TravelProgram.MVC.ViewModels.OrderItemVMs;

namespace TravelProgram.MVC.ViewModels.OrderVMs
{
    public class OrderGetVM
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public int CardNumber { get; set; }
        public decimal TotalAmount { get; set; }

        public List<OrderItemGetVM> OrderItems { get; set; }
    }
}

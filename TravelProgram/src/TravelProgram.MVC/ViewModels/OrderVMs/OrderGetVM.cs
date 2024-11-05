using TravelProgram.MVC.Enums;
using TravelProgram.MVC.ViewModels.OrderItemVMs;

namespace TravelProgram.MVC.ViewModels.OrderVMs
{
    public class OrderGetVM
    {
        public int Id { get; set; }
        public string AppUserId { get; set; }
        public string AppUserName { get; set; }
        //public int FlightId { get; set; }
        public int CardNumber { get; set; }
        public List<OrderItemGetVM> OrderItems { get; set; }
        public OrderStatus Status { get; set; }
    }
}

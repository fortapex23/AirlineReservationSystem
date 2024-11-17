namespace TravelProgram.MVC.ViewModels.OrderItemVMs
{
    public class OrderItemGetVM
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public string FlightNumber { get; set; }
        public int SeatId { get; set; }
        public int SeatNumber { get; set; }
        public decimal Price { get; set; }
    }
}

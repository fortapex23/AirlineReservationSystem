﻿namespace TravelProgram.MVC.ViewModels.OrderItemVMs
{
    public class OrderItemCreateVM
    {
        public int OrderId { get; set; }
        public int BookingId { get; set; }
        public decimal Price { get; set; }
    }
}
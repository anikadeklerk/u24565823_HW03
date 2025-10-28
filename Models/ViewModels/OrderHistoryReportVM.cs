using System;
using System.Collections.Generic;

namespace BikeStores_MVC.Models
{
    public class OrderHistoryReportVM
    {
        public string CustomerName { get; set; }
        public List<OrderDetailItem> Orders { get; set; } = new List<OrderDetailItem>();
    }

    public class OrderDetailItem
    {
        public int OrderId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Product { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal Total => UnitPrice * Quantity;
    }
}

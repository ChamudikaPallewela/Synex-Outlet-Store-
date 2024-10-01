using System;

namespace Client.DTO
{
    public class ItemDto
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountRate { get; set; }
        public DateTime ExpireDate { get; set; }
        public int Quantity { get; set; }
    }
}
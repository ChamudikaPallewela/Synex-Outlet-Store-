using System;

namespace Client.DTO
{
    public class StockDto
    {
        public int StockID { get; set; }
        public string ItemCode { get; set; }
        public DateTime DateOfPurchase { get; set; }
        public int QuantityReceived { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
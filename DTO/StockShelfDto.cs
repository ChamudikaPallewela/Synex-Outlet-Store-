using System;

namespace Client.DTO
{
    public class StockShelfDto
    {
        public int StockID { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public DateTime ExpiryDate { get; set; }
    }
}
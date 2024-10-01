namespace Client.DTO
{
    public class SalesReportItemDto
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Revenue { get; set; }
    }
}
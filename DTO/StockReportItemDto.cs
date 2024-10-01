namespace Client.DTO
{
    public class StockReportItemDto
    {
        public int StockID { get; set; }
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
    }
}
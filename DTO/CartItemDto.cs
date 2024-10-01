namespace Client.DTO
{
    public class CartItemDto
    {
        public string ItemCode { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal DiscountRate { get; set; }
        public decimal Total => (Price * Quantity) - ((Price * Quantity) * DiscountRate / 100);
    }
}
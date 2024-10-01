namespace Client.Builder
{
    public class CartItemBuilder
    {
        private readonly DTO.CartItemDto _cartItem;

        public CartItemBuilder()
        {
            _cartItem = new DTO.CartItemDto();
        }

        public CartItemBuilder SetItemCode(string itemCode)
        {
            _cartItem.ItemCode = itemCode;
            return this;
        }

        public CartItemBuilder SetItemName(string itemName)
        {
            _cartItem.ItemName = itemName;
            return this;
        }

        public CartItemBuilder SetQuantity(int quantity)
        {
            _cartItem.Quantity = quantity;
            return this;
        }

        public CartItemBuilder SetPrice(decimal price)
        {
            _cartItem.Price = price;
            return this;
        }

        public CartItemBuilder SetDiscountRate(decimal discountRate)
        {
            _cartItem.DiscountRate = discountRate;
            return this;
        }

        public DTO.CartItemDto Build()
        {
            return _cartItem;
        }
    }
}
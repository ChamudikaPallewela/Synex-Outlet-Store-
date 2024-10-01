using System;

namespace Client.Factory
{
    public static class DtoFactory
    {
        public static DTO.ItemDto CreateItemDto(string itemCode, string itemName, decimal price, decimal discountRate, DateTime expireDate)
        {
            return new DTO.ItemDto
            {
                ItemCode = itemCode,
                ItemName = itemName,
                Price = price,
                DiscountRate = discountRate,
                ExpireDate = expireDate
            };
        }

        public static DTO.StockDto CreateStockDto(string itemCode, DateTime dateOfPurchase, int quantityReceived, DateTime expiryDate)
        {
            return new DTO.StockDto
            {
                ItemCode = itemCode,
                DateOfPurchase = dateOfPurchase,
                QuantityReceived = quantityReceived,
                ExpiryDate = expiryDate
            };
        }

        public static DTO.StockShelfDto CreateStockShelfDto(int stockID, string itemName, int quantity, DateTime expiryDate)
        {
            return new DTO.StockShelfDto
            {
                StockID = stockID,
                ItemName = itemName,
                Quantity = quantity,
                ExpiryDate = expiryDate
            };
        }

        public static DTO.CartItemDto CreateCartItemDto(string itemCode, string itemName, int quantity, decimal price, decimal discountRate)
        {
            return new DTO.CartItemDto
            {
                ItemCode = itemCode,
                ItemName = itemName,
                Quantity = quantity,
                Price = price,
                DiscountRate = discountRate
            };
        }
    }
}
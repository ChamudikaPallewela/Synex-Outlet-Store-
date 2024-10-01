using System;

namespace Client.Builder
{
    public class StockShelfBuilder
    {
        private readonly DTO.StockShelfDto _stockShelf;

        public StockShelfBuilder()
        {
            _stockShelf = new DTO.StockShelfDto();
        }

        public StockShelfBuilder SetStockID(int stockID)
        {
            _stockShelf.StockID = stockID;
            return this;
        }

        public StockShelfBuilder SetItemName(string itemName)
        {
            _stockShelf.ItemName = itemName;
            return this;
        }

        public StockShelfBuilder SetQuantity(int quantity)
        {
            _stockShelf.Quantity = quantity;
            return this;
        }

        public StockShelfBuilder SetExpiryDate(DateTime expiryDate)
        {
            _stockShelf.ExpiryDate = expiryDate;
            return this;
        }

        public DTO.StockShelfDto Build()
        {
            return _stockShelf;
        }
    }
}
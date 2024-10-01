using System;

namespace Client.NullObject
{
    public class NullStockShelf : DTO.StockShelfDto
    {
        private static NullStockShelf _instance;

        private NullStockShelf()
        {
            StockID = -1;
            ItemName = "No Item";
            Quantity = 0;
            ExpiryDate = DateTime.MinValue;
        }

        public static NullStockShelf Instance => _instance ?? (_instance = new NullStockShelf());
    }
}
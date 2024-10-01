using Client.DTO;
using Client.TableDataGateway;
using System;
using System.Data;

namespace Client.Facade
{
    public class StoreFacade
    {
        private readonly ItemGateway _itemGateway = new ItemGateway();
        private readonly StockGateway _stockGateway = new StockGateway();

        public void AddItem(ItemDto item, int quantity)
        {
            _itemGateway.AddItem(item);
            var stock = new StockDto
            {
                ItemCode = item.ItemCode,
                DateOfPurchase = DateTime.Now,
                QuantityReceived = quantity,
                ExpiryDate = DateTime.Now.AddMonths(1)
            };
            _stockGateway.AddStock(stock);
        }

        public DataTable GetAllItems()
        {
            return _stockGateway.RetrieveAllItems();
        }
    }
}
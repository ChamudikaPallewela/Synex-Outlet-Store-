using Client.DTO;
using Client.Facade;

namespace Client.Command
{
    public class TransferItemCommand
    {
        private readonly StockShelfFacade _stockshelfFacade = new StockShelfFacade();

        public void Execute(StockShelfDto stockShelf, int quantity)
        {
            _stockshelfFacade.TransferItem(stockShelf, quantity);
            string message = $"StockTransferred:StockID={stockShelf.StockID}, ItemName={stockShelf.ItemName}, Quantity={quantity}";
            _stockshelfFacade.BroadcastMessage(message);
        }
    }
}
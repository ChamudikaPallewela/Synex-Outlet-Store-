using Client.DTO;
using Client.TableDataGateway;
using Singleton;
using System.Data;

namespace Client.Facade
{
    public class StockShelfFacade
    {
        private readonly StockShelfGateway _stockShelfGateway = new StockShelfGateway();
        private readonly TcpClientSingleton _tcpClient = TcpClientSingleton.GetInstance("127.0.0.1", 5000);

        public DataTable GetStockShelves()
        {
            return _stockShelfGateway.RetrieveStockShelves();
        }

        public void TransferItem(StockShelfDto stockShelf, int quantity)
        {
            _stockShelfGateway.TransferItem(stockShelf, quantity);
        }

        public void BroadcastMessage(string message)
        {
            _tcpClient.SendMessage(message);
        }
    }
}
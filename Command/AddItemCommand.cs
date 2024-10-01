using Client.DTO;
using Client.Facade;
using Singleton;

namespace Client.Command
{
    public class AddItemCommand
    {
        private readonly StoreFacade _storeFacade = new StoreFacade();
        private readonly TcpClientSingleton _tcpClient = TcpClientSingleton.GetInstance("127.0.0.1", 5000);

        public void Execute(ItemDto item, int quantity)
        {
            _storeFacade.AddItem(item, quantity);
            _tcpClient.SendMessage($"ItemAdded:{item.ItemName}");
        }
    }
}
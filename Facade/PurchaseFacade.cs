using Client.DTO;
using Client.TableDataGateway;
using Singleton;
using System.Collections.Generic;
using System.Linq;

namespace Client.Facade
{
    public class PurchaseFacade
    {
        private readonly IPurchaseGateway _purchaseGateway;
        private readonly TcpClientSingleton _tcpClient = TcpClientSingleton.GetInstance("127.0.0.1", 5000);

        // Default constructor uses the concrete PurchaseGateway
        public PurchaseFacade() : this(new PurchaseGateway())
        {
        }

        // Constructor that allows dependency injection
        public PurchaseFacade(IPurchaseGateway purchaseGateway)
        {
            _purchaseGateway = purchaseGateway;
        }

        public List<ItemDto> LoadItems()
        {
            return _purchaseGateway.LoadItems();
        }

        public void FinalizePurchase(List<CartItemDto> cartItems, decimal cashTendered, out int billNumber, out decimal changeDue)
        {
            billNumber = _purchaseGateway.FinalizePurchase(cartItems, cashTendered, out changeDue);
            _tcpClient.SendMessage($"PurchaseCompleted:BillNumber={billNumber}, TotalAmount={cartItems.Sum(item => item.Total)}");
        }
    }
}

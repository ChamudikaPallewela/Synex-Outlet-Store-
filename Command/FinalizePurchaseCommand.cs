using Client.DTO;
using Client.Facade;
using System.Collections.Generic;

namespace Client.Command
{
    public class FinalizePurchaseCommand
    {
        private readonly PurchaseFacade _purchaseFacade = new PurchaseFacade();

        public void Execute(List<CartItemDto> cartItems, decimal cashTendered, out int billNumber, out decimal changeDue)
        {
            _purchaseFacade.FinalizePurchase(cartItems, cashTendered, out billNumber, out changeDue);
        }
    }
}
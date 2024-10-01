using Client.DTO;
using Client.Facade;
using System.Collections.Generic;

namespace Client.Command
{
    public class LoadItemsCommand
    {
        private readonly PurchaseFacade _purchaseFacade = new PurchaseFacade();

        public List<ItemDto> Execute()
        {
            return _purchaseFacade.LoadItems();
        }
    }
}
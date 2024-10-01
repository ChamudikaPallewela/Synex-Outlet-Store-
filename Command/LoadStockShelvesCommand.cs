using Client.Facade;
using System.Data;

namespace Client.Command
{
    public class LoadStockShelvesCommand
    {
        private readonly StockShelfFacade _stockshelfFacade = new StockShelfFacade();

        public DataTable Execute()
        {
            return _stockshelfFacade.GetStockShelves();
        }
    }
}
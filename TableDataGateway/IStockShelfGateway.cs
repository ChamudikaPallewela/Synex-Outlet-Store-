using Client.DTO;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.TableDataGateway
{
    public interface IStockShelfGateway
    {
        void TransferItem(StockShelfDto stockShelf, int quantity);
        DataTable RetrieveStockShelves();
    }
}

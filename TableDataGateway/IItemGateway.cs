using Client.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.TableDataGateway
{
    public interface IItemGateway
    {
        void AddItem(ItemDto item);
        ItemDto RetrieveItem(string itemCode);
    }

}

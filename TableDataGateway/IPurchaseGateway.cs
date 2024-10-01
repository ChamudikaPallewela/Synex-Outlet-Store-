using Client.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.TableDataGateway
{
    public interface IPurchaseGateway
    {
        List<ItemDto> LoadItems();
        int FinalizePurchase(List<CartItemDto> cartItems, decimal cashTendered, out decimal changeDue);
    }

}

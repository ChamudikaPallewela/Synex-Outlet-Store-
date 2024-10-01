using Client.DTO;
using System.Collections.Generic;

namespace Client.Command
{
    public class AddToCartCommand
    {
        public void Execute(List<CartItemDto> cartItems, CartItemDto cartItem)
        {
            cartItems.Add(cartItem);
        }
    }
}
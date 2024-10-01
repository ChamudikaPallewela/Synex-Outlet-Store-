using System;

namespace Client.Builder
{
    public class ItemBuilder
    {
        private readonly DTO.ItemDto _item;

        public ItemBuilder()
        {
            _item = new DTO.ItemDto();
        }

        public ItemBuilder SetItemCode(string itemCode)
        {
            _item.ItemCode = itemCode;
            return this;
        }

        public ItemBuilder SetItemName(string itemName)
        {
            _item.ItemName = itemName;
            return this;
        }

        public ItemBuilder SetPrice(decimal price)
        {
            _item.Price = price;
            return this;
        }

        public ItemBuilder SetDiscountRate(decimal discountRate)
        {
            _item.DiscountRate = discountRate;
            return this;
        }

        public ItemBuilder SetExpireDate(DateTime expireDate)
        {
            _item.ExpireDate = expireDate;
            return this;
        }

        public DTO.ItemDto Build()
        {
            return _item;
        }
    }
}
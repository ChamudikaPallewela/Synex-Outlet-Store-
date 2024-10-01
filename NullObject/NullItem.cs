using System;

namespace Client.NullObject
{
    public class NullItem : DTO.ItemDto
    {
        private static NullItem _instance;

        private NullItem()
        {
            ItemCode = "N/A";
            ItemName = "No Item";
            Price = 0m;
            DiscountRate = 0m;
            ExpireDate = DateTime.MinValue;
        }

        public static NullItem Instance => _instance ?? (_instance = new NullItem());
    }
}
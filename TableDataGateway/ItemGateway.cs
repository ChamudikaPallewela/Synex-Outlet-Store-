using Client.DTO;
using Client.NullObject;
using Client.Template;
using System;
using System.Data.SqlClient;

namespace Client.TableDataGateway
{
    public class ItemGateway : BaseDatabaseOperation, IItemGateway
    {
        public virtual void AddItem(ItemDto item) // Marking as virtual for testing flexibility
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var query = "INSERT INTO Items (ItemCode, ItemName, Price, DiscountRate, ExpireDate) VALUES (@ItemCode, @ItemName, @Price, @DiscountRate, @ExpireDate)";
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ItemCode", item.ItemCode);
                cmd.Parameters.AddWithValue("@ItemName", item.ItemName);
                cmd.Parameters.AddWithValue("@Price", item.Price);
                cmd.Parameters.AddWithValue("@DiscountRate", item.DiscountRate);
                cmd.Parameters.AddWithValue("@ExpireDate", item.ExpireDate);
                cmd.ExecuteNonQuery();
            }
        }

        public virtual ItemDto RetrieveItem(string itemCode) // Marking as virtual for testing flexibility
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var query = "SELECT * FROM Items WHERE ItemCode = @ItemCode";
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ItemCode", itemCode);

                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new ItemDto
                        {
                            ItemCode = reader["ItemCode"].ToString(),
                            ItemName = reader["ItemName"].ToString(),
                            Price = (decimal)reader["Price"],
                            DiscountRate = (decimal)reader["DiscountRate"],
                            ExpireDate = (DateTime)reader["ExpireDate"]
                        };
                    }
                }
            }
            return NullItem.Instance;
        }
    }

}

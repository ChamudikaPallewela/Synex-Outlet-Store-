using Client.DTO;
using Client.Template;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace Client.TableDataGateway
{
    public class PurchaseGateway : BaseDatabaseOperation, IPurchaseGateway
    {
        public List<ItemDto> LoadItems()
        {
            string query = @"
                SELECT i.ItemCode, i.ItemName, MAX(s.QuantityTransferred) AS Quantity, i.Price, i.DiscountRate
                FROM Stock_Shelf s
                INNER JOIN Items i ON s.ItemCode = i.ItemCode
                GROUP BY i.ItemCode, i.ItemName, i.Price, i.DiscountRate";

            using (var conn = CreateConnection())
            {
                var cmd = new SqlCommand(query, conn);
                conn.Open();
                var reader = cmd.ExecuteReader();
                var items = new List<ItemDto>();

                while (reader.Read())
                {
                    items.Add(new ItemDto
                    {
                        ItemCode = reader["ItemCode"].ToString(),
                        ItemName = reader["ItemName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        Price = Convert.ToDecimal(reader["Price"]),
                        DiscountRate = Convert.ToDecimal(reader["DiscountRate"])
                    });
                }

                return items;
            }
        }

        public int FinalizePurchase(List<CartItemDto> cartItems, decimal cashTendered, out decimal changeDue)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var transaction = conn.BeginTransaction();

                try
                {
                    string billQuery = "INSERT INTO Bill (BillDate, TotalBillAmount, CashTendered, ChangeDue) OUTPUT INSERTED.BillNumber VALUES (@BillDate, @TotalBillAmount, @CashTendered, @ChangeDue)";
                    var billCmd = new SqlCommand(billQuery, conn, transaction);

                    decimal totalAmount = cartItems.Sum(item => item.Total);
                    changeDue = cashTendered - totalAmount;

                    billCmd.Parameters.AddWithValue("@BillDate", DateTime.Now);
                    billCmd.Parameters.AddWithValue("@TotalBillAmount", totalAmount);
                    billCmd.Parameters.AddWithValue("@CashTendered", cashTendered);
                    billCmd.Parameters.AddWithValue("@ChangeDue", changeDue);

                    int billNumber = (int)billCmd.ExecuteScalar();

                    string itemBillQuery = "INSERT INTO Item_Bill (ItemCode, BillNumber, Quantity) VALUES (@ItemCode, @BillNumber, @Quantity)";
                    foreach (var cartItem in cartItems)
                    {
                        var itemBillCmd = new SqlCommand(itemBillQuery, conn, transaction);
                        itemBillCmd.Parameters.AddWithValue("@ItemCode", cartItem.ItemCode);
                        itemBillCmd.Parameters.AddWithValue("@BillNumber", billNumber);
                        itemBillCmd.Parameters.AddWithValue("@Quantity", cartItem.Quantity);
                        itemBillCmd.ExecuteNonQuery();

                        UpdateStockAndShelf(cartItem.ItemCode, cartItem.Quantity, conn, transaction);
                    }

                    transaction.Commit();
                    return billNumber;
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private void UpdateStockAndShelf(string itemCode, int quantityPurchased, SqlConnection conn, SqlTransaction transaction)
        {
            // Reduce quantity from Stock_Shelf
            string stockShelfQuery = @"
                UPDATE Stock_Shelf
                SET QuantityTransferred = QuantityTransferred - @QuantityPurchased
                WHERE StockID = (
                    SELECT TOP 1 StockID
                    FROM Stock_Shelf
                    WHERE ItemCode = @ItemCode AND QuantityTransferred > 0
                    ORDER BY StockID
                );";

            var stockShelfCmd = new SqlCommand(stockShelfQuery, conn, transaction);
            stockShelfCmd.Parameters.AddWithValue("@QuantityPurchased", quantityPurchased);
            stockShelfCmd.Parameters.AddWithValue("@ItemCode", itemCode);
            stockShelfCmd.ExecuteNonQuery();

            // Reduce quantity from Shelf
            string shelfQuery = @"
                UPDATE Shelf
                SET Quantity = Quantity - @QuantityPurchased
                WHERE ShelfID = (
                    SELECT TOP 1 ShelfID
                    FROM Stock_Shelf
                    WHERE ItemCode = @ItemCode AND QuantityTransferred > 0
                    ORDER BY ShelfID
                );";

            var shelfCmd = new SqlCommand(shelfQuery, conn, transaction);
            shelfCmd.Parameters.AddWithValue("@QuantityPurchased", quantityPurchased);
            shelfCmd.Parameters.AddWithValue("@ItemCode", itemCode);
            shelfCmd.ExecuteNonQuery();
        }
    }
}
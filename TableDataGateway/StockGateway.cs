using Client.DTO;
using Client.Template;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Client.TableDataGateway
{
    public class StockGateway : BaseDatabaseOperation, IStockGateway
    {
        public virtual void AddStock(StockDto stock)  // Marking as virtual for testing flexibility
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var query = "INSERT INTO Stocks (DateOfPurchase, QuantityReceived, ExpiryDate) VALUES (@DateOfPurchase, @QuantityReceived, @ExpiryDate); SELECT SCOPE_IDENTITY();";
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@DateOfPurchase", stock.DateOfPurchase);
                cmd.Parameters.AddWithValue("@QuantityReceived", stock.QuantityReceived);
                cmd.Parameters.AddWithValue("@ExpiryDate", stock.ExpiryDate);

                stock.StockID = Convert.ToInt32(cmd.ExecuteScalar());

                var itemStockQuery = "INSERT INTO Item_Stock (ItemCode, StockID, Quantity) VALUES (@ItemCode, @StockID, @Quantity)";
                var itemStockCmd = new SqlCommand(itemStockQuery, conn);
                itemStockCmd.Parameters.AddWithValue("@ItemCode", stock.ItemCode);
                itemStockCmd.Parameters.AddWithValue("@StockID", stock.StockID);
                itemStockCmd.Parameters.AddWithValue("@Quantity", stock.QuantityReceived);
                itemStockCmd.ExecuteNonQuery();
            }
        }

        public virtual DataTable RetrieveAllItems()  // Marking as virtual for testing flexibility
        {
            using (var conn = CreateConnection())
            {
                conn.Open();
                var query = @"
                        SELECT i.ItemCode, i.ItemName, isStock.Quantity, i.ExpireDate
                        FROM Items i
                        JOIN Item_Stock isStock ON i.ItemCode = isStock.ItemCode
                        JOIN Stocks s ON isStock.StockID = s.StockID";

                var cmd = new SqlCommand(query, conn);
                var adapter = new SqlDataAdapter(cmd);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }
    }

}

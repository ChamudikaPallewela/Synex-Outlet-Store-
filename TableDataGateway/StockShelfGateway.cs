using Client.DTO;
using Client.Template;
using System;
using System.Data;
using System.Data.SqlClient;

namespace Client.TableDataGateway
{
    public class StockShelfGateway : BaseDatabaseOperation, IStockShelfGateway
    {
        public DataTable RetrieveStockShelves()
        {
            string query = @"
                SELECT
                    s.StockID,
                    i.ItemName,
                    isStock.Quantity AS Quantity,
                    s.ExpiryDate
                FROM
                    Stocks s
                JOIN
                    Item_Stock isStock ON s.StockID = isStock.StockID
                JOIN
                    Items i ON isStock.ItemCode = i.ItemCode
                ORDER BY
                    s.ExpiryDate";

            using (var conn = CreateConnection())
            {
                var adapter = new SqlDataAdapter(query, conn);
                var dataTable = new DataTable();
                adapter.Fill(dataTable);
                return dataTable;
            }
        }

        public void TransferItem(StockShelfDto stockShelf, int quantity)
        {
            using (var conn = CreateConnection())
            {
                conn.Open();

                // Start a transaction
                using (SqlTransaction transaction = conn.BeginTransaction())
                {
                    try
                    {
                        string getItemCodeQuery = @"
                            SELECT ItemCode
                            FROM Item_Stock
                            WHERE StockID = @StockID";
                        var getItemCodeCmd = new SqlCommand(getItemCodeQuery, conn, transaction);
                        getItemCodeCmd.Parameters.AddWithValue("@StockID", stockShelf.StockID);
                        string itemCode = (string)getItemCodeCmd.ExecuteScalar();

                        if (string.IsNullOrEmpty(itemCode))
                        {
                            throw new Exception("ItemCode not found for the given StockID.");
                        }

                        // Check if sufficient quantity is available
                        string checkStockQuantityQuery = @"
                            SELECT QuantityReceived
                            FROM Stocks
                            WHERE StockID = @StockID";
                        var checkStockQuantityCmd = new SqlCommand(checkStockQuantityQuery, conn, transaction);
                        checkStockQuantityCmd.Parameters.AddWithValue("@StockID", stockShelf.StockID);
                        int currentStockQuantity = (int)checkStockQuantityCmd.ExecuteScalar();

                        if (quantity > currentStockQuantity)
                        {
                            throw new Exception("Insufficient stock quantity.");
                        }

                        // Check if ShelfID exists or needs to be created
                        string getShelfIDQuery = @"
                            SELECT TOP 1 ShelfID
                            FROM Stock_Shelf
                            WHERE StockID = @StockID";
                        var getShelfIDCmd = new SqlCommand(getShelfIDQuery, conn, transaction);
                        getShelfIDCmd.Parameters.AddWithValue("@StockID", stockShelf.StockID);
                        object shelfIDObj = getShelfIDCmd.ExecuteScalar();
                        int shelfID;

                        if (shelfIDObj == null)
                        {
                            // Insert a new Shelf if none exists for this StockID
                            string insertShelfQuery = @"
                                INSERT INTO Shelf (Quantity)
                                VALUES (0);
                                SELECT SCOPE_IDENTITY();";
                            var insertShelfCmd = new SqlCommand(insertShelfQuery, conn, transaction);
                            shelfID = Convert.ToInt32(insertShelfCmd.ExecuteScalar());
                        }
                        else
                        {
                            shelfID = Convert.ToInt32(shelfIDObj);
                        }

                        // Update the Stocks table
                        string updateStockQuery = @"
                            UPDATE Stocks
                            SET QuantityReceived = QuantityReceived - @Quantity
                            WHERE StockID = @StockID";
                        var updateStockCmd = new SqlCommand(updateStockQuery, conn, transaction);
                        updateStockCmd.Parameters.AddWithValue("@Quantity", quantity);
                        updateStockCmd.Parameters.AddWithValue("@StockID", stockShelf.StockID);
                        updateStockCmd.ExecuteNonQuery();

                        // Update the Item_Stock table
                        string updateItemStockQuery = @"
                            UPDATE Item_Stock
                            SET Quantity = Quantity - @Quantity
                            WHERE ItemCode = @ItemCode AND StockID = @StockID";
                        var updateItemStockCmd = new SqlCommand(updateItemStockQuery, conn, transaction);
                        updateItemStockCmd.Parameters.AddWithValue("@Quantity", quantity);
                        updateItemStockCmd.Parameters.AddWithValue("@ItemCode", itemCode);
                        updateItemStockCmd.Parameters.AddWithValue("@StockID", stockShelf.StockID);
                        updateItemStockCmd.ExecuteNonQuery();

                        // Insert or update Stock_Shelf table
                        string insertOrUpdateStockShelfQuery = @"
                            IF EXISTS (SELECT 1 FROM Stock_Shelf WHERE StockID = @StockID AND ShelfID = @ShelfID)
                            BEGIN
                                UPDATE Stock_Shelf
                                SET QuantityTransferred = QuantityTransferred + @Quantity
                                WHERE StockID = @StockID AND ShelfID = @ShelfID;
                            END
                            ELSE
                            BEGIN
                                INSERT INTO Stock_Shelf (StockID, ShelfID, ItemCode, QuantityTransferred)
                                VALUES (@StockID, @ShelfID, @ItemCode, @Quantity);
                            END";
                        var insertOrUpdateStockShelfCmd = new SqlCommand(insertOrUpdateStockShelfQuery, conn, transaction);
                        insertOrUpdateStockShelfCmd.Parameters.AddWithValue("@Quantity", quantity);
                        insertOrUpdateStockShelfCmd.Parameters.AddWithValue("@StockID", stockShelf.StockID);
                        insertOrUpdateStockShelfCmd.Parameters.AddWithValue("@ShelfID", shelfID);
                        insertOrUpdateStockShelfCmd.Parameters.AddWithValue("@ItemCode", itemCode);
                        insertOrUpdateStockShelfCmd.ExecuteNonQuery();

                        // Update the Shelf table
                        string updateShelfQuery = @"
                            UPDATE Shelf
                            SET Quantity = Quantity + @Quantity
                            WHERE ShelfID = @ShelfID";
                        var updateShelfCmd = new SqlCommand(updateShelfQuery, conn, transaction);
                        updateShelfCmd.Parameters.AddWithValue("@Quantity", quantity);
                        updateShelfCmd.Parameters.AddWithValue("@ShelfID", shelfID);
                        updateShelfCmd.ExecuteNonQuery();

                        // Commit the transaction
                        transaction.Commit();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }
    }
}
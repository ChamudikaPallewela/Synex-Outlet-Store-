using Client.DTO;
using Client.Template;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace Client.TableDataGateway
{
    public class ReportGateway : BaseDatabaseOperation
    {
        public List<SalesReportItemDto> GenerateTotalSalesReport(DateTime selectedDate)
        {
            string query = @"
                SELECT i.ItemCode, i.ItemName, SUM(ib.Quantity) AS Quantity, SUM(ib.Quantity * i.Price) AS Revenue
                FROM Item_Bill ib
                INNER JOIN Items i ON ib.ItemCode = i.ItemCode
                INNER JOIN Bill b ON ib.BillNumber = b.BillNumber
                WHERE CAST(b.BillDate AS DATE) = @SelectedDate
                GROUP BY i.ItemCode, i.ItemName";

            using (var conn = CreateConnection())
            {
                var cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@SelectedDate", selectedDate);
                conn.Open();
                var reader = cmd.ExecuteReader();
                var salesReport = new List<SalesReportItemDto>();

                while (reader.Read())
                {
                    salesReport.Add(new SalesReportItemDto
                    {
                        ItemCode = reader["ItemCode"].ToString(),
                        ItemName = reader["ItemName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"]),
                        Revenue = Convert.ToDecimal(reader["Revenue"])
                    });
                }

                return salesReport;
            }
        }

        public List<ReshelvedItemDto> GenerateReshelvedItemsReport()
        {
            string query = @"
                SELECT i.ItemCode, i.ItemName, SUM(s.QuantityTransferred) AS Quantity
                FROM Stock_Shelf s
                INNER JOIN Items i ON s.ItemCode = i.ItemCode
                WHERE s.QuantityTransferred > 0
                GROUP BY i.ItemCode, i.ItemName";

            using (var conn = CreateConnection())
            {
                var cmd = new SqlCommand(query, conn);
                conn.Open();
                var reader = cmd.ExecuteReader();
                var reshelvedItemsReport = new List<ReshelvedItemDto>();

                while (reader.Read())
                {
                    reshelvedItemsReport.Add(new ReshelvedItemDto
                    {
                        ItemCode = reader["ItemCode"].ToString(),
                        ItemName = reader["ItemName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"])
                    });
                }

                return reshelvedItemsReport;
            }
        }

        public List<ReorderLevelItemDto> GenerateReorderLevelReport()
        {
            string query = @"
                SELECT i.ItemCode, i.ItemName, SUM(s.QuantityTransferred) AS Quantity
                FROM Stock_Shelf s
                INNER JOIN Items i ON s.ItemCode = i.ItemCode
                GROUP BY i.ItemCode, i.ItemName
                HAVING SUM(s.QuantityTransferred) < 50";

            using (var conn = CreateConnection())
            {
                var cmd = new SqlCommand(query, conn);
                conn.Open();
                var reader = cmd.ExecuteReader();
                var reorderLevelReport = new List<ReorderLevelItemDto>();

                while (reader.Read())
                {
                    reorderLevelReport.Add(new ReorderLevelItemDto
                    {
                        ItemCode = reader["ItemCode"].ToString(),
                        ItemName = reader["ItemName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"])
                    });
                }

                return reorderLevelReport;
            }
        }

        public List<StockReportItemDto> GenerateStockReport()
        {
            string query = @"
                SELECT s.StockID, i.ItemCode, i.ItemName, s.QuantityTransferred AS Quantity
                FROM Stock_Shelf s
                INNER JOIN Items i ON s.ItemCode = i.ItemCode";

            using (var conn = CreateConnection())
            {
                var cmd = new SqlCommand(query, conn);
                conn.Open();
                var reader = cmd.ExecuteReader();
                var stockReport = new List<StockReportItemDto>();

                while (reader.Read())
                {
                    stockReport.Add(new StockReportItemDto
                    {
                        StockID = Convert.ToInt32(reader["StockID"]),
                        ItemCode = reader["ItemCode"].ToString(),
                        ItemName = reader["ItemName"].ToString(),
                        Quantity = Convert.ToInt32(reader["Quantity"])
                    });
                }

                return stockReport;
            }
        }

        public List<BillReportItemDto> GenerateBillReport()
        {
            string query = @"
                SELECT BillNumber, BillDate, TotalBillAmount, CashTendered, ChangeDue
                FROM Bill";

            using (var conn = CreateConnection())
            {
                var cmd = new SqlCommand(query, conn);
                conn.Open();
                var reader = cmd.ExecuteReader();
                var billReport = new List<BillReportItemDto>();

                while (reader.Read())
                {
                    billReport.Add(new BillReportItemDto
                    {
                        BillNumber = Convert.ToInt32(reader["BillNumber"]),
                        BillDate = Convert.ToDateTime(reader["BillDate"]),
                        TotalAmount = Convert.ToDecimal(reader["TotalBillAmount"]),
                        CashTendered = Convert.ToDecimal(reader["CashTendered"]),
                        ChangeDue = Convert.ToDecimal(reader["ChangeDue"])
                    });
                }

                return billReport;
            }
        }
    }
}
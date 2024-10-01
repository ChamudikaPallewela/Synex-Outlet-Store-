using Client.DTO;
using Client.Facade;
using Client.TableDataGateway;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class AsynchronousReportTests
    {
        private const string ServerAddress = "127.0.0.1";
        private const int ServerPort = 5000;

        private ReportFacade _reportFacade;

        [TestInitialize]
        public void Setup()
        {
            // Initialize the ReportFacade before each test
            _reportFacade = new ReportFacade();
        }

        [TestMethod]
        public async Task TestClient1_GenerateTotalSalesReport()
        {
            DateTime selectedDate = DateTime.Today;
            var report = await Task.Run(() => _reportFacade.GenerateTotalSalesReport(selectedDate));

            Console.WriteLine("Total Sales Report:");
            foreach (var item in report)
            {
                Console.WriteLine($"ItemCode: {item.ItemCode}, ItemName: {item.ItemName}, Quantity: {item.Quantity}, Revenue: {item.Revenue}");
            }
        }

        [TestMethod]
        public async Task TestClient2_GenerateReshelvedItemsReport()
        {
            var report = await Task.Run(() => _reportFacade.GenerateReshelvedItemsReport());

            Console.WriteLine("Reshelved Items Report:");
            foreach (var item in report)
            {
                Console.WriteLine($"ItemCode: {item.ItemCode}, ItemName: {item.ItemName}, Quantity: {item.Quantity}");
            }
        }

        [TestMethod]
        public async Task TestClient3_GenerateReorderLevelReport()
        {
            var report = await Task.Run(() => _reportFacade.GenerateReorderLevelReport());

            Console.WriteLine("Reorder Level Report:");
            foreach (var item in report)
            {
                Console.WriteLine($"ItemCode: {item.ItemCode}, ItemName: {item.ItemName}, Quantity: {item.Quantity}");
            }
        }

        [TestMethod]
        public async Task TestClient4_GenerateStockReport()
        {
            var report = await Task.Run(() => _reportFacade.GenerateStockReport());

            Console.WriteLine("Stock Report:");
            foreach (var item in report)
            {
                Console.WriteLine($"StockID: {item.StockID}, ItemCode: {item.ItemCode}, ItemName: {item.ItemName}, Quantity: {item.Quantity}");
            }
        }

        [TestMethod]
        public async Task TestClient5_GenerateBillReport()
        {
            var report = await Task.Run(() => _reportFacade.GenerateBillReport());

            Console.WriteLine("Bill Report:");
            foreach (var item in report)
            {
                Console.WriteLine($"BillNumber: {item.BillNumber}, BillDate: {item.BillDate}, TotalAmount: {item.TotalAmount}, CashTendered: {item.CashTendered}, ChangeDue: {item.ChangeDue}");
            }
        }

        [TestMethod]
        public async Task SimulateMultipleReportClients()
        {
            // Run multiple report generation requests concurrently
            var reportTasks = new[]
            {
                TestClient1_GenerateTotalSalesReport(),
                TestClient2_GenerateReshelvedItemsReport(),
                TestClient3_GenerateReorderLevelReport(),
                TestClient4_GenerateStockReport(),
                TestClient5_GenerateBillReport()
            };

            await Task.WhenAll(reportTasks);
        }
    }
}

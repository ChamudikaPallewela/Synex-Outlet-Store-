using Client.DTO;
using Client.Facade;
using Client.TableDataGateway;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;

namespace Tests
{
    [TestClass]
    public class StockShelfTests
    {
        private readonly object _lock = new object(); // Lock object for thread safety

        [TestMethod]
        public async Task MultipleClients_ShouldAddAndRetrieveItemsSimultaneously()
        {
            // Create mocks for StockShelfFacade and IStockShelfGateway
            var mockStockShelfFacade = new Mock<StockShelfFacade> { CallBase = true };
            var mockStockShelfGateway = new Mock<IStockShelfGateway>();

            // Use an in-memory list to simulate the stock shelf database
            var mockStockShelfData = new List<StockShelfDto>();

            // Mock the TransferItem method to simulate adding items to the stock shelf
            mockStockShelfGateway.Setup(x => x.TransferItem(It.IsAny<StockShelfDto>(), It.IsAny<int>()))
                .Callback<StockShelfDto, int>((stockShelf, quantity) =>
                {
                    lock (_lock) // Ensure thread safety by locking access to the shared resource
                    {
                        var existingItem = mockStockShelfData.Find(item => item.StockID == stockShelf.StockID);
                        if (existingItem != null)
                        {
                            existingItem.Quantity += quantity; // Increment quantity if the item already exists
                        }
                        else
                        {
                            mockStockShelfData.Add(stockShelf); // Add new item if it doesn't exist
                        }
                    }
                });

            // Mock RetrieveStockShelves to return a mock DataTable representing the stock shelf
            mockStockShelfGateway.Setup(x => x.RetrieveStockShelves()).Returns(() =>
            {
                lock (_lock) // Locking to ensure no concurrent modification
                {
                    var dataTable = new DataTable();
                    dataTable.Columns.Add("StockID");
                    dataTable.Columns.Add("ItemName");
                    dataTable.Columns.Add("Quantity");
                    dataTable.Columns.Add("ExpiryDate");

                    // Populate the DataTable with data from the in-memory list
                    foreach (var item in mockStockShelfData)
                    {
                        var row = dataTable.NewRow();
                        row["StockID"] = item.StockID;
                        row["ItemName"] = item.ItemName;
                        row["Quantity"] = item.Quantity;
                        row["ExpiryDate"] = item.ExpiryDate;
                        dataTable.Rows.Add(row);
                    }

                    return dataTable;
                }
            });

            var tasks = new List<Task>();

            // Simulate multiple clients adding and retrieving items
            for (int i = 1; i <= 3; i++)
            {
                int clientId = i;
                tasks.Add(Task.Run(async () =>
                {
                    for (int j = 0; j < 10; j++)
                    {
                        var stockShelf = new StockShelfDto
                        {
                            StockID = clientId * 100 + j,
                            ItemName = $"Item-{clientId}-{j}",
                            Quantity = 10,
                            ExpiryDate = DateTime.Now.AddMonths(6)
                        };

                        int quantity = 5;

                        // Act - Simulate transferring the item to the shelf
                        mockStockShelfGateway.Object.TransferItem(stockShelf, quantity);

                        // Simulate retrieving items from the shelf
                        var items = mockStockShelfGateway.Object.RetrieveStockShelves();

                        // Introduce a small delay to simulate rapid succession
                        await Task.Delay(50);
                    }
                }));
            }

            // Wait for all client tasks to complete
            await Task.WhenAll(tasks);

            // Verify that TransferItem and RetrieveStockShelves were called the expected number of times
            mockStockShelfGateway.Verify(x => x.TransferItem(It.IsAny<StockShelfDto>(), It.IsAny<int>()), Times.Exactly(30), "TransferItem was not called the expected number of times.");
            mockStockShelfGateway.Verify(x => x.RetrieveStockShelves(), Times.AtLeast(30), "RetrieveStockShelves was not called the expected number of times.");
        }
    }
}

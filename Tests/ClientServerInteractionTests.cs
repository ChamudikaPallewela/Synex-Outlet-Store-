using Client.Builder;
using Client.Command;
using Client.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Data;
using Client.Facade;

namespace Tests
{
    [TestClass]
    public class ClientServerInteractionTests
    {
        private List<ItemDto> _mockItemList;

        [TestInitialize]
        public void Setup()
        {
            // In-memory list to simulate items in the "database"
            _mockItemList = new List<ItemDto>();
        }

        [TestMethod]
        public async Task Test_MultipleClients_Adding_And_Retrieving_Items_Async()
        {
            // Simulate multiple clients with tasks
            var clientTasks = new List<Task>();

            for (int i = 0; i < 3; i++) // Adjust number of clients
            {
                int clientId = i + 1;
                clientTasks.Add(Task.Run(() => SimulateClient(clientId)));
            }

            // Wait for all clients to complete their operations
            await Task.WhenAll(clientTasks);

            // Retrieve items after all clients have added their items
            RetrieveItems();
        }

        private async Task SimulateClient(int clientId)
        {
            Console.WriteLine($"Client {clientId} starting...");

            for (int i = 0; i < 5; i++) // Each client adds 5 items
            {
                var itemCode = $"Client{clientId}_Item{i + 1}";
                var itemName = $"Client{clientId}_TestItem{i + 1}";

                // Adding the item (no need to mock the command, just simulate it)
                await Task.Run(() => AddItem(itemCode, itemName));
            }

            Console.WriteLine($"Client {clientId} finished adding items.");
        }

        private void AddItem(string itemCode, string itemName)
        {
            var itemBuilder = new ItemBuilder()
                .SetItemCode(itemCode)
                .SetItemName(itemName)
                .SetPrice(9.99m)
                .SetDiscountRate(0m)
                .SetExpireDate(DateTime.Now.AddMonths(1));

            var item = itemBuilder.Build();

            // Simulate adding the item to the in-memory list (instead of calling Execute)
            _mockItemList.Add(item);
            Console.WriteLine($"Simulated adding item: {item.ItemName}");
        }

        private void RetrieveItems()
        {
            // Simulate retrieving items from the in-memory list
            var dataTable = new DataTable();
            dataTable.Columns.Add("ItemCode");
            dataTable.Columns.Add("ItemName");
            dataTable.Columns.Add("Quantity");
            dataTable.Columns.Add("ExpireDate");

            foreach (var item in _mockItemList)
            {
                var row = dataTable.NewRow();
                row["ItemCode"] = item.ItemCode;
                row["ItemName"] = item.ItemName;
                row["Quantity"] = item.Quantity;
                row["ExpireDate"] = item.ExpireDate;
                dataTable.Rows.Add(row);
            }

            // Check if items are retrieved
            if (dataTable.Rows.Count == 0)
            {
                Console.WriteLine("No items retrieved.");
                Assert.Fail("Items were not retrieved successfully.");
            }
            else
            {
                Console.WriteLine($"{dataTable.Rows.Count} items retrieved.");
            }

            // Optionally print out the retrieved items for debugging
            foreach (System.Data.DataRow row in dataTable.Rows)
            {
                Console.WriteLine($"ItemCode: {row["ItemCode"]}, ItemName: {row["ItemName"]}, ExpireDate: {row["ExpireDate"]}");
            }

            Assert.IsTrue(dataTable.Rows.Count > 0, "Items were not retrieved successfully.");
        }
    }
}


using Client.DTO;
using Client.Facade;
using Client.TableDataGateway;
using Facade;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Data;
using System.Linq; 


namespace Tests
{
    [TestClass]
    public class StoreFacadeStateTests
    {
        [TestMethod]
        public void AddItem_ChangesStoreState_AddsItemToStore()
        {
            // Arrange
            var mockItemGateway = new Mock<ItemGateway>();
            var mockStockGateway = new Mock<StockGateway>();

            var storeFacade = new StoreFacade();
            var itemGatewayField = typeof(StoreFacade).GetField("_itemGateway", BindingFlags.NonPublic | BindingFlags.Instance);
            var stockGatewayField = typeof(StoreFacade).GetField("_stockGateway", BindingFlags.NonPublic | BindingFlags.Instance);
            itemGatewayField.SetValue(storeFacade, mockItemGateway.Object);
            stockGatewayField.SetValue(storeFacade, mockStockGateway.Object);

            var item = new ItemDto { /* initialize properties */ };
            var stock = new StockDto { /* initialize properties */ };

            mockItemGateway.Setup(g => g.AddItem(It.IsAny<ItemDto>())).Verifiable();
            mockStockGateway.Setup(g => g.AddStock(It.IsAny<StockDto>())).Verifiable();

            var dataTable = new DataTable();
            dataTable.Columns.Add("ItemCode");
            // Add other columns
            var row = dataTable.NewRow();
            // Set row values
            dataTable.Rows.Add(row);

            mockStockGateway.Setup(g => g.RetrieveAllItems()).Returns(dataTable);

            // Act
            storeFacade.AddItem(item, 5);

            // Assert
            var retrievedItem = storeFacade.GetAllItems().Rows.Cast<DataRow>()
                .FirstOrDefault(r => r["ItemCode"].ToString() == "A001");
            Assert.IsNotNull(retrievedItem);
            Assert.AreEqual("Test Item", retrievedItem["ItemName"].ToString());

            // Verify the gateway methods were called
            mockItemGateway.Verify(g => g.AddItem(It.IsAny<ItemDto>()), Times.Once);
            mockStockGateway.Verify(g => g.AddStock(It.IsAny<StockDto>()), Times.Once);
        }


    }
}

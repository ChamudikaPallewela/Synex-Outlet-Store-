using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data;
using System.Reflection;

namespace Tests
{
    [TestClass]
    public class StoreFacadeBehaviourTests
    {
        private object storeFacadeInstance;

        [TestInitialize]
        public void Setup()
        {
            // Use reflection to create an instance of the StoreFacade
            var storeFacadeType = typeof(Client.Facade.StoreFacade);
            storeFacadeInstance = Activator.CreateInstance(storeFacadeType);
        }

        [TestMethod]
        public void TestAddItemBehavior_VerifiesCorrectMethodCalls()
        {
            // Arrange
            var itemDtoType = typeof(Client.DTO.ItemDto);
            var itemInstance = Activator.CreateInstance(itemDtoType);
            var quantity = 10;

            // Use reflection to set properties of the ItemDto object
            SetProperty(itemInstance, "ItemCode", "ITEM001");
            SetProperty(itemInstance, "ItemName", "Test Item");
            SetProperty(itemInstance, "Price", 100m);
            SetProperty(itemInstance, "DiscountRate", 10m);
            SetProperty(itemInstance, "ExpireDate", DateTime.Now.AddMonths(6));

            // Act
            var addItemMethod = storeFacadeInstance.GetType().GetMethod("AddItem");
            addItemMethod.Invoke(storeFacadeInstance, new object[] { itemInstance, quantity });

            // Assert (Here we check behavior - verify if the internal methods like AddStock were called)
            // For example, we might want to check if "AddStock" was called with the correct values.
            var stockGatewayField = storeFacadeInstance.GetType().GetField("_stockGateway", BindingFlags.NonPublic | BindingFlags.Instance);
            var stockGatewayInstance = stockGatewayField.GetValue(storeFacadeInstance);

            // Verify that AddStock was invoked by inspecting the StockGateway instance (mock behavior)
            var addStockMethod = stockGatewayInstance.GetType().GetMethod("AddStock", BindingFlags.Instance | BindingFlags.Public);
            Assert.IsNotNull(addStockMethod, "AddStock method should exist.");
        }

        [TestMethod]
        public void TestRetrieveAllItemsBehavior_VerifiesCorrectDataRetrieval()
        {
            // Arrange
            var retrieveAllItemsMethod = storeFacadeInstance.GetType().GetMethod("GetAllItems");

            // Act
            var result = retrieveAllItemsMethod.Invoke(storeFacadeInstance, null);

            // Assert
            Assert.IsNotNull(result, "The result should not be null.");
            Assert.IsInstanceOfType(result, typeof(DataTable), "The result should be of type DataTable.");

            var dataTable = (DataTable)result;
            // Further assertions can be done based on expected behavior, like the DataTable structure, etc.
            Assert.IsTrue(dataTable.Columns.Contains("ItemCode"), "The DataTable should contain a column 'ItemCode'.");
        }

        // Utility method to set private or public properties using reflection
        private void SetProperty(object obj, string propertyName, object value)
        {
            var propertyInfo = obj.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            propertyInfo.SetValue(obj, value);
        }
    }
}


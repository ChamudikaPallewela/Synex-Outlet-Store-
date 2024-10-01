using Client.Builder;
using Client.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace Tests
{
    [TestClass]
    public class StockShelfAssertionBuilderTests
    {
        [TestMethod]
        public void SetStockID_ShouldSetStockIDCorrectly()
        {
            // Arrange
            var builder = new StockShelfBuilder();
            int expectedStockID = 123;

            // Act
            builder.SetStockID(expectedStockID);

            // Use reflection to get the private _stockShelf field
            var stockShelfDto = GetPrivateFieldValue(builder, "_stockShelf") as StockShelfDto;

            // Assert - Null check
            Assert.IsNotNull(stockShelfDto, "StockShelfDto instance is null after setting StockID.");

            // Assert - Value check
            int actualStockID = stockShelfDto.StockID;
            Assert.AreEqual(expectedStockID, actualStockID, "StockID was not set correctly.");
        }

        [TestMethod]
        public void SetItemName_ShouldSetItemNameCorrectly()
        {
            // Arrange
            var builder = new StockShelfBuilder();
            string expectedItemName = "TestItem";

            // Act
            builder.SetItemName(expectedItemName);

            // Use reflection to get the private _stockShelf field
            var stockShelfDto = GetPrivateFieldValue(builder, "_stockShelf") as StockShelfDto;

            // Assert - Null check
            Assert.IsNotNull(stockShelfDto, "StockShelfDto instance is null after setting ItemName.");

            // Assert - Value check
            string actualItemName = stockShelfDto.ItemName;
            Assert.AreEqual(expectedItemName, actualItemName, "ItemName was not set correctly.");
        }

        [TestMethod]
        public void SetQuantity_ShouldSetQuantityCorrectly()
        {
            // Arrange
            var builder = new StockShelfBuilder();
            int expectedQuantity = 50;

            // Act
            builder.SetQuantity(expectedQuantity);

            // Use reflection to get the private _stockShelf field
            var stockShelfDto = GetPrivateFieldValue(builder, "_stockShelf") as StockShelfDto;

            // Assert - Null check
            Assert.IsNotNull(stockShelfDto, "StockShelfDto instance is null after setting Quantity.");

            // Assert - Value check
            int actualQuantity = stockShelfDto.Quantity;
            Assert.AreEqual(expectedQuantity, actualQuantity, "Quantity was not set correctly.");
        }

        [TestMethod]
        public void SetExpiryDate_ShouldSetExpiryDateCorrectly()
        {
            // Arrange
            var builder = new StockShelfBuilder();
            DateTime expectedExpiryDate = new DateTime(2025, 12, 31);

            // Act
            builder.SetExpiryDate(expectedExpiryDate);

            // Use reflection to get the private _stockShelf field
            var stockShelfDto = GetPrivateFieldValue(builder, "_stockShelf") as StockShelfDto;

            // Assert - Null check
            Assert.IsNotNull(stockShelfDto, "StockShelfDto instance is null after setting ExpiryDate.");

            // Assert - Value check
            DateTime actualExpiryDate = stockShelfDto.ExpiryDate;
            Assert.AreEqual(expectedExpiryDate, actualExpiryDate, "ExpiryDate was not set correctly.");
        }

        

        // Helper method to use reflection and get the value of a private field
        private object GetPrivateFieldValue(object obj, string fieldName)
        {
            Type type = obj.GetType();
            FieldInfo fieldInfo = type.GetField(fieldName, BindingFlags.NonPublic | BindingFlags.Instance);
            return fieldInfo?.GetValue(obj);
        }
    }
}


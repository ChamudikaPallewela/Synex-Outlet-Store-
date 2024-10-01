using Client.Builder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests
{
    [TestClass]
    public class ItemBuilderAssertionTests
    {
        [TestMethod]
        public void Build_ShouldSetAllPropertiesCorrectly_WithAssertions()
        {
            // Arrange
            var expectedItemCode = "ABC123";
            var expectedItemName = "TestItem";
            var expectedPrice = 99.99m;
            var expectedDiscountRate = 10m;
            var expectedExpireDate = new DateTime(2025, 12, 31);

            // Act
            var itemBuilder = new ItemBuilder()
                .SetItemCode(expectedItemCode)
                .SetItemName(expectedItemName)
                .SetPrice(expectedPrice)
                .SetDiscountRate(expectedDiscountRate)
                .SetExpireDate(expectedExpireDate);

            var item = itemBuilder.Build();

            // Assert
            Assert.IsNotNull(item, "Item object should not be null.");

            // Assertions for each property
            Assert.AreEqual(expectedItemCode, item.ItemCode, "Item code was not set correctly.");
            Assert.AreEqual(expectedItemName, item.ItemName, "Item name was not set correctly.");
            Assert.AreEqual(expectedPrice, item.Price, "Price was not set correctly.");
            Assert.AreEqual(expectedDiscountRate, item.DiscountRate, "Discount rate was not set correctly.");
            Assert.AreEqual(expectedExpireDate, item.ExpireDate, "Expire date was not set correctly.");

            // Additional Assertions
            Assert.IsInstanceOfType(item.Price, typeof(decimal), "Price should be a decimal value.");
            Assert.IsTrue(item.Price > 0, "Price should be a positive value.");
            Assert.IsTrue(item.DiscountRate >= 0 && item.DiscountRate <= 100, "Discount rate should be between 0 and 100.");
            Assert.IsInstanceOfType(item.ExpireDate, typeof(DateTime), "ExpireDate should be of DateTime type.");
            Assert.IsTrue(item.ExpireDate > DateTime.Now, "ExpireDate should be in the future.");
        }
    }
}

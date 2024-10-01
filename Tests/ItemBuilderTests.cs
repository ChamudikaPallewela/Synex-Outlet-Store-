using Client.Builder;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace Tests
{
    [TestClass]
    public class ItemBuilderTests
    {
        [TestMethod]
        public void Build_ShouldSetAllPropertiesCorrectly()
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
            Assert.AreEqual(expectedItemCode, item.ItemCode, "Item code was not set correctly.");
            Assert.AreEqual(expectedItemName, item.ItemName, "Item name was not set correctly.");
            Assert.AreEqual(expectedPrice, item.Price, "Price was not set correctly.");
            Assert.AreEqual(expectedDiscountRate, item.DiscountRate, "Discount rate was not set correctly.");
            Assert.AreEqual(expectedExpireDate, item.ExpireDate, "Expire date was not set correctly.");
        }
    }
}

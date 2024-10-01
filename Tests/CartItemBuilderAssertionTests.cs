using Client.Builder;
using Client.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace Tests
{
    [TestClass]
    public class CartItemBuilderAssertionTests
    {
        private CartItemBuilder _builder;
        private Type _builderType;
        private FieldInfo _cartItemField;

        [TestInitialize]
        public void Setup()
        {
            _builder = new CartItemBuilder();
            _builderType = typeof(CartItemBuilder);
            // Access the private field '_cartItem' via reflection
            _cartItemField = _builderType.GetField("_cartItem", BindingFlags.NonPublic | BindingFlags.Instance);
        }

        [TestMethod]
        public void Assert_SetItemCode_ShouldSetItemCodeCorrectly()
        {
            // Act
            _builder.SetItemCode("ITEM001");
            var cartItem = GetCartItemDto();

            // Assert
            Assert.IsNotNull(cartItem.ItemCode); // Ensure item code is not null
            Assert.AreEqual("ITEM001", cartItem.ItemCode); // Check if the correct code is set
        }

        [TestMethod]
        public void Assert_SetItemName_ShouldSetItemNameCorrectly()
        {
            // Act
            _builder.SetItemName("TestItem");
            var cartItem = GetCartItemDto();

            // Assert
            Assert.IsNotNull(cartItem.ItemName); // Ensure item name is not null
            Assert.AreEqual("TestItem", cartItem.ItemName); // Check if the correct name is set
        }

        [TestMethod]
        public void Assert_SetQuantity_ShouldSetQuantityCorrectly()
        {
            // Act
            _builder.SetQuantity(10);
            var cartItem = GetCartItemDto();

            // Assert
            Assert.IsTrue(cartItem.Quantity > 0); // Quantity should be greater than zero
            Assert.AreEqual(10, cartItem.Quantity); // Check if the correct quantity is set
        }

        [TestMethod]
        public void Assert_SetPrice_ShouldSetPriceCorrectly()
        {
            // Act
            _builder.SetPrice(99.99m);
            var cartItem = GetCartItemDto();

            // Assert
            Assert.IsTrue(cartItem.Price > 0); // Price should be a positive value
            Assert.AreEqual(99.99m, cartItem.Price); // Check if the correct price is set
        }

        [TestMethod]
        public void Assert_SetDiscountRate_ShouldSetDiscountRateCorrectly()
        {
            // Act
            _builder.SetDiscountRate(5.0m);
            var cartItem = GetCartItemDto();

            // Assert
            Assert.IsTrue(cartItem.DiscountRate >= 0); // Discount rate should be zero or positive
            Assert.AreEqual(5.0m, cartItem.DiscountRate); // Check if the correct discount rate is set
        }

        [TestMethod]
        public void Assert_Build_ShouldReturnCartItemDtoCorrectly()
        {
            // Arrange
            _builder.SetItemCode("ITEM001")
                    .SetItemName("TestItem")
                    .SetQuantity(10)
                    .SetPrice(99.99m)
                    .SetDiscountRate(5.0m);

            // Act
            var cartItem = _builder.Build();

            // Assert
            Assert.IsNotNull(cartItem); // Ensure the object is not null
            Assert.AreEqual("ITEM001", cartItem.ItemCode); // Check ItemCode is correct
            Assert.AreEqual("TestItem", cartItem.ItemName); // Check ItemName is correct
            Assert.AreEqual(10, cartItem.Quantity); // Check Quantity is correct
            Assert.AreEqual(99.99m, cartItem.Price); // Check Price is correct
            Assert.AreEqual(5.0m, cartItem.DiscountRate); // Check DiscountRate is correct
        }

        // Helper method to retrieve private _cartItem field using reflection
        private CartItemDto GetCartItemDto()
        {
            return (CartItemDto)_cartItemField.GetValue(_builder);
        }
    }
}

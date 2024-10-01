using Client.Builder;
using Client.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace Tests
{
    [TestClass]
    public class CartItemBuilderTests
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
        public void SetItemCode_ShouldSetItemCodeCorrectly()
        {
            // Act
            _builder.SetItemCode("ITEM001");
            var cartItem = GetCartItemDto();

            // Assert
            Assert.AreEqual("ITEM001", cartItem.ItemCode);
        }

        [TestMethod]
        public void SetItemName_ShouldSetItemNameCorrectly()
        {
            // Act
            _builder.SetItemName("TestItem");
            var cartItem = GetCartItemDto();

            // Assert
            Assert.AreEqual("TestItem", cartItem.ItemName);
        }

        [TestMethod]
        public void SetQuantity_ShouldSetQuantityCorrectly()
        {
            // Act
            _builder.SetQuantity(10);
            var cartItem = GetCartItemDto();

            // Assert
            Assert.AreEqual(10, cartItem.Quantity);
        }

        [TestMethod]
        public void SetPrice_ShouldSetPriceCorrectly()
        {
            // Act
            _builder.SetPrice(99.99m);
            var cartItem = GetCartItemDto();

            // Assert
            Assert.AreEqual(99.99m, cartItem.Price);
        }

        [TestMethod]
        public void SetDiscountRate_ShouldSetDiscountRateCorrectly()
        {
            // Act
            _builder.SetDiscountRate(5.0m);
            var cartItem = GetCartItemDto();

            // Assert
            Assert.AreEqual(5.0m, cartItem.DiscountRate);
        }

        [TestMethod]
        public void Build_ShouldReturnCartItemDtoCorrectly()
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
            Assert.AreEqual("ITEM001", cartItem.ItemCode);
            Assert.AreEqual("TestItem", cartItem.ItemName);
            Assert.AreEqual(10, cartItem.Quantity);
            Assert.AreEqual(99.99m, cartItem.Price);
            Assert.AreEqual(5.0m, cartItem.DiscountRate);
        }

        // Helper method to retrieve private _cartItem field using reflection
        private CartItemDto GetCartItemDto()
        {
            return (CartItemDto)_cartItemField.GetValue(_builder);
        }
    }
}

using Client.Builder;
using Client.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace Tests
{
    [TestClass]
    public class CartItemBuilderBehaviorTests
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
        public void Verify_BuilderMethodsCalledInSequence()
        {
            // Act
            _builder.SetItemCode("ITEM001")
                    .SetItemName("TestItem")
                    .SetQuantity(5)
                    .SetPrice(99.99m)
                    .SetDiscountRate(10m);

            var cartItem = _builder.Build();

            // Assert behavior (i.e., that the sequence of method calls results in a correctly built object)
            Assert.AreEqual("ITEM001", cartItem.ItemCode, "ItemCode should be set correctly");
            Assert.AreEqual("TestItem", cartItem.ItemName, "ItemName should be set correctly");
            Assert.AreEqual(5, cartItem.Quantity, "Quantity should be set correctly");
            Assert.AreEqual(99.99m, cartItem.Price, "Price should be set correctly");
            Assert.AreEqual(10m, cartItem.DiscountRate, "DiscountRate should be set correctly");
        }

        [TestMethod]
        public void Verify_SetItemCode_ThenBuild()
        {
            // Act
            _builder.SetItemCode("ITEM001");
            var cartItem = _builder.Build();

            // Assert the behavior after calling only SetItemCode
            Assert.AreEqual("ITEM001", cartItem.ItemCode, "ItemCode should be set");
            Assert.IsNull(cartItem.ItemName, "ItemName should not be set yet");
            Assert.AreEqual(0, cartItem.Quantity, "Quantity should not be set yet");
            Assert.AreEqual(0m, cartItem.Price, "Price should not be set yet");
            Assert.AreEqual(0m, cartItem.DiscountRate, "DiscountRate should not be set yet");
        }

        [TestMethod]
        public void Verify_SetItemName_AfterSettingItemCode()
        {
            // Act
            _builder.SetItemCode("ITEM001")
                    .SetItemName("TestItem");
            var cartItem = _builder.Build();

            // Assert the behavior when SetItemName is called after SetItemCode
            Assert.AreEqual("ITEM001", cartItem.ItemCode, "ItemCode should be set correctly");
            Assert.AreEqual("TestItem", cartItem.ItemName, "ItemName should be set correctly");
            Assert.AreEqual(0, cartItem.Quantity, "Quantity should not be set yet");
            Assert.AreEqual(0m, cartItem.Price, "Price should not be set yet");
            Assert.AreEqual(0m, cartItem.DiscountRate, "DiscountRate should not be set yet");
        }

        [TestMethod]
        public void Verify_MultipleMethodCallsModifyCorrectly()
        {
            // Act
            _builder.SetItemCode("ITEM001")
                    .SetItemName("TestItem")
                    .SetQuantity(5);

            var cartItem = _builder.Build();

            // Assert behavior when multiple setters are called
            Assert.AreEqual("ITEM001", cartItem.ItemCode, "ItemCode should be set correctly");
            Assert.AreEqual("TestItem", cartItem.ItemName, "ItemName should be set correctly");
            Assert.AreEqual(5, cartItem.Quantity, "Quantity should be set correctly");
            Assert.AreEqual(0m, cartItem.Price, "Price should not be set yet");
            Assert.AreEqual(0m, cartItem.DiscountRate, "DiscountRate should not be set yet");
        }

        [TestMethod]
        public void Verify_SetPriceAndDiscountRate_AfterSettingItemCode()
        {
            // Act
            _builder.SetItemCode("ITEM001")
                    .SetPrice(100m)
                    .SetDiscountRate(5m);

            var cartItem = _builder.Build();

            // Assert behavior when price and discount rate are set
            Assert.AreEqual("ITEM001", cartItem.ItemCode, "ItemCode should be set correctly");
            Assert.AreEqual(100m, cartItem.Price, "Price should be set correctly");
            Assert.AreEqual(5m, cartItem.DiscountRate, "DiscountRate should be set correctly");
            Assert.AreEqual(0, cartItem.Quantity, "Quantity should not be set yet");
            Assert.IsNull(cartItem.ItemName, "ItemName should not be set yet");
        }

        // Helper method to retrieve private _cartItem field using reflection
        private CartItemDto GetCartItemDto()
        {
            return (CartItemDto)_cartItemField.GetValue(_builder);
        }
    }
}

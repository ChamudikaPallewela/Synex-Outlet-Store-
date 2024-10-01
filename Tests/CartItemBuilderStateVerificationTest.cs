using Client.Builder;
using Client.DTO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Reflection;

namespace Tests
{
    [TestClass]
    public class CartItemBuilderStateVerificationTests
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
        public void VerifyState_AfterSetItemCode()
        {
            // Act
            _builder.SetItemCode("ITEM001");
            var cartItem = GetCartItemDto();

            // Assert state
            Assert.AreEqual("ITEM001", cartItem.ItemCode, "ItemCode should be set to ITEM001");
            AssertStateNotChanged(cartItem, except: "ItemCode");
        }

        [TestMethod]
        public void VerifyState_AfterSetItemName()
        {
            // Act
            _builder.SetItemName("TestItem");
            var cartItem = GetCartItemDto();

            // Assert state
            Assert.AreEqual("TestItem", cartItem.ItemName, "ItemName should be set to TestItem");
            AssertStateNotChanged(cartItem, except: "ItemName");
        }

        [TestMethod]
        public void VerifyState_AfterSetQuantity()
        {
            // Act
            _builder.SetQuantity(10);
            var cartItem = GetCartItemDto();

            // Assert state
            Assert.AreEqual(10, cartItem.Quantity, "Quantity should be set to 10");
            AssertStateNotChanged(cartItem, except: "Quantity");
        }

        [TestMethod]
        public void VerifyState_AfterSetPrice()
        {
            // Act
            _builder.SetPrice(99.99m);
            var cartItem = GetCartItemDto();

            // Assert state
            Assert.AreEqual(99.99m, cartItem.Price, "Price should be set to 99.99");
            AssertStateNotChanged(cartItem, except: "Price");
        }

        [TestMethod]
        public void VerifyState_AfterSetDiscountRate()
        {
            // Act
            _builder.SetDiscountRate(5.0m);
            var cartItem = GetCartItemDto();

            // Assert state
            Assert.AreEqual(5.0m, cartItem.DiscountRate, "DiscountRate should be set to 5.0");
            AssertStateNotChanged(cartItem, except: "DiscountRate");
        }

        [TestMethod]
        public void VerifyState_AfterBuild()
        {
            // Arrange
            _builder.SetItemCode("ITEM001")
                    .SetItemName("TestItem")
                    .SetQuantity(10)
                    .SetPrice(99.99m)
                    .SetDiscountRate(5.0m);

            // Act
            var cartItem = _builder.Build();

            // Assert state after building
            Assert.AreEqual("ITEM001", cartItem.ItemCode, "ItemCode should be set to ITEM001");
            Assert.AreEqual("TestItem", cartItem.ItemName, "ItemName should be set to TestItem");
            Assert.AreEqual(10, cartItem.Quantity, "Quantity should be set to 10");
            Assert.AreEqual(99.99m, cartItem.Price, "Price should be set to 99.99");
            Assert.AreEqual(5.0m, cartItem.DiscountRate, "DiscountRate should be set to 5.0");
        }

        // Helper method to retrieve private _cartItem field using reflection
        private CartItemDto GetCartItemDto()
        {
            return (CartItemDto)_cartItemField.GetValue(_builder);
        }

        // Helper method to assert that no other state fields have changed except the one modified
        private void AssertStateNotChanged(CartItemDto cartItem, string except)
        {
            if (except != "ItemCode")
                Assert.IsNull(cartItem.ItemCode, "ItemCode should not be set");
            if (except != "ItemName")
                Assert.IsNull(cartItem.ItemName, "ItemName should not be set");
            if (except != "Quantity")
                Assert.AreEqual(0, cartItem.Quantity, "Quantity should not be set");
            if (except != "Price")
                Assert.AreEqual(0m, cartItem.Price, "Price should not be set");
            if (except != "DiscountRate")
                Assert.AreEqual(0m, cartItem.DiscountRate, "DiscountRate should not be set");
        }
    }
}

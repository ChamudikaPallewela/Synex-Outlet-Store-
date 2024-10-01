using Client.Command;
using Client.DTO;
using Client.Facade;
using Client.TableDataGateway;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Tests
{
    [TestClass]
    public class PurchaseFacadeTests
    {
        private Mock<IPurchaseGateway> _mockPurchaseGateway;
        private PurchaseFacade _purchaseFacade;

        [TestInitialize]
        public void Setup()
        {
            // Mock the IPurchaseGateway to simulate data layer
            _mockPurchaseGateway = new Mock<IPurchaseGateway>();
            _purchaseFacade = new PurchaseFacade(_mockPurchaseGateway.Object);
        }

        [TestMethod]
        public void TestAddToCartCommand()
        {
            // Arrange
            var cartItems = new List<CartItemDto>();
            var cartItem = new CartItemDto
            {
                ItemCode = "Item1",
                ItemName = "Item 1",
                Quantity = 2,
                Price = 10.00m,
                DiscountRate = 0m
            };

            var addToCartCommand = new AddToCartCommand();

            // Act
            addToCartCommand.Execute(cartItems, cartItem);

            // Assert
            Assert.AreEqual(1, cartItems.Count);  // Ensure an item is added to the cart
            Assert.AreEqual("Item1", cartItems[0].ItemCode);
            Assert.AreEqual(2, cartItems[0].Quantity);
        }

        [TestMethod]
        public void TestPurchaseFacadeLoadItems_NoDatabaseInteraction()
        {
            // Arrange: Mock some item data
            var mockItems = new List<ItemDto>
    {
        new ItemDto
        {
            ItemCode = "Item1",
            ItemName = "Item 1",
            Price = 10.00m,
            DiscountRate = 0m,
            ExpireDate = DateTime.Now.AddDays(10),
            Quantity = 5
        },
        new ItemDto
        {
            ItemCode = "Item2",
            ItemName = "Item 2",
            Price = 20.00m,
            DiscountRate = 0m,
            ExpireDate = DateTime.Now.AddDays(20),
            Quantity = 3
        }
    };

            // Setup the mock PurchaseGateway to return the mockItems list
            _mockPurchaseGateway.Setup(g => g.LoadItems()).Returns(mockItems);

            // Create the PurchaseFacade with the mock PurchaseGateway
            var purchaseFacade = new PurchaseFacade(_mockPurchaseGateway.Object);

            // Act: Call the LoadItems method directly on PurchaseFacade
            var items = purchaseFacade.LoadItems();

            // Assert: Verify the expected items are returned
            Assert.AreEqual(2, items.Count);
            Assert.AreEqual("Item1", items[0].ItemCode);
            Assert.AreEqual("Item2", items[1].ItemCode);

            // Verify that no real database interaction occurs
            _mockPurchaseGateway.Verify(gateway => gateway.LoadItems(), Times.Once); // Ensure LoadItems was called exactly once
        }




    }
}

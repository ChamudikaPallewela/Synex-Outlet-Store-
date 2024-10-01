using Client.Builder;
using Client.Command;
using Client.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace Client
{
    /// <summary>
    /// Interaction logic for PurchaseWindow.xaml
    /// </summary>
    public partial class PurchaseWindow : Window
    {
        private readonly List<CartItemDto> _cartItems = new List<CartItemDto>();
        private readonly AddToCartCommand _addToCartCommand;
        private readonly FinalizePurchaseCommand _finalizePurchaseCommand;
        private readonly LoadItemsCommand _loadItemsCommand;

        public PurchaseWindow()
        {
            InitializeComponent();
            _addToCartCommand = new AddToCartCommand();
            _finalizePurchaseCommand = new FinalizePurchaseCommand();
            _loadItemsCommand = new LoadItemsCommand();
            LoadItems();
        }

        private void LoadItems()
        {
            try
            {
                var items = _loadItemsCommand.Execute();
                ItemComboBox1.ItemsSource = items;
                ItemComboBox1.DisplayMemberPath = "ItemName";
                ItemComboBox1.SelectedValuePath = "ItemCode";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading items: {ex.Message}");
            }
        }

        private void AddToCartButton1_Click(object sender, RoutedEventArgs e)
        {
            var selectedItem = (ItemDto)ItemComboBox1.SelectedItem;

            if (selectedItem != null && int.TryParse(QuantityTextBox1.Text, out int quantity) && quantity > 0)
            {
                if (quantity > selectedItem.Quantity)
                {
                    MessageBox.Show($"Not enough stock. Available quantity: {selectedItem.Quantity}");
                    return;
                }

                var cartItemBuilder = new CartItemBuilder()
                    .SetItemCode(selectedItem.ItemCode)
                    .SetItemName(selectedItem.ItemName)
                    .SetQuantity(quantity)
                    .SetPrice(selectedItem.Price)
                    .SetDiscountRate(selectedItem.DiscountRate);

                var cartItem = cartItemBuilder.Build();
                _addToCartCommand.Execute(_cartItems, cartItem);
                UpdateCartListView();
                UpdatePurchaseSummary();
            }
            else
            {
                MessageBox.Show("Please select an item and enter a valid quantity.");
            }
        }

        private void UpdateCartListView()
        {
            CartListView1.ItemsSource = null;
            CartListView1.ItemsSource = _cartItems;
        }

        private void UpdatePurchaseSummary()
        {
            int totalItems = _cartItems.Sum(item => item.Quantity);
            decimal totalAmount = _cartItems.Sum(item => item.Total);

            TotalItemsTextBlock1.Text = totalItems.ToString();
            TotalAmountTextBlock1.Text = totalAmount.ToString("F2");
        }

        private void FinalizePurchaseButton1_Click(object sender, RoutedEventArgs e)
        {
            if (decimal.TryParse(AmountGivenTextBox1.Text, out decimal cashTendered) && cashTendered >= _cartItems.Sum(item => item.Total))
            {
                try
                {
                    _finalizePurchaseCommand.Execute(_cartItems, cashTendered, out int billNumber, out decimal changeDue);

                    // Open BillWindow
                    var billWindow = new BillWindow(billNumber, DateTime.Now.ToString("g"), _cartItems.Sum(item => item.Total), cashTendered, changeDue, _cartItems);
                    billWindow.ShowDialog();

                    MessageBox.Show("Purchase completed successfully!");
                    ResetPurchase();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error finalizing purchase: {ex.Message}");
                }
            }
            else
            {
                MessageBox.Show("Invalid amount given.");
            }
        }

        private void ResetPurchase()
        {
            _cartItems.Clear();
            UpdateCartListView();
            TotalItemsTextBlock1.Text = "0";
            TotalAmountTextBlock1.Text = "0.00";
            AmountGivenTextBox1.Text = string.Empty;
            ChangeTextBlock1.Text = "0.00";
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
        }

        // Navigation button click event handlers
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new addItems().Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            new StockShelfForm().Show();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            new PurchaseWindow().Show();
            this.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            new GenerateRepotsWindow().Show();
            this.Close();
        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            new MainForm().Show();
            this.Close();
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}
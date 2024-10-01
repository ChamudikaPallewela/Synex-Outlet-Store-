using Client.DTO;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Client
{
    /// <summary>
    /// Interaction logic for BillWindow.xaml
    /// </summary>
    public partial class BillWindow : Window
    {
        public BillWindow(int billNumber, string billDate, decimal totalAmount, decimal cashTendered, decimal changeDue, List<CartItemDto> items)
        {
            InitializeComponent();

            // Display bill details on the UI
            BillNumberTextBlock.Text = billNumber.ToString();
            BillDateTextBlock.Text = billDate;
            TotalAmountTextBlock.Text = totalAmount.ToString("F2");
            CashTenderedTextBlock.Text = cashTendered.ToString("F2");
            ChangeDueTextBlock.Text = changeDue.ToString("F2");

            // Bind the list of items to the ListView
            ItemsListView.ItemsSource = items;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

            // Additional cleanup logic can go here if needed.
            // For example, notifying the main window or server about the closure of this window.
        }
    }
}
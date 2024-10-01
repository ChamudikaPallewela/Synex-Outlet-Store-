using Client.Builder;
using Client.Command;
using System;
using System.Data;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;  // Required for DispatcherTimer

namespace Client
{
    /// <summary>
    /// Interaction logic for StockShelfForm.xaml
    /// </summary>
    public partial class StockShelfForm : Window
    {
        private readonly TransferItemCommand _transferItemCommand;
        private readonly LoadStockShelvesCommand _loadStockShelvesCommand;
        private DispatcherTimer _refreshTimer; // Timer for periodic refresh

        public StockShelfForm()
        {
            InitializeComponent();
            _transferItemCommand = new TransferItemCommand();
            _loadStockShelvesCommand = new LoadStockShelvesCommand();
            LoadStockShelves();

            // Initialize the DispatcherTimer
            _refreshTimer = new DispatcherTimer();
            _refreshTimer.Interval = TimeSpan.FromMilliseconds(2000);  // Set a more reasonable interval of 2 seconds
            _refreshTimer.Tick += RefreshTimer_Tick;  // Attach the Tick event
            _refreshTimer.Start();  // Start the timer
        }

        // Event handler for the DispatcherTimer Tick event
        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            // Refresh stock shelves data periodically
            LoadStockShelves();
        }

        private async void LoadStockShelves()
        {
            try
            {
                // Load stock shelves asynchronously
                DataTable stockShelvesData = await Task.Run(() => _loadStockShelvesCommand.Execute());
                StockShelvesDataGrid1.ItemsSource = stockShelvesData.DefaultView;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading stock shelves: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void RefreshButton1_Click(object sender, RoutedEventArgs e)
        {
            // Manual refresh when user clicks the button
            LoadStockShelves();
        }

        private void SelectButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var row = button?.DataContext as DataRowView;

            if (row != null)
            {
                var stockShelfBuilder = new StockShelfBuilder()
                    .SetStockID(Convert.ToInt32(row["StockID"]))
                    .SetItemName(row["ItemName"].ToString())
                    .SetQuantity(Convert.ToInt32(row["Quantity"]))
                    .SetExpiryDate(Convert.ToDateTime(row["ExpiryDate"]));

                var stockShelf = stockShelfBuilder.Build();

                string input = Microsoft.VisualBasic.Interaction.InputBox(
                    $"Enter quantity to transfer for Item {stockShelf.ItemName}:",
                    "Transfer Quantity",
                    stockShelf.Quantity.ToString());

                if (int.TryParse(input, out int transferQuantity) && transferQuantity > 0)
                {
                    try
                    {
                        _transferItemCommand.Execute(stockShelf, transferQuantity);
                        MessageBox.Show("Item transferred successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                        LoadStockShelves(); // Refresh after transfer
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error transferring item: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _refreshTimer.Stop(); // Stop the timer when the window is closed
        }

        // Navigation buttons click event handlers
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

using Client.Builder;
using Client.Command;
using Singleton;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Threading; // Import for DispatcherTimer

namespace Client
{
    public partial class addItems : Window
    {
        private readonly AddItemCommand _addItemCommand;
        private readonly RetrieveItemsCommand _retrieveItemsCommand;
        private DispatcherTimer _timer;  // Timer to refresh the DataGrid

        public addItems()
        {
            InitializeComponent();
            _addItemCommand = new AddItemCommand();
            _retrieveItemsCommand = new RetrieveItemsCommand();
            this.Loaded += AddItemsWindow_Loaded;

            // Setup and start the DispatcherTimer for regular refreshes
            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(2000)  // 2 seconds, more reasonable than 20ms
            };
            _timer.Tick += Timer_Tick;
        }

        private void AddItemsWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Populate the DataGrid when window loads
            _retrieveItemsCommand.Execute(ItemsDataGrid1);

            // Start the timer to refresh the DataGrid
            _timer.Start();
        }

        // This event handler is called at regular intervals defined by the DispatcherTimer
        private void Timer_Tick(object sender, EventArgs e)
        {
            // Refresh the DataGrid
            _retrieveItemsCommand.Execute(ItemsDataGrid1);
        }

        private async void AddItemButton1_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ItemCode1.Text) ||
                string.IsNullOrWhiteSpace(ItemName1.Text) ||
                string.IsNullOrWhiteSpace(Price1.Text) ||
                string.IsNullOrWhiteSpace(DiscountRate1.Text) ||
                string.IsNullOrWhiteSpace(ExpireDate1.Text) ||
                string.IsNullOrWhiteSpace(Quantity1.Text))
            {
                MessageBox.Show("Please fill in all fields.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!decimal.TryParse(Price1.Text, out var price) ||
                !decimal.TryParse(DiscountRate1.Text, out var discountRate) ||
                !int.TryParse(Quantity1.Text, out var quantity))
            {
                MessageBox.Show("Please enter valid numeric values for Price, Discount Rate, and Quantity.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (!DateTime.TryParse(ExpireDate1.Text, out var expireDate))
            {
                MessageBox.Show("Please enter a valid date for Expire Date in the format yyyy-MM-dd.", "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                var itemBuilder = new ItemBuilder()
                    .SetItemCode(ItemCode1.Text)
                    .SetItemName(ItemName1.Text)
                    .SetPrice(price)
                    .SetDiscountRate(discountRate)
                    .SetExpireDate(expireDate);

                var item = itemBuilder.Build();

                await Task.Run(() => _addItemCommand.Execute(item, quantity));

                MessageBox.Show($"Item Added Successfully: {item.ItemName}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                _retrieveItemsCommand.Execute(ItemsDataGrid1);  // Refresh the DataGrid after item is added
                                                                // Send the added item details to the server to be broadcasted to all clients
                var message = $"New item added: {item.ItemCode}, {item.ItemName}, {quantity} in stock.";
                var tcpClient = TcpClientSingleton.GetInstance("127.0.0.1", 5000); // Assuming server is running on localhost
                tcpClient.SendMessage(message);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _timer.Stop();  // Stop the timer when the window is closed
        }

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

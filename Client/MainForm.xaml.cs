using Client.Command;
using Client.Observer;
using Singleton;
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Windows;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainForm.xaml
    /// </summary>
    public partial class MainForm : Window
    {
        private TcpClientSingleton tcpClient;
        private Thread clientThread;

        public MainForm()
        {
            InitializeComponent();
            tcpClient = TcpClientSingleton.GetInstance("127.0.0.1", 5000);
            StartListeningForServerMessages();
            UpdateTime();
        }

        private void StartListeningForServerMessages()
        {
            clientThread = new Thread(ListenForServerMessages);
            clientThread.Start();
        }

        private void ListenForServerMessages()
        {
            try
            {
                byte[] buffer = new byte[1024];
                int bytesRead;

                NetworkStream stream = tcpClient.GetStream();

                while ((bytesRead = stream.Read(buffer, 0, buffer.Length)) != 0)
                {
                    string response = Encoding.ASCII.GetString(buffer, 0, bytesRead);

                    // Use IndexOf for case-insensitive check
                    if (response.IndexOf("New item added", StringComparison.OrdinalIgnoreCase) >= 0)
                    {
                        Dispatcher.Invoke(() =>
                        {
                            MessageBox.Show(response, "New Item Added", MessageBoxButton.OK, MessageBoxImage.Information);
                        });
                    }
                }
            }
            catch (ObjectDisposedException)
            {
                // Handle the case where the stream has been disposed, such as by logging or notifying the user.
            }
            catch (Exception ex)
            {
                Dispatcher.Invoke(() =>
                {
                    MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                });
            }
        }



        private void SendMessageToServer(string message)
        {
            tcpClient.SendMessage(message);
        }

        private void UpdateTime()
        {
            TimeTextBlock.Text = DateTime.Now.ToString("HH:mm:ss");
        }

        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            SendMessageToServer("Exit"); // Notify server that the application is exiting
            Application.Current.Shutdown();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SendMessageToServer("OpenAddItemsWindow"); // Notify server that Add Items window is being opened
            addItems addItemsWindow = new addItems();
            addItemsWindow.Show();
            this.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            SendMessageToServer("OpenStockShelfForm"); // Notify server that Stock Shelf Form window is being opened
            StockShelfForm stockShelfForm = new StockShelfForm();
            stockShelfForm.Show();
            this.Close();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            SendMessageToServer("OpenPurchaseWindow"); // Notify server that Purchase Window is being opened
            PurchaseWindow purchaseWindow = new PurchaseWindow();
            purchaseWindow.Show();
            this.Close();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            SendMessageToServer("OpenGenerateReportsWindow"); // Notify server that Generate Reports window is being opened
            GenerateRepotsWindow repotsWindow = new GenerateRepotsWindow();
            repotsWindow.Show();
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);


        }
    }
}
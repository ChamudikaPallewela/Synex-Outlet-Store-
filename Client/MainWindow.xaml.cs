using Client.Observer;
using Singleton;
using System;
using System.Windows;
using System.Windows.Threading;

namespace Client
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer timer;
        private TcpClientSingleton tcpClient;

        public MainWindow()
        {
            InitializeComponent();

            // Initialize and start the timer
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromMilliseconds(50);
            timer.Tick += Timer_Tick;
            timer.Start();

            // Initialize the TcpClientSingleton
            tcpClient = TcpClientSingleton.GetInstance("127.0.0.1", 5000);
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            // Increment the progress bar value
            progressBar.Value += 1;

            // Update the progress text with the current percentage
            progressText.Text = $"{progressBar.Value}%";

            // Check if progress bar is complete
            if (progressBar.Value >= progressBar.Maximum)
            {
                timer.Stop();
                ShowMainForm();
            }
        }

        private void ShowMainForm()
        {
            // Notify the server that the main form is being opened
            tcpClient.SendMessage("MainFormOpened");

            // Create and show the main form after the splash screen
            MainForm mainForm = new MainForm();
            mainForm.Show();
            this.Close();
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);

        }
    }
}
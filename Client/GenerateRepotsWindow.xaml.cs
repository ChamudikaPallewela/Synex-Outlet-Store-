using Client.Command;
using Client.DTO;
using Client.Facade;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Threading; // Required for DispatcherTimer

namespace Client
{
    /// <summary>
    /// Interaction logic for GenerateRepotsWindow.xaml
    /// </summary>
    public partial class GenerateRepotsWindow : Window
    {
        private readonly ReportFacade _reportFacade;
        private DispatcherTimer _refreshTimer;

        public GenerateRepotsWindow()
        {
            InitializeComponent();
            _reportFacade = new ReportFacade();

            // Initialize and start the DispatcherTimer
            _refreshTimer = new DispatcherTimer();
            _refreshTimer.Interval = TimeSpan.FromSeconds(5); // Auto-refresh every 5 seconds
            _refreshTimer.Tick += RefreshTimer_Tick;
            _refreshTimer.Start();
        }

        // This method is called at every tick of the timer (e.g., every 5 seconds)
        private void RefreshTimer_Tick(object sender, EventArgs e)
        {
            // Automatically refresh all reports
            GenerateTotalSalesReport();
            GenerateReshelvedItemsReport();
            GenerateReorderLevelReport();
            GenerateStockReport();
            GenerateBillReport();
        }

        // Manually trigger the report generation on button clicks
        private void GenerateTotalSalesReport_Click(object sender, RoutedEventArgs e)
        {
            GenerateTotalSalesReport();
        }

        private void GenerateReshelvedItemsReport_Click(object sender, RoutedEventArgs e)
        {
            GenerateReshelvedItemsReport();
        }

        private void GenerateReorderLevelReport_Click(object sender, RoutedEventArgs e)
        {
            GenerateReorderLevelReport();
        }

        private void GenerateStockReport_Click(object sender, RoutedEventArgs e)
        {
            GenerateStockReport();
        }

        private void GenerateBillReport_Click(object sender, RoutedEventArgs e)
        {
            GenerateBillReport();
        }

        // Helper methods for generating each report
        private void GenerateTotalSalesReport()
        {
            DateTime selectedDate = SalesDatePicker.SelectedDate ?? DateTime.Today;
            var generateReportCommand = new GenerateTotalSalesReportCommand(_reportFacade, selectedDate, UpdateTotalSalesListView);
            generateReportCommand.Execute();
        }

        private void GenerateReshelvedItemsReport()
        {
            var generateReportCommand = new GenerateReshelvedItemsReportCommand(_reportFacade, UpdateReshelvedItemsListView);
            generateReportCommand.Execute();
        }

        private void GenerateReorderLevelReport()
        {
            var generateReportCommand = new GenerateReorderLevelReportCommand(_reportFacade, UpdateReorderLevelListView);
            generateReportCommand.Execute();
        }

        private void GenerateStockReport()
        {
            var generateReportCommand = new GenerateStockReportCommand(_reportFacade, UpdateStockReportListView);
            generateReportCommand.Execute();
        }

        private void GenerateBillReport()
        {
            var generateReportCommand = new GenerateBillReportCommand(_reportFacade, UpdateBillReportListView);
            generateReportCommand.Execute();
        }

        // Update ListViews with the new report data
        private void UpdateTotalSalesListView(List<SalesReportItemDto> report)
        {
            TotalSalesListView.ItemsSource = report;
        }

        private void UpdateReshelvedItemsListView(List<ReshelvedItemDto> report)
        {
            ReshelvedItemsListView.ItemsSource = report;
        }

        private void UpdateReorderLevelListView(List<ReorderLevelItemDto> report)
        {
            ReorderLevelListView.ItemsSource = report;
        }

        private void UpdateStockReportListView(List<StockReportItemDto> report)
        {
            StockReportListView.ItemsSource = report;
        }

        private void UpdateBillReportListView(List<BillReportItemDto> report)
        {
            BillReportListView.ItemsSource = report;
        }

        protected override void OnClosed(EventArgs e)
        {
            base.OnClosed(e);
            _refreshTimer.Stop(); // Stop the timer when the window is closed
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

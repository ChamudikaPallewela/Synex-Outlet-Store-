using Client.DTO;
using Client.Observer;
using Client.TableDataGateway;
using Singleton;
using System;
using System.Collections.Generic;

namespace Client.Facade
{
    public class ReportFacade
    {
        private readonly ReportGateway _reportGateway = new ReportGateway();
        private readonly TcpClientSingleton _tcpClient = TcpClientSingleton.GetInstance("127.0.0.1", 5000);
        private readonly ReportObserver _reportObserver = new ReportObserver();

        public List<SalesReportItemDto> GenerateTotalSalesReport(DateTime selectedDate)
        {
            var report = _reportGateway.GenerateTotalSalesReport(selectedDate);
            _reportObserver.Notify($"ReportGenerated:TotalSales for {selectedDate:yyyy-MM-dd}");
            return report;
        }

        public List<ReshelvedItemDto> GenerateReshelvedItemsReport()
        {
            var report = _reportGateway.GenerateReshelvedItemsReport();
            _reportObserver.Notify("ReportGenerated:ReshelvedItems");
            return report;
        }

        public List<ReorderLevelItemDto> GenerateReorderLevelReport()
        {
            var report = _reportGateway.GenerateReorderLevelReport();
            _reportObserver.Notify("ReportGenerated:ReorderLevel");
            return report;
        }

        public List<StockReportItemDto> GenerateStockReport()
        {
            var report = _reportGateway.GenerateStockReport();
            _reportObserver.Notify("ReportGenerated:StockReport");
            return report;
        }

        public List<BillReportItemDto> GenerateBillReport()
        {
            var report = _reportGateway.GenerateBillReport();
            _reportObserver.Notify("ReportGenerated:BillReport");
            return report;
        }
    }
}
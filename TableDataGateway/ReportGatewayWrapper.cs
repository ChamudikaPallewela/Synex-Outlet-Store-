using Client.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Client.TableDataGateway
{
    public class ReportGatewayWrapper : IReportGateway
    {
        private readonly ReportGateway _reportGateway;

        public ReportGatewayWrapper(ReportGateway reportGateway)
        {
            _reportGateway = reportGateway;
        }

        public List<SalesReportItemDto> GenerateTotalSalesReport(DateTime selectedDate)
        {
            return _reportGateway.GenerateTotalSalesReport(selectedDate);
        }

        public List<StockReportItemDto> GenerateStockReport()
        {
            return _reportGateway.GenerateStockReport();
        }
    }
}

using Client.Facade;
using System.Collections.Generic;

namespace Client.Strategy
{
    public class StockReportStrategy : IReportStrategy
    {
        private readonly ReportFacade _reportFacade;

        public StockReportStrategy(ReportFacade reportFacade)
        {
            _reportFacade = reportFacade;
        }

        public List<object> GenerateReport()
        {
            var report = _reportFacade.GenerateStockReport();
            return new List<object>(report);
        }
    }
}
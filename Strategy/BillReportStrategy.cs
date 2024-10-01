using Client.Facade;
using System.Collections.Generic;

namespace Client.Strategy
{
    public class BillReportStrategy : IReportStrategy
    {
        private readonly ReportFacade _reportFacade;

        public BillReportStrategy(ReportFacade reportFacade)
        {
            _reportFacade = reportFacade;
        }

        public List<object> GenerateReport()
        {
            var report = _reportFacade.GenerateBillReport();
            return new List<object>(report);
        }
    }
}
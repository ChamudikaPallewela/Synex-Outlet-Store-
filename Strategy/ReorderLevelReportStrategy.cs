using Client.Facade;
using System.Collections.Generic;

namespace Client.Strategy
{
    public class ReorderLevelReportStrategy : IReportStrategy
    {
        private readonly ReportFacade _reportFacade;

        public ReorderLevelReportStrategy(ReportFacade reportFacade)
        {
            _reportFacade = reportFacade;
        }

        public List<object> GenerateReport()
        {
            var report = _reportFacade.GenerateReorderLevelReport();
            return new List<object>(report);
        }
    }
}
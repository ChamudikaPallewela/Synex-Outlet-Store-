using Client.Facade;
using System.Collections.Generic;

namespace Client.Strategy
{
    public class ReshelvedItemsReportStrategy : IReportStrategy
    {
        private readonly ReportFacade _reportFacade;

        public ReshelvedItemsReportStrategy(ReportFacade reportFacade)
        {
            _reportFacade = reportFacade;
        }

        public List<object> GenerateReport()
        {
            var report = _reportFacade.GenerateReshelvedItemsReport();
            return new List<object>(report);
        }
    }
}
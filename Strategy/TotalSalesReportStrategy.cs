using Client.Facade;
using System;
using System.Collections.Generic;

namespace Client.Strategy
{
    public class TotalSalesReportStrategy : IReportStrategy
    {
        private readonly ReportFacade _reportFacade;
        private readonly DateTime _selectedDate;

        public TotalSalesReportStrategy(ReportFacade reportFacade, DateTime selectedDate)
        {
            _reportFacade = reportFacade;
            _selectedDate = selectedDate;
        }

        public List<object> GenerateReport()
        {
            var report = _reportFacade.GenerateTotalSalesReport(_selectedDate);
            return new List<object>(report);
        }
    }
}
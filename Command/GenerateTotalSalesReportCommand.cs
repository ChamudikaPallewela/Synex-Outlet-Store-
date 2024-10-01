using Client.DTO;
using Client.Facade;
using System;
using System.Collections.Generic;

namespace Client.Command
{
    public class GenerateTotalSalesReportCommand : GenerateReportCommand
    {
        private readonly ReportFacade _reportFacade;
        private readonly DateTime _selectedDate;
        private readonly Action<List<SalesReportItemDto>> _updateUiAction;

        public GenerateTotalSalesReportCommand(ReportFacade reportFacade, DateTime selectedDate, Action<List<SalesReportItemDto>> updateUiAction)
        {
            _reportFacade = reportFacade;
            _selectedDate = selectedDate;
            _updateUiAction = updateUiAction;
        }

        public override void Execute()
        {
            var report = _reportFacade.GenerateTotalSalesReport(_selectedDate);
            _updateUiAction(report);
        }
    }
}
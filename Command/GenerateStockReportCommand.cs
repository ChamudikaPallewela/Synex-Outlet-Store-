using Client.DTO;
using Client.Facade;
using System;
using System.Collections.Generic;

namespace Client.Command
{
    public class GenerateStockReportCommand : GenerateReportCommand
    {
        private readonly ReportFacade _reportFacade;
        private readonly Action<List<StockReportItemDto>> _updateUiAction;

        public GenerateStockReportCommand(ReportFacade reportFacade, Action<List<StockReportItemDto>> updateUiAction)
        {
            _reportFacade = reportFacade;
            _updateUiAction = updateUiAction;
        }

        public override void Execute()
        {
            var report = _reportFacade.GenerateStockReport();
            _updateUiAction(report);
        }
    }
}
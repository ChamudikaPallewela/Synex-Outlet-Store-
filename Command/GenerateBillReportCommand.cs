using Client.DTO;
using Client.Facade;
using System;
using System.Collections.Generic;

namespace Client.Command
{
    public class GenerateBillReportCommand : GenerateReportCommand
    {
        private readonly ReportFacade _reportFacade;
        private readonly Action<List<BillReportItemDto>> _updateUiAction;

        public GenerateBillReportCommand(ReportFacade reportFacade, Action<List<BillReportItemDto>> updateUiAction)
        {
            _reportFacade = reportFacade;
            _updateUiAction = updateUiAction;
        }

        public override void Execute()
        {
            var report = _reportFacade.GenerateBillReport();
            _updateUiAction(report);
        }
    }
}
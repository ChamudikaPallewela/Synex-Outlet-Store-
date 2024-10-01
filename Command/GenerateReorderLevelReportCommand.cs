using Client.DTO;
using Client.Facade;
using System;
using System.Collections.Generic;

namespace Client.Command
{
    public class GenerateReorderLevelReportCommand : GenerateReportCommand
    {
        private readonly ReportFacade _reportFacade;
        private readonly Action<List<ReorderLevelItemDto>> _updateUiAction;

        public GenerateReorderLevelReportCommand(ReportFacade reportFacade, Action<List<ReorderLevelItemDto>> updateUiAction)
        {
            _reportFacade = reportFacade;
            _updateUiAction = updateUiAction;
        }

        public override void Execute()
        {
            var report = _reportFacade.GenerateReorderLevelReport();
            _updateUiAction(report);
        }
    }
}
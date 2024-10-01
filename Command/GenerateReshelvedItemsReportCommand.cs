using Client.DTO;
using Client.Facade;
using System;
using System.Collections.Generic;

namespace Client.Command
{
    public class GenerateReshelvedItemsReportCommand : GenerateReportCommand
    {
        private readonly ReportFacade _reportFacade;
        private readonly Action<List<ReshelvedItemDto>> _updateUiAction;

        public GenerateReshelvedItemsReportCommand(ReportFacade reportFacade, Action<List<ReshelvedItemDto>> updateUiAction)
        {
            _reportFacade = reportFacade;
            _updateUiAction = updateUiAction;
        }

        public override void Execute()
        {
            var report = _reportFacade.GenerateReshelvedItemsReport();
            _updateUiAction(report);
        }
    }
}
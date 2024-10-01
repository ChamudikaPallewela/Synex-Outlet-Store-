using Client.Strategy;
using System.Collections.Generic;

namespace Client.ChainOfResponsibility
{
    public class ReshelvedItemsReportHandler : ReportHandler
    {
        public override void HandleRequest(IReportStrategy strategy, out List<object> reportData)
        {
            if (strategy is ReshelvedItemsReportStrategy)
            {
                reportData = strategy.GenerateReport();
            }
            else if (_nextHandler != null)
            {
                _nextHandler.HandleRequest(strategy, out reportData);
            }
            else
            {
                reportData = null;
            }
        }
    }
}
using Client.Strategy;
using System.Collections.Generic;

namespace Client.ChainOfResponsibility
{
    public abstract class ReportHandler
    {
        protected ReportHandler _nextHandler;

        public void SetNext(ReportHandler handler)
        {
            _nextHandler = handler;
        }

        public abstract void HandleRequest(IReportStrategy strategy, out List<object> reportData);
    }
}
using System.Collections.Generic;

namespace Client.Strategy
{
    public interface IReportStrategy
    {
        List<object> GenerateReport();
    }
}
using Singleton;

namespace Client.Observer
{
    public class ReportObserver : IReportObserver
    {
        private readonly TcpClientSingleton _tcpClient = TcpClientSingleton.GetInstance("127.0.0.1", 5000);

        public void Notify(string message)
        {
            _tcpClient.SendMessage(message);
        }
    }
}
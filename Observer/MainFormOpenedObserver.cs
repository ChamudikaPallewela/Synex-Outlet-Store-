using Singleton;

namespace Client.Observer
{
    public class MainFormOpenedObserver : IObserver
    {
        private readonly TcpClientSingleton _tcpClient;

        public MainFormOpenedObserver(TcpClientSingleton tcpClient)
        {
            _tcpClient = tcpClient;
        }

        public void Notify(string message)
        {
            _tcpClient.SendMessage(message);
        }
    }
}
using Singleton;

namespace Client.Observer
{
    public class ServerNotificationObserver : IObserver
    {
        private readonly TcpClientSingleton _tcpClient;

        public ServerNotificationObserver(TcpClientSingleton tcpClient)
        {
            _tcpClient = tcpClient;
        }

        public void Notify(string message)
        {
            _tcpClient.SendMessage(message);
        }
    }
}
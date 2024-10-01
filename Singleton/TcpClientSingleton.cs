using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace Singleton
{
    public class TcpClientSingleton
    {
        private static TcpClientSingleton instance;
        private TcpClient client;
        private NetworkStream stream;
        private string serverAddress;
        private int port;

        private TcpClientSingleton(string serverAddress, int port)
        {
            this.serverAddress = serverAddress;
            this.port = port;
            client = new TcpClient(serverAddress, port);
            stream = client.GetStream();

            // Handle application exit to close the connection automatically
            AppDomain.CurrentDomain.ProcessExit += (s, e) => CloseConnection();
        }

        public static TcpClientSingleton GetInstance(string serverAddress, int port)
        {
            if (instance == null)
            {
                instance = new TcpClientSingleton(serverAddress, port);
            }
            return instance;
        }

        public void SendMessage(string message)
        {
            byte[] data = Encoding.ASCII.GetBytes(message);
            stream.Write(data, 0, data.Length);
        }

        public NetworkStream GetStream()
        {
            return stream;
        }

        // Automatically close the connection when the application exits
        private void CloseConnection()
        {
            try
            {
                SendMessage("ClientDisconnecting");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Failed to notify server before disconnecting: {ex.Message}");
            }
            finally
            {
                stream?.Close();
                client?.Close();
                instance = null;
            }
        }
    }
}

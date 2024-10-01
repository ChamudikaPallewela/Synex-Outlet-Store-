using System;
using System.Collections.Concurrent;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        private static TcpListener listener;
        private static readonly ConcurrentDictionary<int, TcpClient> connectedClients = new ConcurrentDictionary<int, TcpClient>();
        private static readonly ConcurrentQueue<string> requestQueue = new ConcurrentQueue<string>();
        private static readonly object lockObject = new object();

        static void Main(string[] args)
        {
            Console.WriteLine("Server is starting...");
            listener = new TcpListener(IPAddress.Any, 5000);
            listener.Start();
            Console.WriteLine("Server started on port 5000.");

            Thread acceptClientsThread = new Thread(AcceptClients);
            acceptClientsThread.Start();

            Thread processRequestsThread = new Thread(ProcessRequests);
            processRequestsThread.Start();
        }

        private static void AcceptClients()
        {
            int clientId = 0;
            while (true)
            {
                var client = listener.AcceptTcpClient();
                int currentClientId = clientId; // Capture the correct clientId for this client
                Console.WriteLine($"Client {currentClientId} connected.");
                connectedClients[currentClientId] = client;

                // Start a new thread to handle this specific client
                Thread clientThread = new Thread(() => HandleClientAsync(client, currentClientId));
                clientThread.Start();

                clientId++; // Increment for the next client
            }
        }

        private static async Task HandleClientAsync(TcpClient client, int clientId)
        {
            NetworkStream stream = client.GetStream();
            byte[] buffer = new byte[1024];

            try
            {
                while (true)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break; // Connection closed

                    string request = Encoding.ASCII.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received request from client {clientId}: {request}");

                    lock (lockObject)
                    {
                        requestQueue.Enqueue(request);
                        Monitor.Pulse(lockObject);
                    }
                }
            }
            catch (IOException)
            {
                Console.WriteLine($"Client {clientId} disconnected.");
            }
            finally
            {
                connectedClients.TryRemove(clientId, out _);
                client.Close();
            }
        }



        private static void ProcessRequests()
        {
            while (true)
            {
                string request;
                lock (lockObject)
                {
                    while (requestQueue.IsEmpty)
                    {
                        Monitor.Wait(lockObject);
                    }

                    requestQueue.TryDequeue(out request);
                }

                if (!string.IsNullOrEmpty(request))
                {
                    Console.WriteLine($"Processing request: {request}");
                    BroadcastToClients(request);
                }
            }
        }

        private static void BroadcastToClients(string message)
        {
            byte[] messageBytes = Encoding.ASCII.GetBytes(message);
            foreach (var kvp in connectedClients)
            {
                var client = kvp.Value;
                try
                {
                    NetworkStream stream = client.GetStream();
                    stream.Write(messageBytes, 0, messageBytes.Length);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error broadcasting to client {kvp.Key}: {ex.Message}");
                }
            }
        }
    }
}

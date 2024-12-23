using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace FSAClient.Classes
{
    public class ChatServer
    {
        private readonly TcpListener _listener;
        private readonly List<TcpClient> _clients;
        private bool _isRunning;

        public ChatServer(string ip, int port)
        {
            _listener = new TcpListener(IPAddress.Parse(ip), port);
            _clients = new List<TcpClient>();
            Start();
        }

        public void Start()
        {
            _isRunning = true;
            _listener.Start();
            Console.WriteLine("Server started...");

            Task.Run(() => AcceptClients());
        }

        private async Task AcceptClients()
        {
            while (_isRunning)
            {
                TcpClient client = await _listener.AcceptTcpClientAsync();
                _clients.Add(client);
                Console.WriteLine("Client connected...");

                Task.Run(() => HandleClient(client));
            }
        }

        private async Task HandleClient(TcpClient client)
        {
            NetworkStream stream = client.GetStream();
            StreamReader reader = new StreamReader(stream);
            StreamWriter writer = new StreamWriter(stream) { AutoFlush = true };

            while (_isRunning)
            {
                try
                {
                    string message = await reader.ReadLineAsync();
                    if (message != null)
                    {
                        Console.WriteLine($"Received: {message}");
                        BroadcastMessage(message, client);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error handling client: {ex.Message}");
                    _clients.Remove(client);
                    client.Close();
                    break;
                }
            }
        }

        private void BroadcastMessage(string message, TcpClient sender)
        {
            foreach (var client in _clients)
            {
                if (client != sender)
                {
                    StreamWriter writer = new StreamWriter(client.GetStream()) { AutoFlush = true };
                    writer.WriteLine(message);
                }
            }
        }

        public void Stop()
        {
            _isRunning = false;
            _listener.Stop();
            foreach (var client in _clients)
            {
                client.Close();
            }
            _clients.Clear();
            Console.WriteLine("Server stopped...");
        }
    }
}
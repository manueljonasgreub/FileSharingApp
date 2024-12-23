using System;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Windows;

namespace FSAClient.Classes
{
    public class ChatService
    {
        private readonly TcpClient _client;
        private readonly ChatViewModel _viewModel;
        private StreamReader _reader;
        private StreamWriter _writer;
        private Task _receiveTask;
        private bool _isRunning;

        public ChatService(ChatViewModel chatViewModel, string ip, string port)
        {
            _viewModel = chatViewModel;

            _client = new TcpClient(ip, int.Parse(port));
            NetworkStream stream = _client.GetStream();

            _reader = new StreamReader(stream);
            _writer = new StreamWriter(stream) { AutoFlush = true };

            _isRunning = true;
            _receiveTask = Task.Run(() => ReceiveMessages());
        }

        public void SendMessage(string username, string message)
        {
            _writer.WriteLine($"{username}: {message}");
            Application.Current.Dispatcher.Invoke(() => _viewModel.Messages.Add($"{username}: {message}"));
        }

        private void ReceiveMessages()
        {
            while (_isRunning)
            {
                try
                {
                    string message = _reader.ReadLine();
                    if (message != null)
                    {
                        Application.Current.Dispatcher.Invoke(() => _viewModel.Messages.Add(message));
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error receiving message: {ex.Message}");
                    _isRunning = false;
                }
            }
        }

        public void Close()
        {
            _isRunning = false;
            _reader.Close();
            _writer.Close();
            _client.Close();
        }
    }
}
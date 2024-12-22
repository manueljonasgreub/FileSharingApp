using System;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WebSocketSharp;

namespace FSAClient.Classes
{
    public class ChatService
    {
        private WebSocket _webSocket;
        private readonly ChatViewModel _viewModel;

        public ChatService(ChatViewModel chatViewModel)
        {
            _viewModel = chatViewModel;
        }

        public void ConnectToChat(string url)
        {
            _webSocket = new WebSocket(url);

            _webSocket.OnMessage += (sender, e) =>
            {
                Application.Current.Dispatcher.Invoke(() =>
                {
                    _viewModel.Messages.Add(e.Data);
                });
            };

            _webSocket.OnOpen += (sender, e) =>
            {
                MessageBox.Show("Connected to chat!");
            };

            _webSocket.OnError += (sender, e) =>
            {
                MessageBox.Show($"Error connecting to chat: {e.Message}");
            };

            _webSocket.Connect();
        }

        public void SendMessage(string user, string message)
        {
            if (_webSocket == null || !_webSocket.IsAlive)
            {
                MessageBox.Show("WebSocket is not connected.");
                return;
            }

            var fullMessage = $"{user}: {message}";
            _webSocket.Send(fullMessage);
        }
    }
}
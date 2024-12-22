using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.AspNetCore.SignalR.Client;


namespace FSAClient.Classes
{
    public class ChatService
    {
        private HubConnection _connection;
        private readonly ChatViewModel _viewModel;

        public ChatService(ChatViewModel chatViewModel)
        {
            _viewModel = chatViewModel;
        }

        public async Task ConnectToChat(string url)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(url)
                .Build();

            _connection.On<string, string>("ReceiveMessage",
                (user, message) =>
                {
                    Application.Current.Dispatcher.Invoke(() => { _viewModel.Messages.Add($"{user}: {message}"); });
                });

            try
            {
                await _connection.StartAsync();
                MessageBox.Show("Connected to chat!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error connecting to chat: {ex.Message}");
            }
        }

        public async Task SendMessage(string user, string message)
        {
            if (_connection == null)
            {
                MessageBox.Show("Connection is null.");
                return;
            }

            // Warte, bis die Verbindung vollständig hergestellt ist
            while (_connection.State == HubConnectionState.Connecting)
            {
                await Task.Delay(100);
            }

            if (_connection.State != HubConnectionState.Connected)
            {
                MessageBox.Show($"Connection state: {_connection.State}");
                return;
            }

            try
            {
                await _connection.InvokeAsync("SendMessage", user, message);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error sending message: {ex.Message}");
            }
        }
    }
}
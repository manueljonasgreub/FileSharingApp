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

        public async Task ConnectToChat(string url)
        {
            _connection = new HubConnectionBuilder()
                .WithUrl(url)
                .Build();

            _connection.On<string, string>("ReceiveMessage", (user, message) =>
            {
                MessageBox.Show($"{user}: {message}");
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

    }
}

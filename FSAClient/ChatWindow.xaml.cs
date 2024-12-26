using System;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using FSAClient.Classes;

namespace FSAClient
{
    public partial class ChatWindow : Window
    {
        private readonly ChatServiceClient _chatService;
        private readonly ChatViewModel _viewModel;
        private readonly ChatServiceClient _client;
        private readonly ChatServiceListener _listener;


        public ChatWindow(string username, string remoteIP, string remotePort)
        {
            InitializeComponent();
            _viewModel = new ChatViewModel();
            DataContext = _viewModel;

            ConnectedUserName.Content = username;
            IpContent.Content = remoteIP;
            PortContent.Content = remotePort;


            // Start the listener
            // Only the position of this code was suggested from ChatGPT
            _listener = new ChatServiceListener(_viewModel);


            Task.Run(() => _listener.Listen());


            // Initialize and connect to the remote client
            // Only the position of this code was suggested from ChatGPT
            _chatService = new ChatServiceClient(remoteIP, remotePort);


            ConnectToServerAsync();
        }

        private async void ConnectToServerAsync()
        {
            try
            {
                await _chatService.ConnectAsyncFunkTask();
            }
            catch (SocketException ex)
            {
                MessageBox.Show($"Connection failed: {ex.Message}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
        }

        private void ButtonSendMessage_Click(object sender, RoutedEventArgs e)
        {
            string message = TextBoxMessage.Text;
            _chatService.SendMessage(ConnectedUserName.Content.ToString(), message);
            _viewModel.AddMessage($"Me: {message}", Brushes.Black);
            TextBoxMessage.Clear();
        }

        private void Window_Closed(object sender, EventArgs eventArgs)
        {
        }
    }
}
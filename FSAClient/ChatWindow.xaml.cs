using System;
using System.Windows;
using FSAClient.Classes;

namespace FSAClient
{
    public partial class ChatWindow : Window
    {
        private readonly ChatService _chatService;
        private readonly ChatViewModel _viewModel;

        public ChatWindow(string username, string ip, string port)
        {
            InitializeComponent();
            _viewModel = new ChatViewModel();
            DataContext = _viewModel;

            ConnectedUserName.Content = username;
            IpContent.Content = ip;
            PortContent.Content = port;

            _chatService = new ChatService(_viewModel, ip, port);
        }

        private void ButtonSendMessage_Click(object sender, RoutedEventArgs e)
        {
            string message = TextBoxMessage.Text;
            _chatService.SendMessage(ConnectedUserName.Content.ToString(), message);
            TextBoxMessage.Clear();
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _chatService.Close();
        }
    }
}
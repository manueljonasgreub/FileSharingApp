using System.Windows;
using FSAClient.Classes;

namespace FSAClient
{
    public partial class ChatWindow : Window
    {
        private readonly ChatService _chatService;
        private readonly ChatViewModel _viewModel;

        public ChatWindow(string ipAddress, string port, string username)
        {
            InitializeComponent();
            _viewModel = new ChatViewModel();
            DataContext = _viewModel;

            ConnectedUserName.Content = username;
            IpContent.Content = ipAddress;
            PortContent.Content = port;

            _chatService = new ChatService(_viewModel);

            ConnectToChat(ipAddress, port);
        }

        private void ConnectToChat(string ipAddress, string port)
        {
            string url = $"ws://{ipAddress}:{port}/chat";
            _chatService.ConnectToChat(url);
        }

        private void ButtonSendMessage_Click(object sender, RoutedEventArgs e)
        {
            string message = TextBoxMessage.Text;
            _chatService.SendMessage(UserData.Name, message);
        }
    }
}
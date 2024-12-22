using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using FSAClient.Classes;

namespace FSAClient
{
    /// <summary>
    /// Interaction logic for ChatWindow.xaml
    /// </summary>
    public partial class ChatWindow : Window
    {
        private readonly ChatService _chatService;
        private readonly ChatViewModel _viewModel;


        record ConnectedUserDataInformation(string UserName, string IpAddress, string Port);

        public ChatWindow(string connection, string username, string ip, string port)
        {
            InitializeComponent();
            _viewModel = new ChatViewModel();
            DataContext = _viewModel;

            ConnectedUserDataInformation connectedUserDataInformation = new ConnectedUserDataInformation(username, ip,
                port);
            this.DataContext = connectedUserDataInformation;

            ConnectedUserName.Content = username;
            IpContent.Content = ip;
            PortContent.Content = port;

            _chatService = new ChatService(_viewModel);

            ConnectToChatAsync(connection);
        }

        private async void ConnectToChatAsync(string connection)
        {
            await _chatService.ConnectToChat(connection);
        }

        private void ButtonSendMessage_Click(object sender, RoutedEventArgs e)
        {
            string message = TextBoxMessage.Text;
            _chatService.SendMessage(UserData.Name, message);
        }
    }
}
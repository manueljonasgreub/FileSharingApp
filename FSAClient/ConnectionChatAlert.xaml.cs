using FSAClient.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace FSAClient
{
    /// <summary>
    /// Interaction logic for ConnectionChatAlert.xaml
    /// </summary>
    public partial class ConnectionChatAlert : Window
    {
        record P2PConnectionData(int SenderId, int ReceiverId, string answer, string Protocol);

        ConnectionChatAlertData IncomingConnection;
        WebSocketSharp.WebSocket webSocket;

        public ConnectionChatAlert(string serializedConnectionAlert, WebSocketSharp.WebSocket ws)
        {
            InitializeComponent();

            ButtonAcceptConnection.IsEnabled = false;
            IncomingConnection = JsonSerializer.Deserialize<ConnectionChatAlertData>(serializedConnectionAlert);
            this.DataContext = IncomingConnection;
            webSocket = ws;
        }

        private void CheckBoxTrustRequest_Checked(object sender, RoutedEventArgs e)
        {
            ButtonAcceptConnection.IsEnabled = true;
            ButtonAcceptConnection.Background = new SolidColorBrush(Color.FromRgb(173, 208, 170));
        }

        private void CheckBoxTrustRequest_Unchecked(object sender, RoutedEventArgs e)
        {
            ButtonAcceptConnection.IsEnabled = false;
            ButtonAcceptConnection.Background = new SolidColorBrush(Color.FromRgb(221, 221, 221));
        }

        private void ButtonAcceptConnection_Click(object sender, RoutedEventArgs e)
        {
            Listener listener = new Listener();
            listener.Listen();
            P2PConnectionData p2pConnectionData = new P2PConnectionData(UserData.UserId, IncomingConnection.UserId,
                "accept", "openChat");
            string serializedP2PConnection = JsonSerializer.Serialize(p2pConnectionData);
            string message = $"P2PConnectionResponse;{serializedP2PConnection}";
            webSocket.Send(message);

            Application.Current.Dispatcher.Invoke(() =>
            {
                MessageBox.Show(
                    $"LorenzoClient P2PConnectionResponse ConnectionChatAlert: Connecting to chat at http://{IncomingConnection
                        .IpAddress}:{IncomingConnection.Port}/chatHub");
                var chatWindow = new ChatWindow(
                    $"http://{IncomingConnection.IpAddress}:{IncomingConnection.Port}/chatHub",
                    $"{IncomingConnection.UserName}",
                    $"{IncomingConnection.IpAddress}",
                    $"{IncomingConnection.Port}"
                    );
                chatWindow.Show();
            });

            this.Close();
        }

        private void ButtonDeclineConnection_Click(object sender, RoutedEventArgs e)
        {
            P2PConnectionData p2pConnectionData = new P2PConnectionData(UserData.UserId, IncomingConnection.UserId,
                "decline", "openChat");
            string serializedP2PConnection = JsonSerializer.Serialize(p2pConnectionData);
            string message = $"P2PConnectionResponse;{serializedP2PConnection}";
            webSocket.Send(message);
            this.Close();
        }
    }
}
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace FSAClient.Classes
{
    public class ChatServiceListener
    {
        private TcpListener _listener;
        private readonly ChatViewModel _viewModel;


        public ChatServiceListener(ChatViewModel viewModel)
        {
            _viewModel = viewModel;
        }

        public async Task Listen()
        {
            _listener = new TcpListener(UserData.LocalIP, UserData.LocalPort);
            _listener.Start();
            await AcceptClientsAsync();
        }

        private async Task AcceptClientsAsync()
        {
            try
            {
                while (true)
                {
                    TcpClient client = await _listener.AcceptTcpClientAsync();
                    _ = Task.Run(() => HandleClientAsync(client));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                _listener.Stop();
            }
        }

        private async Task HandleClientAsync(TcpClient client)
        {
            try
            {
                NetworkStream stream = client.GetStream();
                byte[] buffer = new byte[1024];
                int bytesRead;

                while ((bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length)) != 0)
                {
                    string message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    Console.WriteLine($"Received: {message}");
                    Application.Current.Dispatcher.Invoke(() => { _viewModel.AddMessage(message, Brushes.DarkGreen); });
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                client.Close();
            }
        }
    }
}
using System;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

public class ChatServiceClient
{
    private readonly string _serverIp;
    private readonly int _serverPort;
    private TcpClient _client;
    private NetworkStream _stream;

    public ChatServiceClient(string serverIp, string serverPort)
    {
        _serverIp = serverIp;
        _serverPort = int.Parse(serverPort);
    }

    public async Task ConnectAsyncFunkTask()
    {
        _client = new TcpClient();
        await _client.ConnectAsync(_serverIp, _serverPort);
        _stream = _client.GetStream();
    }

    public async Task SendMessage(string userName, string message)
    {
        string fullMessage = $"{userName}: {message}";
        byte[] data = Encoding.UTF8.GetBytes(fullMessage);
        await _stream.WriteAsync(data, 0, data.Length);
    }
}
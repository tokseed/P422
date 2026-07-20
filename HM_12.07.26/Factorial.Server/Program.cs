using Factorial.Protocol;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

IPEndPoint ipEndPoint = new(IPAddress.Loopback, 8888);
using var server = new Server(ipEndPoint, 100);

Console.WriteLine("Сервер запущен. Ожидание подключений...");
Console.WriteLine($"Адрес: {ipEndPoint.Address}, порт: {ipEndPoint.Port}");

while (true)
{
    var request = await server.ReceiveAsync();
    var response = server.HandleRequest(request);
    await server.SendAsync(response);
}

public class Server : IDisposable
{
    private readonly Socket _serverSocket;
    private Socket? _clientSocket;

    private const int BufferSize = 1024;

    public Server(EndPoint endPoint, int backlog)
    {
        _serverSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        _serverSocket.Bind(endPoint);
        _serverSocket.Listen(backlog);
    }

    public async Task<string> ReceiveAsync()
    {
        _clientSocket = await _serverSocket.AcceptAsync();
        Console.WriteLine($"Клиент подключился: {_clientSocket.RemoteEndPoint}");

        return await ReadResponseAsync(_clientSocket);
    }

    public string HandleRequest(string requestJson)
    {
        Message response = new();

        try
        {
            var request = JsonSerializer.Deserialize<Message>(requestJson);
            if (request is null)
            {
                response.Error = "Некорректный запрос.";
            }
            else
            {
                Console.WriteLine($"Получено число: {request.Number}");
                response.Number = request.Number;
                response.Result = FactorialCalculator.Compute(request.Number);
                Console.WriteLine($"Факториал({request.Number}) = {response.Result}");
            }
        }
        catch (Exception ex)
        {
            response.Error = ex.Message;
            Console.WriteLine($"Ошибка: {ex.Message}");
        }

        return JsonSerializer.Serialize(response);
    }

    public async Task SendAsync(string response)
    {
        if (_clientSocket is not null)
        {
            var responseBytes = Encoding.UTF8.GetBytes(response);
            await _clientSocket.SendAsync(responseBytes);
            _clientSocket.Dispose();
            _clientSocket = null;
        }
    }

    private async Task<string> ReadResponseAsync(Socket clientSocket)
    {
        int readBytes;
        var buffer = new byte[BufferSize];
        var builder = new StringBuilder();

        do
        {
            readBytes = await clientSocket.ReceiveAsync(buffer);
            builder.Append(Encoding.UTF8.GetString(buffer, 0, readBytes));
        }
        while (readBytes > 0);

        return builder.ToString();
    }

    public void Dispose()
    {
        _clientSocket?.Dispose();
        _serverSocket.Dispose();
    }
}

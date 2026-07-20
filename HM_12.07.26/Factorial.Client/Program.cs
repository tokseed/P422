using Factorial.Protocol;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

const string address = "127.0.0.1";
const int port = 8888;

Console.WriteLine("Введите число для вычисления факториала (или 'exit' для выхода):");

while (true)
{
    var input = Console.ReadLine();
    if (string.IsNullOrWhiteSpace(input))
    {
        continue;
    }

    if (input.Equals("exit", StringComparison.OrdinalIgnoreCase))
    {
        break;
    }

    if (!int.TryParse(input, out int number))
    {
        Console.WriteLine("Ошибка: введите целое число.");
        continue;
    }

    using var client = new Client(address, port);
    await client.OpenConnectionAsync();

    var request = new Message { Number = number };
    var requestJson = JsonSerializer.Serialize(request);
    await client.SendAsync(requestJson);

    var responseText = await client.ReceiveAsync();
    var response = JsonSerializer.Deserialize<Message>(responseText);

    if (response?.Error is not null)
    {
        Console.WriteLine($"Ошибка сервера: {response.Error}");
    }
    else
    {
        Console.WriteLine($"Факториал({response?.Number}) = {response?.Result}");
    }

    Console.WriteLine("Введите число (или 'exit'):");
}

public class Client : IDisposable
{
    private readonly Socket _socket;
    private readonly string _address;
    private readonly int _port;

    private const int BufferSize = 1024;

    public Client(string address, int port)
    {
        _address = address;
        _port = port;
        _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    }

    public async Task OpenConnectionAsync()
    {
        await _socket.ConnectAsync(_address, _port);
    }

    public async Task SendAsync(string request)
    {
        var requestBytes = Encoding.UTF8.GetBytes(request);
        await _socket.SendAsync(requestBytes);
        _socket.Shutdown(SocketShutdown.Send);
    }

    public async Task<string> ReceiveAsync()
    {
        int readBytes;
        var buffer = new byte[BufferSize];
        var builder = new StringBuilder();

        do
        {
            readBytes = await _socket.ReceiveAsync(buffer);
            builder.Append(Encoding.UTF8.GetString(buffer, 0, readBytes));
        }
        while (readBytes > 0);

        return builder.ToString();
    }

    public void Dispose()
    {
        _socket.Dispose();
    }
}

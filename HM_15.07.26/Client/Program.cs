using Core.Model;
using Core.ModelRequest;
using Core.ModelResponce;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

Console.WriteLine("Клиентское приложение для взаимодействия с сервером");
Console.WriteLine("Доступные команды: create, read, update, delete, exit");

while (true)
{
    Console.Write("\nВведите команду: ");
    var command = Console.ReadLine()?.ToLower();

    if (command == "exit")
    {
        break;
    }

    if (string.IsNullOrEmpty(command))
    {
        continue;
    }

    TypeRequest? typeRequest = command switch
    {
        "create" => TypeRequest.Create,
        "read" => TypeRequest.Read,
        "update" => TypeRequest.Update,
        "delete" => TypeRequest.Delete,
        _ => null
    };

    if (typeRequest == null)
    {
        Console.WriteLine("Неизвестная команда");
        continue;
    }

    string body = typeRequest == TypeRequest.Read ? "" : GetBodyFromUser(typeRequest.Value);

    var request = new Request
    {
        TypeRequest = typeRequest.Value,
        Body = body
    };

    var requestJson = JsonSerializer.Serialize(request);

    try
    {
        using var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        await client.ConnectAsync(new IPEndPoint(IPAddress.Loopback, 8888));

        var requestBytes = Encoding.UTF8.GetBytes(requestJson);
        await client.SendAsync(requestBytes);

        var buffer = new byte[1024];
        var responseBuilder = new StringBuilder();
        int readBytes;

        do
        {
            readBytes = await client.ReceiveAsync(buffer);
            var responsePart = Encoding.UTF8.GetString(buffer, 0, readBytes);
            responseBuilder.Append(responsePart);
        }
        while (readBytes > 0);

        var responseJson = responseBuilder.ToString();
        var response = JsonSerializer.Deserialize<Responce>(responseJson);

        Console.WriteLine($"Ответ сервера ({response.TypeResponse}): {response.Body}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка подключения: {ex.Message}");
        Console.WriteLine("Убедитесь что сервер запущен на порту 8888");
    }
}

static string GetBodyFromUser(TypeRequest typeRequest)
{
    return typeRequest switch
    {
        TypeRequest.Create => GetProductFromUser(),
        TypeRequest.Update => GetProductFromUser(),
        TypeRequest.Delete => GetIdFromUser(),
        _ => ""
    };
}

static string GetProductFromUser()
{
    Console.Write("Введите Id: ");
    var id = int.Parse(Console.ReadLine() ?? "0");
    Console.Write("Введите Name: ");
    var name = Console.ReadLine() ?? "";
    Console.Write("Введите Description: ");
    var description = Console.ReadLine() ?? "";

    var product = new Product { Id = id, Name = name, Description = description };
    return JsonSerializer.Serialize(product);
}

static string GetIdFromUser()
{
    Console.Write("Введите Id для удаления: ");
    var id = int.Parse(Console.ReadLine() ?? "0");
    return JsonSerializer.Serialize(id);
}

using Lesson2;
using System.Net;
using System.Net.Sockets;
using System.Text;

var ipEndPoint = new IPEndPoint(IPAddress.Loopback, 8888);
var server = new Server(ipEndPoint, 1000);

Console.WriteLine("Сервер запущен");

while (true)
{
    var request = await server.ReceiveAsync();
    var responce = await server.HandleRequestAsync(request);
    await server.SendAsync(responce);
}

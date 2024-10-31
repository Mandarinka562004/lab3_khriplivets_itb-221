using System;
using System.Net;
using System.Net.Sockets;
using System.Text;
using static System.Formats.Asn1.AsnWriter;


LinkedList<string> values = new LinkedList<string>();

int port_server = 8888;
IPEndPoint ipPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), port_server);
using (Socket socke_server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp))
    try
    {
        socke_server.Bind(ipPoint);
        socke_server.Listen(1000);
        Console.WriteLine("Сервер запущен, Ожидаем...");

        using Socket client = await socke_server.AcceptAsync();
        Console.WriteLine($"IP подключения: {client.RemoteEndPoint}");

        var buffer_receive = new byte[512];
        StringBuilder stringBuilder = new StringBuilder();
        int bytes;

        do
        {
            bytes = await client.ReceiveAsync(buffer_receive);
            var messageBytes = Encoding.UTF8.GetString(buffer_receive, 0, bytes);
            stringBuilder.Append(messageBytes);
        }
        while (bytes > 0);
        Console.WriteLine(stringBuilder.ToString());
    }
    catch (SocketException e)
    {
        Console.WriteLine("Принудительное отключение хоста");
    }

















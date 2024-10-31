using System.Net;
using System.Net.Sockets;
using System.Text;


string ip = "127.0.0.1";
int port = 8888;

SendMessageToHost(ip, port);

async Task<Socket?> ConnectToHostAsync(string url, int port)
{
    Socket? socke = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
    try
    {
        await socke.ConnectAsync(url, port);
        return socke;
    }
    catch (Exception ex)
    {
        Console.WriteLine("Не подключился\n" + ex.Message);
        socke.Close();
    }
    return null;
}

async Task<string> SendMessageToHost(string ip, int port)
{
    using Socket? sockee = await ConnectToHostAsync(ip, port);
    if (sockee is null)
        return $"Не законнектился";

    Console.Write("\nВведи текст для отправки\n");
    var message = Console.ReadLine();
    var messageBytes = Encoding.UTF8.GetBytes(message);
    await sockee.SendAsync(messageBytes);

    sockee.Shutdown(SocketShutdown.Send);

    var massBytes = new byte[512];
    var builder = new StringBuilder();
    int bytes;

    do
    {
        bytes = await sockee.ReceiveAsync(massBytes);
        string respText = Encoding.UTF8.GetString(massBytes, 0, bytes);
        builder.Append(respText);
    }
    while (bytes > 0);

    await sockee.DisconnectAsync(true);
    return builder.ToString();
}

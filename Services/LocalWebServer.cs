using System.Net;
using System.Net.Sockets;
using EmbedIO;

namespace PomodoroMaui.Services;

public class LocalWebServer
{
    private WebServer? _server;
    private readonly int _port;

    public LocalWebServer()
    {
        _port = GetFreePort();
        Console.WriteLine($"Using dynamic port: {_port}");
    }

    public string BaseUrl => $"http://localhost:{_port}/index.html";
    public async void StartAsync()
    {


#if ANDROID || IOS
        string rootPath = Path.Combine(FileSystem.AppDataDirectory, "browser");
#else
        string rootPath = Path.Combine(AppContext.BaseDirectory, "browser");
#endif
        Console.WriteLine($"Serving Angular from: {rootPath}");



        _server = new WebServer(o => o
        .WithUrlPrefix($"http://localhost:{_port}")
        .WithMode(HttpListenerMode.Microsoft))
        .WithStaticFolder("/", rootPath, false);

        await _server.RunAsync();

    }

    public void Stop()
    {
        if (_server != null)
        {
            _server.Dispose();
            _server = null;
        }
    }

    private static int GetFreePort()
    {
        try
        {
            var listener = new TcpListener(IPAddress.Loopback, 0);
            listener.Start();
            int port = ((IPEndPoint)listener.LocalEndpoint).Port;
            listener.Stop();
            return port;
        }
        catch (System.Exception e)
        {
            Console.WriteLine("Error at getting free port: " + e.Message);
            return 5467;
        }
        
    }
}
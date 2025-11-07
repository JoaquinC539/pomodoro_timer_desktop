using EmbedIO;

namespace PomodoroMaui.Services;

public class LocalWebServer
{
    private WebServer? _server;
    private readonly int _port;

    public LocalWebServer(int port = 5467)
    {
        _port = port;
    }

    public string BaseUrl => $"http://localhost:{_port}/index.html";
    public async void StartAsync()
    {
        

#if ANDROID
        string rootPath = Path.Combine(FileSystem.AppDataDirectory, "browser");
#else
        string rootPath = Path.Combine(AppContext.BaseDirectory,"browser");
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
}
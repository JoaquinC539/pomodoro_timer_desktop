using System.Threading.Tasks;
using EmbedIO;
using PomodoroMaui.Services;

namespace PomodoroMaui;

public partial class WebviewPage : ContentPage
{
    private LocalWebServer? _server;
    public WebviewPage()
    {
        InitializeComponent();
        // OnLoaded();
        // LoadInitialUrl();

        // this.Loaded += OnLoaded;
        // LoadAngularApp();

    }
    protected override void OnAppearing()
    {
        var services = Application.Current?.Handler?.MauiContext?.Services;
        if(services != null)
        {
            _server = services?.GetService<LocalWebServer>();
            _server!.StartAsync();
            webView.Source = new UrlWebViewSource { Url = _server.BaseUrl };
        }
        
       
    }
    public void LoadAngularApp()
    {

#if ANDROID
        webView.Source = "https://www.google.com/";
        // webView.Source = "file:///android_asset/browser/index.html";
#elif WINDOWS
    var path = getWindowsAngularPath();
    webView.Source= new UrlWebViewSource {Url = path};   
#endif
    }
    public string getWindowsAngularPath()
    {
        // var path = Path.Combine(AppContext.BaseDirectory, "browser", "index.html");
        // if (path != null)
        // {
        //     return path;
        // }
        return "https://www.google.com/";
    }

    private string GetDataPath()
    {
#if ANDROID
        string rootPath = Path.Combine(FileSystem.AppDataDirectory, "browser");
#else
        string rootPath = Path.Combine(AppContext.BaseDirectory,"browser");
#endif
        return rootPath;
    }
    private string StartWebServer()
    {
        var url = "http://localhost:5287/";
        var basePath = GetDataPath();

        var server = new WebServer(o => o
        .WithUrlPrefix(url)
        .WithMode(HttpListenerMode.Microsoft))
        .WithStaticFolder("/", basePath, true);

        server.RunAsync();
        return url;
    }
    private void LoadInitialUrl()
    {
        string serverUrl = StartWebServer();
        webView.Source = new UrlWebViewSource {Url = serverUrl};
    }
}
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace ConsoleApp;

class Program
{
    static async Task Main(string[] args)
    {
        var builder = Host.CreateDefaultBuilder(args)
        .ConfigureServices((hostContext, services) =>
        {
            services.AddTransient<IMyLogic, MyLogic>();
            services.AddSingleton<MyApp>();
        });
        var app = builder.Build();
        await app.Services.GetRequiredService<MyApp>().StartAsync();
        Console.WriteLine("Done! Press any key to exit...");
        Console.ReadKey();
    }
}

public interface IMyLogic
{
    void Say(string message);
}

public class MyLogic : IMyLogic
{
    public void Say(string message)
    {
        Console.WriteLine(message);
    }
}

class MyApp
{
    private ILogger<MyApp> _logger;
    private IConfiguration _config;
    private IMyLogic _logic;

    public MyApp(ILogger<MyApp> logger, IConfiguration config, IMyLogic logic)
    {
        _logger = logger;
        _config = config;
        _logic = logic;
    }

    public Task StartAsync()
    {
        _logger.LogInformation(_config["App:Value1"]);
        _logger.LogInformation(_config["App:Value2"]);
        _logic.Say("Hello World!");
        return Task.CompletedTask;
    }
}
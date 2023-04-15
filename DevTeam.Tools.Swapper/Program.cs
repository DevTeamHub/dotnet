using DevTeam.Tools.Swapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var commandLineConfig = new ConfigurationBuilder()
    .AddCommandLine(args)
    .Build();

var commandLineArgs = commandLineConfig.Get<CommandLineArgs>();

var swapperConfig = new ConfigurationBuilder()
    .AddJsonFile(commandLineArgs!.Config + "\\swapper.json")
    .Build();

var swapperSection = swapperConfig.GetSection("Swapper"); 
var swapperSettings = swapperSection.Get<SwapperConfig>();

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile(swapperSettings!.BaseProjectPath + "\\swapper.json")
    .AddConfiguration(commandLineConfig)
    .AddConfiguration(swapperConfig)
    .Build();

if (commandLineArgs == null || string.IsNullOrEmpty(commandLineArgs.To))
{
    Console.WriteLine("Please provide command line arguments!");
    return;
}

if (string.IsNullOrEmpty(commandLineArgs.Config))
{
    commandLineArgs.Config = AppContext.BaseDirectory;
}

var builder = new HostBuilder()
    .ConfigureAppConfiguration((hostingContext, config) => 
    {
        config.AddConfiguration(configuration);
    })
    .ConfigureServices((hostContext, services) =>
    {
        var config = hostContext.Configuration;

        services
            .AddOptions()
            .Configure<CommandLineArgs>(config)
            .Configure<SwapperConfig>(config.GetSection("Swapper"))
            .AddSingleton<ProjectService>()
            .AddSingleton<SwapperService>();
    });

var app = builder.Build();

var swapper = app.Services.GetRequiredService<SwapperService>();
await swapper.Execute();



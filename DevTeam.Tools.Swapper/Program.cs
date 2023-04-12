using DevTeam.Tools.Swapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var configuration = new ConfigurationBuilder()
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile(AppDomain.CurrentDomain.BaseDirectory + "\\swapper.json")
    .AddCommandLine(args)
    .Build();

var builder = new HostBuilder()
    .ConfigureServices((hostContext, services) =>
    {
        services
            .AddOptions()
            .Configure<SwapperConfig>(configuration.GetSection("Swapper"))
            .AddHostedService<SwapperService>();
    });

var app = builder.Build();

await app.RunAsync();
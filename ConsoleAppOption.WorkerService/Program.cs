using System.Reflection;
using ConsoleAppOption.Processor.Data;
using ConsoleAppOption.WorkerService.DependencyIncjetion;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;


namespace ConsoleAppOption.WorkerService
{
    internal class Program
    {
        private static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args);
            await host.RunConsoleAsync();
        }

        private static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder()
                .UseConsoleLifetime()
                .ConfigureLogging(logging =>
                {
                    logging.ClearProviders();
                    logging.AddConsole();
                })
                .UseSerilog((hostContext, loggerConfiguration) =>
                {
                    loggerConfiguration.ReadFrom.Configuration(hostContext.Configuration);
                })
                .ConfigureAppConfiguration((hostContext, builder) =>
                {
                    IHostEnvironment env = hostContext.HostingEnvironment;
                    
                    builder.AddJsonFile("appsettings.json", true, true)
                    .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                    .AddCommandLine(args, CommanlineOptions())
                    .AddEnvironmentVariables()
                    .Build();
                })
                .ConfigureServices(services =>
                {
                    services.AddHostedService<Worker>();

                    services.AddOptions<Settings>().Configure<IConfiguration>((options, configuration) =>
                    {
                        configuration.GetSection(nameof(Settings)).Bind(options);
                    });

                    services.GetServiceProvider();
                });
        }

        private static IDictionary<string, string> CommanlineOptions()
        {
            return new Dictionary<string, string>()
            {
                { "--message", "settings:message" },
                { "--value", "settings:value" }
            };
        }

        private static string? GetVersion()
            => typeof(Program).Assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>()?.InformationalVersion;
    }
}
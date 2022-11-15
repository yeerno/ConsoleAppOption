using ConsoleAppOption.Processor;
using Microsoft.Extensions.DependencyInjection;

namespace ConsoleAppOption.WorkerService.DependencyIncjetion
{
    public static class ServiceProviderExtension
    {
        public static IServiceProvider GetServiceProvider(this IServiceCollection services)
        {
            services.AddTransient<IMainProcessor, MainProcessor>();

            return services.BuildServiceProvider();
        }
    }
}

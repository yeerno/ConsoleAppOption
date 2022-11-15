using ConsoleAppOption.Processor.Data;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ConsoleAppOption.Processor
{
    public class MainProcessor : IMainProcessor
    {
        private readonly ILogger<MainProcessor> logger;
        private readonly Settings settings;
        private readonly IConfiguration configuration;

        public MainProcessor(ILogger<MainProcessor> logger, IOptions<Settings> options, IConfiguration configuration)
        {
            this.logger = logger;
            this.settings = options.Value ??  throw new ArgumentException(nameof(options));
            this.configuration = configuration ?? throw new ArgumentException(nameof(configuration)); 
        }

        public async Task StartProcess(int options)
        {   
            logger.LogInformation($"{nameof(MainProcessor)}: Start Process");
            logger.LogInformation($"Checking for items to proceed: {DateTime.Now.ToString()}");
            logger.LogInformation($"From IOption, value: {settings.Value}, message: {settings.Message}");

            await Task.Delay(10000);
        }
    }
}
